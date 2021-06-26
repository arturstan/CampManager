using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class Garbage
    {
        public int Id { get; set; }

        public SeasonOrganization Season { get; set; }

        public DateTime Date { get; set; }


    }
}
