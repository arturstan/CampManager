using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;

namespace CampManagerWebUI.Controllers
{
    public class MealBidsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MealBids
        public ActionResult Index()
        {
            int idSeason = UserSeasonHelper.GetSeason(User.Identity.Name).Id;
            List<MealBid> mealBidList = db.MealBid.Where(x => x.Season.Id == idSeason).OrderBy(x => x.Date).ToList();
            return View(mealBidList.ConvertAll(x => Mapper.Map<MealBidViewModel>(x)));
        }

        // GET: MealBids/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealBidViewModel mealBidViewModel = Mapper.Map<MealBidViewModel>(db.MealBid.Find(id));
            if (mealBidViewModel == null)
            {
                return HttpNotFound();
            }
            return View(mealBidViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
