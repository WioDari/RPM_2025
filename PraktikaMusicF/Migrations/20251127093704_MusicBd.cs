using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PraktikaMusicF.Migrations
{
    /// <inheritdoc />
    public partial class MusicBd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    art_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    art_name = table.Column<string>(type: "character varying", nullable: true),
                    year_active = table.Column<string>(type: "text", nullable: true),
                    art_desc = table.Column<string>(type: "text", nullable: true),
                    art_photo_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Artist_pkey", x => x.art_id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    genres_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genres_pkey", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    pl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pl_name = table.Column<string>(type: "character varying", nullable: true),
                    pl_tracks_quantity = table.Column<int>(type: "integer", nullable: true),
                    pl_date_create = table.Column<DateOnly>(type: "date", nullable: true),
                    pl_likes = table.Column<int>(type: "integer", nullable: true),
                    pl_desc = table.Column<string>(type: "text", nullable: true),
                    total_duration = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlist_pkey", x => x.pl_id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_pkey", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    sub_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sub_name = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subscription_pkey", x => x.sub_id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tag_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_pkey", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    alb_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alb_name = table.Column<string>(type: "character varying", nullable: true),
                    artist_id = table.Column<int>(type: "integer", nullable: true),
                    alb_image = table.Column<string>(type: "text", nullable: true),
                    alb_release = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Albums_pkey", x => x.alb_id);
                    table.ForeignKey(
                        name: "albums_artist_fk",
                        column: x => x.artist_id,
                        principalTable: "Artist",
                        principalColumn: "art_id");
                });

            migrationBuilder.CreateTable(
                name: "ArtistGenres",
                columns: table => new
                {
                    art_genre_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    art_id = table.Column<int>(type: "integer", nullable: true),
                    genre_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ArtistGenres_pkey", x => x.art_genre_id);
                    table.ForeignKey(
                        name: "artistgenres_artist_fk",
                        column: x => x.art_id,
                        principalTable: "Artist",
                        principalColumn: "art_id");
                    table.ForeignKey(
                        name: "artistgenres_genres_fk",
                        column: x => x.genre_id,
                        principalTable: "Genres",
                        principalColumn: "genre_id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    usr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usr_f_name = table.Column<string>(type: "text", nullable: true),
                    usr_email = table.Column<string>(type: "text", nullable: true),
                    role_id = table.Column<int>(type: "integer", nullable: true),
                    sub_id = table.Column<int>(type: "integer", nullable: true),
                    usr_reg_date = table.Column<DateOnly>(type: "date", nullable: true),
                    usr_last_entry = table.Column<DateOnly>(type: "date", nullable: true),
                    usr_playlist_id = table.Column<int>(type: "integer", nullable: true),
                    usr_pass = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.usr_id);
                    table.ForeignKey(
                        name: "users_playlist_fk",
                        column: x => x.usr_playlist_id,
                        principalTable: "Playlist",
                        principalColumn: "pl_id");
                    table.ForeignKey(
                        name: "users_roles_fk",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "users_subscription_fk",
                        column: x => x.sub_id,
                        principalTable: "Subscription",
                        principalColumn: "sub_id");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsTags",
                columns: table => new
                {
                    pl_tag_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pl_id = table.Column<int>(type: "integer", nullable: true),
                    tag_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaylistsTags_pkey", x => x.pl_tag_id);
                    table.ForeignKey(
                        name: "playliststags_playlist_fk",
                        column: x => x.pl_id,
                        principalTable: "Playlist",
                        principalColumn: "pl_id");
                    table.ForeignKey(
                        name: "playliststags_tags_fk",
                        column: x => x.tag_id,
                        principalTable: "Tags",
                        principalColumn: "tag_id");
                });

            migrationBuilder.CreateTable(
                name: "AlbumsGenres",
                columns: table => new
                {
                    alb_genres_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alb_id = table.Column<int>(type: "integer", nullable: true),
                    genre_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AlbumsGenres_pkey", x => x.alb_genres_id);
                    table.ForeignKey(
                        name: "albumsgenres_albums_fk",
                        column: x => x.alb_id,
                        principalTable: "Albums",
                        principalColumn: "alb_id");
                    table.ForeignKey(
                        name: "albumsgenres_genres_fk",
                        column: x => x.genre_id,
                        principalTable: "Genres",
                        principalColumn: "genre_id");
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    track_name = table.Column<string>(type: "character varying", nullable: true),
                    alb_id = table.Column<int>(type: "integer", nullable: true),
                    track_release = table.Column<DateOnly>(type: "date", nullable: true),
                    track_bitrate = table.Column<int>(type: "integer", nullable: true),
                    track_path = table.Column<string>(type: "text", nullable: true),
                    track_rating = table.Column<double>(type: "double precision", nullable: true),
                    track_count = table.Column<long>(type: "bigint", nullable: true),
                    track_duration = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Track_pkey", x => x.track_id);
                    table.ForeignKey(
                        name: "track_albums_fk",
                        column: x => x.alb_id,
                        principalTable: "Albums",
                        principalColumn: "alb_id");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistCreator",
                columns: table => new
                {
                    cr_pl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usr_id = table.Column<int>(type: "integer", nullable: true),
                    pl_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaylistCreator_pkey", x => x.cr_pl_id);
                    table.ForeignKey(
                        name: "playlistcreator_playlist_fk",
                        column: x => x.pl_id,
                        principalTable: "Playlist",
                        principalColumn: "pl_id");
                    table.ForeignKey(
                        name: "playlistcreator_users_fk",
                        column: x => x.usr_id,
                        principalTable: "Users",
                        principalColumn: "usr_id");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTracks",
                columns: table => new
                {
                    pl_track_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pl_id = table.Column<int>(type: "integer", nullable: true),
                    track_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaylistTracks_pkey", x => x.pl_track_id);
                    table.ForeignKey(
                        name: "playlisttracks_playlist_fk",
                        column: x => x.pl_id,
                        principalTable: "Playlist",
                        principalColumn: "pl_id");
                    table.ForeignKey(
                        name: "playlisttracks_track_fk",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id");
                });

            migrationBuilder.CreateTable(
                name: "TrackArtists",
                columns: table => new
                {
                    track_art_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    track_id = table.Column<int>(type: "integer", nullable: true),
                    art_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TrackArtists_pkey", x => x.track_art_id);
                    table.ForeignKey(
                        name: "trackartists_artist_fk",
                        column: x => x.art_id,
                        principalTable: "Artist",
                        principalColumn: "art_id");
                    table.ForeignKey(
                        name: "trackartists_track_fk",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id");
                });

            migrationBuilder.CreateTable(
                name: "TrackList",
                columns: table => new
                {
                    track_list_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alb_id = table.Column<int>(type: "integer", nullable: true),
                    track_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TrackList_pkey", x => x.track_list_id);
                    table.ForeignKey(
                        name: "tracklist_albums_fk",
                        column: x => x.alb_id,
                        principalTable: "Albums",
                        principalColumn: "alb_id");
                    table.ForeignKey(
                        name: "tracklist_track_fk",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id");
                });

            migrationBuilder.CreateTable(
                name: "TracksGenres",
                columns: table => new
                {
                    track_genre_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    track_id = table.Column<int>(type: "integer", nullable: true),
                    genre_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TracksGenres_pkey", x => x.track_genre_id);
                    table.ForeignKey(
                        name: "tracksgenres_genres_fk",
                        column: x => x.genre_id,
                        principalTable: "Genres",
                        principalColumn: "genre_id");
                    table.ForeignKey(
                        name: "tracksgenres_track_fk",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_artist_id",
                table: "Albums",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsGenres_alb_id",
                table: "AlbumsGenres",
                column: "alb_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsGenres_genre_id",
                table: "AlbumsGenres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistGenres_art_id",
                table: "ArtistGenres",
                column: "art_id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistGenres_genre_id",
                table: "ArtistGenres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCreator_pl_id",
                table: "PlaylistCreator",
                column: "pl_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCreator_usr_id",
                table: "PlaylistCreator",
                column: "usr_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsTags_pl_id",
                table: "PlaylistsTags",
                column: "pl_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsTags_tag_id",
                table: "PlaylistsTags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_pl_id",
                table: "PlaylistTracks",
                column: "pl_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_track_id",
                table: "PlaylistTracks",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_Track_alb_id",
                table: "Track",
                column: "alb_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_art_id",
                table: "TrackArtists",
                column: "art_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_track_id",
                table: "TrackArtists",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackList_alb_id",
                table: "TrackList",
                column: "alb_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackList_track_id",
                table: "TrackList",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_TracksGenres_genre_id",
                table: "TracksGenres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_TracksGenres_track_id",
                table: "TracksGenres",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_sub_id",
                table: "Users",
                column: "sub_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_usr_playlist_id",
                table: "Users",
                column: "usr_playlist_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumsGenres");

            migrationBuilder.DropTable(
                name: "ArtistGenres");

            migrationBuilder.DropTable(
                name: "PlaylistCreator");

            migrationBuilder.DropTable(
                name: "PlaylistsTags");

            migrationBuilder.DropTable(
                name: "PlaylistTracks");

            migrationBuilder.DropTable(
                name: "TrackArtists");

            migrationBuilder.DropTable(
                name: "TrackList");

            migrationBuilder.DropTable(
                name: "TracksGenres");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artist");
        }
    }
}
