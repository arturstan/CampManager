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
using CampManager.Domain.User;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;

namespace CampManagerWebUI.Controllers
{
    [AuthorizeCustom(Role.adminOrganization, Role.deputyCommander)]
    public class SepticTankKindOrganizationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SepticTankKindOrganizations
        public ActionResult Index()
        {
            return View(db.SepticTankKindOrganization.ToList().ConvertAll(x => Mapper.Map<SepticTankKindViewModel>(x)));
        }

        // GET: SepticTankKindOrganizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SepticTankKindOrganization septicTankKindOrganization = db.SepticTankKindOrganization.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            SepticTankKindViewModel septicTankKindViewModel = Mapper.Map<SepticTankKindViewModel>(septicTankKindOrganization);
            if (septicTankKindOrganization == null)
            {
                return HttpNotFound();
            }
            return View(septicTankKindViewModel);
        }

        // GET: SepticTankKindOrganizations/Create
        public ActionResult Create()
        {
            SepticTankKindViewModel septicKind = new SepticTankKindViewModel();
            septicKind.IdOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            return View(septicKind);
        }

        // POST: SepticTankKindOrganizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Capacity,IdOrganization")] SepticTankKindViewModel septicTankKindViewModel)
        {
            if (ModelState.IsValid)
            {
                SepticTankKindOrganization septicKind = new SepticTankKindOrganization();
                septicKind.Name = septicTankKindViewModel.Name;
                septicKind.Capacity = septicTankKindViewModel.Capacity;
                septicKind.Organization = db.Organization.Find(septicTankKindViewModel.IdOrganization);
                db.SepticTankKindOrganization.Add(septicKind);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(septicTankKindViewModel);
        }

        // GET: SepticTankKindOrganizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SepticTankKindOrganization septicTankKindOrganization = db.SepticTankKindOrganization.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            SepticTankKindViewModel septicTankKindViewModel = Mapper.Map<SepticTankKindViewModel>(septicTankKindOrganization);
            if (septicTankKindViewModel == null)
            {
                return HttpNotFound();
            }
            return View(septicTankKindViewModel);
        }

        // POST: SepticTankKindOrganizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Capacity,IdOrganization")] SepticTankKindViewModel septicTankKindViewModel)
        {
            if (ModelState.IsValid)
            {
                SepticTankKindOrganization septicTankKindOrganization = new SepticTankKindOrganization();
                septicTankKindOrganization.Id = septicTankKindViewModel.Id;
                septicTankKindOrganization.Name = septicTankKindViewModel.Name;
                septicTankKindOrganization.Capacity = septicTankKindViewModel.Capacity;
                septicTankKindOrganization.Organization = db.Organization.Find(septicTankKindViewModel.IdOrganization);

                db.Entry(septicTankKindOrganization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(septicTankKindViewModel);
        }

        // GET: SepticTankKindOrganizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SepticTankKindViewModel septicTankKindViewModel = Mapper.Map<SepticTankKindViewModel>(db.SepticTankKindOrganization.Find(id));
            if (septicTankKindViewModel == null)
            {
                return HttpNotFound();
            }
            return View(septicTankKindViewModel);
        }

        // POST: SepticTankKindOrganizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SepticTankKindOrganization septicTankKindOrganization = db.SepticTankKindOrganization.Find(id);
            db.SepticTankKindOrganization.Remove(septicTankKindOrganization);
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
