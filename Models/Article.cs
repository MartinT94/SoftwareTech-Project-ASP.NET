using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace ASP.NET_Blog.Models
{
    public class Article
    {
        public Article()
        {
            this.Date = DateTime.Now;
        }
        
        
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public List<Comment> Comments { get; set; }
        

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}