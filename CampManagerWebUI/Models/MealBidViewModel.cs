using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class MealBidViewModel
    {
        public int Id { get; set; }

        public int IdSeason { get; set; }

        public string SeasonName { get; set; }

        [DisplayName("Data")]
        public DateTime Date { get; set; }

        [DisplayName("Stawka")]
        public decimal Bid { get; set; }

        [DisplayName("Rozchód")]
        public decimal Expend { get; set; }

        [DisplayName("Ilość ludzi")]
        public decimal PeopleCount { get; set; }
    }
}