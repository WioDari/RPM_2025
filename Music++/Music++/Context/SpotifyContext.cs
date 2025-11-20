using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Music__.Models;

namespace Music__.Context;

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

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=spotify;Username=postgres;Password=1488");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Albumid).HasName("albums_pkey");

            entity.ToTable("albums");

            entity.HasIndex(e => e.Coverpath, "albums_coverpath_key").IsUnique();

            entity.Property(e => e.Albumid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("albumid");
            entity.Property(e => e.Albumduration)
                .HasMaxLength(10)
                .HasColumnName("albumduration");
            entity.Property(e => e.Albumname)
                .HasColumnType("character varying")
                .HasColumnName("albumname");
            entity.Property(e => e.Albumreleaseyear)
                .HasMaxLength(10)
                .HasColumnName("albumreleaseyear");
            entity.Property(e => e.Coverpath)
                .HasColumnType("character varying")
                .HasColumnName("coverpath");

            entity.HasMany(d => d.Genres).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "Albumgenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("Genreid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("albumgenres_genreid_fkey"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("Albumid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("albumgenres_albumid_fkey"),
                    j =>
                    {
                        j.HasKey("Albumid", "Genreid").HasName("albumgenres_pkey");
                        j.ToTable("albumgenres");
                        j.IndexerProperty<int>("Albumid").HasColumnName("albumid");
                        j.IndexerProperty<int>("Genreid").HasColumnName("genreid");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "Albumtrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("Trackid")
                        .HasConstraintName("albumtracks_trackid_fkey"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("Albumid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("albumtracks_albumid_fkey"),
                    j =>
                    {
                        j.HasKey("Albumid", "Trackid").HasName("albumtracks_pkey");
                        j.ToTable("albumtracks");
                        j.IndexerProperty<int>("Albumid").HasColumnName("albumid");
                        j.IndexerProperty<int>("Trackid").HasColumnName("trackid");
                    });
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Artistid).HasName("artists_pkey");

            entity.ToTable("artists");

            entity.HasIndex(e => e.Artistname, "artists_artistname_key").IsUnique();

            entity.HasIndex(e => e.Photopath, "artists_photopath_key").IsUnique();

            entity.Property(e => e.Artistid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("artistid");
            entity.Property(e => e.Activeyears)
                .HasMaxLength(10)
                .HasColumnName("activeyears");
            entity.Property(e => e.Artistdescription)
                .HasColumnType("character varying")
                .HasColumnName("artistdescription");
            entity.Property(e => e.Artistname)
                .HasMaxLength(30)
                .HasColumnName("artistname");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Photopath)
                .HasColumnType("character varying")
                .HasColumnName("photopath");

            entity.HasOne(d => d.Country).WithMany(p => p.Artists)
                .HasForeignKey(d => d.Countryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("artists_countryid_fkey");

            entity.HasMany(d => d.Albums).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "Artistalbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("Albumid")
                        .HasConstraintName("artistalbums_albumid_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("Artistid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("artistalbums_artistid_fkey"),
                    j =>
                    {
                        j.HasKey("Artistid", "Albumid").HasName("artistalbums_pkey");
                        j.ToTable("artistalbums");
                        j.IndexerProperty<int>("Artistid").HasColumnName("artistid");
                        j.IndexerProperty<int>("Albumid").HasColumnName("albumid");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "Artistgenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("Genreid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("artistgenres_genreid_fkey"),
                    l => l.HasOne<Artist>().WithMany()
                        .HasForeignKey("Artistid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("artistgenres_artistid_fkey"),
                    j =>
                    {
                        j.HasKey("Artistid", "Genreid").HasName("artistgenres_pkey");
                        j.ToTable("artistgenres");
                        j.IndexerProperty<int>("Artistid").HasColumnName("artistid");
                        j.IndexerProperty<int>("Genreid").HasColumnName("genreid");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.HasIndex(e => e.Countryname, "countries_countryname_key").IsUnique();

            entity.Property(e => e.Countryid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("countryid");
            entity.Property(e => e.Countryname)
                .HasMaxLength(30)
                .HasColumnName("countryname");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Genreid).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.Genrename, "genres_genrename_key").IsUnique();

            entity.Property(e => e.Genreid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("genreid");
            entity.Property(e => e.Genrename)
                .HasMaxLength(30)
                .HasColumnName("genrename");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Playlistid).HasName("playlists_pkey");

            entity.ToTable("playlists");

            entity.Property(e => e.Playlistid)
                .ValueGeneratedNever()
                .HasColumnName("playlistid");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Playlistcreationdate).HasColumnName("playlistcreationdate");
            entity.Property(e => e.Playlistdescription)
                .HasColumnType("character varying")
                .HasColumnName("playlistdescription");
            entity.Property(e => e.Playlistname)
                .HasColumnType("character varying")
                .HasColumnName("playlistname");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlists_userid_fkey");

            entity.HasMany(d => d.Tags).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "Playlisttag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("Tagid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playlisttags_tagid_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("Playlistid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playlisttags_playlistid_fkey"),
                    j =>
                    {
                        j.HasKey("Playlistid", "Tagid").HasName("playlisttags_pkey");
                        j.ToTable("playlisttags");
                        j.IndexerProperty<short>("Playlistid").HasColumnName("playlistid");
                        j.IndexerProperty<short>("Tagid").HasColumnName("tagid");
                    });

            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "Playlisttrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("Trackid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("playlisttracks_trackid_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("Playlistid")
                        .HasConstraintName("playlisttracks_playlistid_fkey"),
                    j =>
                    {
                        j.HasKey("Playlistid", "Trackid").HasName("playlisttracks_pkey");
                        j.ToTable("playlisttracks");
                        j.IndexerProperty<short>("Playlistid").HasColumnName("playlistid");
                        j.IndexerProperty<int>("Trackid").HasColumnName("trackid");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Rolename, "roles_rolename_key").IsUnique();

            entity.Property(e => e.Roleid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(20)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Subscriptionid).HasName("subscriptions_pkey");

            entity.ToTable("subscriptions");

            entity.HasIndex(e => e.Subscriptionname, "subscriptions_subscriptionname_key").IsUnique();

            entity.Property(e => e.Subscriptionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("subscriptionid");
            entity.Property(e => e.Subscriptionname)
                .HasMaxLength(20)
                .HasColumnName("subscriptionname");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Tagid).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.HasIndex(e => e.Tagname, "tags_tagname_key").IsUnique();

            entity.Property(e => e.Tagid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("tagid");
            entity.Property(e => e.Tagname)
                .HasMaxLength(30)
                .HasColumnName("tagname");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.Trackid).HasName("tracks_pkey");

            entity.ToTable("tracks");

            entity.HasIndex(e => e.Filepath, "tracks_filepath_key").IsUnique();

            entity.Property(e => e.Trackid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("trackid");
            entity.Property(e => e.Bitrate).HasColumnName("bitrate");
            entity.Property(e => e.Filepath)
                .HasColumnType("character varying")
                .HasColumnName("filepath");
            entity.Property(e => e.Playcount).HasColumnName("playcount");
            entity.Property(e => e.Rating)
                .HasPrecision(3, 2)
                .HasColumnName("rating");
            entity.Property(e => e.Trackduration)
                .HasMaxLength(10)
                .HasColumnName("trackduration");
            entity.Property(e => e.Trackname)
                .HasColumnType("character varying")
                .HasColumnName("trackname");
            entity.Property(e => e.Trackreleasedate).HasColumnName("trackreleasedate");

            entity.HasMany(d => d.Artists).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "Trackartist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("Artistid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("trackartists_artistid_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("Trackid")
                        .HasConstraintName("trackartists_trackid_fkey"),
                    j =>
                    {
                        j.HasKey("Trackid", "Artistid").HasName("trackartists_pkey");
                        j.ToTable("trackartists");
                        j.IndexerProperty<int>("Trackid").HasColumnName("trackid");
                        j.IndexerProperty<int>("Artistid").HasColumnName("artistid");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "Trackgenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("Genreid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("trackgenres_genreid_fkey"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("Trackid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("trackgenres_trackid_fkey"),
                    j =>
                    {
                        j.HasKey("Trackid", "Genreid").HasName("trackgenres_pkey");
                        j.ToTable("trackgenres");
                        j.IndexerProperty<int>("Trackid").HasColumnName("trackid");
                        j.IndexerProperty<int>("Genreid").HasColumnName("genreid");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(30)
                .HasColumnName("fullname");
            entity.Property(e => e.Lastlogindate).HasColumnName("lastlogindate");
            entity.Property(e => e.Login)
                .HasMaxLength(30)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.Registrationdate).HasColumnName("registrationdate");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Subscriptionid).HasColumnName("subscriptionid");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_roleid_fkey");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.Subscriptionid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_subscriptionid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
