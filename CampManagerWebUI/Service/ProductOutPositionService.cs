using System;
using System.Data.Entity;
using System.Transactions;

using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;

namespace CampManagerWebUI.Service
{
    public class ProductOutPositionService
    {
        private ApplicationDbContext _db;

        public ProductOutPositionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(string userName, ProductOutPosition productOutPosition, ref string error)
        {
            ProductExpendService expendService = new ProductExpendService(_db);
            using (TransactionScope scope = new TransactionScope())
            {
                _db.ProductOutPosition.Add(productOutPosition);
                _db.SaveChanges();
                int idSeason = UserSeasonHelper.GetSeason(userName).Id;
                expendService.Fill(productOutPosition, idSeason, ref error);

                if (string.IsNullOrEmpty(error))
                    scope.Complete();
            }

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(userName, productOutPosition.ProductOut.Date, ref error);
        }

        public void Edit(string userName, ProductOutPosition productOutPosition, ref string error)
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
            count.CountAndSave(userName, productOutPosition.ProductOut.Date, ref error);
        }

        public void Remove(string userName, ProductOutPosition productOutPosition, ref string error)
        {
            DateTime date = productOutPosition.ProductOut.Date;
            using (TransactionScope scope = new TransactionScope())
            {
                ProductExpendService expendService = new ProductExpendService(_db);
                expendService.Remove(productOutPosition);
                _db.ProductOutPosition.Remove(productOutPosition);
                _db.SaveChanges();

                scope.Complete();
            }

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(userName, date, ref error);
        }
    }
}