using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class InvoiceService
    {
        private ApplicationDbContext _db;

        public InvoiceService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Invoice invoice, ref string error)
        {
            invoice.Number = invoice.Number.Trim();
            string number = invoice.Number;
            var invoiceExist = _db.Invoice.FirstOrDefault(x => x.Number == number);
            if (invoiceExist != null)
            {
                error = "Istnieje faktura o tym numerze";
                return;
            }

            _db.Invoice.Add(invoice);
            _db.SaveChanges();
        }

        public void Edit(Invoice invoice, ref string error)
        {
            //invoice.Number = invoice.Number.Trim();
            //string number = invoice.Number;
            //int idInvoiceEdit = invoice.Id;
            //var invoiceExist = _db.Invoice.FirstOrDefault(x => x.Number == number && x.Id != idInvoiceEdit);
            //if (invoiceExist != null)
            //{
            //    error = "Istnieje faktura o tym numerze";
            //    return;
            //}

            _db.Entry(invoice).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}