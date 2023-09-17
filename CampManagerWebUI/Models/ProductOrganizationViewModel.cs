using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Miara")]
        public int IdMeasure { get; set; }
        [Required]
        public int IdOrganization { get; set; }

        [DisplayName("Miara")]
        public string MeasureName { get; set; }

        [DisplayName("Ilość")]
        public decimal Amount { get; set; }

        [DisplayName("Wartość")]
        public decimal Worth { get; set; }
        [DisplayName("Aktywny")]
        public bool Active { get; set; }


        public List<MeasureOrganization> Measures;

        public List<ProductAmount> ProductAmount;

        public List<ProductExpend> ProductExpend;

        public string NameDescriptionMeasuresAmount
        {
            get
            {
                if (string.IsNullOrEmpty(Description))
                    return string.Format("{0} [{1}] - {2}", Name, MeasureName, Amount);
                else
                    return string.Format("{0} ({1}) [{2}] - {3}", Name, Description, MeasureName, Amount);
            }
        }
    }
}