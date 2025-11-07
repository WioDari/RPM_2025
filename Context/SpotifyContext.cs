using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Models;

namespace SpotApp_wpf.Context;

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

    public virtual DbSet<ArtistsInTrack> ArtistsInTracks { get; set; }

    public virtual DbSet<GenersInTrack> GenersInTracks { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<GenresInAlbum> GenresInAlbums { get; set; }

    public virtual DbSet<GenresInArtist> GenresInArtists { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagsInPlaylist> TagsInPlaylists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<TracksInAlbum> TracksInAlbums { get; set; }

    public virtual DbSet<TracksInPlaylist> TracksInPlaylists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersPlaylist> UsersPlaylists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=26.192.39.184;Port=5434;Database=spotify;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("Album_pkey");

            entity.ToTable("Album");

            entity.Property(e => e.AlbumId).HasColumnName("Album_id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("Album_Artist_id_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("Artists_pkey");

            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
        });

        modelBuilder.Entity<ArtistsInTrack>(entity =>
        {
            entity.HasKey(e => e.AiTId).HasName("Artists_in_Tracks_pkey");

            entity.ToTable("Artists_in_Tracks");

            entity.Property(e => e.AiTId).HasColumnName("AiT_id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
            entity.Property(e => e.TrackId).HasColumnName("Track_id");

            entity.HasOne(d => d.Artist).WithMany(p => p.ArtistsInTracks)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Artists_in_Tracks_Artist_id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.ArtistsInTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Artists_in_Tracks_Track_id_fkey");
        });

        modelBuilder.Entity<GenersInTrack>(entity =>
        {
            entity.HasKey(e => e.GiTId).HasName("Geners_in_Tracks_pkey");

            entity.ToTable("Geners_in_Tracks");

            entity.Property(e => e.GiTId).HasColumnName("GiT_id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_id");
            entity.Property(e => e.TrackId).HasColumnName("Track_id");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenersInTracks)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Geners_in_Tracks_Genre_id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.GenersInTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Geners_in_Tracks_Track_id_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Genres_pkey");

            entity.Property(e => e.GenreId).HasColumnName("Genre_id");
            entity.Property(e => e.GenreTittle)
                .HasMaxLength(255)
                .HasColumnName("Genre_tittle");
        });

        modelBuilder.Entity<GenresInAlbum>(entity =>
        {
            entity.HasKey(e => e.GiAlId).HasName("Genres_in_Albums_pkey");

            entity.ToTable("Genres_in_Albums");

            entity.Property(e => e.GiAlId).HasColumnName("GiAl_id");
            entity.Property(e => e.AlbumId).HasColumnName("Album_id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_id");

            entity.HasOne(d => d.Album).WithMany(p => p.GenresInAlbums)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Genres_in_Albums_Album_id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenresInAlbums)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Genres_in_Albums_Genre_id_fkey");
        });

        modelBuilder.Entity<GenresInArtist>(entity =>
        {
            entity.HasKey(e => e.GiAId).HasName("Genres_in_Artists_pkey");

            entity.ToTable("Genres_in_Artists");

            entity.Property(e => e.GiAId).HasColumnName("GiA_id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_id");

            entity.HasOne(d => d.Artist).WithMany(p => p.GenresInArtists)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("Genres_in_Artists_Artist_id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenresInArtists)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("Genres_in_Artists_Genre_id_fkey");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("Playlists_pkey");

            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_id");
            entity.Property(e => e.DateCreated).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PlaylistName).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Playlists_User_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.RoleName).HasColumnName("Role_name");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("Subscription_pkey");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionId).HasColumnName("Subscription_id");
            entity.Property(e => e.SubscriptionTittle).HasColumnName("Subscription_tittle");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tags_pkey");

            entity.Property(e => e.TagId).HasColumnName("Tag_id");
            entity.Property(e => e.TagTittle).HasColumnName("Tag_tittle");
        });

        modelBuilder.Entity<TagsInPlaylist>(entity =>
        {
            entity.HasKey(e => e.TiPId).HasName("Tags_in_Playlists_pkey");

            entity.ToTable("Tags_in_Playlists");

            entity.Property(e => e.TiPId).HasColumnName("TiP_id");
            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_id");
            entity.Property(e => e.TagId).HasColumnName("Tag_id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.TagsInPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("Tags_in_Playlists_Playlist_id_fkey");

            entity.HasOne(d => d.Tag).WithMany(p => p.TagsInPlaylists)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tags_in_Playlists_Tag_id_fkey");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("Track_pkey");

            entity.ToTable("Track");

            entity.Property(e => e.TrackId).HasColumnName("Track_id");
            entity.Property(e => e.AlbumId).HasColumnName("Album_id");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .HasConstraintName("Track_Album_id_fkey");
        });

        modelBuilder.Entity<TracksInAlbum>(entity =>
        {
            entity.HasKey(e => e.TiAId).HasName("Tracks_in_Albums_pkey");

            entity.ToTable("Tracks_in_Albums");

            entity.Property(e => e.TiAId).HasColumnName("TiA_id");
            entity.Property(e => e.AlbumId).HasColumnName("Album_id");
            entity.Property(e => e.TrackId).HasColumnName("Track_id");

            entity.HasOne(d => d.Album).WithMany(p => p.TracksInAlbums)
                .HasForeignKey(d => d.AlbumId)
                .HasConstraintName("Tracks_in_Albums_Album_id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.TracksInAlbums)
                .HasForeignKey(d => d.TrackId)
                .HasConstraintName("Tracks_in_Albums_Track_id_fkey");
        });

        modelBuilder.Entity<TracksInPlaylist>(entity =>
        {
            entity.HasKey(e => e.TiPId).HasName("Tracks_in_Playlists_pkey");

            entity.ToTable("Tracks_in_Playlists");

            entity.Property(e => e.TiPId).HasColumnName("TiP_id");
            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_id");
            entity.Property(e => e.TrackId).HasColumnName("Track_id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.TracksInPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tracks_in_Playlists_Playlist_id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.TracksInPlaylists)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tracks_in_Playlists_Track_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.LastLogin).HasColumnType("timestamp without time zone");
            entity.Property(e => e.RegistrationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.SubscriptionId).HasColumnName("Subscription_id");
            entity.Property(e => e.UserLogin).HasMaxLength(255);
            entity.Property(e => e.UserPassword).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Role_id_fkey");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_Subscription_id_fkey");
        });

        modelBuilder.Entity<UsersPlaylist>(entity =>
        {
            entity.HasKey(e => e.UserPlaylistId).HasName("Users_Playlists_pkey");

            entity.ToTable("Users_Playlists");

            entity.Property(e => e.UserPlaylistId).HasColumnName("UserPlaylist_id");
            entity.Property(e => e.PlaylistId).HasColumnName("Playlist_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Playlist).WithMany(p => p.UsersPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Playlists_Playlist_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UsersPlaylists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Playlists_User_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
