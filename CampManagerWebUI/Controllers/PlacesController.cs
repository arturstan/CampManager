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
    public class PlacesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Places
        public ActionResult Index()
        {
            int idBase = UserBaseHelper.GetBase(db).Id;
            return View(db.Place.Where(x => x.Base.Id == idBase).ToList().ConvertAll(x => Mapper.Map<PlaceViewModel>(x)));
        }

        // GET: Places/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Place.Find(id);
            PlaceViewModel placeViewModel = Mapper.Map<PlaceViewModel>(place);

            if (placeViewModel == null)
            {
                return HttpNotFound();
            }
            return View(placeViewModel);
        }

        // GET: Places/Create
        public ActionResult Create()
        {
            PlaceViewModel place = new PlaceViewModel();
            place.IdBaseOrganization = UserBaseHelper.GetBase(db).Id;
            return View(place);
        }

        // POST: Places/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IdBaseOrganization")] PlaceViewModel placeViewModel)
        {
            if (ModelState.IsValid)
            {
                Place place = new Place();
                place.Base = db.BaseOrganization.Find(placeViewModel.IdBaseOrganization);
                place.Name = placeViewModel.Name;
                place.Description = placeViewModel.Description;
                db.Place.Add(place);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(placeViewModel);
        }

        // GET: Places/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Place.Find(id);
            PlaceViewModel placeViewModel = Mapper.Map<PlaceViewModel>(place);
            if (placeViewModel == null)
            {
                return HttpNotFound();
            }
            return View(placeViewModel);
        }

        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] PlaceViewModel placeViewModel)
        {
            if (ModelState.IsValid)
            {
                Place place = new Place();
                place.Id = placeViewModel.Id;
                place.Base = db.BaseOrganization.Find(placeViewModel.IdBaseOrganization);
                place.Name = placeViewModel.Name;
                place.Description = placeViewModel.Description;
                db.Entry(place).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(placeViewModel);
        }

        // GET: Places/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Place.Find(id);
            PlaceViewModel placeViewModel = Mapper.Map<PlaceViewModel>(place);
            if (placeViewModel == null)
            {
                return HttpNotFound();
            }
            return View(placeViewModel);
        }

        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Place place = db.Place.Find(id);
            db.Place.Remove(place);
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
