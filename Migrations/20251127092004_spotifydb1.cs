using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpotApp_wpf.Migrations
{
    /// <inheritdoc />
    public partial class spotifydb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Artist_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtistName = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    YearsActive = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoPath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Artists_pkey", x => x.Artist_id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Genre_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Genre_tittle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genres_pkey", x => x.Genre_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_pkey", x => x.Role_id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Subscription_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subscription_tittle = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subscription_pkey", x => x.Subscription_id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Tag_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag_tittle = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_pkey", x => x.Tag_id);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    Album_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumTitle = table.Column<string>(type: "text", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    CoverPath = table.Column<string>(type: "text", nullable: false),
                    Artist_id = table.Column<int>(type: "integer", nullable: true),
                    TotalDuration = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Album_pkey", x => x.Album_id);
                    table.ForeignKey(
                        name: "Album_Artist_id_fkey",
                        column: x => x.Artist_id,
                        principalTable: "Artists",
                        principalColumn: "Artist_id");
                });

            migrationBuilder.CreateTable(
                name: "Genres_in_Artists",
                columns: table => new
                {
                    GiA_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Genre_id = table.Column<int>(type: "integer", nullable: true),
                    Artist_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genres_in_Artists_pkey", x => x.GiA_id);
                    table.ForeignKey(
                        name: "Genres_in_Artists_Artist_id_fkey",
                        column: x => x.Artist_id,
                        principalTable: "Artists",
                        principalColumn: "Artist_id");
                    table.ForeignKey(
                        name: "Genres_in_Artists_Genre_id_fkey",
                        column: x => x.Genre_id,
                        principalTable: "Genres",
                        principalColumn: "Genre_id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    User_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserLogin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserPassword = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role_id = table.Column<int>(type: "integer", nullable: false),
                    Subscription_id = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.User_id);
                    table.ForeignKey(
                        name: "User_Role_id_fkey",
                        column: x => x.Role_id,
                        principalTable: "Role",
                        principalColumn: "Role_id");
                    table.ForeignKey(
                        name: "User_Subscription_id_fkey",
                        column: x => x.Subscription_id,
                        principalTable: "Subscription",
                        principalColumn: "Subscription_id");
                });

            migrationBuilder.CreateTable(
                name: "Genres_in_Albums",
                columns: table => new
                {
                    GiAl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Genre_id = table.Column<int>(type: "integer", nullable: false),
                    Album_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genres_in_Albums_pkey", x => x.GiAl_id);
                    table.ForeignKey(
                        name: "Genres_in_Albums_Album_id_fkey",
                        column: x => x.Album_id,
                        principalTable: "Album",
                        principalColumn: "Album_id");
                    table.ForeignKey(
                        name: "Genres_in_Albums_Genre_id_fkey",
                        column: x => x.Genre_id,
                        principalTable: "Genres",
                        principalColumn: "Genre_id");
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    Track_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackName = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Bitrate = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    AlbumCoverPath = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false),
                    PlayCount = table.Column<int>(type: "integer", nullable: false),
                    Album_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Track_pkey", x => x.Track_id);
                    table.ForeignKey(
                        name: "Track_Album_id_fkey",
                        column: x => x.Album_id,
                        principalTable: "Album",
                        principalColumn: "Album_id");
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Playlist_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaylistName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    User_id = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlists_pkey", x => x.Playlist_id);
                    table.ForeignKey(
                        name: "Playlists_User_id_fkey",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "Artists_in_Tracks",
                columns: table => new
                {
                    AiT_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Artist_id = table.Column<int>(type: "integer", nullable: false),
                    Track_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Artists_in_Tracks_pkey", x => x.AiT_id);
                    table.ForeignKey(
                        name: "Artists_in_Tracks_Artist_id_fkey",
                        column: x => x.Artist_id,
                        principalTable: "Artists",
                        principalColumn: "Artist_id");
                    table.ForeignKey(
                        name: "Artists_in_Tracks_Track_id_fkey",
                        column: x => x.Track_id,
                        principalTable: "Track",
                        principalColumn: "Track_id");
                });

            migrationBuilder.CreateTable(
                name: "Geners_in_Tracks",
                columns: table => new
                {
                    GiT_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Genre_id = table.Column<int>(type: "integer", nullable: false),
                    Track_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Geners_in_Tracks_pkey", x => x.GiT_id);
                    table.ForeignKey(
                        name: "Geners_in_Tracks_Genre_id_fkey",
                        column: x => x.Genre_id,
                        principalTable: "Genres",
                        principalColumn: "Genre_id");
                    table.ForeignKey(
                        name: "Geners_in_Tracks_Track_id_fkey",
                        column: x => x.Track_id,
                        principalTable: "Track",
                        principalColumn: "Track_id");
                });

            migrationBuilder.CreateTable(
                name: "Tracks_in_Albums",
                columns: table => new
                {
                    TiA_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Track_id = table.Column<int>(type: "integer", nullable: true),
                    Album_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tracks_in_Albums_pkey", x => x.TiA_id);
                    table.ForeignKey(
                        name: "Tracks_in_Albums_Album_id_fkey",
                        column: x => x.Album_id,
                        principalTable: "Album",
                        principalColumn: "Album_id");
                    table.ForeignKey(
                        name: "Tracks_in_Albums_Track_id_fkey",
                        column: x => x.Track_id,
                        principalTable: "Track",
                        principalColumn: "Track_id");
                });

            migrationBuilder.CreateTable(
                name: "Tags_in_Playlists",
                columns: table => new
                {
                    TiP_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tag_id = table.Column<int>(type: "integer", nullable: false),
                    Playlist_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_in_Playlists_pkey", x => x.TiP_id);
                    table.ForeignKey(
                        name: "Tags_in_Playlists_Playlist_id_fkey",
                        column: x => x.Playlist_id,
                        principalTable: "Playlists",
                        principalColumn: "Playlist_id");
                    table.ForeignKey(
                        name: "Tags_in_Playlists_Tag_id_fkey",
                        column: x => x.Tag_id,
                        principalTable: "Tags",
                        principalColumn: "Tag_id");
                });

            migrationBuilder.CreateTable(
                name: "Tracks_in_Playlists",
                columns: table => new
                {
                    TiP_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Track_id = table.Column<int>(type: "integer", nullable: false),
                    Playlist_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tracks_in_Playlists_pkey", x => x.TiP_id);
                    table.ForeignKey(
                        name: "Tracks_in_Playlists_Playlist_id_fkey",
                        column: x => x.Playlist_id,
                        principalTable: "Playlists",
                        principalColumn: "Playlist_id");
                    table.ForeignKey(
                        name: "Tracks_in_Playlists_Track_id_fkey",
                        column: x => x.Track_id,
                        principalTable: "Track",
                        principalColumn: "Track_id");
                });

            migrationBuilder.CreateTable(
                name: "Users_Playlists",
                columns: table => new
                {
                    UserPlaylist_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Playlist_id = table.Column<int>(type: "integer", nullable: false),
                    User_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_Playlists_pkey", x => x.UserPlaylist_id);
                    table.ForeignKey(
                        name: "Users_Playlists_Playlist_id_fkey",
                        column: x => x.Playlist_id,
                        principalTable: "Playlists",
                        principalColumn: "Playlist_id");
                    table.ForeignKey(
                        name: "Users_Playlists_User_id_fkey",
                        column: x => x.User_id,
                        principalTable: "User",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_Artist_id",
                table: "Album",
                column: "Artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_in_Tracks_Artist_id",
                table: "Artists_in_Tracks",
                column: "Artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_in_Tracks_Track_id",
                table: "Artists_in_Tracks",
                column: "Track_id");

            migrationBuilder.CreateIndex(
                name: "IX_Geners_in_Tracks_Genre_id",
                table: "Geners_in_Tracks",
                column: "Genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Geners_in_Tracks_Track_id",
                table: "Geners_in_Tracks",
                column: "Track_id");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_in_Albums_Album_id",
                table: "Genres_in_Albums",
                column: "Album_id");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_in_Albums_Genre_id",
                table: "Genres_in_Albums",
                column: "Genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_in_Artists_Artist_id",
                table: "Genres_in_Artists",
                column: "Artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_in_Artists_Genre_id",
                table: "Genres_in_Artists",
                column: "Genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_User_id",
                table: "Playlists",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_in_Playlists_Playlist_id",
                table: "Tags_in_Playlists",
                column: "Playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_in_Playlists_Tag_id",
                table: "Tags_in_Playlists",
                column: "Tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_Track_Album_id",
                table: "Track",
                column: "Album_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_in_Albums_Album_id",
                table: "Tracks_in_Albums",
                column: "Album_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_in_Albums_Track_id",
                table: "Tracks_in_Albums",
                column: "Track_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_in_Playlists_Playlist_id",
                table: "Tracks_in_Playlists",
                column: "Playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_in_Playlists_Track_id",
                table: "Tracks_in_Playlists",
                column: "Track_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_id",
                table: "User",
                column: "Role_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Subscription_id",
                table: "User",
                column: "Subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Playlists_Playlist_id",
                table: "Users_Playlists",
                column: "Playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Playlists_User_id",
                table: "Users_Playlists",
                column: "User_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artists_in_Tracks");

            migrationBuilder.DropTable(
                name: "Geners_in_Tracks");

            migrationBuilder.DropTable(
                name: "Genres_in_Albums");

            migrationBuilder.DropTable(
                name: "Genres_in_Artists");

            migrationBuilder.DropTable(
                name: "Tags_in_Playlists");

            migrationBuilder.DropTable(
                name: "Tracks_in_Albums");

            migrationBuilder.DropTable(
                name: "Tracks_in_Playlists");

            migrationBuilder.DropTable(
                name: "Users_Playlists");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
