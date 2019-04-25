using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Route
    {
        public string RouteId { get; set; }
        [Display(Prompt ="Cod. Ruta", Name = "Cod. Ruta")]
        public string Code { get; set; }
        [Display(Prompt ="Nombre Ruta", Name = "Nombre Ruta")]
        public string Name { get; set; }
        [Display(Prompt ="Cobrador", Name = "Cobrador")]
        public string DebtCollectorId { get; set; }
        public DebtCollector DebtCollector { get; set; }
        public List<Customer> Customers { get; set; }

        public Route()
        {
        }
    }
}