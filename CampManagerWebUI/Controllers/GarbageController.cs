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
using CampManagerWebUI.Service;

namespace CampManagerWebUI.Controllers
{
    public class GarbageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Garbages
        public ActionResult Index()
        {
            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (season == null)
                return View(new List<GarbageViewModel>());

            int idSeason = season.Id;
            ViewBag.SeasonActive = season.Active;

            GarbageService service = new GarbageService(db);
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            return View(service.Get(idOrganization, idSeason));
        }

        // GET: Garbages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Garbage garbage = db.Garbage.Find(id);
            if (garbage == null)
            {
                return HttpNotFound();
            }
            return View(garbage);
        }

        // GET: Garbages/Create
        public ActionResult Create()
        {
            var organization = UserOrganizationHelper.GetOrganization(User.Identity.Name);
            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (season == null || !season.Active)
                return RedirectToAction("Index");

            GarbageService service = new GarbageService(db);
            ViewBag.Error = null;
            return View(service.GetNew(organization.Id, season.Id));
        }

        // POST: Garbages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,IdSeason")] GarbageViewModel garbageViewModel,
            string[] selected)
        {            
            if (ModelState.IsValid)
            {                
                string error = null;
                GarbageService service = new GarbageService(db);
                service.SaveNew(garbageViewModel, selected, out error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Index");
            }

            return View(garbageViewModel);
        }

        // GET: Garbages/Edit/5
        public ActionResult Edit(DateTime? date)
        {
            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GarbageService service = new GarbageService(db);
            var seasonId = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            var garbageViewModel = service.Get(idOrganization, seasonId, date.Value);
            if (garbageViewModel == null)
            {
                return HttpNotFound();
            }

            return View(garbageViewModel);
        }

        // POST: Garbages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,IdSeason")] GarbageViewModel garbageViewModel,
            string[] selected)
        {
            GarbageService service = new GarbageService(db);
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            service.SaveEdit(idOrganization, garbageViewModel, selected);

            return RedirectToAction("Index");
        }

        // GET: Garbages/Delete/5
        public ActionResult Delete(DateTime? date)
        {
            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GarbageService service = new GarbageService(db);
            var seasonId = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            var garbageViewModel = service.Get(idOrganization, seasonId, date.Value);
            if (garbageViewModel == null)
            {
                return HttpNotFound();
            }

            return View(garbageViewModel);

        }

        // POST: Garbages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Garbage garbage = db.Garbage.Find(id);
            db.Garbage.Remove(garbage);
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
