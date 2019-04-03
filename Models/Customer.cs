using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Prompt ="Nombre", Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Prompt ="Apellido", Name = "Apellido")]
        public string SurName { get; set; }
        
        [Display(Prompt ="Fecha de nacimiento", Name = "Fecha de nacimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Birthdate { get; set; }
        [Display(Prompt ="Teléfono Celular", Name = "Teléfono Celular")]
        public string CellPhone { get; set; }
        [Display(Prompt ="Contacto Opcional", Name = "Contacto Opcional")]
        public string OptionalContact { get; set; }
        public string RouteId { get; set; }
        [Display(Prompt ="Ruta", Name = "Ruta")]
        public Route Route { get; set; }
        public List<Loan> Loans { get; set; }
        public List<Address> Addresses { get; set; }

        public Customer()
        {
        }
    }
}