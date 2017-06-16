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
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static Dictionary<string, CampManager.Domain.Domain.SeasonOrganization> _userSeason = new Dictionary<string, CampManager.Domain.Domain.SeasonOrganization>();

        public static CampManager.Domain.Domain.SeasonOrganization GetSeason(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return null;

            if (!_userSeason.ContainsKey(userName))
                _userSeason[userName] = db.SeasonOrganization.ToList().Last();

            return _userSeason[userName];
        }

        public static string GetSeasonName(string userName)
        {
            var season = GetSeason(userName);
            return season != null ? season.Name : "";
        }
    }
}