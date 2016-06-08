using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class ProductOrganizationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int IdMeasure { get; set; }
        [Required]
        public int IdOrganization { get; set; }

        public string MeasureName { get; set; }

        public List<MeasureOrganization> Measures;
    }
}