using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_Blog.Models
{
    public class Comment
    {
        
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        
        public string AuthorId { get; set; }

        public int ArticleId { get; set; }

        public string FullName { get; set; }

        public byte[] UserPhoto { get; set; }
    }
}