using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class InvoicePosition
    {
        public int Id { get; set; }
        [Required]
        public Invoice Invoice { get; set; }

        [Required]
        public ProductOrganization Product { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [DataType("decimal(18,8)")]
        public decimal Price { get; set; }
        [Required]
        public decimal Worth { get; set; }
    }
}
