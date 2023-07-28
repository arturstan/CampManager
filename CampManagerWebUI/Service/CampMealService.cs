using System;
using System.Linq;

using CampManager.Domain.Domain;
using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class CampMealService
    {
        private ApplicationDbContext _db;

        public CampMealService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(string userName, CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper, ref string error)
        {
            bool exist = _db.CampMeal.FirstOrDefault(x => x.Camp.Id == campMealBreakfast.Camp.Id
                && x.Date == campMealBreakfast.Date) != null;
            if (exist)
            {
                error = "Istnieje wpis na taką datę";
                return;
            }

            _db.CampMeal.Add(campMealBreakfast);
            _db.CampMeal.Add(campMealDinner);
            _db.CampMeal.Add(campMealSupper);
            _db.SaveChanges();

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(userName, campMealBreakfast.Date, ref error);
        }

        public void Edit(string userName, CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper, ref string error)
        {
            _db.SaveChanges();

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(userName, campMealBreakfast.Date, ref error);
        }

        public void Remove(string userName, CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper, ref string error)
        {
            _db.CampMeal.Remove(campMealBreakfast);
            _db.CampMeal.Remove(campMealDinner);
            _db.CampMeal.Remove(campMealSupper);
            _db.SaveChanges();

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(userName, campMealBreakfast.Date, ref error);
        }
    }
}