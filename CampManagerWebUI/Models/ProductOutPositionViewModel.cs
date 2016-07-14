using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class ProductOutPositionViewModel
    {
        public int Id { get; set; }

        [Required]
        public int IdProductOut { get; set; }

        [DisplayName("Data")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Produkt")]
        public int IdProduct { get; set; }

        [DisplayName("Produkt")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Ilość")]
        public decimal Amount { get; set; }
        [Required]
        [DataType("decimal(18,8)")]
        [DisplayName("Cena")]
        public decimal Price { get; set; }
        [Required]
        [DisplayName("Wartość")]
        public decimal Worth { get; set; }

        public List<ProductOrganizationViewModel> Products;
    }
}