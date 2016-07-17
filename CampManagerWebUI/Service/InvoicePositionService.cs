using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class InvoicePositionService
    {
        private ApplicationDbContext _db;

        public InvoicePositionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(InvoicePosition invoicePosition, ref string error)
        {
            ProductAmount productAmount = new ProductAmount();
            productAmount.InvoicePosition = invoicePosition;
            productAmount.AmountBuy = invoicePosition.Amount;
            productAmount.WorthBuy = invoicePosition.Worth;

            _db.ProductAmount.Add(productAmount);
            _db.InvoicePosition.Add(invoicePosition);
            _db.SaveChanges();
        }

        public void Edit(InvoicePosition invoicePosition, ref string error)
        {
            var productAmount = _db.ProductAmount.Where(x => x.InvoicePosition.Id == invoicePosition.Id).FirstOrDefault();

            if (productAmount == null)
            {
                error = "productAmount == null";
            }

            if (productAmount.AmountExpend != 0)
            {
                error = "Nie można edytować pozycji faktury, z której jest rozchód";
            }

            productAmount.AmountBuy = invoicePosition.Amount;
            productAmount.WorthBuy = invoicePosition.Worth;

            _db.Entry(invoicePosition).State = EntityState.Modified;
            _db.Entry(productAmount).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(InvoicePosition invoicePosition, ref string error)
        {
            var productAmount = _db.ProductAmount.Where(x => x.InvoicePosition.Id == invoicePosition.Id).FirstOrDefault();
            if (productAmount == null)
            {
                error = "productAmount == null";
            }

            if (productAmount.AmountExpend != 0)
            {
                error = "Nie można usunąć pozycji faktury, z której jest rozchód";
            }

            _db.ProductAmount.Remove(productAmount);
            _db.InvoicePosition.Remove(invoicePosition);
            _db.SaveChanges();
        }
    }
}