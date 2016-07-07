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
    public class CampMealsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CampMeals
        public ActionResult Index()
        {
            return View(db.CampMeal.ToList().ConvertAll(x => Mapper.Map<CampMealViewModel>(x)));
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
            int idSeason = UserSeasonHelper.GetSeason(db).Id;
            CampMealViewModel campMealViewModel = new CampMealViewModel();
            campMealViewModel.IdCamp = idCamp;
            campMealViewModel.CampName = db.Camp.Find(idCamp).Name;
            campMealViewModel.Date = DateTime.Now.Date;
            // campMealViewModel.Camps = db.Camp.Where(x => x.CampOrganization.Id == idSeason).OrderBy(x => x.Name).ToList();
            return View(campMealViewModel);
        }

        // POST: CampMeals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCamp,CampName,Date,BreakfastEat,BreakfastEatSupplies,BreakfastCash,DinnerEat,DinnerEatSupplies,DinnerCash,SupperEat,SupperEatSupplies,SupperCash")] CampMealViewModel campMealViewModel)
        {
            if (ModelState.IsValid)
            {
                CampMeal campMealBreakfast = new CampMeal();
                CampMeal campMealDinner = new CampMeal();
                CampMeal campMealSupper = new CampMeal();
                CampMealCopy(campMealBreakfast, campMealDinner, campMealSupper, campMealViewModel);
                db.CampMeal.Add(campMealBreakfast);
                db.CampMeal.Add(campMealDinner);
                db.CampMeal.Add(campMealSupper);

                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "Id,IdCamp,CampName,Date,BreakfastEat,BreakfastEatSupplies,BreakfastCash,DinnerEat,DinnerEatSupplies,DinnerCash,SupperEat,SupperEatSupplies,SupperCash")] CampMealViewModel campMealViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campMealViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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

            db.CampMeal.Remove(campMealBreakfast);
            db.CampMeal.Remove(campMealDinner);
            db.CampMeal.Remove(campMealSupper);
            db.SaveChanges();
            return RedirectToAction("Index");
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

            campMealDinner.Camp = camp;
            campMealDinner.Date = campMealViewModel.Date;
            campMealDinner.Kind = KinfOfMeal.dinner;
            campMealDinner.Eat = campMealViewModel.DinnerEat;
            campMealDinner.EatSupplies = campMealViewModel.DinnerEatSupplies;
            campMealDinner.Cash = campMealViewModel.DinnerCash;

            campMealSupper.Camp = camp;
            campMealSupper.Date = campMealViewModel.Date;
            campMealSupper.Kind = KinfOfMeal.supper;
            campMealSupper.Eat = campMealViewModel.SupperEat;
            campMealSupper.EatSupplies = campMealViewModel.DinnerEatSupplies;
            campMealSupper.Cash = campMealViewModel.SupperCash;
        }

        private CampMealViewModel CampMealViewModelCopy(CampMeal campMealBreakfast, CampMeal campMealDinner, CampMeal campMealSupper)
        {
            CampMealViewModel campMealViewModel = new CampMealViewModel();
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
            // int idSeason = UserSeasonHelper.GetSeason(db).Id;
            // campMealViewModel.Camps = db.Camp.Where(x => x.CampOrganization.Id == idSeason).ToList();
            return campMealViewModel;
        }
    }
}
