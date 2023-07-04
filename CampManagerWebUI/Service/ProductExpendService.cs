using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Service
{
    public class ProductExpendService
    {
        private Db.ApplicationDbContext _db;

        public ProductExpendService(Db.ApplicationDbContext db)
        {
            _db = db;
        }

        public void Fill(List<ProductOutPosition> productOutPositionList, ref string error)
        {
            var productAmountList = _db.ProductAmount.Where(x => x.AmountBuy != x.AmountExpend)
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Invoice)
                .Include(x => x.InvoicePosition.Product)
                .Include(x => x.InvoicePosition.Product.Measure)
                .ToList();

            List<ProductExpend> productExpendList = _db.ProductExpend.ToList();
            foreach (var productOutPosition in productOutPositionList)
            {
                AddExpend(productOutPosition, productAmountList, productExpendList, ref error);
            }

            if (string.IsNullOrEmpty(error))
                _db.SaveChanges();
        }

        public void Fill(ProductOutPosition productOutPosition, int idSeason, ref string error)
        {
            var productAmountList = _db.ProductAmount.Where(x => x.AmountBuy != x.AmountExpend)
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Invoice)
                .Include(x => x.InvoicePosition.Product)
                .Include(x => x.InvoicePosition.Product.Measure)
                .Where(ip => ip.InvoicePosition.Product.Id == productOutPosition.Product.Id)
                .Where(x => x.InvoicePosition.Invoice.Season.Id == idSeason)
                .ToList();

            List<ProductExpend> productExpendList = _db.ProductExpend.ToList();
            AddExpend(productOutPosition, productAmountList, productExpendList, ref error);

            _db.SaveChanges();
        }

        public void Remove(ProductOutPosition productOutPosition)
        {
            var productExpendList = _db.ProductExpend.Where(x => x.ProductOutPosition.Id == productOutPosition.Id)
                .Include(x => x.InvoicePosition)
                .ToList();

            List<int> idInvoicePosition = productExpendList.ConvertAll(x => x.InvoicePosition.Id);
            var productAmountList = _db.ProductAmount.Where(x => idInvoicePosition.Contains(x.InvoicePosition.Id))
                .ToList();

            foreach (var productExpend in productExpendList)
            {
                var productAmount = productAmountList.Find(x => x.InvoicePosition.Id == productExpend.InvoicePosition.Id);
                productAmount.AmountExpend -= productExpend.Amount;
                productAmount.WorthExpend -= productExpend.Worth;
                _db.Entry(productAmount).State = EntityState.Modified;
                _db.ProductExpend.Remove(productExpend);
            }
        }

        public void Edit(ProductOutPosition productOutPosition, ref string error)
        {
            ProductOutPosition productOutPositionOrginal = _db.ProductOutPosition.Find(productOutPosition.Id);


            decimal diff = productOutPosition.Amount - productOutPositionOrginal.Amount;
            if (diff > 0)
            {
                var productAmountList = _db.ProductAmount.Where(x => x.AmountBuy != x.AmountExpend)
                    .Include(x => x.InvoicePosition)
                    .Include(x => x.InvoicePosition.Invoice)
                    .Include(x => x.InvoicePosition.Product)
                    .Include(x => x.InvoicePosition.Product.Measure)
                    .ToList();

                List<ProductExpend> productExpendList = _db.ProductExpend.ToList();
                AddExpend(productOutPosition, productAmountList, productExpendList, diff, ref error);
            }
            else
            {
                decimal amountToReturn = -diff;

                var productExpendList = _db.ProductExpend.Where(x => x.ProductOutPosition.Id == productOutPosition.Id)
                    .Include(x => x.InvoicePosition)
                    .ToList();

                List<int> idInvoicePosition = productExpendList.ConvertAll(x => x.InvoicePosition.Id);
                var productAmountList = _db.ProductAmount.Where(x => idInvoicePosition.Contains(x.InvoicePosition.Id))
                    .OrderByDescending(x => x.InvoicePosition.Invoice.DateDelivery)
                    .ToList();

                foreach (var productExpend in productExpendList)
                {
                    var productAmount = productAmountList.Find(x => x.InvoicePosition.Id == productExpend.InvoicePosition.Id);

                    //decimal amountToReturnPosition = Math.Min(amountToReturn, pa.AmountBuy - pa.AmountExpend);


                    //if (diff > productAmount.AmountExpend)
                    //{
                    //    diff -= productAmount.AmountExpend;
                    //    productAmount.AmountExpend -= productExpend.Amount;
                    //    productAmount.WorthExpend -= productExpend.Worth;
                    //    _db.Entry(productAmount).State = EntityState.Modified;
                    //    _db.ProductExpend.Remove(productExpend);
                    //}
                    //else
                    //{

                    //    productAmount.AmountExpend -= diff;
                    //    _db.Entry(productAmount).State = EntityState.Modified;
                    //    productOutPosition.Price = productOutPosition.Worth / productOutPosition.Amount;
                    //    _db.Entry(productOutPosition).State = EntityState.Modified;
                    //    break;
                    //}
                }
            }

            _db.SaveChanges();
        }

        private void AddExpend(ProductOutPosition productOutPosition, List<ProductAmount> productAmountList, List<ProductExpend> productExpendList, ref string error)
        {
            AddExpend(productOutPosition, productAmountList, productExpendList, productOutPosition.Amount, ref error);
        }

        private void AddExpend(ProductOutPosition productOutPosition,
            List<ProductAmount> productAmountList,
            List<ProductExpend> productExpendList,
            decimal amountToExpend,
            ref string error)
        {
            if (productExpendList.Exists(x => x.ProductOutPosition == productOutPosition))
            {
                error = string.Format("Nieudany rozchód: {0}", productOutPosition.Product.NameDescriptionMeasures);
                return;
            }

            var productAmount = productAmountList
                .FindAll(x => x.InvoicePosition.Product == productOutPosition.Product
                && x.InvoicePosition.Invoice.DateDelivery <= productOutPosition.ProductOut.Date)
                    .OrderBy(x => x.InvoicePosition.Invoice.DateDelivery);

            // decimal amountToExpend = productOutPosition.Amount;
            foreach (var pa in productAmount)
            {
                decimal amountToExpendPosition = Math.Min(amountToExpend, pa.AmountBuy - pa.AmountExpend);
                decimal worth = Math.Round(((amountToExpendPosition + pa.AmountExpend) / pa.AmountBuy) * pa.WorthBuy - pa.WorthExpend, 2, MidpointRounding.AwayFromZero);

                pa.AmountExpend += amountToExpendPosition;
                pa.WorthExpend += worth;
                amountToExpend -= amountToExpendPosition;

                ProductExpend productExpend = new ProductExpend();
                productExpend.InvoicePosition = pa.InvoicePosition;
                productExpend.ProductOutPosition = productOutPosition;
                productExpend.Amount = amountToExpendPosition;
                productExpend.Worth = worth;

                productExpendList.Add(productExpend);

                _db.Entry(pa).State = EntityState.Modified;
                _db.ProductExpend.Add(productExpend);

                productOutPosition.Worth += worth;
                if (amountToExpend == 0)
                {
                    productOutPosition.Price = productOutPosition.Worth / productOutPosition.Amount;
                    _db.Entry(productOutPosition).State = EntityState.Modified;
                    return;
                }
            }

            if (amountToExpend != 0)
                error = string.Format("Nieudany rozchód: {0}, próba rozchodowania: {1}, pozostało: {2}", productOutPosition.Product.NameDescriptionMeasures, productOutPosition.Amount, amountToExpend);
        }
    }
}