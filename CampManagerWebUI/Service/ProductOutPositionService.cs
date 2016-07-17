using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class ProductOutPositionService
    {
        private ApplicationDbContext _db;

        public ProductOutPositionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(ProductOutPosition productOutPosition, ref string error)
        {
            ProductExpendService expendService = new ProductExpendService(_db);
            _db.ProductOutPosition.Add(productOutPosition);
            _db.SaveChanges();
            expendService.Fill(productOutPosition);
        }

        public void Edit(ProductOutPosition productOutPosition, ref string error)
        {
            error = "Edycja niedostępna. Funkcja w przygotowaniu.";
            return;

            ProductExpendService expendService = new ProductExpendService(_db);
            expendService.Edit(productOutPosition);

            _db.Entry(productOutPosition).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Remove(ProductOutPosition productOutPosition, ref string error)
        {
            ProductExpendService expendService = new ProductExpendService(_db);
            expendService.Remove(productOutPosition);
            _db.ProductOutPosition.Remove(productOutPosition);
            _db.SaveChanges();
        }
    }
}