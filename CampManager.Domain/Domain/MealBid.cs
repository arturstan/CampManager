using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class MealBid
    {
        public int Id { get; set; }

        public SeasonOrganization Season { get; set; }

        public DateTime Date { get; set; }

        public decimal Bid { get; set; }

        public decimal Expend { get; set; }

        public decimal PeopleCount { get; set; }
    }
}
