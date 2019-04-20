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
    public class LoanTypesController : Controller
    {
        private readonly PicashDbContext _context;

        public LoanTypesController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: LoanTypes
        public async Task<IActionResult> Index()
        {
            //al cargar la tabla, se debe recibir el contexto Loantypes con el CollectionWeek cargado
            //para que pueda acceder al Codigo de Semana
            var picashDbContext = _context.LoanTypes.Include(p => p.CollectionWeek);
            return View(await picashDbContext.ToListAsync());
        }

        // GET: LoanTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanType = await _context.LoanTypes
                .FirstOrDefaultAsync(m => m.LoanTypeId == id);
            if (loanType == null)
            {
                return NotFound();
            }

            return View(loanType);
        }

        // GET: LoanTypes/Create
        public IActionResult Create()
        {
            ViewData["CollectionWeekId"] = new SelectList(_context.CollectionWeeks, "CollectionWeekId", "Code");
            return View();
        }

        // POST: LoanTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanTypeId,Code,Description,InterestPercentage,InstallmentsAmount,CollectionWeekId")] LoanType loanType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loanType);
        }

        // GET: LoanTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanType = await _context.LoanTypes.FindAsync(id);
            if (loanType == null)
            {
                return NotFound();
            }
            return View(loanType);
        }

        // POST: LoanTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LoanTypeId,Code,Description,InterestPercentage,InstallmentsAmount")] LoanType loanType)
        {
            if (id != loanType.LoanTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanTypeExists(loanType.LoanTypeId))
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
            return View(loanType);
        }

        // GET: LoanTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanType = await _context.LoanTypes
                .FirstOrDefaultAsync(m => m.LoanTypeId == id);
            if (loanType == null)
            {
                return NotFound();
            }

            return View(loanType);
        }

        // POST: LoanTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var loanType = await _context.LoanTypes.FindAsync(id);
            _context.LoanTypes.Remove(loanType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanTypeExists(string id)
        {
            return _context.LoanTypes.Any(e => e.LoanTypeId == id);
        }
    }
}
