using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Service
{
    public class ProductAmountService
    {
        private Db.ApplicationDbContext _db;

        public ProductAmountService(Db.ApplicationDbContext db)
        {
            _db = db;
        }

        public void Fill(List<InvoicePosition> invoicePositionList)
        {
            foreach(var invoicePosition in invoicePositionList.OrderBy(x => x.Invoice.DateDelivery))
            {
                AddProductAmount(invoicePosition);
            }

            _db.SaveChanges();
        }

        private void AddProductAmount(InvoicePosition invoicePosition)
        {
            ProductAmount pa = new ProductAmount();
            pa.InvoicePosition = invoicePosition;
            pa.AmountBuy = invoicePosition.Amount;
            pa.WorthBuy = invoicePosition.Worth;

            _db.ProductAmount.Add(pa);
        }
    }
}
