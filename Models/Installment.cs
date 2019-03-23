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
        public float Amount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public int InstallmentNumber { get; set; }
        public DateTime Duedate { get; set; }
        public InstallmentStatus InstallmentStatus { get; set; }

        public Installment()
        {
        }
    }
}