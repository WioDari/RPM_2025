using BelokonevPRM.Models;
using Microsoft.EntityFrameworkCore;

namespace BelokonevPRM.Context;

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

    public virtual DbSet<Albumgenre> Albumgenres { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Artistalbum> Artistalbums { get; set; }

    public virtual DbSet<Artistgenre> Artistgenres { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Playlisttrack> Playlisttracks { get; set; }
    
    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<Trackscreator> Trackscreators { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Database=postgres; Username=postgres; Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("albums_pkey");

            entity.ToTable("albums");

            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.AlbumName).HasColumnName("album_name");
            entity.Property(e => e.CoverBinary).HasColumnName("cover_binary");
            entity.Property(e => e.CoverPath).HasColumnName("cover_path");
            entity.Property(e => e.Realeaseyear).HasColumnName("realeaseyear");
            entity.Property(e => e.Totaldurations).HasColumnName("totaldurations");
        });

        modelBuilder.Entity<Albumgenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("albumgenres_pkey");

            entity.ToTable("albumgenres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Album).HasColumnName("album");
            entity.Property(e => e.Genre).HasColumnName("genre");

            entity.HasOne(d => d.AlbumNavigation).WithMany(p => p.Albumgenres)
                .HasForeignKey(d => d.Album)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("albumgenres_albums_fk");

            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.Albumgenres)
                .HasForeignKey(d => d.Genre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("albumgenres_genres_fk");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("artists_pkey");

            entity.ToTable("artists");

            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.ArtistName).HasColumnName("artist_name");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.PhotoPath).HasColumnName("photo_path");
            entity.Property(e => e.YearactiveUntill).HasColumnName("yearactive_untill");
            entity.Property(e => e.Yearbegin).HasColumnName("yearbegin");
        });

        modelBuilder.Entity<Artistalbum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artistalbums_pkey");

            entity.ToTable("artistalbums");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Album).HasColumnName("album");
            entity.Property(e => e.Creator).HasColumnName("creator");

            entity.HasOne(d => d.AlbumNavigation).WithMany(p => p.Artistalbums)
                .HasForeignKey(d => d.Album)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("artistalbums_albums_fk");

            entity.HasOne(d => d.CreatorNavigation).WithMany(p => p.Artistalbums)
                .HasForeignKey(d => d.Creator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("artistalbums_artists_fk");
        });

        modelBuilder.Entity<Artistgenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artistgenres_pkey");

            entity.ToTable("artistgenres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creator).HasColumnName("creator");
            entity.Property(e => e.Genre).HasColumnName("genre");

            entity.HasOne(d => d.CreatorNavigation).WithMany(p => p.Artistgenres)
                .HasForeignKey(d => d.Creator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("artistgenres_artists_fk");

            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.Artistgenres)
                .HasForeignKey(d => d.Genre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("artistgenres_genres_fk");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Genre1).HasColumnName("genre");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("playlists_pkey");

            entity.ToTable("playlists");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.Creator).HasColumnName("creator");
            entity.Property(e => e.Datecreated).HasColumnName("datecreated");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Likes)
                .HasDefaultValue(0)
                .HasColumnName("likes");
            entity.Property(e => e.PlaylistTitle).HasColumnName("playlist_title");

            entity.HasOne(d => d.CreatorNavigation).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.Creator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlists_users_fk");
        });

        modelBuilder.Entity<Playlisttrack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("playlisttracks_pkey");

            entity.ToTable("playlisttracks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Playlist).HasColumnName("playlist");
            entity.Property(e => e.Track).HasColumnName("track");

            entity.HasOne(d => d.PlaylistNavigation).WithMany(p => p.Playlisttracks)
                .HasForeignKey(d => d.Playlist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlisttracks_playlists_fk");

            entity.HasOne(d => d.TrackNavigation).WithMany(p => p.Playlisttracks)
                .HasForeignKey(d => d.Track)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlisttracks_tracks_fk");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("subscriptions_pkey");

            entity.ToTable("subscriptions");

            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");
            entity.Property(e => e.Subscription1).HasColumnName("subscription");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("tracks_pkey");

            entity.ToTable("tracks");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.Album).HasColumnName("album");
            entity.Property(e => e.Bitrate).HasColumnName("bitrate");
            entity.Property(e => e.CoverPath).HasColumnName("cover_path");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Playcount)
                .HasDefaultValue(0L)
                .HasColumnName("playcount");
            entity.Property(e => e.Rating)
                .HasDefaultValueSql("3.0")
                .HasColumnName("rating");
            entity.Property(e => e.Realeasedate).HasColumnName("realeasedate");
            entity.Property(e => e.TrackName).HasColumnName("track_name");

            entity.HasOne(d => d.AlbumNavigation).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.Album)
                .HasConstraintName("tracks_albums_fk");
        });

        modelBuilder.Entity<Trackscreator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trackscreators_pkey");

            entity.ToTable("trackscreators");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creator).HasColumnName("creator");
            entity.Property(e => e.TrackId).HasColumnName("track_id");

            entity.HasOne(d => d.CreatorNavigation).WithMany(p => p.Trackscreators)
                .HasForeignKey(d => d.Creator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trackscreators_artists_fk");

            entity.HasOne(d => d.Track).WithMany(p => p.Trackscreators)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trackscreators_tracks_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.LastentryDate).HasColumnName("lastentry_date");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("role_id");
            entity.Property(e => e.SubscriptionId)
                .HasDefaultValue(1)
                .HasColumnName("subscription_id");
            entity.Property(e => e.UserEmail).HasColumnName("user_email");
            entity.Property(e => e.UserLogin).HasColumnName("user_login");
            entity.Property(e => e.UserName).HasColumnName("user_name");
            entity.Property(e => e.UserPwd).HasColumnName("user_pwd");

            entity.HasOne(d => d.Roles).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_roles_fk");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Users)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("users_subscriptions_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
