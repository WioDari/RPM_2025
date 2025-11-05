using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PraktikaMusicF.Models;

namespace PraktikaMusicF.Context;

public partial class MusicBdFContext : DbContext
{
    public MusicBdFContext()
    {
    }

    public MusicBdFContext(DbContextOptions<MusicBdFContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<AlbumsGenre> AlbumsGenres { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<ArtistGenre> ArtistGenres { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistCreator> PlaylistCreators { get; set; }

    public virtual DbSet<PlaylistTrack> PlaylistTracks { get; set; }

    public virtual DbSet<PlaylistsTag> PlaylistsTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<TrackArtist> TrackArtists { get; set; }

    public virtual DbSet<TrackList> TrackLists { get; set; }

    public virtual DbSet<TracksGenre> TracksGenres { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=MusicBdF; Username=postgres; Password=SKALrawcn12s3");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbId).HasName("Albums_pkey");

            entity.Property(e => e.AlbId).HasColumnName("alb_id");
            entity.Property(e => e.AlbImage).HasColumnName("alb_image");
            entity.Property(e => e.AlbName)
                .HasColumnType("character varying")
                .HasColumnName("alb_name");
            entity.Property(e => e.AlbRelease).HasColumnName("alb_release");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("albums_artist_fk");
        });

        modelBuilder.Entity<AlbumsGenre>(entity =>
        {
            entity.HasKey(e => e.AlbGenresId).HasName("AlbumsGenres_pkey");

            entity.Property(e => e.AlbGenresId).HasColumnName("alb_genres_id");
            entity.Property(e => e.AlbId).HasColumnName("alb_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            entity.HasOne(d => d.Alb).WithMany(p => p.AlbumsGenres)
                .HasForeignKey(d => d.AlbId)
                .HasConstraintName("albumsgenres_albums_fk");

            entity.HasOne(d => d.Genre).WithMany(p => p.AlbumsGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("albumsgenres_genres_fk");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtId).HasName("Artist_pkey");

            entity.ToTable("Artist");

            entity.Property(e => e.ArtId).HasColumnName("art_id");
            entity.Property(e => e.ArtDesc).HasColumnName("art_desc");
            entity.Property(e => e.ArtName)
                .HasColumnType("character varying")
                .HasColumnName("art_name");
            entity.Property(e => e.ArtPhotoPath).HasColumnName("art_photo_path");
            entity.Property(e => e.YearActive).HasColumnName("year_active");
        });

        modelBuilder.Entity<ArtistGenre>(entity =>
        {
            entity.HasKey(e => e.ArtGenreId).HasName("ArtistGenres_pkey");

            entity.Property(e => e.ArtGenreId).HasColumnName("art_genre_id");
            entity.Property(e => e.ArtId).HasColumnName("art_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            entity.HasOne(d => d.Art).WithMany(p => p.ArtistGenres)
                .HasForeignKey(d => d.ArtId)
                .HasConstraintName("artistgenres_artist_fk");

            entity.HasOne(d => d.Genre).WithMany(p => p.ArtistGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("artistgenres_genres_fk");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("Genres_pkey");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.GenresName)
                .HasColumnType("character varying")
                .HasColumnName("genres_name");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlId).HasName("Playlist_pkey");

            entity.ToTable("Playlist");

            entity.Property(e => e.PlId).HasColumnName("pl_id");
            entity.Property(e => e.PlDateCreate).HasColumnName("pl_date_create");
            entity.Property(e => e.PlDesc).HasColumnName("pl_desc");
            entity.Property(e => e.PlLikes).HasColumnName("pl_likes");
            entity.Property(e => e.PlName)
                .HasColumnType("character varying")
                .HasColumnName("pl_name");
            entity.Property(e => e.PlTracksQuantity).HasColumnName("pl_tracks_quantity");
            entity.Property(e => e.TotalDuration).HasColumnName("total_duration");
        });

        modelBuilder.Entity<PlaylistCreator>(entity =>
        {
            entity.HasKey(e => e.CrPlId).HasName("PlaylistCreator_pkey");

            entity.ToTable("PlaylistCreator");

            entity.Property(e => e.CrPlId).HasColumnName("cr_pl_id");
            entity.Property(e => e.PlId).HasColumnName("pl_id");
            entity.Property(e => e.UsrId).HasColumnName("usr_id");

            entity.HasOne(d => d.Pl).WithMany(p => p.PlaylistCreators)
                .HasForeignKey(d => d.PlId)
                .HasConstraintName("playlistcreator_playlist_fk");

            entity.HasOne(d => d.Usr).WithMany(p => p.PlaylistCreators)
                .HasForeignKey(d => d.UsrId)
                .HasConstraintName("playlistcreator_users_fk");
        });

        modelBuilder.Entity<PlaylistTrack>(entity =>
        {
            entity.HasKey(e => e.PlTrackId).HasName("PlaylistTracks_pkey");

            entity.Property(e => e.PlTrackId).HasColumnName("pl_track_id");
            entity.Property(e => e.PlId).HasColumnName("pl_id");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.Pl).WithMany(p => p.PlaylistTracks)
                .HasForeignKey(d => d.PlId)
                .HasConstraintName("playlisttracks_playlist_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.PlaylistTracks)
                .HasForeignKey(d => d.TrackId)
                .HasConstraintName("playlisttracks_track_fk");
        });

        modelBuilder.Entity<PlaylistsTag>(entity =>
        {
            entity.HasKey(e => e.PlTagId).HasName("PlaylistsTags_pkey");

            entity.Property(e => e.PlTagId).HasColumnName("pl_tag_id");
            entity.Property(e => e.PlId).HasColumnName("pl_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");

            entity.HasOne(d => d.Pl).WithMany(p => p.PlaylistsTags)
                .HasForeignKey(d => d.PlId)
                .HasConstraintName("playliststags_playlist_fk");

            entity.HasOne(d => d.Tag).WithMany(p => p.PlaylistsTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("playliststags_tags_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Roles_pkey");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubId).HasName("Subscription_pkey");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubId).HasColumnName("sub_id");
            entity.Property(e => e.SubName)
                .HasColumnType("character varying")
                .HasColumnName("sub_name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tags_pkey");

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TagName).HasColumnName("tag_name");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("Track_pkey");

            entity.ToTable("Track");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.AlbId).HasColumnName("alb_id");
            entity.Property(e => e.TrackBitrate).HasColumnName("track_bitrate");
            entity.Property(e => e.TrackCount).HasColumnName("track_count");
            entity.Property(e => e.TrackDuration).HasColumnName("track_duration");
            entity.Property(e => e.TrackName)
                .HasColumnType("character varying")
                .HasColumnName("track_name");
            entity.Property(e => e.TrackPath).HasColumnName("track_path");
            entity.Property(e => e.TrackRating).HasColumnName("track_rating");
            entity.Property(e => e.TrackRelease).HasColumnName("track_release");

            entity.HasOne(d => d.Alb).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbId)
                .HasConstraintName("track_albums_fk");
        });

        modelBuilder.Entity<TrackArtist>(entity =>
        {
            entity.HasKey(e => e.TrackArtId).HasName("TrackArtists_pkey");

            entity.Property(e => e.TrackArtId).HasColumnName("track_art_id");
            entity.Property(e => e.ArtId).HasColumnName("art_id");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.Art).WithMany(p => p.TrackArtists)
                .HasForeignKey(d => d.ArtId)
                .HasConstraintName("trackartists_artist_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.TrackArtists)
                .HasForeignKey(d => d.TrackId)
                .HasConstraintName("trackartists_track_fk");
        });

        modelBuilder.Entity<TrackList>(entity =>
        {
            entity.HasKey(e => e.TrackListId).HasName("TrackList_pkey");

            entity.ToTable("TrackList");

            entity.Property(e => e.TrackListId).HasColumnName("track_list_id");
            entity.Property(e => e.AlbId).HasColumnName("alb_id");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.Alb).WithMany(p => p.TrackLists)
                .HasForeignKey(d => d.AlbId)
                .HasConstraintName("tracklist_albums_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.TrackLists)
                .HasForeignKey(d => d.TrackId)
                .HasConstraintName("tracklist_track_fk");
        });

        modelBuilder.Entity<TracksGenre>(entity =>
        {
            entity.HasKey(e => e.TrackGenreId).HasName("TracksGenres_pkey");

            entity.Property(e => e.TrackGenreId).HasColumnName("track_genre_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.Genre).WithMany(p => p.TracksGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("tracksgenres_genres_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.TracksGenres)
                .HasForeignKey(d => d.TrackId)
                .HasConstraintName("tracksgenres_track_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsrId).HasName("Users_pkey");

            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.SubId).HasColumnName("sub_id");
            entity.Property(e => e.UsrEmail).HasColumnName("usr_email");
            entity.Property(e => e.UsrFName).HasColumnName("usr_f_name");
            entity.Property(e => e.UsrLastEntry).HasColumnName("usr_last_entry");
            entity.Property(e => e.UsrPass).HasColumnName("usr_pass");
            entity.Property(e => e.UsrPlaylistId).HasColumnName("usr_playlist_id");
            entity.Property(e => e.UsrRegDate).HasColumnName("usr_reg_date");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_roles_fk");

            entity.HasOne(d => d.Sub).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubId)
                .HasConstraintName("users_subscription_fk");

            entity.HasOne(d => d.UsrPlaylist).WithMany(p => p.Users)
                .HasForeignKey(d => d.UsrPlaylistId)
                .HasConstraintName("users_playlist_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
