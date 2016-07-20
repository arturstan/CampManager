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
    public class CampsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Camps
        public ActionResult Index()
        {
            int idBase = UserBaseHelper.GetBase(db).Id;
            List<CampViewModel> camps = db.Camp.Include(x => x.Place)
                .Where(x => x.CampOrganization.Base.Id == idBase)
                .ToList().ConvertAll(x => Mapper.Map<CampViewModel>(x));
            return View(camps);
        }

        // GET: Camps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Camp camp = db.Camp.Include(x => x.Place).Include(x => x.CampOrganization).FirstOrDefault(x => x.Id == id);
            CampViewModel campViewModel = Mapper.Map<CampViewModel>(camp);
            CampMealCopy.FillMeal(campViewModel, db.CampMeal.Where(x => x.Camp.Id == campViewModel.Id).ToList());
            if (campViewModel == null)
            {
                return HttpNotFound();
            }
            return View(campViewModel);
        }

        // GET: Camps/Create
        public ActionResult Create()
        {
            int idBase = UserBaseHelper.GetBase(db).Id;
            CampViewModel campViewModel = new CampViewModel();
            campViewModel.IdCampOrganization = UserSeasonHelper.GetSeason(db).Id;
            campViewModel.DateStart = DateTime.Now.Date;
            campViewModel.DateEnd = DateTime.Now.Date;
            campViewModel.Places = db.Place.Where(x => x.Base.Id == idBase).ToList();
            //campViewModel.Places = db.Place.Include(x=>x.Base).Include(x => x.Base.Organization).Where(x => x.Base.Id == idBase).ToList();
            return View(campViewModel);
        }

        // POST: Camps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IdPlace,IdCampOrganization,DateStart,DateEnd,PersonCount,PricePerPerson,Note")] CampViewModel campViewModel)
        {
            if (ModelState.IsValid)
            {
                Camp camp = new Camp();
                camp.Name = campViewModel.Name;
                camp.Description = campViewModel.Description;
                camp.CampOrganization = db.SeasonOrganization.Find(campViewModel.IdCampOrganization);
                camp.Place = db.Place.Find(campViewModel.IdPlace);
                camp.DateStart = campViewModel.DateStart;
                camp.DateEnd = campViewModel.DateEnd;
                camp.PersonCount = campViewModel.PersonCount;
                camp.PricePerPerson = campViewModel.PricePerPerson;
                camp.Note = campViewModel.Note;
                db.Camp.Add(camp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            int idBase = UserBaseHelper.GetBase(db).Id;
            campViewModel.Places = db.Place.Where(x => x.Base.Id == idBase).ToList();
            return View(campViewModel);
        }

        // GET: Camps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idBase = UserBaseHelper.GetBase(db).Id;
            Camp camp = db.Camp.Include(x => x.Place).Include(x => x.CampOrganization).FirstOrDefault(x => x.Id == id);
            CampViewModel campViewModel = Mapper.Map<CampViewModel>(camp);
            campViewModel.Places = db.Place.Where(x => x.Base.Id == idBase).ToList();
            CampMealCopy.FillMeal(campViewModel, db.CampMeal.Where(x => x.Camp.Id == campViewModel.Id).ToList());
            if (campViewModel == null)
            {
                return HttpNotFound();
            }

            return View(campViewModel);
        }

        // POST: Camps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IdPlace,PlaceName,IdCampOrganization,DateStart,DateEnd,PersonCount,PricePerPerson,Note")] CampViewModel campViewModel)
        {
            if (ModelState.IsValid)
            {
                Camp camp = new Camp();
                camp.Id = campViewModel.Id;
                camp.Name = campViewModel.Name;
                camp.Description = campViewModel.Description;
                camp.CampOrganization = db.SeasonOrganization.Find(campViewModel.IdCampOrganization);
                camp.Place = db.Place.Find(campViewModel.IdPlace);
                camp.DateStart = campViewModel.DateStart;
                camp.DateEnd = campViewModel.DateEnd;
                camp.PersonCount = campViewModel.PersonCount;
                camp.PricePerPerson = campViewModel.PricePerPerson;
                camp.Note = campViewModel.Note;

                db.Entry(camp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            int idBase = UserBaseHelper.GetBase(db).Id;
            campViewModel.Places = db.Place.Where(x => x.Base.Id == idBase).ToList();
            return View(campViewModel);
        }

        // GET: Camps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Camp camp = db.Camp.Include(x => x.Place).Include(x => x.CampOrganization).FirstOrDefault(x => x.Id == id);
            CampViewModel campViewModel = Mapper.Map<CampViewModel>(camp);
            if (campViewModel == null)
            {
                return HttpNotFound();
            }
            return View(campViewModel);
        }

        // POST: Camps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Camp campViewModel = db.Camp.Find(id);
            db.Camp.Remove(campViewModel);
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
    }
}
