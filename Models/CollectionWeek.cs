using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    //Semana de cobranza
    public class CollectionWeek
    {
        public string CollectionWeekId { get; set; }

        [Display(Prompt ="Código", Name = "Código")]
        public string Code { get; set; }
        [Display(Prompt ="Descripción", Name = "Descripción")]
        public string Description { get; set; }

        [Display(Prompt ="Lunes", Name = "Lunes")]
        public bool Monday { get; set; }

        [Display(Prompt ="Martes", Name = "Martes")]
        public bool Tuesday { get; set; }
        
        [Display(Prompt ="Miércoles", Name = "Miércoles")]
        public bool Wednesday  { get; set; }
        
        [Display(Prompt ="Jueves", Name = "Jueves")]
        public bool Thursday  { get; set; }
        
        [Display(Prompt ="Viernes", Name = "Viernes")]
        public bool Friday { get; set; }
        
        [Display(Prompt ="Sabado", Name = "Sabado")]
        public bool Saturday  { get; set; }
        
        [Display(Prompt ="Domingo", Name = "Domingo")]
        public bool Sunday  { get; set; }

        [Display(Prompt ="Feriados", Name = "Feriados")]
        public bool Holiday  { get; set; }
        public List<LoanType> LoanTypes { get; set; }

        public CollectionWeek()
        {
        }
    }
}