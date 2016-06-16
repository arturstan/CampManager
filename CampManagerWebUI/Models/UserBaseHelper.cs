using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Models
{
    // TODO temporary
    public class UserBaseHelper
    {
        public static CampManager.Domain.Domain.BaseOrganization GetBase(ApplicationDbContext db)
        {
            return db.BaseOrganization.First();
        }
    }
}