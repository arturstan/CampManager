using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using CampManager.Domain.Db;
using CampManager.Domain.Domain;

namespace CampManager.Domain.Service
{
    public class OrganizationService // : IDisposable
    {
        //private ApplicationDbContext _db = new ApplicationDbContext();

        //public void Add(string name, string description)
        //{
        //    Organization org = new Organization()
        //    {
        //        Name = name,
        //        Description = description
        //    };

        //    _db.Organization.Add(org);
        //    _db.SaveChanges();
        //}

        //public void Edit(int idOrganization, string name, string description)
        //{
        //    Organization org = _db.Organization.Find(idOrganization);
        //    org.Name = name;
        //    org.Description = description;
        //    _db.SaveChanges();
        //}

        //public void Dispose()
        //{
        //    if (_db != null)
        //        _db.Dispose();
        //}
    }
}
