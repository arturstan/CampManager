using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class ProductOut
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public List<ProductOutPosition> Positions { get; set; }

        public SeasonOrganization Season { get; set; }

        public ProductOut()
        {
            Positions = new List<ProductOutPosition>();
        }
    }
}
