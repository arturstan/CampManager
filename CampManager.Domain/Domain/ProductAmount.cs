using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class ProductAmount
    {
        public int Id { get; set; }

        public InvoicePosition InvoicePosition { get; set; }

        public decimal AmountBuy { get; set; }
        public decimal AmountExpend { get; set; }
        public decimal WorthBuy { get; set; }
        public decimal WorthExpend { get; set; }
    }
}
