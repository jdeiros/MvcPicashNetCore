using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPicashNetCore.Models;

namespace MvcPicashNetCore.Controllers
{
    public class LoansController : Controller
    {
        private readonly PicashDbContext _context;

        public LoansController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            var picashDbContext = _context.Loans.Include(p => p.Customer);
            return View(await picashDbContext.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Loan = await _context.Loans
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (Loan == null)
            {
                return NotFound();
            }

            return View(Loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {            
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,InterestPercentage,InstalmentsAmount,CustomerId,LoanStatus")] Loan Loan)
        {
            Loan.CreationDate = DateTime.Today;
            Loan.DateFrom = DateTime.Today;
            Loan.DateTo = DateTime.Today.AddDays(Loan.InstalmentsAmount);
                    
            if (ModelState.IsValid)
            {
                /************* */
                List<Installment> completeListOfInstallments = SimulateCalendar(Loan);
                ViewBag.MensajeCuotas = completeListOfInstallments;
                /************* */


                _context.Add(Loan);
                await _context.SaveChangesAsync();

                
                /****************** */
                ViewBag.CustomerId = new SelectList(_context.Customers, "CustomerId", "Name", Loan.CustomerId);
                return View(Loan);
                /********************* */

                //return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", Loan.CustomerId);
            
            return View(Loan);
        }

        private static List<Installment> SimulateCalendar(Loan Loan)
        {
            /************ aca creo las cuotas para este acuerdo (simulacion debería ser) */
            List<Installment> completeListOfInstallments = new List<Installment>();
            
            float installmentTotalAmount = (Loan.TotalAmmount + Loan.TotalAmmount * Loan.InterestPercentage / 100) / Loan.InstalmentsAmount;
            for (int i = 1; i <= Loan.InstalmentsAmount; i++)
            {
                completeListOfInstallments.Add(
                            new Installment()
                            {
                                InstallmentId = Guid.NewGuid().ToString(),
                                InstallmentNumber = i,
                                InstallmentStatus = InstallmentStatus.Pending,
                                LoanId = Loan.LoanId,
                                Amount = installmentTotalAmount,
                                Duedate = DateTime.Today.AddDays(i)
                            });
            }

            return completeListOfInstallments;
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Loan = await _context.Loans.FindAsync(id);
            if (Loan == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", Loan.CustomerId);
            return View(Loan);
        }
 
        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,CustomerId,LoanStatus")] Loan Loan)
        {
            if (id != Loan.LoanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(Loan.LoanId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", Loan.CustomerId);
            return View(Loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Loan = await _context.Loans
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (Loan == null)
            {
                return NotFound();
            }

            return View(Loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(Loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(string id)
        {
            return _context.Loans.Any(e => e.LoanId == id);
        }
    }
}
