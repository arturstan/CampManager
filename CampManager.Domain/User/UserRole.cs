using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.User
{
    public class UserRole
    {
        public Role Role { get; set; }
        public bool Active { get; set; }
    }
}
