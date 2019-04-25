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
        [Display(Prompt ="Monto", Name = "Monto")]
        public float Amount { get; set; }
        
        [Display(Prompt ="#Cuota", Name = "#Cuota")]
        public int InstallmentNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Prompt ="Venc.", Name = "Venc.")]
        public DateTime Duedate { get; set; }
        
        [Display(Prompt ="Status", Name = "Status")]
        public InstallmentStatus InstallmentStatus { get; set; }

        
        [Display(Prompt ="Pago", Name = "Pago")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public float PaymentAmount { get; set; }
        public Installment()
        {
        }
    }
}