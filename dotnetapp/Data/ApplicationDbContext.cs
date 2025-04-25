using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using dotnetapp.Controllers;
using dotnetapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 
namespace dotnetapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
 
        }
 
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Announcement> Announcements { get; set; } 

        public DbSet<ApplicationUser> ApplicationUsers {get; set;}

    }
}