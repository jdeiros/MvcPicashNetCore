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
    public class InstallmentsController : Controller
    {
        private readonly PicashDbContext _context;

        public InstallmentsController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: Installments
        public async Task<IActionResult> Index(string loanTypeId, string dueDate)
        {
            var picashDbContext = _context.Installments
                                    .Include(i => i.Loan)
                                    .Include(x => x.Loan.Customer)
                                    .OrderBy(x => x.Loan.LoanId)
                                    .OrderBy(x => x.InstallmentNumber)
                                    .Where(x => x.Duedate == DateTime.Today);
            
            if (!String.IsNullOrEmpty(loanTypeId))
            {
                picashDbContext = picashDbContext.Where(s => s.Loan.LoanTypeId == loanTypeId);
            }
            if (!String.IsNullOrEmpty(dueDate))
                picashDbContext = picashDbContext.Where(s => s.Duedate.Date == DateTime.Parse(dueDate).Date);
            else
                picashDbContext = picashDbContext.Where(s => s.Duedate == DateTime.Today);
            
            ViewData["LoanTypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "Code", loanTypeId);
            //ViewData["LoanTypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "Code");

            return View(await picashDbContext.ToListAsync());
        }

        // GET: Installments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var installment = await _context.Installments
                .Include(i => i.Loan)
                .FirstOrDefaultAsync(m => m.InstallmentId == id);
            if (installment == null)
            {
                return NotFound();
            }

            return View(installment);
        }

        // GET: Installments/Create
        public IActionResult Create()
        {
            ViewData["LoanId"] = new SelectList(_context.Loans, "LoanId", "LoanId");
            return View();
        }

        // POST: Installments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstallmentId,LoanId,Amount,InstallmentNumber,Duedate,InstallmentStatus")] Installment installment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(installment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoanId"] = new SelectList(_context.Loans, "LoanId", "LoanId", installment.LoanId);
            return View(installment);
        }

        // GET: Installments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var installment = await _context.Installments.FindAsync(id);
            var installment = await _context.Installments
                .Include (x => x.Loan)
                .Include(x => x.Loan.Customer)
                .FirstOrDefaultAsync (m => m.InstallmentId == id);

            if (installment == null)
            {
                return NotFound();
            }
            installment.PaymentAmount = installment.Amount;
            //ViewData["LoanId"] = new SelectList(_context.Loans, "LoanId", "LoanId", installment.LoanId);
            return View(installment);
        }

        // POST: Installments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InstallmentId,LoanId,Amount,InstallmentNumber,Duedate,InstallmentStatus,PaymentAmount")] Installment installment)
        {
            if (id != installment.InstallmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                    
                    if(installment.PaymentAmount == 0)
                        installment.InstallmentStatus = InstallmentStatus.C;
                    else
                        if(installment.Amount == installment.PaymentAmount)
                            installment.InstallmentStatus = InstallmentStatus.Pago;
                        else
                            if(installment.Amount > installment.PaymentAmount)
                                installment.InstallmentStatus = InstallmentStatus.Parcial;
                            else
                                installment.InstallmentStatus = InstallmentStatus.AD;
                    
                    _context.Update(installment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstallmentExists(installment.InstallmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                installment = await _context.Installments
                                        .Include (x => x.Loan)
                                        .FirstOrDefaultAsync (m => m.InstallmentId == installment.InstallmentId);

                ViewData["LoanTypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "Code", installment.Loan.LoanTypeId);
                return RedirectToAction(nameof(Index));
                
            }
            ViewData["LoanId"] = new SelectList(_context.Loans, "LoanId", "LoanId", installment.LoanId);
            return View(installment);
        }

        // GET: Installments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var installment = await _context.Installments
                .Include(i => i.Loan)
                .FirstOrDefaultAsync(m => m.InstallmentId == id);
            if (installment == null)
            {
                return NotFound();
            }

            return View(installment);
        }

        // POST: Installments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var installment = await _context.Installments.FindAsync(id);
            _context.Installments.Remove(installment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstallmentExists(string id)
        {
            return _context.Installments.Any(e => e.InstallmentId == id);
        }
    }
}
