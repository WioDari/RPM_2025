using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DemoExam.Models
{
    public class AppDbContext : DbContext
    {
        public static string host = "localhost";
        public static string port = "5438";
        public static string database = "demo_exam";
        public static string username = "postgres";
        public static string password = "postgres";
        public string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> userrole { get; set; }
        public DbSet<SubscriptionType> subscriptiontype { get; set; }
        public DbSet<Playlist> playlist { get; set; }
        public DbSet<Track> track { get; set; }
        public DbSet<Artist> artist { get; set; }
        public DbSet<Country> countrie { get; set; }
        public DbSet<Album> album { get; set; }
        public DbSet<Genre> genre { get; set; }
        public DbSet<Tag> tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}