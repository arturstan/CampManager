using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampManager.Domain.Domain
{
    public class UserEmailAllow
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string StartRoles { get; set; }
        public DateTime? DateExpire { get; set; }

        [Required]
        public Organization Organization { get; set; }
    }
}
