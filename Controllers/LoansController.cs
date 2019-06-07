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
using Rotativa.AspNetCore;

namespace MvcPicashNetCore.Controllers {
    public class LoansController : Controller {
        private readonly PicashDbContext _context;

        public LoansController (PicashDbContext context) {
            _context = context;
        }

        // GET: Loans
        public async Task<IActionResult> Index (string searchString, string routeId) {
            var loansFilter = from l in _context.Loans.Include(p => p.LoanType).Include(l => l.Customer).Include(l => l.Customer.Route) select l;
            //_context.Loans.Include (p => p.Customer).Include (p => p.LoanType);

             if(!String.IsNullOrEmpty(searchString))
            {
                //s => s.Name.Contains(searchString) es una expresion Lambda
                loansFilter = loansFilter.Where(s => (s.Customer.Name+" "+s.Customer.SurName).Contains(searchString)); 
            }
            if (!String.IsNullOrEmpty(routeId))
            {
                loansFilter = loansFilter.Where(s => s.Customer.Route.RouteId == routeId);
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code", routeId);
                ViewData["RouteIdSelected"] = routeId;
            }
            else
            {
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code");
            }

            return View (await loansFilter.ToListAsync ());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details (string id) {
            if (id == null) {
                return NotFound ();
            }

            var Loan = await _context.Loans
                .Include (p => p.Customer)
                .Include(p => p.LoanType)
                .Include(p => p.Installments)
                .FirstOrDefaultAsync (m => m.LoanId == id);
            if (Loan == null) {
                return NotFound ();
            }

            Loan.Installments = Loan.Installments.OrderBy(x => x.InstallmentNumber).ToList();
            var totalWithInterest = Loan.TotalAmmount + Loan.TotalAmmount * Loan.LoanType.InterestPercentage / 100;
            ViewBag.LoanTotalAmountWithInterest = String.Format ("{0:C}", totalWithInterest) + " en " + Loan.LoanType.InstallmentsAmount + " Cuotas de " + String.Format ("{0:C}", GetInstallmentTotalAmount (Loan)) + " Cada Una.";


            return View (Loan);
        }

        public async Task<IActionResult> PrintAsPdf (string id) {
            if (id == null) {
                return NotFound ();
            }

            var Loan = await _context.Loans
                .Include (p => p.Customer)
                .Include(p => p.LoanType)
                .Include(p => p.Installments)
                .Include(p => p.Customer.Addresses)
                .Include(p => p.Customer.Route)
                .Include(p => p.Customer.Route.DebtCollector)
                .Include(p => p.Customer.Route.DebtCollector)
                .FirstOrDefaultAsync (m => m.LoanId == id);
            if (Loan == null) {
                return NotFound ();
            }
            Loan.Installments = Loan.Installments.OrderBy(x => x.InstallmentNumber).ToList();
            
            //comento pq las variables viewBag llegan en null, lo calculo en la vista
            //var totalWithInterest = Loan.TotalAmmount + Loan.TotalAmmount * Loan.LoanType.InterestPercentage / 100;
            //ViewBag.LoanTotalAmountWithInterest = String.Format ("{0:C}", totalWithInterest) + " en " + Loan.LoanType.InstallmentsAmount + " Cuotas de " + String.Format ("{0:C}", GetInstallmentTotalAmount (Loan)) + " Cada Una.";
            
            return new ViewAsPdf("PrintAsPdf.1", Loan); 
            //return View (Loan);
        }
        // GET: Loans/Create
        public IActionResult Create (string routeId) {
            var customersfiltered = from a in _context.Customers.Include(p => p.Route) select a;
            if (!String.IsNullOrEmpty(routeId))
            {
                customersfiltered = customersfiltered.Where(s => s.Route.RouteId == routeId);
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code", routeId);
                ViewData["RouteIdSelected"] = routeId;
                ViewData["CustomerId"] = new SelectList (LoadListForCustomersInCombo(routeId), "Id", "Name");
            }
            else
            {
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code");
                ViewData["CustomerId"] = new SelectList (LoadListForCustomersInCombo(), "Id", "Name");
            }
            LoadCombos ();
            
            return View ();
        }

        public IActionResult Simulate ([Bind ("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,LoanTypeId,CustomerId,LoanStatus")] Loan Loan) {

            Loan.CreationDate = DateTime.Today;
            Loan.DateFrom = DateTime.Today;
            //TODO: Cargar LoanType con LoanTypeId            
            Loan.LoanType = _context.LoanTypes.Where (l => l.LoanTypeId == Loan.LoanTypeId).FirstOrDefault ();
            Loan.DateTo = DateTime.Today.AddDays (Loan.LoanType.InstallmentsAmount);
            Loan.LoanStatus = LoanStatus.Created;

            if (ModelState.IsValid) {
                //ahora creo las cuotas y le va a mandar el id de loanFF
                List<Installment> completeListOfInstallments = SimulateAmortization (Loan);
                ViewBag.MensajeCuotas = completeListOfInstallments;

                /****************** */
                LoadCombos (Loan);
                ViewData["CustomerId"] = new SelectList (LoadListForCustomersInCombo(), "Id", "Name", Loan.CustomerId);
                return View ("Create", Loan);
                /********************* */

                // return RedirectToAction(nameof(Index));
            }
            LoadCombos (Loan);
            ViewData["CustomerId"] = new SelectList (LoadListForCustomersInCombo(), "Id", "Name", Loan.CustomerId);
            return View ("Create", Loan);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind ("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,LoanTypeId,CustomerId,LoanStatus")] Loan Loan) {
            Loan.CreationDate = DateTime.Today;

            //TODO: Cargar LoanType con LoanTypeId

            Loan.LoanType = _context.LoanTypes.Where (l => l.LoanTypeId == Loan.LoanTypeId).FirstOrDefault ();
            Loan.DateFrom = GetNextInstallmentDueDate (DateTime.Today, Loan.LoanType);
            //Loan.DateTo = DateTime.Today.AddDays (Loan.LoanType.InstallmentsAmount);
            Loan.LoanStatus = LoanStatus.Created;
            Loan.Customer = _context.Customers.Where(c => c.CustomerId == Loan.CustomerId).FirstOrDefault();

            if (ModelState.IsValid) {
                try {
                    //primero guardo el prestamo para tener el id que luego se va a colocar a las cuotas
                    _context.Add (Loan);
                    await _context.SaveChangesAsync ();

                    //ahora creo las cuotas y le va a mandar el id de loan
                    List<Installment> completeListOfInstallments = SimulateAmortization (Loan);
                    ViewBag.MensajeCuotas = completeListOfInstallments;

                    foreach (Installment inst in completeListOfInstallments) {
                        _context.Add (inst);
                        await _context.SaveChangesAsync ();
                    }
                    Loan.DateTo = completeListOfInstallments.LastOrDefault().Duedate;
                    _context.Update(Loan);
                    await _context.SaveChangesAsync ();
                    var totalWithInterest = Loan.TotalAmmount + Loan.TotalAmmount * Loan.LoanType.InterestPercentage / 100;
                    ViewBag.LoanTotalAmountWithInterest = String.Format ("{0:C}", totalWithInterest) + " en " + Loan.LoanType.InstallmentsAmount + " Cuotas de " + String.Format ("{0:C}", GetInstallmentTotalAmount (Loan)) + " Cada Una.";

                } catch (Exception ex) {
                    //Log 
                    throw new Exception ("error creando el prestamo. detalle del error: " + ex.Message);
                }

                /****************** */
                LoadCombos (Loan);
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code",Loan.Customer.RouteId);
                ViewData["CustomerId"] = new SelectList (LoadListForCustomersInCombo(), "Id", "Name", Loan.CustomerId);
                return View (Loan);
                /********************* */

                // return RedirectToAction(nameof(Index));
            }
            LoadCombos (Loan);
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code",Loan.Customer.RouteId);
            ViewData["CustomerId"] = new SelectList (LoadListForCustomersInCombo(), "Id", "Name", Loan.CustomerId);
            return View (Loan);
        }

        /// recibe fecha de referencia y el tipo de prestamos y calcula el dia siguiente a vencer cuota
        private DateTime GetNextInstallmentDueDate (DateTime refDate, LoanType loanType) {
            loanType = _context.LoanTypes.Where (lt => lt.LoanTypeId == loanType.LoanTypeId).Include (c => c.CollectionWeek).FirstOrDefault ();
            DateTime next = refDate.AddDays (1);
            bool ready = false;
            bool isHolyday = false;
            while (!ready) {
                switch (next.DayOfWeek) {
                    case DayOfWeek.Monday:
                        if (!loanType.CollectionWeek.Monday) next = next.AddDays (1);
                        break;
                    case DayOfWeek.Tuesday:
                        if (!loanType.CollectionWeek.Tuesday) next = next.AddDays (1);
                        break;
                    case DayOfWeek.Wednesday:
                        if (!loanType.CollectionWeek.Wednesday) next = next.AddDays (1);
                        break;
                    case DayOfWeek.Thursday:
                        if (!loanType.CollectionWeek.Thursday) next = next.AddDays (1);
                        break;
                    case DayOfWeek.Friday:
                        if (!loanType.CollectionWeek.Friday) next = next.AddDays (1);
                        break;
                    case DayOfWeek.Saturday:
                        if (!loanType.CollectionWeek.Saturday) next = next.AddDays (1);
                        break;
                    case DayOfWeek.Sunday:
                        if (!loanType.CollectionWeek.Sunday) next = next.AddDays (1);
                        break;
                    default:
                        break;
                }

                if (!loanType.CollectionWeek.Holiday)
                    isHolyday = _context.Holydays.Any (h => h.Date.Date == next.Date.Date);

                switch (next.DayOfWeek) {
                    case DayOfWeek.Monday:
                        if (loanType.CollectionWeek.Monday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    case DayOfWeek.Tuesday:
                        if (loanType.CollectionWeek.Tuesday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    case DayOfWeek.Wednesday:
                        if (loanType.CollectionWeek.Wednesday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    case DayOfWeek.Thursday:
                        if (loanType.CollectionWeek.Thursday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    case DayOfWeek.Friday:
                        if (loanType.CollectionWeek.Friday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    case DayOfWeek.Saturday:
                        if (loanType.CollectionWeek.Saturday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    case DayOfWeek.Sunday:
                        if (loanType.CollectionWeek.Sunday && !isHolyday) ready = true;
                        else next = next.AddDays (1);
                        break;
                    default:
                        break;
                }

            }

            return next;
        }

        private void LoadCombos (Loan Loan) {
            ViewData["LoanTypeId"] = new SelectList (_context.LoanTypes, "LoanTypeId", "Code");
            
        }
        private void LoadCombos () {
            ViewData["LoanTypeId"] = new SelectList (_context.LoanTypes, "LoanTypeId", "Code");
            
        }
        private List<object> LoadListForCustomersInCombo()
        {
            List<object> customersForCombo = new List<object>();
            foreach (var customer in _context.Customers)
            {
                customersForCombo.Add(
                    new
                    {
                        Id = customer.CustomerId,
                        Name = customer.Name + " " + customer.SurName
                    }
                );
            }

            return customersForCombo;
        }

        private List<object> LoadListForCustomersInCombo(string routeId)
        {
            List<object> customersForCombo = new List<object>();
            var customersfiltered = from a in _context.Customers.Include(p => p.Route) select a;
            foreach (var customer in customersfiltered.Where(c => c.RouteId == routeId))
            {
                customersForCombo.Add(
                    new
                    {
                        Id = customer.CustomerId,
                        Name = customer.Name + " " + customer.SurName
                    }
                );
            }

            return customersForCombo;
        }
        private List<Installment> SimulateAmortization (Loan Loan) {
            /************ aca creo las cuotas para este acuerdo (simulacion debería ser) */
            List<Installment> completeListOfInstallments = new List<Installment> ();

            float installmentTotalAmount = GetInstallmentTotalAmount (Loan);

            DateTime refDate = DateTime.Today;
            for (int i = 1; i <= Loan.LoanType.InstallmentsAmount; i++) {
                DateTime next = GetNextInstallmentDueDate (refDate, Loan.LoanType);
                completeListOfInstallments.Add (
                    new Installment () {
                        InstallmentId = Guid.NewGuid ().ToString (),
                            InstallmentNumber = i,
                            InstallmentStatus = InstallmentStatus.Pendiente,
                            LoanId = Loan.LoanId,
                            Amount = installmentTotalAmount,
                            PaymentAmount = 0,
                            Duedate = next
                    });
                refDate = next;
            }

            return completeListOfInstallments;
        }

        private static float GetInstallmentTotalAmount (Loan Loan) {
            return (Loan.TotalAmmount + Loan.TotalAmmount * Loan.LoanType.InterestPercentage / 100) / Loan.LoanType.InstallmentsAmount;
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit (string id) {
            if (id == null) {
                return NotFound ();
            }

            var Loan = await _context.Loans.FindAsync (id);
            if (Loan == null) {
                return NotFound ();
            }
            ViewData["CustomerId"] = new SelectList (_context.Customers, "CustomerId", "Name", Loan.CustomerId);
            return View (Loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, [Bind ("LoanId,CreationDate,DateFrom,DateTo,TotalAmmount,CustomerId,LoanStatus")] Loan Loan) {
            if (id != Loan.LoanId) {
                return NotFound ();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update (Loan);
                    await _context.SaveChangesAsync ();
                } catch (DbUpdateConcurrencyException) {
                    if (!LoanExists (Loan.LoanId)) {
                        return NotFound ();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction (nameof (Index));
            }
            ViewData["CustomerId"] = new SelectList (_context.Customers, "CustomerId", "CustomerId", Loan.CustomerId);
            return View (Loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete (string id) {
            if (id == null) {
                return NotFound ();
            }

            var Loan = await _context.Loans
                .Include (p => p.Customer)
                .FirstOrDefaultAsync (m => m.LoanId == id);
            if (Loan == null) {
                return NotFound ();
            }

            return View (Loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (string id) {
            var Loan = await _context.Loans.FindAsync (id);

            List<Installment> installments = _context.Installments.Where (i => i.LoanId == Loan.LoanId).ToList ();
            _context.Installments.RemoveRange (installments);

            _context.Loans.Remove (Loan);

            await _context.SaveChangesAsync ();
            return RedirectToAction (nameof (Index));
        }

        private bool LoanExists (string id) {
            return _context.Loans.Any (e => e.LoanId == id);
        }
    }
}