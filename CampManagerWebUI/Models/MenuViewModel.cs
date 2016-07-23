using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }

        public int IdSeason { get; set; }

        [Required]
        [DisplayName("Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Śniadanie")]
        public string Breakfast { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Obiad")]
        public string Dinner { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Kolacja")]
        public string Supper { get; set; }
    }
}