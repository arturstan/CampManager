using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CampManager.Domain.Domain;
using CampManager.Domain.User;
using CampManagerWebUI.Models;
using CampManagerWebUI.Db;

namespace CampManagerWebUI.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = _db.Users.ToList();
            var usersOrganizationList = _db.UserOrganization.ToList();
            List<UserViewModel> userVMList = new List<UserViewModel>();
            foreach (var user in users)
            {
                UserViewModel userVM = new UserViewModel();
                var userOrg = usersOrganizationList.Find(x => x.IdUser == user.Email);

                userVM.Email = user.Email;
                userVM.Active = true;
                userVM.Roles = "";
                if (userOrg != null)
                {
                    userVM.Active = userOrg.Active;
                    userVM.DateExpire = userOrg.DateExpire;

                }

                userVMList.Add(userVM);
            }

            return View(userVMList);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
