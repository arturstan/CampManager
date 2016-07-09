using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CampManagerWebUI.Db;
using CampManagerWebUI.Service;

namespace CampManagerWebUI.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        public ActionResult ProductAmount()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductAmountService service = new ProductAmountService(db);
            var invoicePosition = db.InvoicePosition.Include(x => x.Invoice).ToList();

            service.Fill(invoicePosition);

            return null;
        }

        public ActionResult ProductExpend()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductExpendService service = new ProductExpendService(db);

            DateTime date = new DateTime(2016, 7, 2);

            var productOutList = db.ProductOutPosition.Include(x => x.ProductOut)
                .Include(x => x.Product)
                .Include(x => x.ProductOut)
                .Include(x => x.ProductOut)
                .Where(x => x.ProductOut.Date <= date)
                .ToList();
            service.Fill(productOutList);

            return null;
        }
    }
}