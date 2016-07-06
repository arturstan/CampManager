using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class ProductOutViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Data")]
        public DateTime Date { get; set; }

        [DisplayName("Opis")]
        public string Description { get; set; }

        public int IdSeason { get; set; }

        public int SeasonName { get; set; }

        public List<ProductOutPositionViewModel> Positions { get; }

        [DisplayName("Wartość")]
        public decimal Worth
        {
            get { return Positions.Sum(x => x.Worth); }
        }

        [DisplayName("Ilość pozycji")]
        public decimal PositionCount
        {
            get { return Positions.Count; }
        }

        public ProductOutViewModel()
        {
            Positions = new List<ProductOutPositionViewModel>();
        }
    }
}