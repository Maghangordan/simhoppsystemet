using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using simhoppsystemet.Data;
using simhoppsystemet.Models;

namespace simhoppsystemet.Controllers
{
    [Authorize]
    public class DivesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DivesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dives
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dive.Include(d => d.Competition).Include(d => d.Competitor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dive = await _context.Dive
                .Include(d => d.Competition)
                .Include(d => d.Competitor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dive == null)
            {
                return NotFound();
            }

            return View(dive);
        }

        // GET: Dives/Create
        public IActionResult Create()
        {
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id");
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id");
            return View();
        }

        // POST: Dives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompetitionId,CompetitorId,DiveGroup,Judge1,Judge2,Judge3,FinalScore")] Dive dive)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);
            return View(dive);
        }

        // GET: Dives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dive = await _context.Dive.FindAsync(id);
            if (dive == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);

            // Gets all of the divecategories and outs them in a list
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);

            return View(dive);
        }

        // POST: Dives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompetitionId,CompetitorId,DiveGroup,Judge1,Judge2,Judge3")] Dive dive)
        {

            if (id != dive.Id)
            {
                return NotFound();
            }

            //Returns list of dives
            List<Dive> dives = null;

            //Returns the link
            CompetitionCompetitor link = await _context.CompetitionCompetitor.Where(cc => cc.CompetitionId == dive.CompetitionId && cc.CompetitorId == dive.CompetitorId).FirstAsync();

            // Searches through the DB for the match where the divegroups match. The DiveGroup which match are put in a variable
            DiveGroup grupp = await _context.DiveGroup.Where(cc => cc.Dive == dive.DiveGroup).FirstAsync();
            double? diff = grupp.Difficulty; //Gets the difficulty from the variable that matched the dive


            // Checks the median of all dives and takes the difficulty and multiplies it to the median score
            // E.g: If J1 < J2 and J2 < J3 OR J3 < J2  and J2 < J1, then J2 is the median
            if (ModelState.IsValid)
            {
                try
                {
                    if ((dive.Judge1 < dive.Judge2 && dive.Judge2 < dive.Judge3) || (dive.Judge3 < dive.Judge2 && dive.Judge2 < dive.Judge1))
                        dive.Score = dive.Judge2 * diff * 3;

                    else if ((dive.Judge2 < dive.Judge1 && dive.Judge1 < dive.Judge3) || (dive.Judge3 < dive.Judge1 && dive.Judge1 < dive.Judge2))
                        dive.Score = dive.Judge1 * diff * 3;

                    else
                        dive.Score = dive.Judge3 * diff * 3;

                    _context.Dive.Update(dive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiveExists(dive.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                dives = await _context.Dive.Where(cc => cc.CompetitionId == dive.CompetitionId && cc.CompetitorId == dive.CompetitorId).ToListAsync();

                //Iterates over the list of dives and adds the scores together
                double? FinalScore = 0;
                foreach (var div in dives)
                {
                    FinalScore += (double?)div.Score;
                }
                link.FinalScore = FinalScore;


                _context.CompetitionCompetitor.Update(link);
                await _context.SaveChangesAsync();


                return RedirectToAction("Details", "Competitions", new { id = dive.CompetitionId });
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);

            // Gets all of the divecategories and outs them in a list
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);

            return RedirectToAction("JudgeDives", new { CompetitorId = dive.CompetitorId, CompetitionId = dive.CompetitionId });
        }

        // GET: Dives/Edit/5
        public async Task<IActionResult> JudgeView(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dive = await _context.Dive.FindAsync(id);
            if (dive == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);

            // Gets all of the divecategories and outs them in a list
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);  
            return View(dive);
        }

        public async Task<IActionResult> JudgeOne(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dive = await _context.Dive.FindAsync(id);
            if (dive == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);

            // Gets all of the divecategories and outs them in a list
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);
            return View(dive);
        }

        // POST: Dives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JudgeView(int id, [Bind("Id,CompetitionId,CompetitorId,DiveGroup,Judge1,Judge2,Judge3")] Dive dive)
        {
            if (id != dive.Id)
            {
                return NotFound();
            }

            // Searches through the DB for the match where the divegroups match. The DiveGroup which match are put in a variable
            DiveGroup link = await _context.DiveGroup.Where(cc => cc.Dive == dive.DiveGroup).FirstAsync();
            double? diff = link.Difficulty; //Gets the difficulty from the variable that matched the dive

            // Checks the median of all dives and takes the difficulty and multiplies it to the median score
            // E.g: If J1 < J2 and J2 < J3 OR J3 < J2  and J2 < J1, then J2 is the median
            if (ModelState.IsValid)
            {
                try
                {
                    if ((dive.Judge1 < dive.Judge2 && dive.Judge2 < dive.Judge3) || (dive.Judge3 < dive.Judge2 && dive.Judge2 < dive.Judge1))
                        dive.Score = dive.Judge2 * diff * 3;

                    else if ((dive.Judge2 < dive.Judge1 && dive.Judge1 < dive.Judge3) || (dive.Judge3 < dive.Judge1 && dive.Judge1 < dive.Judge2))
                        dive.Score = dive.Judge1 * diff * 3;

                    else
                        dive.Score = dive.Judge3 * diff * 3;


                    _context.Update(dive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiveExists(dive.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("JudgeDives", new { dive.CompetitorId, dive.CompetitionId });
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);

            // Gets all of the divecategories and outs them in a list
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);

            return View(dive);
        }


        // GET: Dives/JudgeDives/5
        public async Task<IActionResult> JudgeDives(int CompetitorId, int CompetitionId)
        {
            ViewData["competitorId"] = CompetitorId;
            ViewData["competitionId"] = CompetitionId;

            //Returns the link
            CompetitionCompetitor link = await _context.CompetitionCompetitor.Where(cc => cc.CompetitionId == CompetitionId && cc.CompetitorId == CompetitorId).FirstAsync();

            //Returns list of dives for this link
            List<Dive> dives = await _context.Dive.Where(cc => cc.CompetitionId == CompetitionId && cc.CompetitorId == CompetitorId).ToListAsync();
            
            Competitor divers = _context.Competitor.Where(d => d.Id == CompetitorId).First(); // Returns the competitor
            ViewData["competitorName"] = divers.Name;
            Competition divecomp = _context.Competition.Where(c => c.Id == CompetitionId).First(); // Rturns the competition
            ViewData["competitionName"] = divecomp.Name;

            ViewData["divedifficulty"] = _context.DiveGroup.ToList(); //Full list of all Divegroups


            //Iterates over the list of dives and adds the scores together
            double? FinalScore = 0;
            foreach (var dive in dives)
            {
                FinalScore += (double?)dive.Score;
            }

            //Displays final score
            ViewData["FinalScore"] = FinalScore;

            //Updates the final score to the database
            link.FinalScore = FinalScore;
            _context.CompetitionCompetitor.Update(link);
            await _context.SaveChangesAsync();

            return View("JudgeDives", await _context.Dive.ToListAsync());
        }

        // GET: Dives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dive = await _context.Dive
                .Include(d => d.Competition)
                .Include(d => d.Competitor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dive == null)
            {
                return NotFound();
            }

            return View(dive);
        }

        // POST: Dives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dive = await _context.Dive.FindAsync(id);
            _context.Dive.Remove(dive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiveExists(int id)
        {
            return _context.Dive.Any(e => e.Id == id);
        }
    }
}
