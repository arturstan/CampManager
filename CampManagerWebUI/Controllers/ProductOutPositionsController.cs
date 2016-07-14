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
    public class ProductOutPositionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductOutPositions
        public ActionResult Index()
        {
            return View(db.ProductOutPosition.Include(x => x.Product).Include(x => x.Product.Measure)
                .Include(x => x.ProductOut).ToList().ConvertAll(x => Mapper.Map<ProductOutPositionViewModel>(x)));
        }

        // GET: ProductOutPositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOutPosition productPosition = db.ProductOutPosition.Include(x => x.ProductOut).Include(x => x.Product).Include(x => x.Product.Measure)
                .SingleOrDefault(x => x.Id == id);
            ProductOutPositionViewModel productOutPositionViewModel = Mapper.Map<ProductOutPositionViewModel>(productPosition);
            if (productOutPositionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(productOutPositionViewModel);
        }

        // GET: ProductOutPositions/Create
        public ActionResult Create(int idProductOut)
        {
            ProductOutPositionViewModel productOutPositionViewModel = new ProductOutPositionViewModel();
            productOutPositionViewModel.IdProductOut = idProductOut;
            productOutPositionViewModel.Products = GetProducts();

            ViewBag.Error = null;
            return View(productOutPositionViewModel);
        }

        // POST: ProductOutPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdProductOut,IdProduct,ProductName,Amount,Price,Worth")] ProductOutPositionViewModel productOutPositionViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductOutPosition productPosition = new ProductOutPosition();
                productPosition.ProductOut = db.ProductOut.Find(productOutPositionViewModel.IdProductOut);
                productPosition.Product = db.ProductOrganization.Find(productOutPositionViewModel.IdProduct);
                productPosition.Amount = productOutPositionViewModel.Amount;
                productPosition.Price = productOutPositionViewModel.Price;
                productPosition.Worth = productOutPositionViewModel.Worth;

                Service.ProductOutPositionService service = new Service.ProductOutPositionService(db);
                string error = null;
                service.Add(productPosition, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Edit", "ProductOut", new { id = productOutPositionViewModel.IdProductOut });
            }

            productOutPositionViewModel.Products = GetProducts();
            return View(productOutPositionViewModel);
        }

        // GET: ProductOutPositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOutPosition productPosition = db.ProductOutPosition.Include(x => x.ProductOut).Include(x => x.Product).Include(x => x.Product.Measure)
                .SingleOrDefault(x => x.Id == id);

            ProductOutPositionViewModel productOutPositionViewModel = Mapper.Map<ProductOutPositionViewModel>(productPosition);
            productOutPositionViewModel.Products = GetProducts();
            if (productOutPositionViewModel == null)
            {
                return HttpNotFound();
            }

            productOutPositionViewModel.Products = GetProducts();
            ViewBag.Error = null;
            return View(productOutPositionViewModel);
        }

        // POST: ProductOutPositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdProductOut,IdProduct,ProductName,Amount,Price,Worth")] ProductOutPositionViewModel productOutPositionViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductOutPosition productPosition = new ProductOutPosition();
                productPosition.Id = productOutPositionViewModel.Id;
                productPosition.ProductOut = db.ProductOut.Find(productOutPositionViewModel.IdProductOut);
                productPosition.Product = db.ProductOrganization.Find(productOutPositionViewModel.IdProduct);
                productPosition.Amount = productOutPositionViewModel.Amount;
                productPosition.Price = productOutPositionViewModel.Price;
                productPosition.Worth = productOutPositionViewModel.Worth;

                Service.ProductOutPositionService service = new Service.ProductOutPositionService(db);
                string error = null;
                service.Edit(productPosition, ref error);
                if (string.IsNullOrEmpty(error))
                {
                    productOutPositionViewModel.Products = GetProducts();
                    ViewBag.Error = error;
                    return View(productOutPositionViewModel);
                }
                else
                    return RedirectToAction("Edit", "ProductOut", new { id = productOutPositionViewModel.IdProductOut });
            }

            productOutPositionViewModel.Products = GetProducts();
            return View(productOutPositionViewModel);
        }

        // GET: ProductOutPositions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductOutPosition productPosition = db.ProductOutPosition.Include(x => x.ProductOut).Include(x => x.Product).Include(x => x.Product.Measure)
                .SingleOrDefault(x => x.Id == id);

            ProductOutPositionViewModel productOutPositionViewModel = Mapper.Map<ProductOutPositionViewModel>(productPosition);
            if (productOutPositionViewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Error = null;
            return View(productOutPositionViewModel);
        }

        // POST: ProductOutPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductOutPosition productPosition = db.ProductOutPosition.Include(x => x.ProductOut).SingleOrDefault(x => x.Id == id);
            int idProductOut = productPosition.ProductOut.Id;
            Service.ProductOutPositionService service = new Service.ProductOutPositionService(db);
            string error = null;
            service.Remove(productPosition, ref error);
            if (!string.IsNullOrEmpty(error))
            {
                ProductOutPositionViewModel productOutPositionViewModel = Mapper.Map<ProductOutPositionViewModel>(productPosition);
                ViewBag.Error = error;
                return View(productOutPositionViewModel);
            }
            else
                return RedirectToAction("Edit", "ProductOut", new { id = idProductOut });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<ProductOrganizationViewModel> GetProducts()
        {
            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            var productList = db.ProductOrganization.Include(x => x.Measure).Where(x => x.Organization.Id == idOrganization)
                .ToList()
                .OrderBy(x => x.NameDescriptionMeasures).ToList()
                .ConvertAll(x => Mapper.Map<ProductOrganizationViewModel>(x));

            FillAmount(productList);
            return productList;
        }

        private void FillAmount(List<ProductOrganizationViewModel> productList)
        {
            int idSeason = UserSeasonHelper.GetSeason(db).Id;
            var productAmountList = db.ProductAmount
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Product)
                // .Include(x => x.InvoicePosition.Invoice)
                .Where(x => x.InvoicePosition.Invoice.Season.Id == idSeason);

            foreach (var productAmount in productAmountList)
            {
                var product = productList.Find(x => x.Id == productAmount.InvoicePosition.Product.Id);
                product.Amount += productAmount.AmountBuy - productAmount.AmountExpend;
                product.Worth += productAmount.WorthBuy - productAmount.WorthExpend;
            }
        }
    }
}
