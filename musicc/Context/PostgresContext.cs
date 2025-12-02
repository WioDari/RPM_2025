using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using musicc.Models;

namespace musicc.Context;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Gener> Geners { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Fliner");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("album_pkey");

            entity.ToTable("album", "music");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AlbumTitle)
                .HasMaxLength(255)
                .HasColumnName("album_title");
            entity.Property(e => e.Coverpath)
                .HasMaxLength(255)
                .HasColumnName("coverpath");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Realease).HasColumnName("realease");

            entity.HasMany(d => d.Tracks).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("album_track_track_id_fkey"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("album_track_album_id_fkey"),
                    j =>
                    {
                        j.HasKey("AlbumId", "TrackId").HasName("album_track_pkey");
                        j.ToTable("album_track", "music");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("album_id");
                        j.IndexerProperty<int>("TrackId").HasColumnName("track_id");
                    });
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("artist_pkey");

            entity.ToTable("artist", "music");

            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.ArtName)
                .HasMaxLength(255)
                .HasColumnName("art_name");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .HasColumnName("photo");
            entity.Property(e => e.YearActive)
                .HasMaxLength(255)
                .HasColumnName("year_active");

            entity.HasMany(d => d.Albums).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("artist_album_album_id_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("artist_album_artist_id_fkey"),
                    j =>
                    {
                        j.HasKey("ArtistId", "AlbumId").HasName("artist_album_pkey");
                        j.ToTable("artist_album", "music");
                        j.IndexerProperty<int>("ArtistId").HasColumnName("artist_id");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("album_id");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "TrackArtist",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("track_artist_track_id_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("track_artist_artist_id_fkey"),
                    j =>
                    {
                        j.HasKey("ArtistId", "TrackId").HasName("track_artist_pkey");
                        j.ToTable("track_artist", "music");
                        j.IndexerProperty<int>("ArtistId").HasColumnName("artist_id");
                        j.IndexerProperty<int>("TrackId").HasColumnName("track_id");
                    });
        });

        modelBuilder.Entity<Gener>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("geners_pkey");

            entity.ToTable("geners", "music");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Geners)
                .HasMaxLength(255)
                .HasColumnName("geners");

            entity.HasMany(d => d.Albums).WithMany(p => p.Geners)
                .UsingEntity<Dictionary<string, object>>(
                    "GenerAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gener_album_album_id_fkey"),
                    l => l.HasOne<Gener>().WithMany()
                        .HasForeignKey("GenerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gener_album_gener_id_fkey"),
                    j =>
                    {
                        j.HasKey("GenerId", "AlbumId").HasName("gener_album_pkey");
                        j.ToTable("gener_album", "music");
                        j.IndexerProperty<int>("GenerId").HasColumnName("gener_id");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("album_id");
                    });
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("playlist_pkey");

            entity.ToTable("playlist", "music");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.DateCreate).HasColumnName("date_create");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.PlayName)
                .HasMaxLength(255)
                .HasColumnName("play_name");

            entity.HasMany(d => d.IdTracks).WithMany(p => p.IdPlaylists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("IdTrack")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("playlist_track_id_track_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("IdPlaylist")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("playlist_track_id_playlist_fkey"),
                    j =>
                    {
                        j.HasKey("IdPlaylist", "IdTrack").HasName("playlist_track_pkey");
                        j.ToTable("playlist_track", "music");
                        j.IndexerProperty<int>("IdPlaylist").HasColumnName("id_playlist");
                        j.IndexerProperty<int>("IdTrack").HasColumnName("id_track");
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "TagsPlaylist",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tags_playlist_tags_id_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("tags_playlist_playlist_id_fkey"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TagsId").HasName("tags_playlist_pkey");
                        j.ToTable("tags_playlist", "music");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("playlist_id");
                        j.IndexerProperty<int>("TagsId").HasColumnName("tags_id");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role", "music");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Role1)
                .HasMaxLength(255)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagsId).HasName("tags_pkey");

            entity.ToTable("tags", "music");

            entity.Property(e => e.TagsId).HasColumnName("tags_id");
            entity.Property(e => e.Tag1)
                .HasMaxLength(255)
                .HasColumnName("tag");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("track_pkey");

            entity.ToTable("track", "music");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bitrate).HasColumnName("bitrate");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Filepath)
                .HasMaxLength(255)
                .HasColumnName("filepath");
            entity.Property(e => e.PlayCount).HasColumnName("play_count");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Realise).HasColumnName("realise");
            entity.Property(e => e.TrackName)
                .HasColumnType("character varying")
                .HasColumnName("track_name");

            entity.HasMany(d => d.Geners).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "GenerTrack",
                    r => r.HasOne<Gener>().WithMany()
                        .HasForeignKey("GenerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gener_track_gener_id_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gener_track_track_id_fkey"),
                    j =>
                    {
                        j.HasKey("TrackId", "GenerId").HasName("gener_track_pkey");
                        j.ToTable("gener_track", "music");
                        j.IndexerProperty<int>("TrackId").HasColumnName("track_id");
                        j.IndexerProperty<int>("GenerId").HasColumnName("gener_id");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user", "music");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.LastLog).HasColumnName("last_log");
            entity.Property(e => e.Registration).HasColumnName("registration");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Ssubsription).HasColumnName("ssubsription");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(255)
                .HasColumnName("user_login");
            entity.Property(e => e.UserPass)
                .HasMaxLength(255)
                .HasColumnName("user_pass");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_fkey");

            entity.HasMany(d => d.Playlists).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPlaylist",
                    r => r.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_playlist_playlist_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_playlist_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "PlaylistId").HasName("user_playlist_pkey");
                        j.ToTable("user_playlist", "music");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("playlist_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
