using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
            _db.CampMeal.Add(campMealBreakfast);
            _db.CampMeal.Add(campMealDinner);
            _db.CampMeal.Add(campMealSupper);
            _db.SaveChanges();

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(userName, campMealBreakfast.Date, ref error);
        }

        public void Edit(string userName, CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper, ref string error)
        {
            _db.Entry(campMealBreakfast).State = EntityState.Modified;
            _db.Entry(campMealDinner).State = EntityState.Modified;
            _db.Entry(campMealSupper).State = EntityState.Modified;
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