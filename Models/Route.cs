using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Route
    {
        public string RouteId { get; set; }
        [Display(Prompt ="Código de Ruta", Name = "Código de Ruta")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string DebtCollectorId { get; set; }
        public DebtCollector DebtCollector { get; set; }
        public List<Customer> Customers { get; set; }

        public Route()
        {
        }
    }
}