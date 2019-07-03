using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class UserViewModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Aktywny")]
        public bool Active { get; set; }
        [DisplayName("Data wygaśnięcia")]
        public DateTime? DateExpire { get; set; }
        [DisplayName("Role")]
        public string Roles { get; set; }
    }
}