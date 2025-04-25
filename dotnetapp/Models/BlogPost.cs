using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace dotnetapp.Models
{
    public class BlogPost
    {
        public int BlogPostId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; } // Nullable User object
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime PublishedDate { get; set; }

    }
}