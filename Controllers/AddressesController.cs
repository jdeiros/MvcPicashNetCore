using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPicashNetCore.Models;

namespace MvcPicashNetCore.Controllers
{
    public class AddressesController : Controller
    {
        private readonly PicashDbContext _context;

        public AddressesController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index(string searchString, string routeId)
        {
            var adressessFilter = from a in _context.Addresses.Include(a => a.Customer).Include(p => p.Customer.Route) select a;

             if(!String.IsNullOrEmpty(searchString))
            {
                //s => s.Name.Contains(searchString) es una expresion Lambda
                adressessFilter = adressessFilter.Where(s => (s.Customer.Name+" "+s.Customer.SurName).Contains(searchString)); 
            }
            if (!String.IsNullOrEmpty(routeId))
            {
                adressessFilter = adressessFilter.Where(s => s.Customer.Route.RouteId == routeId);
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code", routeId);
                ViewData["RouteIdSelected"] = routeId;
            }
            else
            {
                ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "Code");
            }

            return View(await adressessFilter.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,Description,Latitude,Longitude,IsMain,CustomerId")] Address address)
        {
            ViewBag.PersonalizedError = "";

            if (ModelState.IsValid)
            {
                if(address.IsMain == true && _context.Addresses.Any(a => a.CustomerId == address.CustomerId && a.IsMain == true))
                {  address = await _context.Addresses
                                        .Include (p => p.Customer)
                                        .FirstOrDefaultAsync (m => m.CustomerId == address.CustomerId);
                    
                    ViewBag.PersonalizedError = "Ya existe una dirección principal para el cliente "+ address.Customer.Name +" "+ address.Customer.SurName; 
                    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", address.CustomerId);
                    return View(address);                   
                }
                else
                    ViewBag.PersonalizedError = "";

                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", address.CustomerId);
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AddressId,Description,Latitude,Longitude,IsMain,CustomerId")] Address address)
        {
            if (id != address.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
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
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var address = await _context.Addresses.FindAsync(id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(string id)
        {
            return _context.Addresses.Any(e => e.AddressId == id);
        }
    }
}
