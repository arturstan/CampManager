using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CampManager.Domain.Domain;
using CampManager.Domain.User;
using CampManagerWebUI.Models;
using CampManagerWebUI.Db;

using Newtonsoft.Json;

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
                var userOrg = usersOrganizationList.Find(x => x.IdUser == user.Email);
                UserViewModel userVM = GetUser(user, userOrg);

                userVMList.Add(userVM);
            }

            return View(userVMList);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userApp = _db.Users.FirstOrDefault(x => x.Id == id);
            if (userApp == null)
            {
                return HttpNotFound();
            }

            var userOrg = _db.UserOrganization.FirstOrDefault(x => x.IdUser == userApp.Email);
            var userVM = GetUser(userApp, userOrg);
            return View(userVM);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userApp = _db.Users.FirstOrDefault(x => x.Id == id);
            if (userApp == null)
            {
                return HttpNotFound();
            }

            var userOrg = _db.UserOrganization.FirstOrDefault(x => x.IdUser == userApp.Email);
            var userVM = GetUser(userApp, userOrg);
            return View(userVM);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Email,Active,DateExpire,Accountant,Warehouseman,DeputyCommander")] UserViewModel userViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userOrg = _db.UserOrganization.FirstOrDefault(x => x.IdUser == userViewModel.Email);
                    if (userOrg == null)
                    {
                        userOrg = new UserOrganization();
                        var organization = UserOrganizationHelper.GetOrganization(userViewModel.Email);
                        userOrg.IdUser = userViewModel.Email;
                        userOrg.Organization = _db.Organization.First(x => x.Id == organization.Id);
                        userOrg.Active = userViewModel.Active;
                        userOrg.DateExpire = userViewModel.DateExpire;
                        SetUserRoles(userOrg, userViewModel);
                        _db.UserOrganization.Add(userOrg);
                        _db.SaveChanges();
                    }
                    else
                    {
                        userOrg.Active = userViewModel.Active;
                        userOrg.DateExpire = userViewModel.DateExpire;
                        SetUserRoles(userOrg, userViewModel);
                        _db.Entry(userOrg).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                return View(userViewModel);
            }
            catch
            {
                return View();
            }
        }

        private UserViewModel GetUser(ApplicationUser user, UserOrganization userOrg)
        {
            UserViewModel userVM = new UserViewModel();
            userVM.Id = user.Id;
            userVM.Email = user.Email;
            userVM.Active = true;
            userVM.Roles = "";
            string[] rolesTab = { };
            if (userOrg != null)
            {
                userVM.Active = userOrg.Active;
                userVM.DateExpire = userOrg.DateExpire;
                List<UserRole> roles = JsonConvert.DeserializeObject<List<UserRole>>(userOrg.Roles);

                userVM.AdminOrganization = roles.Exists(x => x.Role == Role.adminOrganization && x.Active);
                userVM.Accountant = roles.Exists(x => x.Role == Role.accountant && x.Active);
                userVM.Warehouseman = roles.Exists(x => x.Role == Role.warehouseman && x.Active);
                userVM.DeputyCommander = roles.Exists(x => x.Role == Role.deputyCommander && x.Active);

                if (userVM.AdminOrganization)
                    userVM.Roles = "admin";
                if (userVM.Accountant)
                    userVM.Roles += " księgowy";
                if (userVM.Warehouseman)
                    userVM.Roles += " magazynier";
                if (userVM.DeputyCommander)
                    userVM.Roles += " oboźny";
            }

            return userVM;
        }

        private void SetUserRoles(UserOrganization user, UserViewModel userViewModel)
        {
            if (string.IsNullOrEmpty(user.Roles))
            {
                user.Roles = JsonConvert.SerializeObject(new List<UserRole>());
            }

            List<UserRole> userRoles = JsonConvert.DeserializeObject<List<UserRole>>(user.Roles);
            SetUserRole(userRoles, Role.accountant, userViewModel.Accountant);
            SetUserRole(userRoles, Role.warehouseman, userViewModel.Warehouseman);
            SetUserRole(userRoles, Role.deputyCommander, userViewModel.DeputyCommander);
            user.Roles = JsonConvert.SerializeObject(userRoles);
        }

        private void SetUserRole(List<UserRole> userRoles, Role role, bool value)
        {
            var userRole = userRoles.Find(x => x.Role == role);
            if (value)
            {
                if (userRole == null)
                {
                    userRole = new UserRole { Role = role };
                    userRoles.Add(userRole);
                }

                if (!userRole.Active)
                {
                    userRole.Active = true;
                }
            }
            else
            {
                if (userRole != null && userRole.Active)
                {
                    userRole.Active = false;
                }
            }
        }
    }
}
