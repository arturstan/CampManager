﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class MeasureOrganization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public Organization Organization { get; set; }
    }
}
