using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        [Required]
        public int IdSupplier { get; set; }
        [DisplayName("Dostawca")]
        public string SupplierName { get; set; }

        [Required]
        [DisplayName("Numer")]
        public string Number { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Data dostawy")]
        public DateTime DateDelivery { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Data wystawienia")]
        public DateTime DateIssue { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Data wprowadzenia")]
        public DateTime DateIntroduction { get; set; }

        public List<InvoicePositionViewModel> Positions { get; }

        [Required]
        public int IdSeason { get; set; }

        [DisplayName("Sezon")]
        public string SeasonName { get; set; }

        public List<SupplierOrganization> Suppliers;

        public InvoiceViewModel()
        {
            Positions = new List<InvoicePositionViewModel>();
        }
    }
}