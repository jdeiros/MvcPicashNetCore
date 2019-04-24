using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Installment
    {
       public string InstallmentId { get; set; }
        public string LoanId { get; set; }
        public Loan Loan { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Display(Prompt ="Monto Total", Name = "Monto Total")]
        public float Amount { get; set; }
        
        public int InstallmentNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Prompt ="Fecha de Vencimiento", Name = "Fecha de Vencimiento")]
        public DateTime Duedate { get; set; }
        public InstallmentStatus InstallmentStatus { get; set; }

        
        [Display(Prompt ="Monto de pago", Name = "Monto de pago")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public float PaymentAmount { get; set; }
        public Installment()
        {
        }
    }
}