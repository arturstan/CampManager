using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class Camp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Place Place { get; set; }

        public CampOrganization CampOrganization { get; set; }
    }
}
