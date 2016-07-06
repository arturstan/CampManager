using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class CampMealViewModel
    {
        public int Id { get; set; }

        [Required]
        public int IdCamp { get; set; }

        public string CampName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int BreakfastEat { get; set; }
        [Required]
        public int BreakfastEatSupplies { get; set; }
        [Required]
        public int BreakfastCash { get; set; }

        [Required]
        public int DinnerEat { get; set; }
        [Required]
        public int DinnerEatSupplies { get; set; }
        [Required]
        public int DinnerCash { get; set; }


        [Required]
        public int SupperEat { get; set; }
        [Required]
        public int SupperEatSupplies { get; set; }
        [Required]
        public int SupperCash { get; set; }


        public List<Camp> Camps;


        public int IdCampMealBreakfast { get; set; }
        public int IdCampMealDinner { get; set; }
        public int IdCampMealSupper { get; set; }
    }
}