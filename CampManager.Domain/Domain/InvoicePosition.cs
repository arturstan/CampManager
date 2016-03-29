using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class InvoicePosition
    {
        public int Id { get; set; }
        public Invoice Invoice { get; set; }

        public ProductOrganization Product { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
