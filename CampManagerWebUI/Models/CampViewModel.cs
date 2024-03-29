﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CampManager.Domain.Domain;

namespace CampManagerWebUI.Models
{
    public class CampViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Miejsce obozowe")]
        public int IdPlace { get; set; }

        [DisplayName("Miejsce obozowe")]
        public string PlaceName { get; set; }

        [Required]
        public int IdCampOrganization { get; set; }

        [DisplayName("Data rozpoczęcia")]
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [DisplayName("Data zakończenia")]
        [DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }

        [DisplayName("Ilość osób")]
        public decimal PersonCount { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Uwagi")]
        public string Note { get; set; }

        [DisplayName("Cena za osobę")]
        public decimal PricePerPerson { get; set; }

        public List<CampMealViewModel> Meal { get; set; }

        public List<Place> Places;

        public CampViewModel()
        {
            Meal = new List<CampMealViewModel>();
        }
    }
}