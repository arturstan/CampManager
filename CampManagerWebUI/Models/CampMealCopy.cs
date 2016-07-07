﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public static class CampMealCopy
    {
        public static void Copy2ViewModel(CampMealViewModel campMealViewModel, CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper)
        {
            campMealViewModel.IdCamp = campMealBreakfast.Camp.Id;
            campMealViewModel.Date = campMealBreakfast.Date;

            campMealViewModel.IdCampMealBreakfast = campMealBreakfast.Id;
            campMealViewModel.BreakfastEat = campMealBreakfast.Eat;
            campMealViewModel.BreakfastEatSupplies = campMealBreakfast.EatSupplies;
            campMealViewModel.BreakfastCash = campMealBreakfast.Cash;

            campMealViewModel.IdCampMealDinner = campMealDinner.Id;
            campMealViewModel.DinnerEat = campMealDinner.Eat;
            campMealViewModel.DinnerEatSupplies = campMealDinner.EatSupplies;
            campMealViewModel.DinnerCash = campMealDinner.Cash;

            campMealViewModel.IdCampMealSupper = campMealSupper.Id;
            campMealViewModel.SupperEat = campMealSupper.Eat;
            campMealViewModel.DinnerEatSupplies = campMealSupper.EatSupplies;
            campMealViewModel.SupperCash = campMealSupper.Cash;
        }

        public static void FillMeal(List<CampViewModel> campList, List<CampMeal> campMealList)
        {
            foreach(var camp in campList)
            {
                var campMeal = campMealList.FindAll(x => x.Camp.Id == camp.Id);
                FillMeal(camp, campMeal);
            }
        }

        private static void FillMeal(CampViewModel camp, List<CampMeal> campMealList)
        {
            foreach (var group in campMealList.GroupBy(x => x.Date))
            {
                DateTime date = group.Key;
                var campMealDate = campMealList.FindAll(x => x.Date == date);

                CampMeal campMealBreakfast = campMealDate.Find(x => x.Kind == KinfOfMeal.breakfast);
                CampMeal campMealDinner = campMealDate.Find(x => x.Kind == KinfOfMeal.dinner);
                CampMeal campMealSupper = campMealDate.Find(x => x.Kind == KinfOfMeal.supper);

                //Copy2ViewModel(camp, campMealBreakfast, campMealDinner, campMealSupper);
            }
        }
    }
}