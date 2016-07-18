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

        public void Add(CampMeal campMeal, ref string error)
        {
            _db.CampMeal.Add(campMeal);
            _db.SaveChanges();

            MealBidCount count = new MealBidCount(_db);
            count.CountAndSave(campMeal.Date, ref error);
        }
    }
}