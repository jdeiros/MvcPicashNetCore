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
    public class PaymentCommitmentsController : Controller
    {
        private readonly PicashDbContext _context;

        public PaymentCommitmentsController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: PaymentCommitments
        public async Task<IActionResult> Index()
        {
            var picashDbContext = _context.PaymentCommitments.Include(p => p.Customer);
            return View(await picashDbContext.ToListAsync());
        }

        // GET: PaymentCommitments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentCommitment = await _context.PaymentCommitments
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.PaymentCommitmentId == id);
            if (paymentCommitment == null)
            {
                return NotFound();
            }

            return View(paymentCommitment);
        }

        // GET: PaymentCommitments/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();
        }

        // POST: PaymentCommitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentCommitmentId,CreationDate,DateFrom,DateTo,TotalAmmount,CustomerId,PaymentcommitmentStatus")] PaymentCommitment paymentCommitment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentCommitment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", paymentCommitment.CustomerId);
            return View(paymentCommitment);
        }

        // GET: PaymentCommitments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentCommitment = await _context.PaymentCommitments.FindAsync(id);
            if (paymentCommitment == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", paymentCommitment.CustomerId);
            return View(paymentCommitment);
        }

        // POST: PaymentCommitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PaymentCommitmentId,CreationDate,DateFrom,DateTo,TotalAmmount,CustomerId,PaymentcommitmentStatus")] PaymentCommitment paymentCommitment)
        {
            if (id != paymentCommitment.PaymentCommitmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentCommitment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentCommitmentExists(paymentCommitment.PaymentCommitmentId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", paymentCommitment.CustomerId);
            return View(paymentCommitment);
        }

        // GET: PaymentCommitments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentCommitment = await _context.PaymentCommitments
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.PaymentCommitmentId == id);
            if (paymentCommitment == null)
            {
                return NotFound();
            }

            return View(paymentCommitment);
        }

        // POST: PaymentCommitments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var paymentCommitment = await _context.PaymentCommitments.FindAsync(id);
            _context.PaymentCommitments.Remove(paymentCommitment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentCommitmentExists(string id)
        {
            return _context.PaymentCommitments.Any(e => e.PaymentCommitmentId == id);
        }
    }
}
