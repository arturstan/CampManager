using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class ProductOrganization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        [Required]
        public MeasureOrganization Measure { get; set; }

        [Required]
        public Organization Organization { get; set; }

        public string NameDescription
        {
            get { return string.Format("{0} ({1})", Name, Description); }
        }

        public string NameDescriptionMeasures
        {
            get
            {
                if(string.IsNullOrEmpty(Description))
                    return string.Format("{0} [{1}]", Name, Measure.Name);
                else
                    return string.Format("{0} ({1}) [{2}]", Name, Description, Measure.Name);
            }
        }

    }
}
