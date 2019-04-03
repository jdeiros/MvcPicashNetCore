using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class LoanType
    {
        public string LoanTypeId { get; set; }
        [Display(Prompt ="Código Tipo Préstamo", Name = "Código Tipo Préstamo")]
        public string Code { get; set; }
        [Display(Prompt ="Descripción", Name = "Descripción")]
        public string Description { get; set; }
        [Display(Prompt ="Interés Total (%)", Name = "Interés Total (%)")]
        [Required(ErrorMessage = "El Interés puede ser entre 0-100 %")]
        [Range(0, 100)]
        public int InterestPercentage { get; set; }

        [Display(Prompt ="Cantidad de Cuotas", Name = "Cantidad de Cuotas")]
        [Required(ErrorMessage = "Debe completar la cantidad de cuotas.")]
        public int InstallmentsAmount { get; set; }
        public List<Loan> Loans { get; set; }
        public string CollectionWeekId { get; set; }
        public CollectionWeek CollectionWeek { get; set; }
        
        public LoanType()
        {
        }
    }
}