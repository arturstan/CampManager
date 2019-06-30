using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampManagerWebUI.Models
{
    public class UserEmailAllowViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}