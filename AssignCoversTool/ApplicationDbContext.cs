using Microsoft.EntityFrameworkCore;
using MusicPlus.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace MusicPlus.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<AlbumGenre> AlbumGenres { get; set; }
        public DbSet<TrackArtist> TrackArtists { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<PlaylistTag> PlaylistTags { get; set; }
        public DbSet<PlaylistSubscription> PlaylistSubscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Server=localhost;Database=musicplus_db;User=root;Password=;Port=3306;CharSet=utf8mb4;";
                var serverVersion = ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql);
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AlbumGenre>()
                .HasKey(ag => new { ag.AlbumID, ag.GenreID });

            modelBuilder.Entity<AlbumGenre>()
                .HasOne(ag => ag.Album)
                .WithMany(a => a.AlbumGenres)
                .HasForeignKey(ag => ag.AlbumID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AlbumGenre>()
                .HasOne(ag => ag.Genre)
                .WithMany(g => g.AlbumGenres)
                .HasForeignKey(ag => ag.GenreID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrackArtist>()
                .HasKey(ta => new { ta.TrackID, ta.ArtistID });

            modelBuilder.Entity<TrackArtist>()
                .HasOne(ta => ta.Track)
                .WithMany(t => t.TrackArtists)
                .HasForeignKey(ta => ta.TrackID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrackArtist>()
                .HasOne(ta => ta.Artist)
                .WithMany(a => a.TrackArtists)
                .HasForeignKey(ta => ta.ArtistID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistTrack>()
                .HasKey(pt => new { pt.PlaylistID, pt.TrackID });

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.Playlist)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(pt => pt.PlaylistID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.Track)
                .WithMany(t => t.PlaylistTracks)
                .HasForeignKey(pt => pt.TrackID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistTag>()
                .HasKey(pt => new { pt.PlaylistID, pt.TagName });

            modelBuilder.Entity<PlaylistTag>()
                .HasOne(pt => pt.Playlist)
                .WithMany(p => p.PlaylistTags)
                .HasForeignKey(pt => pt.PlaylistID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSubscription>()
                .HasKey(ps => new { ps.UserID, ps.PlaylistID });

            modelBuilder.Entity<PlaylistSubscription>()
                .HasOne(ps => ps.User)
                .WithMany(u => u.PlaylistSubscriptions)
                .HasForeignKey(ps => ps.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSubscription>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSubscriptions)
                .HasForeignKey(ps => ps.PlaylistID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
