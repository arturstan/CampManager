using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class GarbageViewModel
    {
        public GarbageViewModel()
        {
            Collections = new List<GarbageDate>();
        }

        public int Id { get; set; }

        public int IdSeason { get; set; }

        public string SeasonName { get; set; }

        [DisplayName("Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<GarbageDate> Collections { get; set; }

        [DisplayName("Rodzaje śmieci")]
        public string Kinds { get; set; }
    }

    public class GarbageDate
    {
        public int IdKind { get; set; }

        [DisplayName("Rodzaj")]
        public string KindName { get; set; }

        public bool Collection { get; set; }
    }
}