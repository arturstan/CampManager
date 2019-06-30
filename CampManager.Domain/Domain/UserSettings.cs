using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class UserSettings
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public byte GroupSettings { get; set; }
        public string Settings { get; set; }
    }
}
