using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Zone
    {
        public string ZoneId { get; set; }
        [Display(Prompt ="Cod. Zona", Name = "Cod. Zona")]
        public string Code { get; set; }
        [Display(Prompt ="Zona", Name = "Zona")]
        public string Name { get; set; }
        
        
        public List<DebtCollector> DebtCollectors { get; set; }

        public Zone()
        {
        }
    }
}