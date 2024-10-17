using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Switch_and_Shift.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Switch_and_Shift.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Admin> Admins { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users");
        }

        public DbSet<Switch_and_Shift.Models.Admin> Admin{ get; set; }

        public DbSet<Switch_and_Shift.Models.UserReview> UserReview { get; set; }

        public DbSet<Switch_and_Shift.Models.Report> Report { get; set; }

        public DbSet<Switch_and_Shift.Models.Reports_Admin> Reports_Admin { get; set; }

        public DbSet<Switch_and_Shift.Models.Categories> Categories{ get; set; }





       

    }
}
