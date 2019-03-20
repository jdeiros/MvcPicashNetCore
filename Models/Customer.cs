using System;
using System.Collections.Generic;

namespace MvcPicashNetCore.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime Birthdate { get; set; }
        public string CellPhone { get; set; }
        public string OptionalContact { get; set; }
        public string RouteId { get; set; }
        public Route Route { get; set; }
        public List<PaymentCommitment> PaymentCommitments { get; set; }
        public List<Address> Addresses { get; set; }

        public Customer()
        {
        }
    }
}