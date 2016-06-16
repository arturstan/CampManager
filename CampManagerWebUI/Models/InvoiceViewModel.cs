using System;
using System.Collections.Generic;
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
        public string SupplierName { get; set; }

        [Required]
        public string Number { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDelivery { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateIssue { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateIntroduction { get; set; }

        List<InvoicePositionViewModel> Positions { get; }

        [Required]
        public int IdSeason { get; set; }

        public string SeasonName { get; set; }

        public List<SupplierOrganization> Suppliers;

        public InvoiceViewModel()
        {
            Positions = new List<InvoicePositionViewModel>();
        }
    }
}