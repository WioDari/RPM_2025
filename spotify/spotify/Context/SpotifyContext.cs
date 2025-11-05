using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using spotify.Models;

namespace spotify.Context;

public partial class SpotifyContext : DbContext
{
    public SpotifyContext()
    {
    }

    public SpotifyContext(DbContextOptions<SpotifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Countrye> Countryes { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=spotify; Username=postgres; Password=123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Albums__pk");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("albums_artists_fk");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_ArtistName__pk");

            entity.HasOne(d => d.Country).WithMany(p => p.Artists)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("artists_countryes_fk");

            entity.HasMany(d => d.Tracks).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistsTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("_ArtistsTracks__Tracks_FK"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("_ArtistsTracks__Artists_FK"),
                    j =>
                    {
                        j.HasKey("ArtistId", "TrackId").HasName("_ArtistsTracks__pk");
                        j.ToTable("ArtistsTracks");
                    });
        });

        modelBuilder.Entity<Countrye>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Countryes__pk");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Genre__pk");

            entity.Property(e => e.Genre1).HasColumnName("Genre");

            entity.HasMany(d => d.Albums).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "GenresAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("_GenresAlbums__Albums_FK"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("_GenresAlbums__Genres_FK"),
                    j =>
                    {
                        j.HasKey("GenreId", "AlbumId").HasName("_GenresAlbums__pk");
                        j.ToTable("GenresAlbums");
                    });

            entity.HasMany(d => d.Artists).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "GenresArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("genresartists_artists_fk"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("genresartists_genres_fk"),
                    j =>
                    {
                        j.HasKey("GenreId", "ArtistId").HasName("_GenresArtists__pk");
                        j.ToTable("GenresArtists");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "GenresTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("_GenresTracks__Tracks_FK"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("_GenresTracks__Genres_FK"),
                    j =>
                    {
                        j.HasKey("GenreId", "TrackId").HasName("_GenresTracks__pk");
                        j.ToTable("GenresTracks");
                    });
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Playlist__pk");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("playlist_users_fk");

            entity.HasMany(d => d.Tags).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistsTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playliststags_tags_fk"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playliststags_playlist_fk"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TagId").HasName("_PlaylistsTags__pk");
                        j.ToTable("PlaylistsTags");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistsTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playliststracks_tracks_fk"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playliststracks_playlist_fk"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TrackId").HasName("_PlaylistsTracks__pk");
                        j.ToTable("PlaylistsTracks");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.PlaylistsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistsUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playlistsusers_users_fk"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playlistsusers_playlist_fk"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "UserId").HasName("_PlaylistsUsers__pk");
                        j.ToTable("PlaylistsUsers");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Roles__pk");

            entity.Property(e => e.Role1).HasColumnName("Role");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Subscriptions__pk");

            entity.Property(e => e.Subscription1).HasColumnName("Subscription");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Tags__pk");

            entity.Property(e => e.Tag1).HasColumnName("Tag");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Tracks__pk");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tracks_albums_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_Users__pk");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_roles_fk");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_subscriptions_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
