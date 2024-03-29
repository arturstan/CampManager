﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using AutoMapper;
using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;


namespace CampManagerWebUI.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index(DateTime? date)
        {
            var productList = db.ProductOrganization.Include(x => x.Measure).ToList()
                .OrderBy(x => x.NameDescriptionMeasures).ToList()
                .ConvertAll(x => Mapper.Map<ProductOrganizationViewModel>(x));

            FillAmount(productList, date);
            return View(productList);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.ProductOrganization.Include(x => x.Organization).Include(x => x.Measure).SingleOrDefault(x => x.Id == id);
            ProductOrganizationViewModel productOrganizationViewModel = Mapper.Map<ProductOrganizationViewModel>(product);

            productOrganizationViewModel.ProductAmount = db.ProductAmount.Where(x => x.InvoicePosition.Product.Id == id.Value)
                .Include(x => x.InvoicePosition).Include(x => x.InvoicePosition.Invoice)
                .OrderBy(x => x.InvoicePosition.Invoice.DateDelivery)
                .ToList();
            productOrganizationViewModel.ProductExpend = db.ProductExpend.Where(x => x.ProductOutPosition.Product.Id == id.Value)
                .Include(x => x.ProductOutPosition)
                .Include(x => x.ProductOutPosition.ProductOut)
                .OrderBy(x => x.ProductOutPosition.ProductOut.Date)
                .ToList();

            if (productOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(productOrganizationViewModel);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ProductOrganizationViewModel product = new ProductOrganizationViewModel();
            product.IdOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            product.Measures = db.MeasureOrganization.ToList();
            ViewBag.Error = null;
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IdMeasure,IdOrganization")] ProductOrganizationViewModel productOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductOrganization product = new ProductOrganization();
                product.Name = productOrganizationViewModel.Name;
                product.Description = productOrganizationViewModel.Description;
                product.Active = true;
                product.Organization = db.Organization.Find(productOrganizationViewModel.IdOrganization);
                product.Measure = db.MeasureOrganization.Find(productOrganizationViewModel.IdMeasure);

                Service.ProductService service = new Service.ProductService(db);
                string error = null;
                service.Add(product, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Index");
            }

            return View(productOrganizationViewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.ProductOrganization.Include(x => x.Organization).Include(x => x.Measure).SingleOrDefault(x => x.Id == id);
            ProductOrganizationViewModel productOrganizationViewModel = Mapper.Map<ProductOrganizationViewModel>(product);
            productOrganizationViewModel.Measures = db.MeasureOrganization.ToList();
            if (productOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = null;
            return View(productOrganizationViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Active,IdMeasure,IdOrganization")] ProductOrganizationViewModel productOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductOrganization product = new ProductOrganization();
                product.Id = productOrganizationViewModel.Id;
                product.Name = productOrganizationViewModel.Name;
                product.Description = productOrganizationViewModel.Description;
                product.Active = productOrganizationViewModel.Active;
                product.Organization = db.Organization.Find(productOrganizationViewModel.IdOrganization);
                product.Measure = db.MeasureOrganization.Find(productOrganizationViewModel.IdMeasure);
                Service.ProductService service = new Service.ProductService(db);
                string error = null;
                service.Edit(product, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Index");
            }

            productOrganizationViewModel.Measures = db.MeasureOrganization.ToList();
            return View(productOrganizationViewModel);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.ProductOrganization.Include(x => x.Organization).Include(x => x.Measure).SingleOrDefault(x => x.Id == id);
            ProductOrganizationViewModel productOrganizationViewModel = Mapper.Map<ProductOrganizationViewModel>(product);
            if (productOrganizationViewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Error = null;
            return View(productOrganizationViewModel);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductOrganization product = db.ProductOrganization.Find(id);
            Service.ProductService service = new Service.ProductService(db);
            string error = null;
            service.Remove(product, ref error);
            if (!string.IsNullOrEmpty(error))
            {
                ProductOrganizationViewModel productOrganizationViewModel = Mapper.Map<ProductOrganizationViewModel>(product);
                ViewBag.Error = error;
                return View(productOrganizationViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillAmountOld(List<ProductOrganizationViewModel> productList)
        {
            int idSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            var invoicePositionList = db.InvoicePosition.Where(x => x.Invoice.Season.Id == idSeason);
            foreach (var invoicePosition in invoicePositionList)
            {
                var product = productList.Find(x => x.Id == invoicePosition.Product.Id);
                product.Amount += invoicePosition.Amount;
            }
        }

        private void FillAmount(List<ProductOrganizationViewModel> productList, DateTime? date)
        {
            if (date.HasValue)
                FillAmount(productList, date.Value);
            else
                FillAmount(productList);
        }

        private void FillAmount(List<ProductOrganizationViewModel> productList)
        {
            int idSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            var productAmountList = db.ProductAmount
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Product)
                .Where(x => x.InvoicePosition.Invoice.Season.Id == idSeason);

            foreach (var productAmount in productAmountList)
            {
                var product = productList.Find(x => x.Id == productAmount.InvoicePosition.Product.Id);
                product.Amount += productAmount.AmountBuy - productAmount.AmountExpend;
                product.Worth += productAmount.WorthBuy - productAmount.WorthExpend;
            }
        }

        private void FillAmount(List<ProductOrganizationViewModel> productList, DateTime date)
        {
            int idSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            var productAmountList = db.ProductAmount
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Product)
                .Where(x => x.InvoicePosition.Invoice.Season.Id == idSeason && x.InvoicePosition.Invoice.DateDelivery <= date);

            foreach (var productAmount in productAmountList)
            {
                var product = productList.Find(x => x.Id == productAmount.InvoicePosition.Product.Id);
                product.Amount += productAmount.AmountBuy;
                product.Worth += productAmount.WorthBuy;
            }

            var productExpendList = db.ProductExpend
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Product)
                .Where(x => x.ProductOutPosition.ProductOut.Season.Id == idSeason && x.ProductOutPosition.ProductOut.Date <= date);

            foreach (var productExpend in productExpendList)
            {
                var product = productList.Find(x => x.Id == productExpend.InvoicePosition.Product.Id);
                product.Amount -= productExpend.Amount;
                product.Worth -= productExpend.Worth;
            }
        }
    }
}
