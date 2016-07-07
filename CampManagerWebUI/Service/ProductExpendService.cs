using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

        public void Fill(List<ProductOutPosition> productOutPositionList)
        {
            var productAmountList = _db.ProductAmount.Where(x => x.AmountBuy != x.AmountExpend)
                .Include(x => x.InvoicePosition)
                .Include(x => x.InvoicePosition.Invoice)
                .Include(x => x.InvoicePosition.Product)
                .ToList();

            List<ProductExpend> productExpendList = new List<ProductExpend>();
            foreach (var productOutPosition in productOutPositionList)
            {
                AddExpend(productOutPosition, productAmountList, productExpendList);
            }

            _db.SaveChanges();
        }

        private void AddExpend(ProductOutPosition productOutPosition, List<ProductAmount> productAmountList, List<ProductExpend> productExpendList)
        {
            var productAmount = productAmountList
                .FindAll(x => x.InvoicePosition.Product == productOutPosition.Product
                && x.InvoicePosition.Invoice.DateDelivery <= productOutPosition.ProductOut.Date)
                    .OrderBy(x => x.InvoicePosition.Invoice.DateDelivery);

            decimal amountToExpend = productOutPosition.Amount;
            foreach (var pa in productAmount)
            {
                decimal amountToExpendPosition = Math.Min(amountToExpend, pa.AmountBuy - pa.AmountExpend);
                decimal worth = ((amountToExpendPosition + pa.AmountExpend) / pa.AmountBuy) * pa.WorthBuy - pa.WorthExpend;

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
            {
                throw new Exception("");
            }
        }
    }
}