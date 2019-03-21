using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPicashNetCore.Models
{
    public class PaymentCommitment
    {
        [Display(Prompt ="Id de Préstamo", Name = "Id Préstamo")]
        public string PaymentCommitmentId { get; set; }

        [Display(Prompt ="Fecha de Creación", Name = "Fecha de Creación")]
        public DateTime CreationDate { get; set; }

        [Display(Prompt ="Fecha de Inicio", Name = "Fecha de Inicio")]
        public DateTime DateFrom { get; set; }

        [Display(Prompt ="Fecha de Fin", Name = "Fecha de Fin")]
        public DateTime DateTo { get; set; }

        [DataType(DataType.Currency)]
        [Display(Prompt ="Monto Total", Name = "Monto Total")]
        [Required(ErrorMessage = "El monto total es requerido")]
        public float TotalAmmount { get; set; }

        [Display(Prompt ="Cantidad de Cuotas", Name = "Cantidad de Cuotas")]
        [Required(ErrorMessage = "Debe completar la cantidad de cuotas.")]
        public int InstalmentsAmount { get; set; }
        
        [Display(Prompt ="Cliente", Name = "Cliente")]
         [Required(ErrorMessage = "Seleccione el cliente.")]
        public string CustomerId { get; set; }
        [Display(Prompt ="Cliente", Name = "Cliente")]
       
        public Customer Customer { get; set; }

        [Display(Prompt ="Cuotas", Name = "Cuotas")]
        public List<Installment> Installments { get; set; }

        [Display(Prompt ="Estado del Préstamo", Name = "Estado del Préstamo")]
        public PaymentcommitmentStatus PaymentcommitmentStatus { get; set; }

        public PaymentCommitment()
        {
        }
    }
}