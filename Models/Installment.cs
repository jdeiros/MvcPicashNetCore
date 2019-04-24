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
        [Display(Prompt ="Monto Total de cuota", Name = "Monto Total de cuota")]
        public float Amount { get; set; }
        
        [Display(Prompt ="Cuota Número", Name = "Cuota Número")]
        public int InstallmentNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Prompt ="Fecha de Vencimiento de cuota", Name = "Fecha de Vencimiento de cuota")]
        public DateTime Duedate { get; set; }
        
        [Display(Prompt ="Estado de la Cuota", Name = "Estado de la Cuota")]
        public InstallmentStatus InstallmentStatus { get; set; }

        
        [Display(Prompt ="Monto de pago de cuota", Name = "Monto de pago de cuota")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public float PaymentAmount { get; set; }
        public Installment()
        {
        }
    }
}