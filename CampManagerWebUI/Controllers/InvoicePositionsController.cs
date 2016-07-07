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
    public class InvoicePositionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InvoicePositions
        //public ActionResult Index()
        //{
        //    return View(db.InvoicePositionViewModels.ToList());
        //}

        // GET: InvoicePositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvoicePosition invoicePosition = db.InvoicePosition.Include(x => x.Product)
                .Include(x => x.Product.Measure)
                .Include(x => x.Invoice).SingleOrDefault(x => x.Id == id);
            InvoicePositionViewModel invoicePositionViewModel = Mapper.Map<InvoicePositionViewModel>(invoicePosition);
            if (invoicePositionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(invoicePositionViewModel);
        }

        // GET: InvoicePositions/Create
        public ActionResult Create(int idInvoice)
        {
            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            InvoicePositionViewModel pos = new InvoicePositionViewModel();
            pos.IdInvoice = idInvoice;
            pos.Products = GetProducts(); 
            return View(pos);
        }

        // POST: InvoicePositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdInvoice,IdProduct,ProductName,Amount,Price,Worth")] InvoicePositionViewModel invoicePositionViewModel)
        {
            if (ModelState.IsValid)
            {
                InvoicePosition invoicePosition = new InvoicePosition();
                invoicePosition.Invoice = db.Invoice.Find(invoicePositionViewModel.IdInvoice);
                invoicePosition.Product = db.ProductOrganization.Find(invoicePositionViewModel.IdProduct);
                invoicePosition.Amount = invoicePositionViewModel.Amount;
                invoicePosition.Worth = invoicePositionViewModel.Worth;
                invoicePosition.Price = invoicePosition.Worth / invoicePosition.Amount;

                db.InvoicePosition.Add(invoicePosition);
                db.SaveChanges();
                return RedirectToAction("Edit", "Invoices", new { id = invoicePosition.Invoice.Id });
            }
            
            invoicePositionViewModel.Products = GetProducts();
            return View(invoicePositionViewModel);
        }

        // GET: InvoicePositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            InvoicePosition invoicePosition = db.InvoicePosition.Include(x => x.Product)
                .Include(x => x.Product.Measure).Include(x => x.Invoice).SingleOrDefault(x => x.Id == id);
            InvoicePositionViewModel invoicePositionViewModel = Mapper.Map<InvoicePositionViewModel>(invoicePosition);            
            if (invoicePositionViewModel == null)
            {
                return HttpNotFound();
            }

            invoicePositionViewModel.Products = GetProducts();
            return View(invoicePositionViewModel);
        }

        // POST: InvoicePositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdInvoice,IdProduct,ProductName,Amount,Price,Worth")] InvoicePositionViewModel invoicePositionViewModel)
        {
            if (ModelState.IsValid)
            {
                InvoicePosition invoicePosition = new InvoicePosition();
                invoicePosition.Id = invoicePositionViewModel.Id;
                invoicePosition.Invoice = db.Invoice.Find(invoicePositionViewModel.IdInvoice);
                invoicePosition.Product = db.ProductOrganization.Find(invoicePositionViewModel.IdProduct);
                invoicePosition.Amount = invoicePositionViewModel.Amount;
                invoicePosition.Worth = invoicePositionViewModel.Worth;
                invoicePosition.Price = invoicePosition.Worth / invoicePosition.Amount;

                db.Entry(invoicePosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Invoices", new { id = invoicePosition.Invoice.Id });
            }

            invoicePositionViewModel.Products = GetProducts();
            return View(invoicePositionViewModel);
        }

        // GET: InvoicePositions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoicePosition invoicePosition = db.InvoicePosition.Include(x => x.Product).Include(x => x.Product.Measure).Include(x => x.Invoice)
                .SingleOrDefault(x => x.Id == id);
            InvoicePositionViewModel invoicePositionViewModel = Mapper.Map<InvoicePositionViewModel>(invoicePosition);

            if (invoicePositionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(invoicePositionViewModel);
        }

        // POST: InvoicePositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoicePosition invoicePosition = db.InvoicePosition.Include(x => x.Invoice).Include(x => x.Product.Measure)
                .SingleOrDefault(x => x.Id == id);
            int idInvoice = invoicePosition.Invoice.Id;
            db.InvoicePosition.Remove(invoicePosition);
            db.SaveChanges();
            // return RedirectToAction("Index");
            return RedirectToAction("Edit", "Invoices", new { id = idInvoice });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<ProductOrganization> GetProducts()
        {
            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            return db.ProductOrganization.Include(x => x.Measure)                
                .Where(x => x.Organization.Id == idOrganization)
                .ToList().OrderBy(x => x.NameDescriptionMeasures).ToList();
        }
    }
}
