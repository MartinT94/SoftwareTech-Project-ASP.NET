using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_Blog.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public string CategoryId { get; set; }


    }
}