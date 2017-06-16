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
    public class ProductOutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductOut
        public ActionResult Index()
        {
            int idSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            return View(db.ProductOut.Include(x => x.Season).Include(x => x.Positions)
                .Where(x => x.Season.Id == idSeason)
                .OrderByDescending(x => x.Date)
                .ToList().ConvertAll(x => Mapper.Map<ProductOutViewModel>(x)));
        }

        // GET: ProductOut/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductOut productOut = db.ProductOut.Include(x => x.Positions)
                .Include(x => x.Positions.Select(y => y.Product))
                .Include(x => x.Positions.Select(y => y.Product.Measure))
                .SingleOrDefault(x => x.Id == id);
            productOut.Positions = productOut.Positions.OrderBy(x => x.Product.NameDescriptionMeasures).ToList();
            ProductOutViewModel productOutViewModel = Mapper.Map<ProductOutViewModel>(productOut);
            if (productOutViewModel == null)
            {
                return HttpNotFound();
            }
            return View(productOutViewModel);
        }

        // GET: ProductOut/Create
        public ActionResult Create()
        {
            ProductOutViewModel productOutViewModel = new ProductOutViewModel();
            productOutViewModel.Date = DateTime.Now.Date;
            productOutViewModel.IdSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            ViewBag.Error = null;
            return View(productOutViewModel);
        }

        // POST: ProductOut/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Description,IdSeason")] ProductOutViewModel productOutViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductOut productOut = new ProductOut();
                productOut.Season = db.SeasonOrganization.Find(productOutViewModel.IdSeason);
                productOut.Date = productOutViewModel.Date;
                productOut.Description = productOutViewModel.Description;

                Service.ProductOutService service = new Service.ProductOutService(db);
                string error = null;
                service.Add(productOut, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Index");
            }

            return View(productOutViewModel);
        }

        // GET: ProductOut/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductOut productOut = db.ProductOut.Include(x => x.Positions)
                .Include(x => x.Positions.Select(y => y.Product))
                .Include(x => x.Positions.Select(y => y.Product.Measure))
                .SingleOrDefault(x => x.Id == id);
            // TODO
            //productOut.Positions = productOut.Positions.OrderBy(x => x.Product.NameDescriptionMeasures).ToList();
            productOut.Positions = productOut.Positions;
            ProductOutViewModel productOutViewModel = Mapper.Map<ProductOutViewModel>(productOut);
            if (productOutViewModel == null)
            {
                return HttpNotFound();
            }
            return View(productOutViewModel);
        }

        // POST: ProductOut/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Description,IdSeason")] ProductOutViewModel productOutViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductOut productOut = new ProductOut();
                productOut.Id = productOutViewModel.Id;
                productOut.Season = db.SeasonOrganization.Find(productOutViewModel.IdSeason);
                productOut.Date = productOutViewModel.Date;
                productOut.Description = productOutViewModel.Description;

                db.Entry(productOut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productOutViewModel);
        }

        // GET: ProductOut/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOutViewModel productOutViewModel = Mapper.Map<ProductOutViewModel>(db.ProductOut.Find(id));
            if (productOutViewModel == null)
            {
                return HttpNotFound();
            }
            return View(productOutViewModel);
        }

        // POST: ProductOut/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductOut productOut = db.ProductOut.Find(id);
            db.ProductOut.Remove(productOut);
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
