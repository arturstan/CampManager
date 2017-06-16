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
    [Authorize]
    public class MeasuresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Measures
        public ActionResult Index()
        {
            return View(db.MeasureOrganization.ToList().ConvertAll(x => Mapper.Map<MeasureOrganizationViewModel>(x)));
        }

        // GET: Measures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var measure = db.MeasureOrganization.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            MeasureOrganizationViewModel measureOrganizationViewModel = Mapper.Map<MeasureOrganizationViewModel>(measure);
            if (measureOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(measureOrganizationViewModel);
        }

        // GET: Measures/Create
        public ActionResult Create()
        {
            MeasureOrganizationViewModel measure = new MeasureOrganizationViewModel();
            measure.IdOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            return View(measure);
        }

        // POST: Measures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IdOrganization")] MeasureOrganizationViewModel measureOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                MeasureOrganization measure = new MeasureOrganization();
                measure.Name = measureOrganizationViewModel.Name;
                measure.Organization = db.Organization.Find(measureOrganizationViewModel.IdOrganization);
                db.MeasureOrganization.Add(measure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(measureOrganizationViewModel);
        }

        // GET: Measures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var measure = db.MeasureOrganization.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            MeasureOrganizationViewModel measureOrganizationViewModel = Mapper.Map<MeasureOrganizationViewModel>(measure);
            if (measureOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(measureOrganizationViewModel);
        }

        // POST: Measures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IdOrganization")] MeasureOrganizationViewModel measureOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                MeasureOrganization measure = new MeasureOrganization();
                measure.Id = measureOrganizationViewModel.Id;
                measure.Name = measureOrganizationViewModel.Name;
                measure.Organization = db.Organization.Find(measureOrganizationViewModel.IdOrganization);

                db.Entry(measure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(measureOrganizationViewModel);
        }

        // GET: Measures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureOrganizationViewModel measureOrganizationViewModel = Mapper.Map<MeasureOrganizationViewModel>(db.MeasureOrganization.Find(id));
            if (measureOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(measureOrganizationViewModel);
        }

        // POST: Measures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeasureOrganization measure = db.MeasureOrganization.Find(id);
            db.MeasureOrganization.Remove(measure);
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
