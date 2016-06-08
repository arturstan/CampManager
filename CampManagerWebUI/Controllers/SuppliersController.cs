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
    public class SuppliersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SupplierOrganization
        public ActionResult Index()
        {
            return View(db.SupplierOrganizations.ToList().ConvertAll(x => Mapper.Map<SupplierOrganizationViewModel>(x)));
        }

        // GET: SupplierOrganization/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var supplier = db.SupplierOrganizations.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            SupplierOrganizationViewModel supplierOrganizationViewModel = Mapper.Map<SupplierOrganizationViewModel>(supplier);
            if (supplierOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(supplierOrganizationViewModel);
        }

        // GET: SupplierOrganization/Create
        public ActionResult Create()
        {
            SupplierOrganizationViewModel supplier = new SupplierOrganizationViewModel();
            supplier.IdOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            return View(supplier);
        }

        // POST: SupplierOrganization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IdOrganization")] SupplierOrganizationViewModel supplierOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                SupplierOrganization supplier = new SupplierOrganization();
                supplier.Name = supplierOrganizationViewModel.Name;
                supplier.Description = supplierOrganizationViewModel.Description;
                supplier.Organization = db.Organization.Find(supplierOrganizationViewModel.IdOrganization);
                db.SupplierOrganizations.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplierOrganizationViewModel);
        }

        // GET: SupplierOrganization/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var supplier = db.SupplierOrganizations.Include(x => x.Organization).SingleOrDefault(x => x.Id == id);
            SupplierOrganizationViewModel supplierOrganizationViewModel = Mapper.Map<SupplierOrganizationViewModel>(supplier);
            if (supplierOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(supplierOrganizationViewModel);
        }

        // POST: SupplierOrganization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,IdOrganization")] SupplierOrganizationViewModel supplierOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                SupplierOrganization supplier = new SupplierOrganization();
                supplier.Id = supplierOrganizationViewModel.Id;
                supplier.Name = supplierOrganizationViewModel.Name;
                supplier.Description = supplierOrganizationViewModel.Description;
                supplier.Organization = db.Organization.Find(supplierOrganizationViewModel.IdOrganization);

                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplierOrganizationViewModel);
        }

        // GET: SupplierOrganization/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SupplierOrganizationViewModel supplierOrganizationViewModel = Mapper.Map<SupplierOrganizationViewModel>(db.SupplierOrganizations.Find(id));
            if (supplierOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(supplierOrganizationViewModel);
        }

        // POST: SupplierOrganization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplierOrganization supplier = db.SupplierOrganizations.Find(id);
            db.SupplierOrganizations.Remove(supplier);
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
