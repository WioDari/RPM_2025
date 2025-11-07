using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Spotify_wpf.Models;

namespace Spotify_wpf.Context;

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

    public virtual DbSet<AlbumGenre> AlbumGenres { get; set; }

    public virtual DbSet<AlbumTrack> AlbumTracks { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<GenreArtist> GenreArtists { get; set; }

    public virtual DbSet<GenreTrack> GenreTracks { get; set; }

    public virtual DbSet<PlayListTrack> PlayListTracks { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistUser> PlaylistUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagsPlaylist> TagsPlaylists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=music;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("Album_pkey");

            entity.ToTable("Album");

            entity.Property(e => e.AlbumId).HasColumnName("Album_Id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("album_artist_fk");
        });

        modelBuilder.Entity<AlbumGenre>(entity =>
        {
            entity.HasKey(e => e.AlbumGenreId).HasName("Album_Genre_pkey");

            entity.ToTable("Album_Genre");

            entity.Property(e => e.AlbumGenreId).HasColumnName("Album_Genre_Id");
            entity.Property(e => e.AlbumId).HasColumnName("Album_Id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

            entity.HasOne(d => d.Album).WithMany(p => p.AlbumGenres)
                .HasForeignKey(d => d.AlbumId)
                .HasConstraintName("Album_Genre_Album_Id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.AlbumGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Album_Genre_Genre_Id_fkey");
        });

        modelBuilder.Entity<AlbumTrack>(entity =>
        {
            entity.HasKey(e => e.AlbumTrackId).HasName("Album_Track_pkey");

            entity.ToTable("Album_Track");

            entity.Property(e => e.AlbumTrackId).HasColumnName("Album_Track_Id");
            entity.Property(e => e.AlbumId).HasColumnName("Album_Id");
            entity.Property(e => e.TrackId).HasColumnName("Track_Id");

            entity.HasOne(d => d.Album).WithMany(p => p.AlbumTracks)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Album_Track_Album_Id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.AlbumTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Album_Track_Track_Id_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("Artist_pkey");

            entity.ToTable("Artist");

            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");
            entity.Property(e => e.ArtistName).HasColumnName("Artist_Name");
            entity.Property(e => e.YearsActivity).HasColumnName("Years_Activity");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Genre_pkey");

            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("Genre_Id");
            entity.Property(e => e.GenreName).HasColumnName("Genre_Name");
        });

        modelBuilder.Entity<GenreArtist>(entity =>
        {
            entity.HasKey(e => e.GenreArtistId).HasName("AlbumArtist_pkey");

            entity.ToTable("Genre_Artist");

            entity.Property(e => e.GenreArtistId)
                .HasDefaultValueSql("nextval('\"AlbumArtist_AlbumArtist_Id_seq\"'::regclass)")
                .HasColumnName("GenreArtist_Id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

            entity.HasOne(d => d.Artist).WithMany(p => p.GenreArtists)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumArtist_Artist_Id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreArtists)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("genre_artist_genre_fk");
        });

        modelBuilder.Entity<GenreTrack>(entity =>
        {
            entity.HasKey(e => e.GenreTrackId).HasName("Genre_Track_pkey");

            entity.ToTable("Genre_Track");

            entity.Property(e => e.GenreTrackId).HasColumnName("Genre_Track_Id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_Id");
            entity.Property(e => e.TrackId).HasColumnName("Track_Id");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreTracks)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("Genre_Track_Genre_Id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.GenreTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Genre_Track_Track_Id_fkey");
        });

        modelBuilder.Entity<PlayListTrack>(entity =>
        {
            entity.HasKey(e => e.PlayListTrackId).HasName("PlayList_Track_pkey");

            entity.ToTable("PlayList_Track");

            entity.Property(e => e.PlayListTrackId).HasColumnName("PlayList_Track_id");
            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_Id");
            entity.Property(e => e.TrackId).HasColumnName("Track_Id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.PlayListTracks)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayList_Track_Playlist_Id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.PlayListTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayList_Track_Track_Id_fkey");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlayListId).HasName("Playlists_pkey");

            entity.Property(e => e.PlayListId).HasColumnName("PlayList_Id");
            entity.Property(e => e.PlaylistName).HasColumnName("Playlist_Name");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Playlists_User_Id_fkey");
        });

        modelBuilder.Entity<PlaylistUser>(entity =>
        {
            entity.HasKey(e => e.PlaylistUserId).HasName("Playlist_User_pkey");

            entity.ToTable("Playlist_User");

            entity.Property(e => e.PlaylistUserId).HasColumnName("Playlist_User_Id");
            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.PlaylistUsers)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Playlist_User_Playlist_Id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.PlaylistUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Playlist_User_User_Id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.Role1).HasColumnName("Role");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("Subscription_pkey");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionId).HasColumnName("Subscription_Id");
            entity.Property(e => e.Subscription1).HasColumnName("Subscription");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagsId).HasName("Tags_pkey");

            entity.Property(e => e.TagsId).HasColumnName("Tags_Id");
            entity.Property(e => e.TagsName).HasColumnName("Tags_Name");
        });

        modelBuilder.Entity<TagsPlaylist>(entity =>
        {
            entity.HasKey(e => e.TagsPlaylistId).HasName("Tags_Playlist_pkey");

            entity.ToTable("Tags_Playlist");

            entity.Property(e => e.TagsPlaylistId).HasColumnName("Tags_Playlist_Id");
            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_Id");
            entity.Property(e => e.TagsId).HasColumnName("Tags_Id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.TagsPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tags_Playlist_Playlist_Id_fkey");

            entity.HasOne(d => d.Tags).WithMany(p => p.TagsPlaylists)
                .HasForeignKey(d => d.TagsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tags_Playlist_Tags_Id_fkey");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("Track_pkey");

            entity.ToTable("Track");

            entity.Property(e => e.TrackId).HasColumnName("Track_Id");
            entity.Property(e => e.AlbumId).HasColumnName("Album_Id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .HasConstraintName("track_album_fk");

            entity.HasOne(d => d.Artist).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("track_artist_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.SubscriptionId).HasColumnName("Subscription_Id");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(255)
                .HasColumnName("User_Login");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .HasColumnName("User_Password");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Role_Id_fkey");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Subscription_Id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
