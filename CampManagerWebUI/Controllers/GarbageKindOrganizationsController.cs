using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;

namespace CampManagerWebUI.Controllers
{
    public class GarbageKindOrganizationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GarbageKindOrganizations
        public ActionResult Index()
        {
            return View(db.GarbageKind.ToList().ConvertAll(x => Mapper.Map<GarbageKindViewModel>(x)));
        }

        // GET: GarbageKindOrganizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GarbageKindOrganization garbageKindOrganization = db.GarbageKind.Find(id);
            if (garbageKindOrganization == null)
            {
                return HttpNotFound();
            }
            return View(garbageKindOrganization);
        }

        // GET: GarbageKindOrganizations/Create
        public ActionResult Create()
        {
            GarbageKindViewModel garbageKindViewModel = new GarbageKindViewModel();
            garbageKindViewModel.IdOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            return View(garbageKindViewModel);
        }

        // POST: GarbageKindOrganizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IdOrganization")] GarbageKindViewModel garbageKindViewModel)
        {
            if (ModelState.IsValid)
            {
                GarbageKindOrganization garbageKind = new GarbageKindOrganization();
                garbageKind.Organization = db.Organization.Find(garbageKindViewModel.IdOrganization);
                garbageKind.Name = garbageKindViewModel.Name;

                db.GarbageKind.Add(garbageKind);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(garbageKindViewModel);
        }

        // GET: GarbageKindOrganizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GarbageKindOrganization garbageKindOrganization = db.GarbageKind.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            GarbageKindViewModel garbageKindViewModel = Mapper.Map<GarbageKindViewModel>(garbageKindOrganization);
            if (garbageKindViewModel == null)
            {
                return HttpNotFound();
            }
            return View(garbageKindViewModel);
        }

        // POST: GarbageKindOrganizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IdOrganization")] GarbageKindViewModel garbageKindViewModel)
        {
            if (ModelState.IsValid)
            {
                GarbageKindOrganization garbageKind = new GarbageKindOrganization();
                garbageKind.Id = garbageKindViewModel.Id;
                garbageKind.Organization = db.Organization.Find(garbageKindViewModel.IdOrganization);
                garbageKind.Name = garbageKindViewModel.Name;

                db.Entry(garbageKind).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(garbageKindViewModel);
        }

        // GET: GarbageKindOrganizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GarbageKindOrganization garbageKindOrganization = db.GarbageKind.Find(id);
            GarbageKindViewModel garbageKindViewModel = Mapper.Map<GarbageKindViewModel>(garbageKindOrganization);
            if (garbageKindViewModel == null)
            {
                return HttpNotFound();
            }
            return View(garbageKindViewModel);
        }

        // POST: GarbageKindOrganizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GarbageKindOrganization garbageKindOrganization = db.GarbageKind.Find(id);
            db.GarbageKind.Remove(garbageKindOrganization);
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
