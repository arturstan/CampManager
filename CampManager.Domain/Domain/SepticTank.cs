using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class SepticTank
    {
        public int Id { get; set; }

        [Required]
        public SeasonOrganization Season { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public SepticTankKindOrganization Kind { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
