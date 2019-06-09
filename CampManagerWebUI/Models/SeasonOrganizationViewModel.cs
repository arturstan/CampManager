using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;
using System.ComponentModel;

namespace CampManagerWebUI.Models
{
    public class SeasonOrganizationViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data rozpoczęcia")]
        public DateTime DateStart { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Data zakończenia")]
        public DateTime DateEnd { get; set; }
        [DisplayName("Aktywny")]
        public bool Active { get; set; }

        [Required]
        public int IdBase { get; set; }
        [DisplayName("Nazwa bazy")]
        public string BaseName { get; set; }

        public List<BaseOrganization> Bases;
    }
}