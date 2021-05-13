using System;
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

            DiveGroup link = await _context.DiveGroup.Where(cc => cc.Dive == dive.DiveGroup).FirstAsync();
            float? diff = link.Difficulty;
            if (ModelState.IsValid)
            {
                try
                {
                    if ((dive.Judge1 < dive.Judge2 && dive.Judge2 < dive.Judge3) || (dive.Judge3 < dive.Judge2 && dive.Judge2 < dive.Judge1))
                        dive.Score = dive.Judge2 * diff;

                    else if ((dive.Judge2 < dive.Judge1 && dive.Judge1 < dive.Judge3) || (dive.Judge3 < dive.Judge1 && dive.Judge1 < dive.Judge2))
                        dive.Score = dive.Judge1 * diff;

                    else
                        dive.Score = dive.Judge3 * diff;

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

                return RedirectToAction("JudgeDive", new { CompetitorId = dive.CompetitorId, CompetitionId = dive.CompetitionId });
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);

            return View(dive);
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
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);
            return View(dive);
        }

        // POST: Dives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JudgeView(int id, [Bind("Judge1,Judge2,Judge3")] Dive dive)
        {
            if (id != dive.Id)
            {
                return NotFound();
            }

            DiveGroup link = await _context.DiveGroup.Where(cc => cc.Dive == dive.DiveGroup).FirstAsync();
            float? diff = link.Difficulty;
            if (ModelState.IsValid)
            {
                try
                {
                    if ((dive.Judge1 < dive.Judge2 && dive.Judge2 < dive.Judge3) || (dive.Judge3 < dive.Judge2 && dive.Judge2 < dive.Judge1))
                        dive.Score = dive.Judge2 * diff;

                    else if ((dive.Judge2 < dive.Judge1 && dive.Judge1 < dive.Judge3) || (dive.Judge3 < dive.Judge1 && dive.Judge1 < dive.Judge2))
                        dive.Score = dive.Judge1 * diff;

                    else
                        dive.Score = dive.Judge3 * diff;


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

                return RedirectToAction("JudgeDive", new { CompetitorId = dive.CompetitorId, CompetitionId = dive.CompetitionId });
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Id", dive.CompetitionId);
            ViewData["CompetitorId"] = new SelectList(_context.Competitor, "Id", "Id", dive.CompetitorId);
            ViewData["divecategories"] = new SelectList(_context.DiveGroup, "Dive", "Dive", dive.DiveGroup);

            return View(dive);
        }


        // GET: Dives/JudgeDive/5
        public async Task<IActionResult> JudgeDive(int CompetitorId, int CompetitionId)
        {
            ViewData["competitorId"] = CompetitorId;
            ViewData["competitionId"] = CompetitionId;

            //Returns the link
            CompetitionCompetitor link = await _context.CompetitionCompetitor.Where(cc => cc.CompetitionId == CompetitionId && cc.CompetitorId == CompetitorId).FirstAsync();

            //Returns list of dives for this link
            List<Dive> dives = await _context.Dive.Where(cc => cc.CompetitionId == CompetitionId && cc.CompetitorId == CompetitorId).ToListAsync();


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

            return View("JudgeDive", await _context.Dive.ToListAsync());
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
