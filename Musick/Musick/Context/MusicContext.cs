using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Musick.Models;

namespace Musick.Context;

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

    public virtual DbSet<AlbumsTreck> AlbumsTrecks { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Asdfg> Asdfgs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Music;Username=postgres;Password=lil801mm");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("Albums_pkey");

            entity.Property(e => e.AlbumId).HasColumnName("AlbumID");
            entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Albums_ArtistID_fkey");

            entity.HasMany(d => d.Genres).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumsGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AlbumsGenres_GenresID_fkey"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AlbumsGenres_AlbumID_fkey"),
                    j =>
                    {
                        j.HasKey("AlbumId", "GenresId").HasName("albumsgenres_pk");
                        j.ToTable("AlbumsGenres");
                        j.IndexerProperty<int>("AlbumId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("AlbumID");
                        j.IndexerProperty<int>("GenresId").HasColumnName("GenresID");
                    });
        });

        modelBuilder.Entity<AlbumsTreck>(entity =>
        {
            entity.HasKey(e => new { e.AlbumId, e.TrackId }).HasName("AlbumsTrecks_pkey");

            entity.HasIndex(e => e.AlbumId, "AlbumsTrecks_AlbumID_key").IsUnique();

            entity.Property(e => e.AlbumId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AlbumID");
            entity.Property(e => e.TrackId).HasColumnName("TrackID");

            entity.HasOne(d => d.Album).WithOne(p => p.AlbumsTreck)
                .HasForeignKey<AlbumsTreck>(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumsTrecks_AlbumID_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.AlbumsTrecks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumsTrecks_TrackID_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistsId).HasName("Artists_pkey");

            entity.Property(e => e.ArtistsId).HasColumnName("ArtistsID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");

            entity.HasOne(d => d.Country).WithMany(p => p.Artists)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("Artists_CountryID_fkey");

            entity.HasMany(d => d.Genres).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistsGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ArtistsGenres_GenresID_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ArtistsGenres_ArtistID_fkey"),
                    j =>
                    {
                        j.HasKey("ArtistId", "GenresId").HasName("artistsgenres_pk");
                        j.ToTable("ArtistsGenres");
                        j.IndexerProperty<int>("ArtistId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("ArtistID");
                        j.IndexerProperty<int>("GenresId").HasColumnName("GenresID");
                    });
        });

        modelBuilder.Entity<Asdfg>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("asdfg");

            entity.Property(e => e.Column1)
                .HasColumnType("character varying")
                .HasColumnName("column1");
            entity.Property(e => e.Column2)
                .HasColumnType("character varying")
                .HasColumnName("column2");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("Countries_pkey");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenresId).HasName("Genres_pkey");

            entity.Property(e => e.GenresId).HasColumnName("GenresID");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("Playlists_pkey");

            entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");

            entity.HasMany(d => d.Tags).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylstTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylstTags_TagId_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylstTags_PlaylistID_fkey"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TagId").HasName("playlsttags_pk");
                        j.ToTable("PlaylstTags");
                        j.IndexerProperty<int>("PlaylistId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("PlaylistID");
                    });
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Rols_pkey");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("Subscriptions_pkey");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tags_pkey");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("Tracks_pkey");

            entity.Property(e => e.TrackId).HasColumnName("TrackID");

            entity.HasMany(d => d.Artists).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TracksArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TracksArtists_ArtistID_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TracksArtists_TrackID_fkey"),
                    j =>
                    {
                        j.HasKey("TrackId", "ArtistId").HasName("tracksartists_pk");
                        j.ToTable("TracksArtists");
                        j.IndexerProperty<int>("TrackId").HasColumnName("TrackID");
                        j.IndexerProperty<int>("ArtistId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("ArtistID");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TracksGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TracksGenres_GenresID_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TracksGenres_TrackID_fkey"),
                    j =>
                    {
                        j.HasKey("TrackId", "GenresId").HasName("tracksgenres_pk");
                        j.ToTable("TracksGenres");
                        j.IndexerProperty<int>("TrackId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("TrackID");
                        j.IndexerProperty<int>("GenresId").HasColumnName("GenresID");
                    });

            entity.HasMany(d => d.Playlists).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TracksPlaylist",
                    r => r.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TracksPlaylists_PlaylistID_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TracksPlaylists_TrackID_fkey"),
                    j =>
                    {
                        j.HasKey("TrackId", "PlaylistId").HasName("tracksplaylists_pk");
                        j.ToTable("TracksPlaylists");
                        j.IndexerProperty<int>("TrackId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("TrackID");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistID");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.IsBan).HasColumnName("IsBan?");
            entity.Property(e => e.LastLogin).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_RoleID_fkey");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_SubscriptionID_fkey");

            entity.HasMany(d => d.Playlists).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersPlaylist",
                    r => r.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usersplaylists_playlists_fk"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usersplaylists_users_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "PlaylistId").HasName("usersplaylists_pk");
                        j.ToTable("UsersPlaylists");
                        j.IndexerProperty<int>("UserId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("UserID");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
