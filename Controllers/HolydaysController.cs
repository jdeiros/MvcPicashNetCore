using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPicashNetCore.Models;

namespace MvcPicashNetCore.Controllers
{
    public class HolydaysController : Controller
    {
        private readonly PicashDbContext _context;

        public HolydaysController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: Holydays
        public async Task<IActionResult> Index()
        {
            return View(await _context.Holydays.ToListAsync());
        }

        // GET: Holydays/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holyday = await _context.Holydays
                .FirstOrDefaultAsync(m => m.HolydayId == id);
            if (holyday == null)
            {
                return NotFound();
            }

            return View(holyday);
        }

        // GET: Holydays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Holydays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HolydayId,Description,Date")] Holyday holyday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holyday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holyday);
        }

        // GET: Holydays/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holyday = await _context.Holydays.FindAsync(id);
            if (holyday == null)
            {
                return NotFound();
            }
            return View(holyday);
        }

        // POST: Holydays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HolydayId,Description,Date")] Holyday holyday)
        {
            if (id != holyday.HolydayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holyday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolydayExists(holyday.HolydayId))
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
            return View(holyday);
        }

        // GET: Holydays/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holyday = await _context.Holydays
                .FirstOrDefaultAsync(m => m.HolydayId == id);
            if (holyday == null)
            {
                return NotFound();
            }

            return View(holyday);
        }

        // POST: Holydays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var holyday = await _context.Holydays.FindAsync(id);
            _context.Holydays.Remove(holyday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolydayExists(string id)
        {
            return _context.Holydays.Any(e => e.HolydayId == id);
        }
    }
}
