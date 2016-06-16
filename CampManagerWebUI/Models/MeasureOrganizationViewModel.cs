using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CampManagerWebUI.Models
{
    public class MeasureOrganizationViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [Required]
        public int IdOrganization { get; set; }
    }
}