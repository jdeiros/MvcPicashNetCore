using System;
using System.Collections.Generic;

namespace MvcPicashNetCore.Models
{
    public class Installment
    {
       public string InstallmentId { get; set; }
        public string PaymentCommitmentId { get; set; }
        public PaymentCommitment PaymentCommitment { get; set; }
        public float Amount { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime Duedate { get; set; }
        public InstallmentStatus InstallmentStatus { get; set; }

        public Installment()
        {
        }
    }
}