using System;
using System.Collections.Generic;

namespace MvcPicashNetCore.Models
{
    public class Route
    {
        public string RouteId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DebtCollectorId { get; set; }
        public DebtCollector DebtCollector { get; set; }
        public List<Customer> Customers { get; set; }

        public Route()
        {
        }
    }
}