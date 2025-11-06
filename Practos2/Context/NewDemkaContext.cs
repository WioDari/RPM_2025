using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Practos2.Models;

namespace Practos2.Context;

public partial class NewDemkaContext : DbContext
{
    public NewDemkaContext()
    {
    }

    public NewDemkaContext(DbContextOptions<NewDemkaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Albumcoverpath> Albumcoverpaths { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<ArtistAlbum> ArtistAlbums { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<GenreArtist> GenreArtists { get; set; }

    public virtual DbSet<GenreTrack> GenreTracks { get; set; }

    public virtual DbSet<LoginsTable> LoginsTables { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistTrack> PlaylistTracks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagPlaylist> TagPlaylists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPlaylist> UserPlaylists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=NewDemka;Username=postgres;password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Albumid).HasName("albums_pk");

            entity.ToTable("albums", "FirstPract");

            entity.Property(e => e.Albumid).HasColumnName("albumid");
            entity.Property(e => e.Albumcoverpathid).HasColumnName("albumcoverpathid");
            entity.Property(e => e.Albumname)
                .HasColumnType("character varying")
                .HasColumnName("albumname");
            entity.Property(e => e.Authorid)
                .HasColumnType("character varying")
                .HasColumnName("authorid");
            entity.Property(e => e.Releaseyear).HasColumnName("releaseyear");
            entity.Property(e => e.Totalduration).HasColumnName("totalduration");

            entity.HasOne(d => d.Albumcoverpath).WithMany(p => p.Albums)
                .HasForeignKey(d => d.Albumcoverpathid)
                .HasConstraintName("albums_albumcoverpath_fk");
        });

        modelBuilder.Entity<Albumcoverpath>(entity =>
        {
            entity.HasKey(e => e.Albumcoverpathid).HasName("albumcoverpath_pk");

            entity.ToTable("albumcoverpath", "FirstPract");

            entity.Property(e => e.Albumcoverpathid).HasColumnName("albumcoverpathid");
            entity.Property(e => e.Covarbinary)
                .HasColumnType("character varying")
                .HasColumnName("covarbinary");
            entity.Property(e => e.Coverpath)
                .HasColumnType("character varying")
                .HasColumnName("coverpath");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Artistid).HasName("artists_pk");

            entity.ToTable("artists", "FirstPract");

            entity.Property(e => e.Artistid).HasColumnName("artistid");
            entity.Property(e => e.Artistname)
                .HasColumnType("character varying")
                .HasColumnName("artistname");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Photopath)
                .HasColumnType("character varying")
                .HasColumnName("photopath");
            entity.Property(e => e.Yearsactive)
                .HasColumnType("character varying")
                .HasColumnName("yearsactive");
        });

        modelBuilder.Entity<ArtistAlbum>(entity =>
        {
            entity.HasKey(e => e.Artistalbumid).HasName("artist_album_pk");

            entity.ToTable("artist_album", "FirstPract");

            entity.Property(e => e.Artistalbumid).HasColumnName("artistalbumid");
            entity.Property(e => e.Albumid).HasColumnName("albumid");
            entity.Property(e => e.Artistid).HasColumnName("artistid");

            entity.HasOne(d => d.Artist).WithMany(p => p.ArtistAlbums)
                .HasForeignKey(d => d.Artistid)
                .HasConstraintName("artist_album_artists_fk");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("county_pk");

            entity.ToTable("country", "FirstPract");

            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Countryname)
                .HasColumnType("character varying")
                .HasColumnName("countryname");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Genreid).HasName("genres_pk");

            entity.ToTable("genres", "FirstPract");

            entity.Property(e => e.Genreid).HasColumnName("genreid");
            entity.Property(e => e.Genrename)
                .HasColumnType("character varying")
                .HasColumnName("genrename");
        });

        modelBuilder.Entity<GenreArtist>(entity =>
        {
            entity.HasKey(e => e.Genreartistid).HasName("genre_artist_pk");

            entity.ToTable("genre_artist", "FirstPract");

            entity.Property(e => e.Genreartistid).HasColumnName("genreartistid");
            entity.Property(e => e.Artistid).HasColumnName("artistid");
            entity.Property(e => e.Genreid).HasColumnName("genreid");

            entity.HasOne(d => d.Artist).WithMany(p => p.GenreArtists)
                .HasForeignKey(d => d.Artistid)
                .HasConstraintName("genre_artist_artists_fk");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreArtists)
                .HasForeignKey(d => d.Genreid)
                .HasConstraintName("genre_artist_genres_fk");
        });

        modelBuilder.Entity<GenreTrack>(entity =>
        {
            entity.HasKey(e => e.Genretrackid).HasName("genre_track_pk");

            entity.ToTable("genre_track", "FirstPract");

            entity.Property(e => e.Genretrackid).HasColumnName("genretrackid");
            entity.Property(e => e.Genreid).HasColumnName("genreid");
            entity.Property(e => e.Trackid).HasColumnName("trackid");

            entity.HasOne(d => d.Genre).WithMany(p => p.GenreTracks)
                .HasForeignKey(d => d.Genreid)
                .HasConstraintName("genre_track_genres_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.GenreTracks)
                .HasForeignKey(d => d.Trackid)
                .HasConstraintName("genre_track_tracks_fk");
        });

        modelBuilder.Entity<LoginsTable>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("loginstable_pk");

            entity.ToTable("logins_table", "FirstPract");

            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.Lastenter).HasColumnName("lastenter");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Playlistid).HasName("newtable_pk");

            entity.ToTable("Playlist", "FirstPract");

            entity.Property(e => e.Playlistid).HasColumnName("playlistid");
            entity.Property(e => e.Datecreated).HasColumnName("datecreated");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Playlistname)
                .HasColumnType("character varying")
                .HasColumnName("playlistname");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("playlist_user_fk");
        });

        modelBuilder.Entity<PlaylistTrack>(entity =>
        {
            entity.HasKey(e => e.Playlisttracksid).HasName("playlist_tracks_pk");

            entity.ToTable("playlist_tracks", "FirstPract");

            entity.Property(e => e.Playlisttracksid).HasColumnName("playlisttracksid");
            entity.Property(e => e.Playlistid).HasColumnName("playlistid");
            entity.Property(e => e.Trackid).HasColumnName("trackid");

            entity.HasOne(d => d.Playlist).WithMany(p => p.PlaylistTracks)
                .HasForeignKey(d => d.Playlistid)
                .HasConstraintName("playlist_tracks_playlist_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.PlaylistTracks)
                .HasForeignKey(d => d.Trackid)
                .HasConstraintName("playlist_tracks_tracks_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pk");

            entity.ToTable("roles", "FirstPract");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasColumnType("character varying")
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Subid).HasName("subscriptions_pk");

            entity.ToTable("subscriptions", "FirstPract");

            entity.Property(e => e.Subid).HasColumnName("subid");
            entity.Property(e => e.Subname)
                .HasColumnType("character varying")
                .HasColumnName("subname");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Tagid).HasName("tags_pk");

            entity.ToTable("tags", "FirstPract");

            entity.Property(e => e.Tagid)
                .ValueGeneratedNever()
                .HasColumnName("tagid");
            entity.Property(e => e.Tagname)
                .HasColumnType("character varying")
                .HasColumnName("tagname");
        });

        modelBuilder.Entity<TagPlaylist>(entity =>
        {
            entity.HasKey(e => e.Tagplaylistid).HasName("tag_playlist_pk");

            entity.ToTable("tag_playlist", "FirstPract");

            entity.Property(e => e.Tagplaylistid).HasColumnName("tagplaylistid");
            entity.Property(e => e.Playlistid).HasColumnName("playlistid");
            entity.Property(e => e.Tagid).HasColumnName("tagid");

            entity.HasOne(d => d.Playlist).WithMany(p => p.TagPlaylists)
                .HasForeignKey(d => d.Playlistid)
                .HasConstraintName("tag_playlist_playlist_fk");

            entity.HasOne(d => d.Tag).WithMany(p => p.TagPlaylists)
                .HasForeignKey(d => d.Tagid)
                .HasConstraintName("tag_playlist_tags_fk");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Trackid).HasName("tracks_pk");

            entity.ToTable("tracks", "FirstPract");

            entity.Property(e => e.Trackid).HasColumnName("trackid");
            entity.Property(e => e.Albumcovarid).HasColumnName("albumcovarid");
            entity.Property(e => e.Albumid).HasColumnName("albumid");
            entity.Property(e => e.Artistid).HasColumnName("artistid");
            entity.Property(e => e.Bitrate).HasColumnName("bitrate");
            entity.Property(e => e.Filepath)
                .HasColumnType("character varying")
                .HasColumnName("filepath");
            entity.Property(e => e.Playcount).HasColumnName("playcount");
            entity.Property(e => e.Raiting).HasColumnName("raiting");
            entity.Property(e => e.Releasedate).HasColumnName("releasedate");
            entity.Property(e => e.Trackname)
                .HasColumnType("character varying")
                .HasColumnName("trackname");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.Albumid)
                .HasConstraintName("tracks_albums_fk");

            entity.HasOne(d => d.Artist).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.Artistid)
                .HasConstraintName("tracks_artists_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("user_pk");

            entity.ToTable("User", "FirstPract");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Lastloginid).HasColumnName("lastloginid");
            entity.Property(e => e.Regdate).HasColumnName("regdate");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Subscriptionid).HasColumnName("subscriptionid");
            entity.Property(e => e.Userlogin)
                .HasColumnType("character varying")
                .HasColumnName("userlogin");
            entity.Property(e => e.Userpassword)
                .HasColumnType("character varying")
                .HasColumnName("userpassword");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.Countryid)
                .HasConstraintName("user_country_fk");

            entity.HasOne(d => d.Lastlogin).WithMany(p => p.Users)
                .HasForeignKey(d => d.Lastloginid)
                .HasConstraintName("user_loginstable_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("user_roles_fk");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.Subscriptionid)
                .HasConstraintName("user_subscriptions_fk");
        });

        modelBuilder.Entity<UserPlaylist>(entity =>
        {
            entity.HasKey(e => e.Userplaylistid).HasName("userplaylist_pk");

            entity.ToTable("user_playlist", "FirstPract");

            entity.Property(e => e.Userplaylistid)
                .ValueGeneratedNever()
                .HasColumnName("userplaylistid");
            entity.Property(e => e.Playlistid).HasColumnName("playlistid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.UserPlaylists)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("userplaylist_user_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
