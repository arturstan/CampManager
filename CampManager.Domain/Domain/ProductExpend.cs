using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class ProductExpend
    {
        public int Id { get; set; }

        public InvoicePosition InvoicePosition { get; set; }

        public ProductOutPosition ProductOutPosition { get; set; }

        public decimal Amount { get; set; }

        public decimal Worth { get; set; }
    }
}
