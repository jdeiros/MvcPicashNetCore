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
    public class CollectionWeeksController : Controller
    {
        private readonly PicashDbContext _context;

        public CollectionWeeksController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: CollectionWeeks
        public async Task<IActionResult> Index()
        {
            return View(await _context.CollectionWeek.ToListAsync());
        }

        // GET: CollectionWeeks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionWeek = await _context.CollectionWeek
                .FirstOrDefaultAsync(m => m.CollectionWeekId == id);
            if (collectionWeek == null)
            {
                return NotFound();
            }

            return View(collectionWeek);
        }

        // GET: CollectionWeeks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CollectionWeeks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CollectionWeekId,Code,Description,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday,Holiday")] CollectionWeek collectionWeek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(collectionWeek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collectionWeek);
        }

        // GET: CollectionWeeks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionWeek = await _context.CollectionWeek.FindAsync(id);
            if (collectionWeek == null)
            {
                return NotFound();
            }
            return View(collectionWeek);
        }

        // POST: CollectionWeeks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CollectionWeekId,Code,Description,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday,Holiday")] CollectionWeek collectionWeek)
        {
            if (id != collectionWeek.CollectionWeekId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collectionWeek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionWeekExists(collectionWeek.CollectionWeekId))
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
            return View(collectionWeek);
        }

        // GET: CollectionWeeks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionWeek = await _context.CollectionWeek
                .FirstOrDefaultAsync(m => m.CollectionWeekId == id);
            if (collectionWeek == null)
            {
                return NotFound();
            }

            return View(collectionWeek);
        }

        // POST: CollectionWeeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var collectionWeek = await _context.CollectionWeek.FindAsync(id);
            _context.CollectionWeek.Remove(collectionWeek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionWeekExists(string id)
        {
            return _context.CollectionWeek.Any(e => e.CollectionWeekId == id);
        }
    }
}
