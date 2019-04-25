using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class DebtCollector
    {
        public string DebtCollectorId { get; set; }
        [Display(Prompt ="Nombre", Name = "Nombre")]
        public string Name { get; set; }
        [Display(Prompt ="Apellido", Name = "Apellido")]
        public string SurName { get; set; }
        [Display(Prompt ="Fec. Nac.", Name = "ApelFec. Nac.lido")]
        public DateTime Birthdate { get; set; }
        [Display(Prompt ="Tel. Cel.", Name = "Tel. Cel.")]
        public string CellPhone { get; set; }
        [Display(Prompt ="Email", Name = "Email")]
        public string OptionalContact { get; set; }

        public List<Route> Routes { get; set; }

        public DebtCollector()
        {
        }
    }
}