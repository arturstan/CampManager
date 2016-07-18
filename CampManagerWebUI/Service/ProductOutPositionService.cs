using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
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
            using (TransactionScope scope = new TransactionScope())
            {
                _db.ProductOutPosition.Add(productOutPosition);
                _db.SaveChanges();
                expendService.Fill(productOutPosition, ref error);

                if (string.IsNullOrEmpty(error))
                    scope.Complete();
            }
        }

        public void Edit(ProductOutPosition productOutPosition, ref string error)
        {
            error = "Edycja niedostępna. Funkcja w przygotowaniu.";
            return;

            using (TransactionScope scope = new TransactionScope())
            {

                ProductExpendService expendService = new ProductExpendService(_db);
                expendService.Edit(productOutPosition, ref error);

                if (string.IsNullOrEmpty(error))
                {
                    _db.Entry(productOutPosition).State = EntityState.Modified;
                    _db.SaveChanges();

                    scope.Complete();
                }
            }

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(productOutPosition.ProductOut.Date, ref error);
        }

        public void Remove(ProductOutPosition productOutPosition, ref string error)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ProductExpendService expendService = new ProductExpendService(_db);
                expendService.Remove(productOutPosition);
                _db.ProductOutPosition.Remove(productOutPosition);
                _db.SaveChanges();

                scope.Complete();
            }
        }
    }
}