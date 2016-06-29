using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class PlaceViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }

        public int IdBaseOrganization { get; set; }
    }
}