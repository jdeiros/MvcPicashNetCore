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
    public class CollectionsController : Controller
    {
        private readonly PicashDbContext _context;

        public CollectionsController(PicashDbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var picashDbContext = _context.Installments.Include(p => p.Loan);
            return View(await picashDbContext.ToListAsync());
        }
        
    }
}
