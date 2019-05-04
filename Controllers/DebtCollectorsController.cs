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
    public class DebtCollectorsController : Controller
    {
        private readonly PicashDbContext _context;

        public DebtCollectorsController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: DebtCollectors
        public async Task<IActionResult> Index()
        {
            var picashDbContext = _context.DebtCollectors.Include (d => d.Zone);
            return View(await picashDbContext.ToListAsync());
        }

        // GET: DebtCollectors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtCollector = await _context.DebtCollectors
                .FirstOrDefaultAsync(m => m.DebtCollectorId == id);
            if (debtCollector == null)
            {
                return NotFound();
            }

            return View(debtCollector);
        }

        // GET: DebtCollectors/Create
        public IActionResult Create()
        {
            ViewData["ZoneId"] = new SelectList(_context.Zones, "ZoneId", "Code");
            return View();
        }

        // POST: DebtCollectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DebtCollectorId,Name,SurName,Birthdate,CellPhone,OptionalContact,ZoneId")] DebtCollector debtCollector)
        {
            if (ModelState.IsValid)
            {
                _context.Add(debtCollector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ZoneId"] = new SelectList(_context.Zones, "ZoneId", "Code", debtCollector.ZoneId);
            return View(debtCollector);
        }

        // GET: DebtCollectors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtCollector = await _context.DebtCollectors.FindAsync(id);
            if (debtCollector == null)
            {
                return NotFound();
            }
            ViewData["ZoneId"] = new SelectList(_context.Zones, "ZoneId", "Code", debtCollector.ZoneId);
            return View(debtCollector);
        }

        // POST: DebtCollectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DebtCollectorId,Name,SurName,Birthdate,CellPhone,OptionalContact,ZoneId")] DebtCollector debtCollector)
        {
            if (id != debtCollector.DebtCollectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(debtCollector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DebtCollectorExists(debtCollector.DebtCollectorId))
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
            ViewData["ZoneId"] = new SelectList(_context.Zones, "ZoneId", "Code", debtCollector.ZoneId);            
            return View(debtCollector);
        }

        // GET: DebtCollectors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtCollector = await _context.DebtCollectors
                .FirstOrDefaultAsync(m => m.DebtCollectorId == id);
            if (debtCollector == null)
            {
                return NotFound();
            }

            return View(debtCollector);
        }

        // POST: DebtCollectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var debtCollector = await _context.DebtCollectors.FindAsync(id);
            _context.DebtCollectors.Remove(debtCollector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DebtCollectorExists(string id)
        {
            return _context.DebtCollectors.Any(e => e.DebtCollectorId == id);
        }
    }
}
