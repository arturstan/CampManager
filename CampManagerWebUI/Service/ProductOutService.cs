using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

using CampManagerWebUI.Db;

namespace CampManagerWebUI.Service
{
    public class ProductOutService
    {
        private ApplicationDbContext _db;

        public ProductOutService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(ProductOut productOut, ref string error)
        {
            var productOutTmp = _db.ProductOut.FirstOrDefault(x => x.Date == productOut.Date);
            if (productOutTmp != null)
            {
                error = "Istnieje taka data";
                return;
            }

            _db.ProductOut.Add(productOut);
            _db.SaveChanges();
        }

        public void Edit(ProductOut productOut, ref string error)
        {
            var productOutTmp = _db.ProductOut.FirstOrDefault(x => x.Date == productOut.Date && x.Id != productOut.Id);
            if (productOutTmp != null)
            {
                error = "Istnieje taka data";
                return;
            }

            _db.Entry(productOut).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}