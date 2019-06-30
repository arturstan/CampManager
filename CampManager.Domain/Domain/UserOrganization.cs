using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class UserOrganization
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public bool Active { get; set; }
        public DateTime? DateExpire { get; set; }
        public string Roles { get; set; }


        public Organization Organization { get; set; }
    }
}
