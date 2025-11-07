using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicWpf.Models;

namespace MusicWpf.Context;

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

    public virtual DbSet<ArtistTrack> ArtistTracks { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<GenreArtist> GenreArtists { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistTag> PlaylistTags { get; set; }

    public virtual DbSet<PlaylistsUser> PlaylistsUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<TracksGenre> TracksGenres { get; set; }

    public virtual DbSet<TracksPlaylist> TracksPlaylists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Music;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("Album_pkey");

            entity.ToTable("Album");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("Album_ArtistId_fkey");
        });

        modelBuilder.Entity<AlbumGenre>(entity =>
        {
            entity.HasKey(e => e.AlbumGenresId).HasName("AlbumGenres_pkey");

            entity.HasOne(d => d.Album).WithMany(p => p.AlbumGenres)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumGenres_AlbumId_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.AlbumGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumGenres_GenreId_fkey");
        });

        modelBuilder.Entity<AlbumTrack>(entity =>
        {
            entity.HasKey(e => e.AlbumTracksId).HasName("AlbumTracks_pkey");

            entity.HasOne(d => d.Album).WithMany(p => p.AlbumTracks)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumTracks_AlbumId_fkey");

            entity.HasOne(d => d.Tracks).WithMany(p => p.AlbumTracks)
                .HasForeignKey(d => d.TracksId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AlbumTracks_TracksId_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("Artist_pkey");

            entity.ToTable("Artist");
        });

        modelBuilder.Entity<ArtistTrack>(entity =>
        {
            entity.HasKey(e => e.ArtistTracksId).HasName("ArtistTracks_pkey");

            entity.HasOne(d => d.Artist).WithMany(p => p.ArtistTracks)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ArtistTracks_ArtistId_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.ArtistTracks)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ArtistTracks_TrackId_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Genre_pkey");

            entity.ToTable("Genre");
        });

        modelBuilder.Entity<GenreArtist>(entity =>
        {
            entity.HasKey(e => e.GenreArtistId).HasName("GenreArtist_pkey");

            entity.ToTable("GenreArtist");

            entity.HasOne(d => d.Artist).WithMany(p => p.GenreArtists)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GenreArtist_ArtistId_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreArtists)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GenreArtist_GenreId_fkey");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("Playlist_pkey");

            entity.ToTable("Playlist");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Playlist_UserId_fkey");
        });

        modelBuilder.Entity<PlaylistTag>(entity =>
        {
            entity.HasKey(e => e.PlaylistTagsId).HasName("PlaylistTags_pkey");

            entity.HasOne(d => d.Playlist).WithMany(p => p.PlaylistTags)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlaylistTags_PlaylistId_fkey");

            entity.HasOne(d => d.Tags).WithMany(p => p.PlaylistTags)
                .HasForeignKey(d => d.TagsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlaylistTags_TagsId_fkey");
        });

        modelBuilder.Entity<PlaylistsUser>(entity =>
        {
            entity.HasKey(e => e.PlaylistsUserId).HasName("PlaylistsUser_pkey");

            entity.ToTable("PlaylistsUser");

            entity.HasOne(d => d.Playlists).WithMany(p => p.PlaylistsUsers)
                .HasForeignKey(d => d.PlaylistsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlaylistsUser_PlaylistsId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.PlaylistsUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlaylistsUser_UserId_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Role_pkey");

            entity.ToTable("Role");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("Subscription_pkey");

            entity.ToTable("Subscription");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagsId).HasName("Tags_pkey");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TracksId).HasName("Tracks_pkey");
        });

        modelBuilder.Entity<TracksGenre>(entity =>
        {
            entity.HasKey(e => e.TracksGenresId).HasName("TracksGenres_pkey");

            entity.HasOne(d => d.Genres).WithMany(p => p.TracksGenres)
                .HasForeignKey(d => d.GenresId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TracksGenres_GenresId_fkey");

            entity.HasOne(d => d.Tracks).WithMany(p => p.TracksGenres)
                .HasForeignKey(d => d.TracksId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TracksGenres_TracksId_fkey");
        });

        modelBuilder.Entity<TracksPlaylist>(entity =>
        {
            entity.HasKey(e => e.TracksPlaylistId).HasName("TracksPlaylist_pkey");

            entity.ToTable("TracksPlaylist");

            entity.Property(e => e.TracksPlaylistId).ValueGeneratedNever();

            entity.HasOne(d => d.Playlist).WithMany(p => p.TracksPlaylists)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TracksPlaylist_PlaylistId_fkey");

            entity.HasOne(d => d.Tracks).WithMany(p => p.TracksPlaylists)
                .HasForeignKey(d => d.TracksId)
                .HasConstraintName("TracksPlaylist_TracksId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.UserPassword).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_RoleId_fkey");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_SubscriptionId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
