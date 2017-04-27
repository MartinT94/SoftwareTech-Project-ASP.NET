using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_Blog.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        
    }
}