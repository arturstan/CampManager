using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class SepticTankKindOrganization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Capacity { get; set; }

        [Required]
        public Organization Organization { get; set; }

        public string NameDescription
        {
            get
            {
                return string.Format("{0} [{1}m3]", Name, Capacity);
            }
        }
    }
}
