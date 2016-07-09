using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void Fill()
        {
            var invoicePosition = _db.InvoicePosition.Include(x => x.Invoice).ToList();
            Fill(invoicePosition);
        }

        public void Fill(List<InvoicePosition> invoicePositionList)
        {
            var productAmountList = _db.ProductAmount.Include(x => x.InvoicePosition).ToList();
            foreach (var invoicePosition in invoicePositionList.OrderBy(x => x.Invoice.DateDelivery))
            {
                AddProductAmount(invoicePosition, productAmountList);
            }

            _db.SaveChanges();
        }

        private void AddProductAmount(InvoicePosition invoicePosition, List<ProductAmount> productAmountList)
        {
            if (productAmountList.Exists(x => x.InvoicePosition == invoicePosition))
                return;

            ProductAmount pa = new ProductAmount();
            pa.InvoicePosition = invoicePosition;
            pa.AmountBuy = invoicePosition.Amount;
            pa.WorthBuy = invoicePosition.Worth;

            _db.ProductAmount.Add(pa);
        }
    }
}
