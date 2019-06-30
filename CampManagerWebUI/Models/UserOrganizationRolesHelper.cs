using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CampManager.Domain.Domain;
using CampManager.Domain.User;
using CampManagerWebUI.Db;

using Newtonsoft.Json;

namespace CampManagerWebUI.Models
{
    public class UserOrganizationRolesHelper
    {
        private static ApplicationDbContext _db = new ApplicationDbContext();
        private static List<UserOrganization> _userOrganization = null;

        public static bool IsUserOrganizationAdmin(string userName)
        {
            var userOrg = GetUserOrganization(userName);
            List<UserRole> userRoles = JsonConvert.DeserializeObject<List<UserRole>>(userOrg.Roles);
            return userRoles.Exists(x => x.Role == Role.adminOrganization && x.Active);
        }

        private static UserOrganization GetUserOrganization(string userName)
        {
            if (_userOrganization == null)
                _userOrganization = _db.UserOrganization.ToList();

            var userOrg = _userOrganization.Find(x => x.IdUser == userName);
            if (userOrg == null)
            {
                userOrg = new UserOrganization { IdUser = userName };
                var organization = UserOrganizationHelper.GetOrganization(userName);
                userOrg.Active = true;
                userOrg.Organization = _db.Organization.First(x => x.Id == organization.Id);
                userOrg.Roles = JsonConvert.SerializeObject(new List<UserRole>());
                //userOrg.Roles = JsonConvert.SerializeObject(new List<UserRole>() { new UserRole {Role = Role.adminOrganization, Active = true } });

                _db.UserOrganization.Add(userOrg);
                _db.SaveChanges();

                _userOrganization.Add(userOrg);
            }

            return userOrg;
        }
    }
}
