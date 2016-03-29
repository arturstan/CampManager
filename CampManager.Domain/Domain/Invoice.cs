using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class Invoice
    {
        public int Id { get; set; }
        public SupplierOrganization Supplier { get; set; }
        public string Number { get; set; }
        public DateTime DateDelivery { get; set; }
        public DateTime DateIssue { get; set; }
        public DateTime DateIntoduction { get; set; }

        public CampOrganization CampOrganization { get; set; }
    }
}
