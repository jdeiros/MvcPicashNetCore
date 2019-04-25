using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Address
    {
        public string AddressId { get; set; }
        [Required(ErrorMessage = "Ingrese una descripción")]
        [Display(Prompt ="Dirección", Name = "Dirección")]

        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [Display(Prompt ="Principal", Name = "Principal")]
        public bool IsMain { get; set; }
        [Display(Prompt ="Cliente", Name = "Cliente")]
        public string CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Address()
        {
        }
    }
}