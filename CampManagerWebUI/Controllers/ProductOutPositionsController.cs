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
            return View(db.ProductOutPosition.Include(x => x.Product).Include(x => x.ProductOut).ToList().ConvertAll(x => Mapper.Map<ProductOutPositionViewModel>(x)));
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

                db.ProductOutPosition.Add(productPosition);
                db.SaveChanges();
                // return RedirectToAction("Index");
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

                db.Entry(productPosition).State = EntityState.Modified;
                db.SaveChanges();
                // return RedirectToAction("Index");
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
            return View(productOutPositionViewModel);
        }

        // POST: ProductOutPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductOutPosition productPosition = db.ProductOutPosition.Include(x => x.ProductOut).SingleOrDefault(x => x.Id == id);
            int idProductOut = productPosition.ProductOut.Id;
            db.ProductOutPosition.Remove(productPosition);
            db.SaveChanges();
            //return RedirectToAction("Index");
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

        private List<ProductOrganization> GetProducts()
        {
            int idOrganization = UserOrganizationHelper.GetOrganization(db).Id;
            return db.ProductOrganization.Include(x => x.Measure).Where(x => x.Organization.Id == idOrganization)
                .ToList()
                .OrderBy(x => x.NameDescriptionMeasures).ToList();
        }
    }
}
