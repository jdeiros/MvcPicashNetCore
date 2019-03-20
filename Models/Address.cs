using System;

namespace MvcPicashNetCore.Models
{
    public class Address
    {
        public string AddressId { get; set; }

        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsMain { get; set; }
        public string CustomerId { get; set; }
        Customer Customer { get; set; }

        public Address()
        {
        }
    }
}