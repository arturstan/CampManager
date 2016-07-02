using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Produkt")]
        public int IdProduct { get; set; }
        [DisplayName("Produkt")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Ilość")]
        public decimal Amount { get; set; }
        [Required]
        [DisplayName("Cena")]
        public decimal Price { get; set; }

        [DisplayName("Wartość")]
        public decimal Worth { get; set; }

        public List<ProductOrganization> Products;
    }
}