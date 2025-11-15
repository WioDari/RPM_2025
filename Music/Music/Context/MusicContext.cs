using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Music.Models;

namespace Music.Context;

public partial class MusicContext : DbContext
{
    public MusicContext()
    {
    }

    public MusicContext(DbContextOptions<MusicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Password=123; Database=music; Username=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_albums__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("albums_artists_fk");

            entity.HasMany(d => d.Genres).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumsGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("albumsgenres_genres_fk"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .HasConstraintName("albumsgenres_albums_fk"),
                    j =>
                    {
                        j.HasKey("AlbumId", "GenreId").HasName("albumsgenres_pk");
                        j.ToTable("AlbumsGenres");
                    });
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_artists__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Country).WithMany(p => p.Artists)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("artists_countries_fk");

            entity.HasMany(d => d.Genres).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistsGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("_artistsgenres__genres_fk"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .HasConstraintName("_artistsgenres__artists_fk"),
                    j =>
                    {
                        j.HasKey("ArtistId", "GenreId").HasName("artistsgenres_pk");
                        j.ToTable("ArtistsGenres");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_countries__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_genres__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_playlists__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.CreatorUser).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.CreatorUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("playlists_users_fk");

            entity.HasMany(d => d.Tags).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistsTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("_playliststags__tags_fk"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .HasConstraintName("_playliststags__playlists_fk"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TagId").HasName("_playliststags__pk");
                        j.ToTable("PlaylistsTags");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistsTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .HasConstraintName("_playliststracks__tracks_fk"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .HasConstraintName("_playliststracks__playlists_fk"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TrackId").HasName("_playliststracks__pk");
                        j.ToTable("PlaylistsTracks");
                    });
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_subscriptions__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_tags__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_tracks__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Raiting).HasPrecision(3, 2);

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tracks_albums_fk");

            entity.HasMany(d => d.Artists).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TracksArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .HasConstraintName("_tracksartists__artists_fk"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .HasConstraintName("_tracksartists__tracks_fk"),
                    j =>
                    {
                        j.HasKey("TrackId", "ArtistId").HasName("_tracksartists__pk");
                        j.ToTable("TracksArtists");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TracksGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("_tracksgenres__genres_fk"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .HasConstraintName("_tracksgenres__tracks_fk"),
                    j =>
                    {
                        j.HasKey("TrackId", "GenreId").HasName("_tracksgenres__pk");
                        j.ToTable("TracksGenres");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_users__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.LastLoginDateTime).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_userroles_fk");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_subscriptions_fk");

            entity.HasMany(d => d.PlaylistsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersPlaylist",
                    r => r.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .HasConstraintName("_usersplaylists__playlists_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("_usersplaylists__users_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "PlaylistId").HasName("_usersplaylists__pk");
                        j.ToTable("UsersPlaylists");
                    });
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("_userroles__pk");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
