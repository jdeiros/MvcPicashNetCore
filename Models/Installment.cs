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
        
        [DataType(DataType.Currency)]
        [Display(Prompt ="Monto Total", Name = "Monto Total")]
        public float Amount { get; set; }
        
        public int InstallmentNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Duedate { get; set; }
        public InstallmentStatus InstallmentStatus { get; set; }

        [DataType(DataType.Currency)]
        [Display(Prompt ="Monto de pago", Name = "Monto de pago")]
        public float PaymentAmount { get; set; }
        public Installment()
        {
        }
    }
}