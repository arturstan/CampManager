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
        public static CampManager.Domain.Domain.Organization GetOrganization(ApplicationDbContext db)
        {
            return db.Organization.First();
        }
    }
}