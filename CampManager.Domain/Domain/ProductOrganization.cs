﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class ProductOrganization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MeasureOrganization Measure { get; set; }

        public Organization Organization { get; set; }
    }
}
