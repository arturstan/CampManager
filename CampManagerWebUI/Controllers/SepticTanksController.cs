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

namespace CampManagerWebUI.Controllers
{
    public class SepticTanksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SepticTanks
        public ActionResult Index()
        {
            return View(db.SepticTank.ToList());
        }

        // GET: SepticTanks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SepticTank septicTank = db.SepticTank.Find(id);
            if (septicTank == null)
            {
                return HttpNotFound();
            }
            return View(septicTank);
        }

        // GET: SepticTanks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SepticTanks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Amount")] SepticTank septicTank)
        {
            if (ModelState.IsValid)
            {
                db.SepticTank.Add(septicTank);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(septicTank);
        }

        // GET: SepticTanks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SepticTank septicTank = db.SepticTank.Find(id);
            if (septicTank == null)
            {
                return HttpNotFound();
            }
            return View(septicTank);
        }

        // POST: SepticTanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Amount")] SepticTank septicTank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(septicTank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(septicTank);
        }

        // GET: SepticTanks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SepticTank septicTank = db.SepticTank.Find(id);
            if (septicTank == null)
            {
                return HttpNotFound();
            }
            return View(septicTank);
        }

        // POST: SepticTanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SepticTank septicTank = db.SepticTank.Find(id);
            db.SepticTank.Remove(septicTank);
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
