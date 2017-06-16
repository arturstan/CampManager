using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Models
{
    // TODO temporary
    public static class UserOrganizationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static Dictionary<string, CampManager.Domain.Domain.Organization> _userOrganization = new Dictionary<string, CampManager.Domain.Domain.Organization>();

        public static CampManager.Domain.Domain.Organization GetOrganization(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;
            
            if (!_userOrganization.ContainsKey(userName))
            {
                var organizations = db.Organization.ToList();
                _userOrganization[userName] = organizations.Count > 0 ? organizations.Last() : null;
            }

            return _userOrganization[userName];
        }

        public static string GetOrganizationName(string userName)
        {
            var organization = GetOrganization(userName);
            return organization != null ? organization.Name : "";
        }
    }
}