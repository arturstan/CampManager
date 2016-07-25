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
    public class InvoicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invoices
        public ActionResult Index(int? idSupplier = null)
        {
            List<Invoice> invoiceList;
            if (idSupplier.HasValue && idSupplier != -1)
            {
                invoiceList = db.Invoice.Include(x => x.Season).Include(x => x.Supplier).Include(x => x.Positions)
                    .Where(x => x.Supplier.Id == idSupplier)
                    .OrderByDescending(x => x.Id)
                    .ToList();
            }
            else
            {
                invoiceList = db.Invoice.Include(x => x.Season).Include(x => x.Supplier).Include(x => x.Positions)
                .OrderByDescending(x => x.Id)
                .ToList();
            }

            List<InvoiceViewModel> invoiceViewModel = invoiceList.ConvertAll(x => Mapper.Map<InvoiceViewModel>(x));
            ViewBag.idSupplier = new SelectList(GetSuppliers(), "Id", "Name", idSupplier);
            return View(invoiceViewModel);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var invoice = db.Invoice.Include(x => x.Supplier).Include(x => x.Positions)
                .Include(x => x.Positions.Select(y => y.Product))
                .Include(x => x.Positions.Select(y => y.Product.Measure))
                .SingleOrDefault(x => x.Id == id);
            InvoiceViewModel invoiceViewModel = Mapper.Map<InvoiceViewModel>(invoice);
            if (invoiceViewModel == null)
            {
                return HttpNotFound();
            }
            return View(invoiceViewModel);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            InvoiceViewModel invoice = new InvoiceViewModel();
            var season = UserSeasonHelper.GetSeason(db);
            invoice.IdSeason = season.Id;
            invoice.SeasonName = season.Name;
            invoice.DateDelivery = DateTime.Now.Date;
            invoice.DateIntroduction = DateTime.Now.Date;
            invoice.DateIssue = DateTime.Now.Date;
            invoice.Suppliers = db.SupplierOrganizations.ToList();
            ViewBag.Error = null;
            return View(invoice);
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdSupplier,SupplierName,Number,DateDelivery,DateIssue,DateIntroduction,IdSeason,SeasonName")] InvoiceViewModel invoiceViewModel)
        {
            if (ModelState.IsValid)
            {
                Invoice invoice = new Invoice();
                invoice.Supplier = db.SupplierOrganizations.Find(invoiceViewModel.IdSupplier);
                invoice.Number = invoiceViewModel.Number;
                invoice.DateDelivery = invoiceViewModel.DateDelivery;
                invoice.DateIntroduction = invoiceViewModel.DateIntroduction;
                invoice.DateIssue = invoiceViewModel.DateIssue;
                invoice.Season = db.SeasonOrganization.Find(invoiceViewModel.IdSeason);

                Service.InvoiceService service = new Service.InvoiceService(db);
                string error = null;
                service.Add(invoice, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Edit", new { id = invoice.Id });
            }

            invoiceViewModel.Suppliers = db.SupplierOrganizations.ToList();
            return View(invoiceViewModel);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var invoice = db.Invoice.Include(x => x.Supplier).Include(x => x.Season).Include(x => x.Positions)
                .Include(x => x.Positions.Select(y => y.Product))
                .Include(x => x.Positions.Select(y => y.Product.Measure))
                .SingleOrDefault(x => x.Id == id);
            InvoiceViewModel invoiceViewModel = Mapper.Map<InvoiceViewModel>(invoice);
            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            invoiceViewModel.Suppliers = db.SupplierOrganizations.ToList()
                .FindAll(x => x.Organization.Id == idOrganization);
            if (invoiceViewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Error = null;
            return View(invoiceViewModel);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdSupplier,SupplierName,Number,DateDelivery,DateIssue,DateIntroduction,IdSeason,SeasonName")] InvoiceViewModel invoiceViewModel)
        {
            if (ModelState.IsValid)
            {
                Invoice invoice = new Invoice();
                invoice.Id = invoiceViewModel.Id;
                invoice.Supplier = db.SupplierOrganizations.Find(invoiceViewModel.IdSupplier);
                invoice.Number = invoiceViewModel.Number;
                invoice.DateDelivery = invoiceViewModel.DateDelivery;
                invoice.DateIntroduction = invoiceViewModel.DateIntroduction;
                invoice.DateIssue = invoiceViewModel.DateIssue;
                invoice.Season = db.SeasonOrganization.Find(invoiceViewModel.IdSeason);

                Service.InvoiceService service = new Service.InvoiceService(db);
                string error = null;
                service.Edit(invoice, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Index");
            }

            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            invoiceViewModel.Suppliers = db.SupplierOrganizations.ToList()
                .FindAll(x => x.Organization.Id == idOrganization);
            return View(invoiceViewModel);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var invoice = db.Invoice.Include(x => x.Supplier).Include(x => x.Season).Include(x => x.Positions).Include(x => x.Positions.Select(y => y.Product))
                .SingleOrDefault(x => x.Id == id);
            InvoiceViewModel invoiceViewModel = Mapper.Map<InvoiceViewModel>(invoice);
            if (invoiceViewModel == null)
            {
                return HttpNotFound();
            }
            return View(invoiceViewModel);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoice.Find(id);
            db.Invoice.Remove(invoice);
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

        private List<SupplierOrganizationViewModel> GetSuppliers()
        {
            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            List<SupplierOrganizationViewModel> result = new List<SupplierOrganizationViewModel>();
            //result.Add(new SupplierOrganizationViewModel { Id = -1, Name = "<wszyscy>" });
            var suppFromDb = db.SupplierOrganizations.Where(x => x.Organization.Id == idOrganization)
                .ToList().ConvertAll(x => Mapper.Map<SupplierOrganizationViewModel>(x));
            result.AddRange(suppFromDb);
            return result;
        }
    }
}
