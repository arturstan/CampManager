using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Models
{
    // TODO temporary
    public class UserSeasonHelper
    {
        public static CampManager.Domain.Domain.SeasonOrganization GetSeason(ApplicationDbContext db)
        {
            // TODO
            return db.SeasonOrganization.ToList().Last();                
        }
    }
}