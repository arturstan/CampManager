using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class CampMeal
    {
        public int Id { get; set; }

        [Required]
        public Camp Camp { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public KinfOfMeal Kind { get; set; }
        [Required]
        public int Eat { get; set; }
        [Required]
        public int EatSupplies { get; set; }
        [Required]
        public int Cash { get; set; }
    }

    public enum KinfOfMeal
    {
        breakfast,
        dinner,
        supper
    }
}
