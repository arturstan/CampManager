using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class SeasonOrganizationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }

        [Required]
        public int IdBase { get; set; }
        public string BaseName { get; set; }

        public List<BaseOrganization> Bases;
    }
}