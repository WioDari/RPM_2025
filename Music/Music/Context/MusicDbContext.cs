using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Music.Models;

namespace Music.Context;

public partial class MusicDbContext : DbContext
{
    public MusicDbContext()
    {
    }

    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<LoginAttempt> LoginAttempts { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MusicDB;Username=postgres;Password=3224");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("Albums_pkey");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Albums_ArtistId_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("Artists_pkey");

            entity.HasIndex(e => e.ArtistName, "Artists_ArtistName_key").IsUnique();

            entity.Property(e => e.CountryId).HasDefaultValue(0);

            entity.HasOne(d => d.Country).WithMany(p => p.Artists)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Artists_CountryId_fkey");

            entity.HasMany(d => d.Genres).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ArtistGenre_GenreId_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ArtistGenre_ArtistId_fkey"),
                    j =>
                    {
                        j.HasKey("ArtistId", "GenreId").HasName("ArtistGenre_pkey");
                        j.ToTable("ArtistGenre");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("Countries_pkey");

            entity.HasIndex(e => e.CountryName, "Countries_CountryName_key").IsUnique();
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Genres_pkey");

            entity.HasIndex(e => e.GenreName, "Genres_GenreName_key").IsUnique();

            entity.HasMany(d => d.Albums).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumGenre",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AlbumGenres_AlbumId_fkey"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AlbumGenres_GenreId_fkey"),
                    j =>
                    {
                        j.HasKey("GenreId", "AlbumId").HasName("AlbumGenres_pkey");
                        j.ToTable("AlbumGenres");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "TrackGenre",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TrackGenres_TrackId_fkey"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TrackGenres_GenreId_fkey"),
                    j =>
                    {
                        j.HasKey("GenreId", "TrackId").HasName("TrackGenres_pkey");
                        j.ToTable("TrackGenres");
                    });
        });

        modelBuilder.Entity<LoginAttempt>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("LoginAttempts_pkey");

            entity.Property(e => e.IsSuccess).HasDefaultValue(false);
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.User).WithMany(p => p.LoginAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LoginAttempts_UserId_fkey");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("Playlists_pkey");

            entity.Property(e => e.Likes).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Playlists_UserId_fkey");

            entity.HasMany(d => d.Tags).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylistTags_TagId_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylistTags_PlaylistId_fkey"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TagId").HasName("PlaylistTags_pkey");
                        j.ToTable("PlaylistTags");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylistTracks_TrackId_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylistTracks_PlaylistId_fkey"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TrackId").HasName("PlaylistTracks_pkey");
                        j.ToTable("PlaylistTracks");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Roles_pkey");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("Subscriptions_pkey");

            entity.HasIndex(e => e.SubscriptionName, "Subscriptions_SubscriptionName_key").IsUnique();
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tag_pkey");

            entity.ToTable("Tag");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("Tracks_pkey");

            entity.HasIndex(e => e.FilePath, "Tracks_FilePath_key").IsUnique();

            entity.HasIndex(e => e.TrackName, "Tracks_TrackName_key").IsUnique();

            entity.Property(e => e.Bitrate).HasDefaultValue(0);
            entity.Property(e => e.PlayCount).HasDefaultValue(0);

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tracks_AlbumId_fkey");

            entity.HasMany(d => d.Artists).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TrackArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TrackArtist_ArtistId_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TrackArtist_TrackId_fkey"),
                    j =>
                    {
                        j.HasKey("TrackId", "ArtistId").HasName("TrackArtist_pkey");
                        j.ToTable("TrackArtist");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.HasIndex(e => e.Email, "Users_Email_key").IsUnique();

            entity.HasIndex(e => e.UserLogin, "Users_UserLogin_key").IsUnique();

            entity.Property(e => e.IsBlocked).HasDefaultValue(false);
            entity.Property(e => e.LastLogin).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_RoleId_fkey");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_SubscriptionId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
