using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNews.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Categories> Category { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<ContactUs> contactUs { get; set; }
        public DbSet<TeamMember> teamMembers { get; set; }
    }
}
