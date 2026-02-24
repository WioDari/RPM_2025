using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PracticeWork.Models;

namespace PracticeWork.Context;

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

    public virtual DbSet<AlbumGenre> AlbumGenres { get; set; }

    public virtual DbSet<AlbumTrack> AlbumTracks { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<ArtistGenre> ArtistGenres { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<PlayList> PlayLists { get; set; }

    public virtual DbSet<PlayistTag> PlayistTags { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<TrackArtist> TrackArtists { get; set; }

    public virtual DbSet<TrackGenre> TrackGenres { get; set; }

    public virtual DbSet<TrackPlaylist> TrackPlaylists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPlaylist> UserPlaylists { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5437;Database=postgres;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("Albums_pkey");

            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.AlbumName).HasColumnName("albumName");
            entity.Property(e => e.Artist).HasColumnName("artist");
            entity.Property(e => e.CoverPath).HasColumnName("coverPath");
            entity.Property(e => e.ReleaseYear).HasColumnName("releaseYear");
            entity.Property(e => e.TotalDuration).HasColumnName("totalDuration");

            entity.HasOne(d => d.ArtistNavigation).WithMany(p => p.Albums)
                .HasForeignKey(d => d.Artist)
                .HasConstraintName("Albums_artist_fkey");
        });

        modelBuilder.Entity<AlbumGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("albumGenres");

            entity.Property(e => e.Agenre).HasColumnName("AGenre");

            entity.HasOne(d => d.AgenreNavigation).WithMany()
                .HasForeignKey(d => d.Agenre)
                .HasConstraintName("albumGenres_AGenre_fkey");

            entity.HasOne(d => d.AlbumNavigation).WithMany()
                .HasForeignKey(d => d.Album)
                .HasConstraintName("albumGenres_Album_fkey");
        });

        modelBuilder.Entity<AlbumTrack>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("albumTracks");

            entity.Property(e => e.Atrack).HasColumnName("ATrack");
            entity.Property(e => e.Talbum).HasColumnName("TAlbum");

            entity.HasOne(d => d.AtrackNavigation).WithMany()
                .HasForeignKey(d => d.Atrack)
                .HasConstraintName("albumTracks_ATrack_fkey");

            entity.HasOne(d => d.TalbumNavigation).WithMany()
                .HasForeignKey(d => d.Talbum)
                .HasConstraintName("albumTracks_TAlbum_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("Artists_pkey");

            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.ActiveYears).HasColumnName("activeYears");
            entity.Property(e => e.ArtistCountry).HasColumnName("artistCountry");
            entity.Property(e => e.ArtistDescription).HasColumnName("artistDescription");
            entity.Property(e => e.ArtistName).HasColumnName("artistName");
            entity.Property(e => e.PhotoPath).HasColumnName("photoPath");

            entity.HasOne(d => d.ArtistCountryNavigation).WithMany(p => p.Artists)
                .HasForeignKey(d => d.ArtistCountry)
                .HasConstraintName("Artists_artistCountry_fkey");
        });

        modelBuilder.Entity<ArtistGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("artistGenre");

            entity.Property(e => e.Agenre).HasColumnName("AGenre");

            entity.HasOne(d => d.AgenreNavigation).WithMany()
                .HasForeignKey(d => d.Agenre)
                .HasConstraintName("artistGenre_AGenre_fkey");

            entity.HasOne(d => d.ArtistNavigation).WithMany()
                .HasForeignKey(d => d.Artist)
                .HasConstraintName("artistGenre_Artist_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName).HasColumnName("countryName");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Genres_pkey");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.GenreName).HasColumnName("genreName");
        });

        modelBuilder.Entity<PlayList>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("playLists_pkey");

            entity.ToTable("playLists");

            entity.HasIndex(e => e.PlaylistName, "playLists_playlistName_key").IsUnique();

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.PlaylistDescription).HasColumnName("playlistDescription");
            entity.Property(e => e.PlaylistName).HasColumnName("playlistName");
            entity.Property(e => e.UserCreator).HasColumnName("userCreator");

            entity.HasOne(d => d.UserCreatorNavigation).WithMany(p => p.PlayLists)
                .HasForeignKey(d => d.UserCreator)
                .HasConstraintName("playLists_userCreator_fkey");
        });

        modelBuilder.Entity<PlayistTag>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("playistTags");

            entity.Property(e => e.PlayList).HasColumnName("playList");

            entity.HasOne(d => d.PlayListNavigation).WithMany()
                .HasForeignKey(d => d.PlayList)
                .HasConstraintName("playistTags_playList_fkey");

            entity.HasOne(d => d.TagNavigation).WithMany()
                .HasForeignKey(d => d.Tag)
                .HasConstraintName("playistTags_Tag_fkey");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("subscriptions_pkey");

            entity.ToTable("subscriptions");

            entity.Property(e => e.SubId).HasColumnName("sub_id");
            entity.Property(e => e.SubName).HasColumnName("sub_name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tags_pkey");

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TagName).HasColumnName("tagName");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("Tracks_pkey");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.AlbumcoverPath).HasColumnName("albumcoverPath");
            entity.Property(e => e.Artist).HasColumnName("artist");
            entity.Property(e => e.Bitrate).HasColumnName("bitrate");
            entity.Property(e => e.Info).HasColumnName("info");
            entity.Property(e => e.ReleaseDate).HasColumnName("releaseDate");
            entity.Property(e => e.TotalPlays)
                .HasDefaultValue(0)
                .HasColumnName("totalPlays");
            entity.Property(e => e.TrackDuration).HasColumnName("trackDuration");
            entity.Property(e => e.TrackName).HasColumnName("trackName");
            entity.Property(e => e.TrackPath).HasColumnName("trackPath");
            entity.Property(e => e.TrackRating)
                .HasPrecision(3, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("trackRating");

            entity.HasOne(d => d.ArtistNavigation).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.Artist)
                .HasConstraintName("Tracks_artist_fkey");
        });

        modelBuilder.Entity<TrackArtist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("trackArtists");

            entity.Property(e => e.RelatedArtist).HasColumnName("relatedArtist");
            entity.Property(e => e.RelatedTrack).HasColumnName("relatedTrack");

            entity.HasOne(d => d.RelatedArtistNavigation).WithMany()
                .HasForeignKey(d => d.RelatedArtist)
                .HasConstraintName("trackArtists_relatedArtist_fkey");

            entity.HasOne(d => d.RelatedTrackNavigation).WithMany()
                .HasForeignKey(d => d.RelatedTrack)
                .HasConstraintName("trackArtists_relatedTrack_fkey");
        });

        modelBuilder.Entity<TrackGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("trackGenres");

            entity.Property(e => e.Tgenre).HasColumnName("TGenre");

            entity.HasOne(d => d.TgenreNavigation).WithMany()
                .HasForeignKey(d => d.Tgenre)
                .HasConstraintName("trackGenres_TGenre_fkey");

            entity.HasOne(d => d.TrackNavigation).WithMany()
                .HasForeignKey(d => d.Track)
                .HasConstraintName("trackGenres_Track_fkey");
        });

        modelBuilder.Entity<TrackPlaylist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("trackPlaylists");

            entity.Property(e => e.Playlist).HasColumnName("playlist");
            entity.Property(e => e.Track).HasColumnName("track");

            entity.HasOne(d => d.PlaylistNavigation).WithMany()
                .HasForeignKey(d => d.Playlist)
                .HasConstraintName("trackPlaylists_playlist_fkey");

            entity.HasOne(d => d.TrackNavigation).WithMany()
                .HasForeignKey(d => d.Track)
                .HasConstraintName("trackPlaylists_track_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Fio).HasColumnName("fio");
            entity.Property(e => e.LastLogin).HasColumnName("lastLogin");
            entity.Property(e => e.RegistrationDate).HasColumnName("registrationDate");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.SubscriptionType).HasColumnName("subscriptionType");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .HasColumnName("userEMail");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .HasColumnName("userLogin");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(10)
                .HasColumnName("userPassword");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("users_role_fkey");

            entity.HasOne(d => d.SubscriptionTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionType)
                .HasConstraintName("users_subscriptionType_fkey");
        });

        modelBuilder.Entity<UserPlaylist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("userPlaylists");

            entity.Property(e => e.Uplaylist).HasColumnName("UPlaylist");

            entity.HasOne(d => d.UplaylistNavigation).WithMany()
                .HasForeignKey(d => d.Uplaylist)
                .HasConstraintName("userPlaylists_UPlaylist_fkey");

            entity.HasOne(d => d.UserNavigation).WithMany()
                .HasForeignKey(d => d.User)
                .HasConstraintName("userPlaylists_User_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("userRoles_pkey");

            entity.ToTable("userRoles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
