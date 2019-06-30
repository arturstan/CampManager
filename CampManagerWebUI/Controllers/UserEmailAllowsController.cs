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
using CampManagerWebUI.Models;
using CampManagerWebUI.Db;

using Newtonsoft.Json;

namespace CampManagerWebUI.Controllers
{
    public class UserEmailAllowsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: UserEmailAllows
        public ActionResult Index()
        {
            var userEmail = _db.UserEmailAllow.ToList();
            var users = _db.Users.ToList();
            userEmail.RemoveAll(ue => users.Exists(x => x.Email == ue.Email));
            return View(userEmail.ConvertAll(x => Mapper.Map<UserEmailAllowViewModel>(x)));
        }

        // GET: UserEmailAllows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmailAllow userEmailAllow = _db.UserEmailAllow.Find(id);
            if (userEmailAllow == null)
            {
                return HttpNotFound();
            }
            return View(userEmailAllow);
        }

        // GET: UserEmailAllows/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserEmailAllows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "Id,Email,StartRoles,DateExpire")] UserEmailAllow userEmailAllow)
        public ActionResult Create([Bind(Include = "Id,Email")] UserEmailAllowViewModel userEmailAllowViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.FirstOrDefault(x => x.Email == userEmailAllowViewModel.Email) != null)
                {
                    ModelState.AddModelError("", "Email istnieje w zarejestrowanych użytkownikach");
                    return View(userEmailAllowViewModel);
                }

                if (_db.UserEmailAllow.FirstOrDefault(x => x.Email == userEmailAllowViewModel.Email) != null)
                {
                    ModelState.AddModelError("", "Email istnieje w oczekujących na rejestrację");
                    return View(userEmailAllowViewModel);
                }

                UserEmailAllow userEmail = new UserEmailAllow();
                userEmail.Email = userEmailAllowViewModel.Email;
                int idOrg = UserOrganizationHelper.GetOrganization(User.Identity.Name).Id;
                var organization = _db.Organization.First(x => x.Id == idOrg);
                userEmail.Organization = organization;
                userEmail.DateExpire = null;
                userEmail.StartRoles = JsonConvert.SerializeObject(new List<UserRole>());
                _db.UserEmailAllow.Add(userEmail);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userEmailAllowViewModel);
        }

        // GET: UserEmailAllows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmailAllow userEmailAllow = _db.UserEmailAllow.Find(id);
            if (userEmailAllow == null)
            {
                return HttpNotFound();
            }
            return View(userEmailAllow);
        }

        // POST: UserEmailAllows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Edit([Bind(Include = "Id,Email,StartRoles,DateExpire")] UserEmailAllow userEmailAllow)
        public ActionResult Edit([Bind(Include = "Id,Email")] UserEmailAllowViewModel userEmailAllowViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.FirstOrDefault(x => x.Email == userEmailAllowViewModel.Email) != null)
                {
                    ModelState.AddModelError("", "Email istnieje w zarejestrowanych użytkownikach");
                    return View(userEmailAllowViewModel);
                }

                if (_db.UserEmailAllow.FirstOrDefault(x => x.Email == userEmailAllowViewModel.Email) != null)
                {
                    ModelState.AddModelError("", "Email istnieje w oczekujących na rejestrację");
                    return View(userEmailAllowViewModel);
                }

                UserEmailAllow userEmailAllowOrg = _db.UserEmailAllow.Include(x => x.Organization)
                    .Single(x => x.Id == userEmailAllowViewModel.Id);

                UserEmailAllow userEmailAllow = new UserEmailAllow();
                userEmailAllow.Id = userEmailAllowViewModel.Id;
                int idOrganization = userEmailAllowOrg.Organization.Id;
                userEmailAllow.Organization = _db.Organization.Find(idOrganization);
                userEmailAllow.DateExpire = userEmailAllowOrg.DateExpire;
                userEmailAllow.StartRoles = userEmailAllowOrg.StartRoles;

                userEmailAllow.Email = userEmailAllowViewModel.Email;
                _db.Entry(userEmailAllow).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userEmailAllowViewModel);
        }

        // GET: UserEmailAllows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmailAllow userEmailAllow = _db.UserEmailAllow.Find(id);
            if (userEmailAllow == null)
            {
                return HttpNotFound();
            }
            return View(userEmailAllow);
        }

        // POST: UserEmailAllows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserEmailAllow userEmailAllow = _db.UserEmailAllow.Find(id);
            _db.UserEmailAllow.Remove(userEmailAllow);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
