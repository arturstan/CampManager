using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;
using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class MealBidCount
    {
        private ApplicationDbContext _db;

        public MealBidCount(ApplicationDbContext db)
        {
            _db = db;
        }

        public void CountAndSave(DateTime date, ref string error)
        {
            SeasonOrganization season = Models.UserSeasonHelper.GetSeason(_db);
            int idSeason = season.Id;
            MealBid mealBid = _db.MealBid.FirstOrDefault(x => x.Season.Id == idSeason && x.Date == date);
            if (mealBid == null)
            {
                mealBid = new MealBid();
                mealBid.Season = season;
                mealBid.Date = date;
            }

            Count(mealBid, idSeason);
            if (mealBid.Id == 0)
                _db.MealBid.Add(mealBid);
            else
                _db.Entry(mealBid).State = EntityState.Modified;

            if (string.IsNullOrEmpty(error))
                _db.SaveChanges();
        }

        private void Count(MealBid mealBid, int idSeason)
        {
            DateTime date = mealBid.Date;
            List<ProductOutPosition> productOutList = _db.ProductOutPosition.Where(x => x.ProductOut.Season.Id == idSeason && x.ProductOut.Date == date).ToList();
            decimal productOutWorth = productOutList.Sum(x => x.Worth);

            List<CampMeal> campMealList = _db.CampMeal.Where(x => x.Camp.CampOrganization.Id == idSeason && x.Date == date).ToList();

            int breakfastCount = campMealList.FindAll(x => x.Kind == KinfOfMeal.breakfast).Sum(x => x.Eat + x.EatSupplies);
            int dinnerCount = campMealList.FindAll(x => x.Kind == KinfOfMeal.dinner).Sum(x => x.Eat + x.EatSupplies);
            int supperCount = campMealList.FindAll(x => x.Kind == KinfOfMeal.supper).Sum(x => x.Eat + x.EatSupplies);
            decimal mealCount = breakfastCount * 0.25m + dinnerCount * 0.5m + supperCount * 0.25m;

            mealBid.Expend = productOutWorth;
            mealBid.PeopleCount = mealCount;
            mealBid.Bid = mealCount > 0 ? productOutWorth / mealCount : 0;
        }
    }
}