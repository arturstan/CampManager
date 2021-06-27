using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class GarbageKindOrganization
    {
        public int Id { get; set; }

        public Organization Organization { get; set; }

        public string Name { get; set; }
    }
}
