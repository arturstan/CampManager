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
    public class SeasonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Seasons
        public ActionResult Index()
        {
            return View(db.SeasonOrganization.Include(x => x.Base).ToList().ConvertAll(x => Mapper.Map<SeasonOrganizationViewModel>(x)));
        }

        // GET: Seasons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SeasonOrganization season = db.SeasonOrganization.Include(x => x.Base).SingleOrDefault(x => x.Id == id);
            SeasonOrganizationViewModel seasonOrganizationViewModel = Mapper.Map<SeasonOrganizationViewModel>(season);
            if (seasonOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(seasonOrganizationViewModel);
        }

        // GET: Seasons/Create
        public ActionResult Create()
        {
            SeasonOrganizationViewModel season = new SeasonOrganizationViewModel();
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            season.Bases = db.BaseOrganization.Include(x => x.Organization).ToList().FindAll(x => x.Organization.Id == idOrganization);
            if (season.Bases.Count > 0)
                season.IdBase = season.Bases.Last().Id;

            season.DateStart = DateTime.Now.Date;
            season.DateEnd = DateTime.Now.Date;
            return View(season);
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Active,Name,Description,DateStart,DateEnd,IdBase,BaseName")] SeasonOrganizationViewModel seasonOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                SeasonOrganization season = new SeasonOrganization();
                season.Active = seasonOrganizationViewModel.Active;
                season.Name = seasonOrganizationViewModel.Name;
                season.Description = seasonOrganizationViewModel.Description;
                season.DateStart = seasonOrganizationViewModel.DateStart;
                season.DateEnd = seasonOrganizationViewModel.DateEnd;
                season.Base = db.BaseOrganization.Find(seasonOrganizationViewModel.IdBase);

                db.SeasonOrganization.Add(season);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(seasonOrganizationViewModel);
        }

        // GET: Seasons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SeasonOrganization season = db.SeasonOrganization.Include(x => x.Base).SingleOrDefault(x => x.Id == id);
            SeasonOrganizationViewModel seasonOrganizationViewModel = Mapper.Map<SeasonOrganizationViewModel>(season);
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            seasonOrganizationViewModel.Bases = db.BaseOrganization.Include(x => x.Organization).ToList()
                .FindAll(x => x.Organization.Id == idOrganization);
            if (seasonOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(seasonOrganizationViewModel);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Active,Name,Description,DateStart,DateEnd,IdBase,BaseName")] SeasonOrganizationViewModel seasonOrganizationViewModel)
        {
            if (ModelState.IsValid)
            {
                SeasonOrganization season = new SeasonOrganization();
                season.Id = seasonOrganizationViewModel.Id;
                season.Active = seasonOrganizationViewModel.Active;
                season.Name = seasonOrganizationViewModel.Name;
                season.Description = seasonOrganizationViewModel.Description;
                season.DateStart = seasonOrganizationViewModel.DateStart;
                season.DateEnd = seasonOrganizationViewModel.DateEnd;
                season.Base = db.BaseOrganization.Find(seasonOrganizationViewModel.IdBase);

                db.Entry(season).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(seasonOrganizationViewModel);
        }

        // GET: Seasons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SeasonOrganization season = db.SeasonOrganization.Include(x => x.Base).SingleOrDefault(x => x.Id == id);
            SeasonOrganizationViewModel seasonOrganizationViewModel = Mapper.Map<SeasonOrganizationViewModel>(season);
            if (seasonOrganizationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(seasonOrganizationViewModel);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SeasonOrganization season = db.SeasonOrganization.Find(id);
            db.SeasonOrganization.Remove(season);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Seasons/Delete/5
        public ActionResult Change(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserSeasonHelper.Change(User.Identity.Name, id.Value);
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
