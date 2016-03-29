using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class CampMeal
    {
        public int Id { get; set; }

        public Camp Camp { get; set; }
        public DateTime Date { get; set; }
        public KinfOfMeal Kind { get; set; }
        public int Eat { get; set; }
        public int EatSupplies { get; set; }
        public int Cash { get; set; }
    }

    public enum KinfOfMeal
    {
        breakfast,
        dinner,
        supper
    }
}
