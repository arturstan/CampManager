using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            {
                var seasons = db.SeasonOrganization
                    .Include(x => x.Base)
                    .Include(x => x.Base.Organization)
                    .ToList();
                _userSeason[userName] = seasons.Count > 0 ? seasons.Last() : null;
            }

            return _userSeason[userName];
        }

        public static void Change(string userName, int idSeason)
        {
            var seasons = db.SeasonOrganization
                    .Include(x => x.Base)
                    .Include(x => x.Base.Organization)
                    .ToList();

            var season = seasons.Find(x => x.Id == idSeason);
            if (season == null)
                return;

            _userSeason[userName] = season;
        }

        public static string GetSeasonName(string userName)
        {
            var season = GetSeason(userName);
            return season != null ? season.Name : "";
        }
    }
}