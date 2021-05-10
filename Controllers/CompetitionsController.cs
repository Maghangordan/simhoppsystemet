using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using simhoppsystemet.Data;
using simhoppsystemet.Models;

namespace simhoppsystemet.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Competition.ToListAsync());
        }

        // ------------------------- SEARCH -----------------------------------------//

        // GET: Competitions/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: Competitions/ShowSearchResult
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.Competition.Where( j=> j.Name.Contains(SearchPhrase)).ToListAsync());
        }

        // ----------------------- DETAILS -------------------------------------- //
        
        // GET: Competitions/Details/5 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var competition = await _context.Competition
                .FirstOrDefaultAsync(m => m.Id == id);   if (competition == null) { return NotFound(); }


            IList<Competitor> competitors = GetCompetitors(id);
            ViewData["competitors"] = competitors;


            IList<Dive> diveList = _context.Dive.Where(j=>j.CompetitionId==competition.Id).ToList();
            ViewData["dives"] = diveList;

            return View(competition);
        }


        // GET: Competitions/Create
        public IActionResult Create()
        {
            IList<Competitor> competitorList = _context.Competitor.ToList();
            ViewData["competitors"] = competitorList;
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Date, Name")] Competition competition)
        {

            if (ModelState.IsValid)
            {

                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddCompetitors", new { id = competition.Id });
            }
            return View(competition);
        }

        public async Task<IActionResult> JudgeDive(int CompetitorId, int CompetitionId)
        {
            ViewData["competitorId"] = CompetitorId;
            ViewData["competitionId"] = CompetitionId;

            return View("../Dives/JudgeDive", await _context.Dive.ToListAsync());
        }

        // ------------------ ADD/DELETE COMPETITORS ------------------------------------------- //

        //GET: Competitions/AddCompetitors/5
        public async Task<IActionResult> AddCompetitors(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IList<Competitor> competitors = GetCompetitors(id);
            ViewData["competitorsAdded"] = competitors; //The competitors that are in the current competition

            ViewData["competitors"] = new SelectList(GetCompetitorsNotAdded(id), "Id", "Name"); //Currently shows all
            TempData["CompetitionId"] = id; //Used to smuggle data to AddCompetitors below
            var competition = await _context.Competition.FindAsync(id);
            return View(competition);
        }

        //POST: Competitions/AddCompetitors/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompetitors(int CompetitorName)
        {
            int competitionId = (int)TempData["CompetitionId"];
            
            var newLink = new CompetitionCompetitor
            {
                CompetitionId = competitionId,
                CompetitorId = CompetitorName
            };
           

            if (!_context.CompetitionCompetitor.Any(cc => cc.CompetitorId == newLink.CompetitorId && cc.CompetitionId == newLink.CompetitionId))
            {
                _context.CompetitionCompetitor.Add(newLink);
            }

            int DivesToCreate = 5;
            // Create 5 dives associated to the added competitor
            for (int i= 0; i < DivesToCreate; i++)
            {
                var newDive = new Dive
                {
                    CompetitionId = competitionId,
                    CompetitorId = CompetitorName
                };

                _context.Dive.Add(newDive);
            }

            _context.SaveChanges();
            return RedirectToAction("AddCompetitors");
        }

    

        //POST Competitions/DeleteCompetitor/5
        [HttpPost, ActionName("DeleteCompetitor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompetitor(int Id)
        {
            int competitionId = (int) TempData["CompetitionId"];
            CompetitionCompetitor Link = _context.CompetitionCompetitor.Where(j => j.CompetitorId == Id).FirstOrDefault(c => c.CompetitionId == competitionId);
            if (Link == null)
            {
                return NotFound();
            }

            _context.CompetitionCompetitor.Remove(Link);
            await _context.SaveChangesAsync();

            return RedirectToAction("addCompetitors", new { id = competitionId });
        }


        //--------------------------------------------------------
        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            IList<Competitor> competitorList = _context.Competitor.ToList();
            ViewData["competitors"] = competitorList;

            var competition = await _context.Competition.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name")] Competition competition)
        {
            if (id != competition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        // GET: Competitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competition = await _context.Competition.FindAsync(id);
            _context.Competition.Remove(competition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionExists(int id)
        {
            return _context.Competition.Any(e => e.Id == id);
        }
        private bool CompetitorExists(int id)
        {
            return _context.Competitor.Any(e => e.Id == id);
        }


        //Returns competitor list from a competition
        private IList<Competitor> GetCompetitors(int? id)
        {
            IList<CompetitionCompetitor> competitioncompetitors = _context.CompetitionCompetitor.Where(j => j.CompetitionId == id).ToList();
            IList<Competitor> competitor = _context.Competitor.ToList(); //Full list of all competitors
            IList<Competitor> competitors = new List<Competitor>(); //Empty list with loads of space and potential!

            competitors = competitioncompetitors.Select(cc => competitor.First(c => c.Id == cc.CompetitorId)).ToList();
            return competitors;
        }


        private IList<Competitor> GetCompetitorsNotAdded(int? id) //Return competitors not added to competition
        {
            return _context.Competitor.ToList().Except(GetCompetitors(id)).ToList(); //Competitors not added
        }
    }
}
