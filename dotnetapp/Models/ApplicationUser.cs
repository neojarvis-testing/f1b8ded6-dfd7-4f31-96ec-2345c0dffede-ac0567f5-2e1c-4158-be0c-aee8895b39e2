using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
 
namespace dotnetapp.Models
{
    public class ApplicationUser : IdentityUser
    {  
        [MaxLength(30 ,ErrorMessage="Name cannot exceed 30 characters.")]
        public string? Name { get; set; }
    }
}