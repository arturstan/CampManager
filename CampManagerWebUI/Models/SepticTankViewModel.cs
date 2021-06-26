using CampManager.Domain.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class SepticTankViewModel
    {
        public int Id { get; set; }

        public int IdSeason { get; set; }

        public string SeasonName { get; set; }

        [DisplayName("Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Rodzaj beczki")]
        public int IdKind { get; set; }

        [DisplayName("Rodzaj beczki")]
        public string KindNameDescription { get; set; }

        [DisplayName("Ilość beczek")]
        public int Amount { get; set; }

        public List<SepticTankKindOrganization> Kinds;
    }
}