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
    [AuthorizeCustom(Role.adminOrganization)]
    public class BasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bases
        public ActionResult Index()
        {
            return View(db.BaseOrganization.ToList().ConvertAll(x => Mapper.Map<BaseOrganizationViewModel>(x)));
        }

        // GET: Bases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var baseOrg = db.BaseOrganization.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            BaseOrganizationViewModel baseOrganizationViewModel = Mapper.Map<BaseOrganizationViewModel>(baseOrg);
            if (baseOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(baseOrganizationViewModel);
        }

        // GET: Bases/Create
        public ActionResult Create()
        {
            BaseOrganizationViewModel baseOrg = new BaseOrganizationViewModel();
            baseOrg.IdOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            return View(baseOrg);
        }

        // POST: Bases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IdOrganization,OrganizationName")] BaseOrganizationViewModel baseOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                BaseOrganization baseOrg = new BaseOrganization();
                baseOrg.Name = baseOrganizationViewModel.Name;
                baseOrg.Description = baseOrganizationViewModel.Description;
                baseOrg.Organization = db.Organization.Find(baseOrganizationViewModel.IdOrganization);
                db.BaseOrganization.Add(baseOrg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(baseOrganizationViewModel);
        }

        // GET: Bases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var baseOrg = db.BaseOrganization.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            BaseOrganizationViewModel baseOrganizationViewModel = Mapper.Map<BaseOrganizationViewModel>(baseOrg);
            if (baseOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(baseOrganizationViewModel);
        }

        // POST: Bases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IdOrganization,OrganizationName")] BaseOrganizationViewModel baseOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                BaseOrganization baseOrg = new BaseOrganization();
                baseOrg.Id = baseOrganizationViewModel.Id;
                baseOrg.Name = baseOrganizationViewModel.Name;
                baseOrg.Description = baseOrganizationViewModel.Description;
                baseOrg.Organization = db.Organization.Find(baseOrganizationViewModel.IdOrganization);

                db.Entry(baseOrg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(baseOrganizationViewModel);
        }

        // GET: Bases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BaseOrganizationViewModel baseOrganizationViewModel = Mapper.Map<BaseOrganizationViewModel>(db.BaseOrganization.Find(id));
            if (baseOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(baseOrganizationViewModel);
        }

        // POST: Bases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaseOrganization baseOrg = db.BaseOrganization.Find(id);
            db.BaseOrganization.Remove(baseOrg);
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
