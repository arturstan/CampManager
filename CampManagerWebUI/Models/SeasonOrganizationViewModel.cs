using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class SeasonOrganizationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        [Required]
        public int IdBase { get; set; }
        public int BaseName { get; set; }
    }
}