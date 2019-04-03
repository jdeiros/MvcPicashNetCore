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
            LoadCombos();
            return View();
        }

        public IActionResult Simulate([Bind("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,LoanTypeId,CustomerId,LoanStatus")] Loan Loan)
        {
            
            Loan.CreationDate = DateTime.Today;
            Loan.DateFrom = DateTime.Today;
            //TODO: Cargar LoanType con LoanTypeId            
            Loan.LoanType = _context.LoanTypes.Where(l => l.LoanTypeId == Loan.LoanTypeId).FirstOrDefault();
            Loan.DateTo = DateTime.Today.AddDays(Loan.LoanType.InstallmentsAmount);
            Loan.LoanStatus = LoanStatus.Created;

            if (ModelState.IsValid)
            {                
                //ahora creo las cuotas y le va a mandar el id de loan
                List<Installment> completeListOfInstallments = SimulateAmortization(Loan);
                ViewBag.MensajeCuotas = completeListOfInstallments;               

                /****************** */
                LoadCombos(Loan);
                return View("Create",Loan);
                /********************* */

                // return RedirectToAction(nameof(Index));
            }
            LoadCombos(Loan);
            return View("Create",Loan);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,LoanTypeId,CustomerId,LoanStatus")] Loan Loan)
        {
            Loan.CreationDate = DateTime.Today;

            
            //TODO: Cargar LoanType con LoanTypeId
            
            Loan.LoanType = _context.LoanTypes.Where(l => l.LoanTypeId == Loan.LoanTypeId).FirstOrDefault();
            Loan.DateFrom = GetNextInstallmentDueDate(DateTime.Today, Loan.LoanType);
            Loan.DateTo = DateTime.Today.AddDays(Loan.LoanType.InstallmentsAmount);
            Loan.LoanStatus = LoanStatus.Created;

            if (ModelState.IsValid)
            {
                try{
                    //primero guardo el prestamo para tener el id que luego se va a colocar a las cuotas
                    _context.Add(Loan);
                    await _context.SaveChangesAsync();

                    //ahora creo las cuotas y le va a mandar el id de loan
                    List<Installment> completeListOfInstallments = SimulateAmortization(Loan);
                    ViewBag.MensajeCuotas = completeListOfInstallments;
                    

                    

                    foreach(Installment inst in completeListOfInstallments)
                    {
                        _context.Add(inst);
                        await _context.SaveChangesAsync();
                    }

                    var totalWithInterest =  Loan.TotalAmmount + Loan.TotalAmmount * Loan.LoanType.InterestPercentage / 100;
                    ViewBag.LoanTotalAmountWithInterest = String.Format("{0:C}", totalWithInterest) + " en " + Loan.LoanType.InstallmentsAmount + " Cuotas de " + String.Format("{0:C}", GetInstallmentTotalAmount(Loan))  + " Cada Una.";

                }
                catch(Exception ex)
                {
                    //Log 
                    throw new Exception("error creando el prestamo. detalle del error: " + ex.Message);
                }
                

                /****************** */
                LoadCombos(Loan);

                return View(Loan);
                /********************* */

                // return RedirectToAction(nameof(Index));
            }
            LoadCombos(Loan);

            return View(Loan);
        }

        /// recibe fecha de referencia y el tipo de prestamos y calcula el dia siguiente a vencer cuota
        private DateTime GetNextInstallmentDueDate(DateTime refDate, LoanType loanType)
        {
            loanType.CollectionWeek = _context.CollectionWeeks.Where(l => l.CollectionWeekId == loanType.CollectionWeekId).FirstOrDefault();
            DateTime next = refDate.AddDays(1);
            switch(next.DayOfWeek){
                case DayOfWeek.Monday: if(!loanType.CollectionWeek.Monday) next = next.AddDays(1); break;
                case DayOfWeek.Tuesday: if(!loanType.CollectionWeek.Tuesday) next = next.AddDays(1); break;
                case DayOfWeek.Wednesday: if(!loanType.CollectionWeek.Wednesday) next = next.AddDays(1); break;
                case DayOfWeek.Thursday: if(!loanType.CollectionWeek.Thursday) next = next.AddDays(1); break;
                case DayOfWeek.Friday: if(!loanType.CollectionWeek.Friday) next = next.AddDays(1); break;
                case DayOfWeek.Saturday: if(!loanType.CollectionWeek.Saturday) next = next.AddDays(1); break;
                case DayOfWeek.Sunday: if(!loanType.CollectionWeek.Sunday) next = next.AddDays(1); break;
                default: break;
            }
            //falta validar los feriados, en un ciclo recorrer mientras sea feriado aumentar un dia.
            return next;
        }

        private void LoadCombos(Loan Loan)
        {
            ViewData["LoanTypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "Description");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", Loan.CustomerId);
        }
        private void LoadCombos()
        {
            ViewData["LoanTypeId"] = new SelectList(_context.LoanTypes, "LoanTypeId", "Description");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
        }
        private List<Installment> SimulateAmortization(Loan Loan)
        {
            /************ aca creo las cuotas para este acuerdo (simulacion debería ser) */
            List<Installment> completeListOfInstallments = new List<Installment>();

            float installmentTotalAmount = GetInstallmentTotalAmount(Loan);

            DateTime refDate = DateTime.Today;
            for (int i = 0; i <= Loan.LoanType.InstallmentsAmount-1; i++)
            {
                DateTime next = GetNextInstallmentDueDate(refDate, Loan.LoanType);                
                completeListOfInstallments.Add(
                            new Installment()
                            {
                                InstallmentId = Guid.NewGuid().ToString(),
                                InstallmentNumber = i+1,
                                InstallmentStatus = InstallmentStatus.Pending,
                                LoanId = Loan.LoanId,
                                Amount = installmentTotalAmount,
                                PaymentAmount = 0,
                                Duedate = next
                            });
                refDate = next;
            }

            return completeListOfInstallments;
        }

        private static float GetInstallmentTotalAmount(Loan Loan)
        {
            return (Loan.TotalAmmount + Loan.TotalAmmount * Loan.LoanType.InterestPercentage / 100) / Loan.LoanType.InstallmentsAmount;
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
