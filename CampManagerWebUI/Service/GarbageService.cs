using AutoMapper;
using CampManager.Domain.Domain;
using CampManagerWebUI.Db;
using CampManagerWebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Service
{
    public class GarbageService
    {
        private ApplicationDbContext _db;

        public GarbageService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<GarbageViewModel> Get(int idOrganization, int IdSeason)
        {
            var garbages = _db.Garbage.Include(x => x.Season)
                .Include(x => x.Kind)
                .Where(x => x.Season.Id == IdSeason)
                .ToList();

            List<GarbageViewModel> garbagesViewModel = new List<GarbageViewModel>();
            foreach (var kv in garbages.GroupBy(x => x.Date).OrderBy(x => x.Key))
            {
                DateTime date = kv.Key;
                var garbagesDate = garbages.FindAll(x => x.Date == date);
                garbagesViewModel.Add(Get(idOrganization, garbagesDate));
            }

            return garbagesViewModel;
        }

        public GarbageViewModel Get(int idOrganization, int IdSeason, DateTime date)
        {
            var garbages = _db.Garbage
                .Include(x => x.Season)
                .Include(x => x.Kind)
                .Where(x => x.Season.Id == IdSeason && x.Date == date)
                .ToList();

            return Get(idOrganization, garbages);
        }

        public GarbageViewModel GetNew(int idOrganization, int idSeason)
        {
            GarbageViewModel garbage = new GarbageViewModel();
            garbage.IdSeason = idSeason;

            var kinds = _db.GarbageKind.Include(x => x.Organization)
                .Where(x => x.Organization.Id == idOrganization);
            foreach (var kind in kinds)
            {
                garbage.Collections.Add(new GarbageDate { IdKind = kind.Id, KindName = kind.Name });
            }

            return garbage;
        }

        public void SaveNew(GarbageViewModel garbageViewModel, string[] selected, out string errorMsg)
        {
            errorMsg = "";
            var garbages = _db.Garbage.Where(x => x.Season.Id == garbageViewModel.IdSeason
                && x.Date == garbageViewModel.Date);

            if (garbages.Any())
            {
                errorMsg = "Istnieje już wpis na wybrany dzień";
                return;
            }

            var season = _db.SeasonOrganization.Find(garbageViewModel.IdSeason);
            foreach (string kind in selected)
            {
                Garbage garbage = new Garbage();
                garbage.Season = season;
                garbage.Date = garbageViewModel.Date;
                int kindId = int.Parse(kind);
                garbage.Kind = _db.GarbageKind.Find(kindId);
                garbage.Collection = true;

                _db.Garbage.Add(garbage);
            }

            _db.SaveChanges();
        }

        public void SaveEdit(int idOrganization, GarbageViewModel garbageViewModel, string[] selected)
        {
            DateTime date = _db.Garbage.Find(garbageViewModel.Id).Date;
            int idSeason = garbageViewModel.IdSeason;
            var garbages = _db.Garbage.Include(x => x.Kind)
                .Where(x => x.Season.Id == idSeason && x.Date == date).ToList();

            var season = _db.SeasonOrganization.Find(garbageViewModel.IdSeason);
            var kinds = _db.GarbageKind.Include(x => x.Organization)
                .Where(x => x.Organization.Id == idOrganization);

            List<int> idsSelected = selected.ToList().ConvertAll<int>(x => int.Parse(x));

            foreach (var kind in kinds)
            {
                int kindId = kind.Id;
                var garbage = garbages.FirstOrDefault(x => x.Kind.Id == kindId);
                bool value = idsSelected.Contains(kindId);
                if (value)
                {
                    if (garbage == null)
                    {
                        garbage = new Garbage();
                        garbage.Season = season;
                        garbage.Date = date;
                        garbage.Kind = _db.GarbageKind.Find(kindId);

                        _db.Garbage.Add(garbage);
                    }

                    garbage.Collection = value;
                }
                else
                {
                    if (garbage != null)
                    {
                        garbage.Collection = value;
                    }
                }
            }

            _db.SaveChanges();
        }

        private GarbageViewModel Get(int idOrganization, List<Garbage> garbages)
        {
            if (garbages.Count == 0)
                return null;

            GarbageViewModel garbageViewModel = Mapper.Map<GarbageViewModel>(garbages.First());

            var kinds = _db.GarbageKind.Include(x => x.Organization)
                .Where(x => x.Organization.Id == idOrganization);
            foreach (var kind in kinds)
            {
                garbageViewModel.Collections.Add(new GarbageDate { IdKind = kind.Id, KindName = kind.Name });
            }

            foreach (var garbage in garbages)
            {
                int idKind = garbage.Kind.Id;
                var garbageKind = garbageViewModel.Collections.Find(x => x.IdKind == idKind);
                garbageKind.Collection = garbage.Collection;
                if (garbageKind.Collection)
                {
                    garbageViewModel.Kinds += " " + garbage.Kind.Name;
                }
            }

            return garbageViewModel;
        }
    }
}