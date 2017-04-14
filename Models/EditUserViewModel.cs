
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ASP.NET_Blog.Models
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }

        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }

        public IList<Role> Role { get; set; }

    }
}