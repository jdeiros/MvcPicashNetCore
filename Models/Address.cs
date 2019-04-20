using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Address
    {
        public string AddressId { get; set; }
         [Required(ErrorMessage = "Ingrese una descripción")]

        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsMain { get; set; }
        public string CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Address()
        {
        }
    }
}