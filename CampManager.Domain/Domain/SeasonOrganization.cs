using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class SeasonOrganization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool Active { get; set; }

        [Required]
        public BaseOrganization Base { get; set; }
    }
}
