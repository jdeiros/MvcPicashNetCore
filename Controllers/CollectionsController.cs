using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPicashNetCore.Models;
using Newtonsoft.Json;

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

        public JsonResult LlamarJson()
        {
            var output = ObtenerListaUsuarios();
            //var json = JsonConvert.SerializeObject(output);
            //json = JsonConvert.SerializeObject(json, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

            return new JsonResult(output);
        }

        private List<Installment> ObtenerListaUsuarios()
        {
            List<Installment> lInstallment = new List<Installment>(){
                new Installment(){ InstallmentNumber = 1,  PaymentAmount = 100 },
                new Installment(){ InstallmentNumber = 2,  PaymentAmount = 100 },
                new Installment(){ InstallmentNumber = 3,  PaymentAmount = 100 },
                new Installment(){ InstallmentNumber = 4,  PaymentAmount = 100 },
                new Installment(){ InstallmentNumber = 5,  PaymentAmount = 100  },
                new Installment(){ InstallmentNumber = 6,  PaymentAmount = 100 },
                new Installment(){ InstallmentNumber = 7,  PaymentAmount = 100  }
            };
            return lInstallment;
        }
        
    }
}
