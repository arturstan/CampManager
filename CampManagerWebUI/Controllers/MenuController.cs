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
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menu
        public ActionResult Index()
        {
            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (season == null)
                return View(new List<MenuViewModel>());

            int idSeason = season.Id;
            List<Menu> menuList = db.Menu.Where(x => x.Season.Id == idSeason).OrderByDescending(x => x.Date).ToList();
            ViewBag.SeasonActive = season.Active;
            return View(menuList.ConvertAll(x => Mapper.Map<MenuViewModel>(x)));
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuViewModel menuViewModel = Mapper.Map<MenuViewModel>(db.Menu.Find(id));
            if (menuViewModel == null)
            {
                return HttpNotFound();
            }
            return View(menuViewModel);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (!season.Active)
                return RedirectToAction("Index");

            MenuViewModel menu = new MenuViewModel();
            menu.IdSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            menu.Date = DateTime.Now.Date;
            return View(menu);
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdSeason,Date,Breakfast,Dinner,Supper")] MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu();
                menu.Season = db.SeasonOrganization.Find(menuViewModel.IdSeason);
                menu.Date = menuViewModel.Date;
                menu.Breakfast = menuViewModel.Breakfast;
                menu.Dinner = menuViewModel.Dinner;
                menu.Supper = menuViewModel.Supper;

                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuViewModel);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (!season.Active)
                return RedirectToAction("Index");

            MenuViewModel menuViewModel = Mapper.Map<MenuViewModel>(db.Menu.Find(id));
            if (menuViewModel == null)
            {
                return HttpNotFound();
            }
            return View(menuViewModel);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdSeason,Date,Breakfast,Dinner,Supper")] MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu();
                menu.Id = menuViewModel.Id;
                menu.Season = db.SeasonOrganization.Find(menuViewModel.IdSeason);
                menu.Date = menuViewModel.Date;
                menu.Breakfast = menuViewModel.Breakfast;
                menu.Dinner = menuViewModel.Dinner;
                menu.Supper = menuViewModel.Supper;

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuViewModel);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var season = UserSeasonHelper.GetSeason(User.Identity.Name);
            if (!season.Active)
                return RedirectToAction("Index");

            MenuViewModel menuViewModel = Mapper.Map<MenuViewModel>(db.Menu.Find(id));
            if (menuViewModel == null)
            {
                return HttpNotFound();
            }
            return View(menuViewModel);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menu.Find(id);
            db.Menu.Remove(menu);
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
