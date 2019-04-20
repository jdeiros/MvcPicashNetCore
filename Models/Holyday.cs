using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class Holyday
    {
        public string HolydayId { get; set; }
        [Required(ErrorMessage = "Ingrese una descripción")]
        [Display(Prompt ="Descripción", Name = "Descripción")]

        public string Description { get; set; }
        [Display(Prompt ="Fecha Feriado", Name = "Fecha Feriado")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public Holyday()
        {
        }
    }
}