using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class InvoicePositionViewModel
    {
        public int Id { get; set; }        

        public int IdInvoice { get; set; }

        [Required]
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}