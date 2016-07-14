using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class ProductService
    {
        private ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(ProductOrganization product, ref string error)
        {
            var productExist = _db.ProductOrganization.FirstOrDefault(x => x.Name == product.Name && x.Description == product.Description);
            if (productExist != null)
            {
                error = "Istnieje już produkt o takiej nazwie i opisie";
                return;
            }

            _db.ProductOrganization.Add(product);
            _db.SaveChanges();
        }

        public void Edit(ProductOrganization product, ref string error)
        {
            var productExist = _db.ProductOrganization.FirstOrDefault(x => x.Name == product.Name
                && x.Description != product.Description
                && x.Id != product.Id);
            if (productExist != null)
            {
                error = "Istnieje już produkt o takiej nazwie i opisie";
                return;
            }

            _db.Entry(product).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(ProductOrganization product, ref string error)
        {
            var invoicePosition = _db.InvoicePosition.FirstOrDefault(x => x.Product.Id == product.Id);
            if (invoicePosition != null)
            {
                error = "Nie można usunąć produktu, bo istnieją faktury z tym produktem";
                return;
            }

            _db.ProductOrganization.Remove(product);
            _db.SaveChanges();
        }
    }
}