using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class Invoice
    {
        public int Id { get; set; }
        [Required]
        public SupplierOrganization Supplier { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime DateDelivery { get; set; }
        [Required]
        public DateTime DateIssue { get; set; }
        [Required]
        public DateTime DateIntroduction { get; set; }

        [Required]
        public SeasonOrganization Season { get; set; }
    }
}
