using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        // GET: Competitions/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Competitions/ShowSearchResult
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.Competition.Where(j => j.Name.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // One way to display the competitors in a specific competition
            //Is to check the ID  to the CompetitionsCompetitors
            // when getting the list, so the list that is 
            // Created below only contains the actual competitors for that competition.

            //Displays all the competitors, not only the ones in the competitiotn for now
            IList<Competitor> competitorList = _context.Competitor.ToList();
            ViewData["competitors"] = competitorList;

            //Här kommer jag skriva in en fullständigt sjuk sql-query some löser alla världsproblem
            //Just nu så visar den samtliga dykningar för alla deltagare. Inge bra.
            IList<Dive> diveList = _context.Dive.ToList();
            ViewData["dives"] = diveList;


            var competition = await _context.Competition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {

            ViewData["Competitors"] = new SelectList(_context.Competitor, "Id", "Name");

            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Date, Name")] Competition competition)
        {

            IList<Competitor> competitorList = _context.Competitor.ToList();
            ViewData["Competitors"] = new SelectList(_context.Competitor, "Id", "Name");

            if (ModelState.IsValid)
            {

                _context.Add(competition);
                await _context.SaveChangesAsync();
                TempData["CompetitionId"] = competition.Id;
                return RedirectToAction("AddCompetitors");
            }
            return View(competition);
        }

        public IActionResult AddCompetitors()
        {
            ViewData["Competitors"] = new SelectList(_context.Competitor, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompetitors(int CompetitorName)
        {
            int competeId = (int)TempData["CompetitionId"];

            CompetitionCompetitor newLink = new CompetitionCompetitor
            {
                CompetitionId = competeId,
                CompetitorId = CompetitorName
          
            };

            _context.CompetitionCompetitor.Add(newLink);
            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = competeId });
        }








        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TempData["CompetitionId"] = id;

            //Gives list of Competitor ID:s where competition is
            IList<CompetitionCompetitor> IDList = _context.CompetitionCompetitor.Where(j => j.CompetitionId.Equals(id)).ToList();

            IList<Competitor> competList = _context.Competitor.ToList();

        
            //_context.Competitor.Where(j => j.Id.Equals(_context.CompetitionCompetitor.Where(j => j.CompetitionId.Equals(id)))).ToList();

            //REturns all competitors in the selected competition

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
    }
}
