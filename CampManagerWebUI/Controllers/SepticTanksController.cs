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
    [AuthorizeCustom(Role.adminOrganization, Role.deputyCommander)]
    public class SepticTanksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SepticTanks
        public ActionResult Index()
        {
            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (season == null)
                return View(new List<SepticTankViewModel>());

            int idSeason = season.Id;
            ViewBag.SeasonActive = season.Active;

            return View(db.SepticTank.Include(x => x.Season).Include(x => x.Kind)
                .Where(x => x.Season.Id == idSeason)
                .OrderByDescending(x => x.Date)
                .ToList().ConvertAll(x => Mapper.Map<SepticTankViewModel>(x)));
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
            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (season == null || !season.Active)
                return RedirectToAction("Index");
            
            SepticTankViewModel septicTank = new SepticTankViewModel();
            septicTank.IdSeason = season.Id;
            septicTank.Kinds = GetSepticTankKinds(null);
            ViewBag.Error = null;
            return View(septicTank);
        }

        // POST: SepticTanks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdSeason,Date,IdKind,Amount")] SepticTankViewModel septicTankViewModel)
        {
            if (ModelState.IsValid)
            {
                SepticTank septicTank = new SepticTank();
                septicTank.Season = db.SeasonOrganization.Find(septicTankViewModel.IdSeason);
                septicTank.Date = septicTankViewModel.Date;
                septicTank.Kind = db.SepticTankKindOrganization.Find(septicTankViewModel.IdKind);
                septicTank.Amount = septicTankViewModel.Amount;

                string error = null;
                Service.SepticTankService service = new Service.SepticTankService(db);
                service.Add(septicTank, ref error);
                if (!string.IsNullOrEmpty(error))
                    ViewBag.Error = error;
                else
                    return RedirectToAction("Index");
            }

            septicTankViewModel.Kinds = GetSepticTankKinds(null);
            return View(septicTankViewModel);
        }

        // GET: SepticTanks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (!season.Active)
                return RedirectToAction("Index");

            SepticTank septicTank = db.SepticTank.Include(x => x.Season).Include(x => x.Kind)
                .SingleOrDefault(x => x.Id == id);
            if (septicTank == null)
            {
                return HttpNotFound();
            }

            var septicTankViewMocel = Mapper.Map<SepticTankViewModel>(septicTank);
            septicTankViewMocel.Kinds = GetSepticTankKinds(null);
            return View(septicTankViewMocel);
        }

        // POST: SepticTanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdSeason,Date,IdKind,Amount")] SepticTankViewModel septicTankViewModel)
        {
            if (ModelState.IsValid)
            {
                SepticTank septicTank = new SepticTank();
                septicTank.Id = septicTankViewModel.Id;
                septicTank.Season = db.SeasonOrganization.Find(septicTankViewModel.IdSeason);
                septicTank.Kind = db.SepticTankKindOrganization.Find(septicTankViewModel.IdKind);
                septicTank.Date = septicTankViewModel.Date;
                septicTank.Amount = septicTankViewModel.Amount;

                db.Entry(septicTank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            septicTankViewModel.Kinds = GetSepticTankKinds(null);
            return View(septicTankViewModel);
        }

        // GET: SepticTanks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SepticTank septicTank = db.SepticTank.Include(x => x.Season).Include(x => x.Kind)
                .SingleOrDefault(x => x.Id == id);
            if (septicTank == null)
            {
                return HttpNotFound();
            }

            var septicTankViewMocel = Mapper.Map<SepticTankViewModel>(septicTank);
            return View(septicTankViewMocel);
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

        private List<SepticTankKindOrganization> GetSepticTankKinds(int? idProductAdd)
        {
            int idOrganization = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
            return db.SepticTankKindOrganization
                .Where(x => x.Organization.Id == idOrganization)
                .ToList().OrderBy(x => x.NameDescription).ToList();
        }

    }
}
