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
    public class DiveGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiveGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DiveGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiveGroup.ToListAsync());
        }

        // GET: DiveGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diveGroup = await _context.DiveGroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diveGroup == null)
            {
                return NotFound();
            }

            return View(diveGroup);
        }

        // GET: DiveGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiveGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dive,Difficulty")] DiveGroup diveGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diveGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diveGroup);
        }

        // GET: DiveGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diveGroup = await _context.DiveGroup.FindAsync(id);
            if (diveGroup == null)
            {
                return NotFound();
            }
            return View(diveGroup);
        }

        // POST: DiveGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dive,Difficulty")] DiveGroup diveGroup)
        {
            if (id != diveGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diveGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiveGroupExists(diveGroup.Id))
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
            return View(diveGroup);
        }

        // GET: DiveGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diveGroup = await _context.DiveGroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diveGroup == null)
            {
                return NotFound();
            }

            return View(diveGroup);
        }

        // POST: DiveGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diveGroup = await _context.DiveGroup.FindAsync(id);
            _context.DiveGroup.Remove(diveGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiveGroupExists(int id)
        {
            return _context.DiveGroup.Any(e => e.Id == id);
        }
    }
}
