using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace CampManagerWebUI.Models
{
    public class CampMealViewModel
    {
        [Required]
        [DisplayName("Obóz")]
        public int IdCamp { get; set; }

        [DisplayName("Obóz")]
        public string CampName { get; set; }

        [Required]
        [DisplayName("Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Śniadanie")]
        public int BreakfastEat { get; set; }
        [Required]
        [DisplayName("Śniadanie prowiant")]
        public int BreakfastEatSupplies { get; set; }
        [Required]
        [DisplayName("Śniadanie kasa")]
        public int BreakfastCash { get; set; }

        [Required]
        [DisplayName("Obiad")]
        public int DinnerEat { get; set; }
        [Required]
        [DisplayName("Obiad prowiant")]
        public int DinnerEatSupplies { get; set; }
        [Required]
        [DisplayName("Obiad kasa")]
        public int DinnerCash { get; set; }


        [Required]
        [DisplayName("Kolacja")]
        public int SupperEat { get; set; }
        [Required]
        [DisplayName("Kolacja prowiant")]
        public int SupperEatSupplies { get; set; }
        [Required]
        [DisplayName("Kolacja kasa")]
        public int SupperCash { get; set; }

        [Required]
        [DisplayName("Zakwaterowani")]
        public int Reside { get; set; }

        // public List<Camp> Camps;


        public int IdCampMealBreakfast { get; set; }
        public int IdCampMealDinner { get; set; }
        public int IdCampMealSupper { get; set; }
    }
}