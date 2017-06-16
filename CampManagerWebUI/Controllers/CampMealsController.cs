using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;

using AutoMapper;

namespace CampManagerWebUI.Controllers
{
    [Authorize]
    public class CampMealsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CampMeals
        public ActionResult Index()
        {
            int idSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            List<CampMeal> campMealList = db.CampMeal.Where(x=> x.Camp.CampOrganization.Id == idSeason).ToList();

            List<CampMealViewModel> result = new List<CampMealViewModel>();

            foreach(var group in campMealList.GroupBy(x => x.Date).OrderByDescending(x => x.Key))
            {
                List<CampMeal> campMealDateList = campMealList.FindAll(x => x.Date == group.Key);
                List<CampMeal> campMealBreakfastDateList = campMealDateList.FindAll(x => x.Kind == KinfOfMeal.breakfast);
                List<CampMeal> campMealDinnerDateList = campMealDateList.FindAll(x => x.Kind == KinfOfMeal.dinner);
                List<CampMeal> campMealSupperDateList = campMealDateList.FindAll(x => x.Kind == KinfOfMeal.supper);
                CampMeal campMealBreakfast = new CampMeal();
                campMealBreakfast.Date = group.Key;
                campMealBreakfast.Eat = campMealBreakfastDateList.Sum(x => x.Eat);
                campMealBreakfast.EatSupplies = campMealBreakfastDateList.Sum(x => x.EatSupplies);
                campMealBreakfast.Cash = campMealBreakfastDateList.Sum(x => x.Cash);
                campMealBreakfast.Reside = campMealBreakfastDateList.Sum(x => x.Reside);

                CampMeal campMealDinner = new CampMeal();
                campMealDinner.Date = group.Key;
                campMealDinner.Eat = campMealDinnerDateList.Sum(x => x.Eat);
                campMealDinner.EatSupplies = campMealDinnerDateList.Sum(x => x.EatSupplies);
                campMealDinner.Cash = campMealDinnerDateList.Sum(x => x.Cash);
                campMealDinner.Reside = campMealDinnerDateList.Sum(x => x.Reside);

                CampMeal campMealSupper = new CampMeal();
                campMealSupper.Date = group.Key;
                campMealSupper.Eat = campMealSupperDateList.Sum(x => x.Eat);
                campMealSupper.EatSupplies = campMealSupperDateList.Sum(x => x.EatSupplies);
                campMealSupper.Cash = campMealSupperDateList.Sum(x => x.Cash);
                campMealSupper.Reside = campMealSupperDateList.Sum(x => x.Reside);

                CampMealViewModel campMealDate = CampMealViewModelCopy(campMealBreakfast, campMealDinner, campMealSupper);
                result.Add(campMealDate);
            }

            return View(result);
        }

        // GET: CampMeals/Details/5
        public ActionResult Details(int? idCamp, DateTime? date)
        {
            if (idCamp == null || date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CampMealViewModel campMealViewModel = GetCampMealViewModel(idCamp.Value, date.Value);
            if (campMealViewModel == null)
            {
                return HttpNotFound();
            }
            return View(campMealViewModel);
        }

        // GET: CampMeals/Create
        public ActionResult Create(int idCamp)
        {
            CampMealViewModel campMealViewModel = new CampMealViewModel();
            campMealViewModel.IdCamp = idCamp;
            Camp camp = db.Camp.Find(idCamp);
            campMealViewModel.CampName = camp.Name;
            campMealViewModel.Date = camp.DateStart;

            List<CampMeal> campMealLastList = db.CampMeal.Where(x => x.Camp.Id == idCamp && x.Kind == KinfOfMeal.supper)
                .OrderByDescending(x => x.Date).Take(1).ToList();            

            if (campMealLastList.Count > 0)
            {
                var campMealLast = campMealLastList[0];

                campMealViewModel.Date = campMealLast.Date.AddDays(1);
                campMealViewModel.BreakfastEat = campMealLast.Eat;
                campMealViewModel.BreakfastEatSupplies = campMealLast.EatSupplies;
                campMealViewModel.BreakfastCash = campMealLast.Cash;

                campMealViewModel.DinnerEat = campMealLast.Eat;
                campMealViewModel.DinnerEatSupplies = campMealLast.EatSupplies;
                campMealViewModel.DinnerCash = campMealLast.Cash;

                campMealViewModel.SupperEat = campMealLast.Eat;
                campMealViewModel.SupperEatSupplies = campMealLast.EatSupplies;
                campMealViewModel.SupperCash = campMealLast.Cash;

                campMealViewModel.Reside = campMealLast.Reside;
            }

            return View(campMealViewModel);
        }

        // POST: CampMeals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCamp,CampName,Date,BreakfastEat,BreakfastEatSupplies,BreakfastCash,DinnerEat,DinnerEatSupplies,DinnerCash,SupperEat,SupperEatSupplies,SupperCash,Reside")] CampMealViewModel campMealViewModel)
        {
            if (ModelState.IsValid)
            {
                CampMeal campMealBreakfast = new CampMeal();
                CampMeal campMealDinner = new CampMeal();
                CampMeal campMealSupper = new CampMeal();
                CampMealCopy(campMealBreakfast, campMealDinner, campMealSupper, campMealViewModel);

                string error = null;
                Service.CampMealService service = new Service.CampMealService(db);
                service.Add(User.Identity.Name, campMealBreakfast, campMealDinner, campMealSupper, ref error);

                return RedirectToAction("Edit", "Camps", new { id = campMealViewModel.IdCamp });
            }

            return View(campMealViewModel);
        }

        // GET: CampMeals/Edit/5
        public ActionResult Edit(int? idCamp, DateTime? date)
        {
            if (idCamp == null || date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampMealViewModel campMealViewModel = GetCampMealViewModel(idCamp.Value, date.Value);
            if (campMealViewModel == null)
            {
                return HttpNotFound();
            }
            return View(campMealViewModel);
        }

        // POST: CampMeals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdCamp,CampName,Date,BreakfastEat,BreakfastEatSupplies,BreakfastCash,DinnerEat,DinnerEatSupplies,DinnerCash,SupperEat,SupperEatSupplies,SupperCash,Reside")] CampMealViewModel campMealViewModel)
        {
            if (ModelState.IsValid)
            {
                int idCamp = campMealViewModel.IdCamp;
                DateTime date = campMealViewModel.Date;
                List<CampMeal> campMealList = db.CampMeal.Where(x => x.Camp.Id == idCamp && x.Date == date)
                    .ToList();

                CampMeal campMealBreakfast = campMealList.Find(x => x.Kind == KinfOfMeal.breakfast);
                CampMeal campMealDinner = campMealList.Find(x => x.Kind == KinfOfMeal.dinner);
                CampMeal campMealSupper = campMealList.Find(x => x.Kind == KinfOfMeal.supper);
                CampMealCopy(campMealBreakfast, campMealDinner, campMealSupper, campMealViewModel);

                string error = null;
                Service.CampMealService service = new Service.CampMealService(db);
                service.Edit(User.Identity.Name, campMealBreakfast, campMealDinner, campMealSupper, ref error);
                return RedirectToAction("Edit", "Camps", new { id = idCamp });
            }
            return View(campMealViewModel);
        }

        // GET: CampMeals/Delete/5
        public ActionResult Delete(int? idCamp, DateTime? date)
        {
            if (idCamp == null || date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampMealViewModel campMealViewModel = GetCampMealViewModel(idCamp.Value, date.Value);

            if (campMealViewModel == null)
            {
                return HttpNotFound();
            }
            return View(campMealViewModel);
        }

        // POST: CampMeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int idCamp, DateTime date)
        {
            List<CampMeal> campMealList = db.CampMeal.Where(x => x.Camp.Id == idCamp && x.Date == date)
                .ToList();

            CampMeal campMealBreakfast = campMealList.Find(x => x.Kind == KinfOfMeal.breakfast);
            CampMeal campMealDinner = campMealList.Find(x => x.Kind == KinfOfMeal.dinner);
            CampMeal campMealSupper = campMealList.Find(x => x.Kind == KinfOfMeal.supper);

            string error = null;
            Service.CampMealService service = new Service.CampMealService(db);
            service.Remove(User.Identity.Name, campMealBreakfast, campMealDinner, campMealSupper, ref error);
            // return RedirectToAction("Index");
            return RedirectToAction("Edit", "Camps", new { id = idCamp });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void CampMealCopy(CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper, CampMealViewModel campMealViewModel)
        {
            Camp camp = db.Camp.Find(campMealViewModel.IdCamp);
            campMealBreakfast.Camp = camp;
            campMealBreakfast.Date = campMealViewModel.Date;
            campMealBreakfast.Kind = KinfOfMeal.breakfast;
            campMealBreakfast.Eat = campMealViewModel.BreakfastEat;
            campMealBreakfast.EatSupplies = campMealViewModel.BreakfastEatSupplies;
            campMealBreakfast.Cash = campMealViewModel.BreakfastCash;
            campMealBreakfast.Reside = campMealViewModel.Reside;

            campMealDinner.Camp = camp;
            campMealDinner.Date = campMealViewModel.Date;
            campMealDinner.Kind = KinfOfMeal.dinner;
            campMealDinner.Eat = campMealViewModel.DinnerEat;
            campMealDinner.EatSupplies = campMealViewModel.DinnerEatSupplies;
            campMealDinner.Cash = campMealViewModel.DinnerCash;
            campMealDinner.Reside = campMealViewModel.Reside;

            campMealSupper.Camp = camp;
            campMealSupper.Date = campMealViewModel.Date;
            campMealSupper.Kind = KinfOfMeal.supper;
            campMealSupper.Eat = campMealViewModel.SupperEat;
            campMealSupper.EatSupplies = campMealViewModel.SupperEatSupplies;
            campMealSupper.Cash = campMealViewModel.SupperCash;
            campMealSupper.Reside = campMealViewModel.Reside;
        }

        private CampMealViewModel CampMealViewModelCopy(CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper)
        {
            CampMealViewModel campMealViewModel = new CampMealViewModel();
            Models.CampMealCopy.Copy2ViewModel(campMealViewModel, campMealBreakfast, campMealDinner, campMealSupper);

            return campMealViewModel;
        }

        private CampMealViewModel GetCampMealViewModel(int idCamp, DateTime date)
        {
            List<CampMeal> campMealList = db.CampMeal.Where(x => x.Camp.Id == idCamp && x.Date == date)
                .Include(x => x.Camp)
                .ToList();

            CampMeal campMealBreakfast = campMealList.Find(x => x.Kind == KinfOfMeal.breakfast);
            CampMeal campMealDinner = campMealList.Find(x => x.Kind == KinfOfMeal.dinner);
            CampMeal campMealSupper = campMealList.Find(x => x.Kind == KinfOfMeal.supper);

            CampMealViewModel campMealViewModel = CampMealViewModelCopy(campMealBreakfast, campMealDinner, campMealSupper);
            campMealViewModel.CampName = db.Camp.Find(idCamp).Name;
            // int idSeason = UserSeasonHelper.GetSeason(db).Id;
            // campMealViewModel.Camps = db.Camp.Where(x => x.CampOrganization.Id == idSeason).ToList();
            return campMealViewModel;
        }
    }
}
