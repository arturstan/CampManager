using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Service
{
    public class SepticTankService
    {
        private ApplicationDbContext _db;

        public SepticTankService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(SepticTank septicTank, ref string error)
        {
            var septicTankFromDb = _db.SepticTank.Where(x => x.Season.Id == septicTank.Season.Id
                && x.Date == septicTank.Date
                && x.Kind.Id == septicTank.Kind.Id).FirstOrDefault();

            if (septicTankFromDb != null)
            {
                error = "Istnieje już wpis na podaną datę i rodzaj wywozu";
                return;
            }

            _db.SepticTank.Add(septicTank);
            _db.SaveChanges();
            _db.SaveChanges();
        }
    }
}