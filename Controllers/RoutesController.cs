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
    public class RoutesController : Controller
    {
        private readonly PicashDbContext _context;

        public RoutesController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            var picashDbContext = _context.Routes.Include(r => r.DebtCollector);
            return View(await picashDbContext.ToListAsync());
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.DebtCollector)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            ViewData["DebtCollectorId"] = new SelectList(LoadListForDebtCollectorInCombo(), "Id", "Name");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteId,Code,Name,DebtCollectorId")] Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DebtCollectorId"] = new SelectList(LoadListForDebtCollectorInCombo(), "Id", "Name", route.DebtCollectorId);
            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            
            ViewData["DebtCollectorId"] = new SelectList(LoadListForDebtCollectorInCombo(), "Id", "Name", route.DebtCollectorId);
            return View(route);
        }

        private List<object> LoadListForDebtCollectorInCombo()
        {
            List<object> debtCollectorsForCombo = new List<object>();
            foreach (var debtCollector in _context.DebtCollectors)
            {
                debtCollectorsForCombo.Add(
                    new
                    {
                        Id = debtCollector.DebtCollectorId,
                        Name = debtCollector.Name + " " + debtCollector.SurName
                    }
                );
            }

            return debtCollectorsForCombo;
        }

        public IActionResult RedirectToInstallments(string id)
        {
            return RedirectToAction("Index", new Microsoft.AspNetCore.Routing.RouteValueDictionary(new { controller = "Installments", action = "Index", routeId = id }));
        }
        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RouteId,Code,Name,DebtCollectorId")] Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
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
            ViewData["DebtCollectorId"] = new SelectList(LoadListForDebtCollectorInCombo(), "Id", "Name", route.DebtCollectorId);
            return View(route);
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.DebtCollector)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var route = await _context.Routes.FindAsync(id);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(string id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
    }
}
