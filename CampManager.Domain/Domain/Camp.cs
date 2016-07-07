using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class Camp
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public Place Place { get; set; }

        [Required]
        public SeasonOrganization CampOrganization { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public decimal PersonCount { get; set; }

        public decimal PricePerPerson {get;set;}

        public List<CampMeal> Meal { get; set; }

        public Camp()
        {
            Meal = new List<CampMeal>();
        }
    }
}
