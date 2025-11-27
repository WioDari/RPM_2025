using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Spotify_wpf.Migrations
{
    /// <inheritdoc />
    public partial class music : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    Artist_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Artist_Name = table.Column<string>(type: "text", nullable: false),
                    Years_Activity = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoPath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Artist_pkey", x => x.Artist_Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Genre_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Genre_Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genre_pkey", x => x.Genre_Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Role_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_pkey", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Subscription_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subscription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subscription_pkey", x => x.Subscription_Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Tags_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tags_Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_pkey", x => x.Tags_Id);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    Album_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumName = table.Column<string>(type: "text", nullable: false),
                    CoverPath = table.Column<string>(type: "text", nullable: false),
                    TotalDuration = table.Column<int>(type: "integer", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    Artist_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Album_pkey", x => x.Album_Id);
                    table.ForeignKey(
                        name: "album_artist_fk",
                        column: x => x.Artist_Id,
                        principalTable: "Artist",
                        principalColumn: "Artist_Id");
                });

            migrationBuilder.CreateTable(
                name: "Genre_Artist",
                columns: table => new
                {
                    GenreArtist_Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"AlbumArtist_AlbumArtist_Id_seq\"'::regclass)"),
                    Genre_Id = table.Column<int>(type: "integer", nullable: false),
                    Artist_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AlbumArtist_pkey", x => x.GenreArtist_Id);
                    table.ForeignKey(
                        name: "AlbumArtist_Artist_Id_fkey",
                        column: x => x.Artist_Id,
                        principalTable: "Artist",
                        principalColumn: "Artist_Id");
                    table.ForeignKey(
                        name: "genre_artist_genre_fk",
                        column: x => x.Genre_Id,
                        principalTable: "Genre",
                        principalColumn: "Genre_Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User_Login = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    User_Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role_Id = table.Column<int>(type: "integer", nullable: false),
                    Subscription_Id = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastLogin = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.User_Id);
                    table.ForeignKey(
                        name: "User_Role_Id_fkey",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Role_Id");
                    table.ForeignKey(
                        name: "User_Subscription_Id_fkey",
                        column: x => x.Subscription_Id,
                        principalTable: "Subscription",
                        principalColumn: "Subscription_Id");
                });

            migrationBuilder.CreateTable(
                name: "Album_Genre",
                columns: table => new
                {
                    Album_Genre_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Genre_Id = table.Column<int>(type: "integer", nullable: false),
                    Album_Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Album_Genre_pkey", x => x.Album_Genre_Id);
                    table.ForeignKey(
                        name: "Album_Genre_Album_Id_fkey",
                        column: x => x.Album_Id,
                        principalTable: "Album",
                        principalColumn: "Album_Id");
                    table.ForeignKey(
                        name: "Album_Genre_Genre_Id_fkey",
                        column: x => x.Genre_Id,
                        principalTable: "Genre",
                        principalColumn: "Genre_Id");
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    Track_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Duration = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Bitrate = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false),
                    Playcount = table.Column<int>(type: "integer", nullable: false),
                    TrackName = table.Column<string>(type: "text", nullable: false),
                    Album_Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Track_pkey", x => x.Track_Id);
                    table.ForeignKey(
                        name: "track_album_fk",
                        column: x => x.Album_Id,
                        principalTable: "Album",
                        principalColumn: "Album_Id");
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    PlayList_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Playlist_Name = table.Column<string>(type: "text", nullable: false),
                    DataCreate = table.Column<DateOnly>(type: "date", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    User_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlists_pkey", x => x.PlayList_Id);
                    table.ForeignKey(
                        name: "Playlists_User_Id_fkey",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id");
                });

            migrationBuilder.CreateTable(
                name: "Album_Track",
                columns: table => new
                {
                    Album_Track_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Album_Id = table.Column<int>(type: "integer", nullable: false),
                    Track_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Album_Track_pkey", x => x.Album_Track_Id);
                    table.ForeignKey(
                        name: "Album_Track_Album_Id_fkey",
                        column: x => x.Album_Id,
                        principalTable: "Album",
                        principalColumn: "Album_Id");
                    table.ForeignKey(
                        name: "Album_Track_Track_Id_fkey",
                        column: x => x.Track_Id,
                        principalTable: "Track",
                        principalColumn: "Track_Id");
                });

            migrationBuilder.CreateTable(
                name: "Genre_Track",
                columns: table => new
                {
                    Genre_Track_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Track_Id = table.Column<int>(type: "integer", nullable: false),
                    Genre_Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genre_Track_pkey", x => x.Genre_Track_Id);
                    table.ForeignKey(
                        name: "Genre_Track_Genre_Id_fkey",
                        column: x => x.Genre_Id,
                        principalTable: "Genre",
                        principalColumn: "Genre_Id");
                    table.ForeignKey(
                        name: "Genre_Track_Track_Id_fkey",
                        column: x => x.Track_Id,
                        principalTable: "Track",
                        principalColumn: "Track_Id");
                });

            migrationBuilder.CreateTable(
                name: "TrackArtist",
                columns: table => new
                {
                    TrackArtist_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Track_Id = table.Column<int>(type: "integer", nullable: false),
                    Artist_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TrackArtist_pkey", x => x.TrackArtist_Id);
                    table.ForeignKey(
                        name: "trackartist_artist_fk",
                        column: x => x.Artist_Id,
                        principalTable: "Artist",
                        principalColumn: "Artist_Id");
                    table.ForeignKey(
                        name: "trackartist_track_fk",
                        column: x => x.Track_Id,
                        principalTable: "Track",
                        principalColumn: "Track_Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayList_Track",
                columns: table => new
                {
                    PlayList_Track_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Playlist_Id = table.Column<int>(type: "integer", nullable: false),
                    Track_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlayList_Track_pkey", x => x.PlayList_Track_id);
                    table.ForeignKey(
                        name: "PlayList_Track_Playlist_Id_fkey",
                        column: x => x.Playlist_Id,
                        principalTable: "Playlists",
                        principalColumn: "PlayList_Id");
                    table.ForeignKey(
                        name: "PlayList_Track_Track_Id_fkey",
                        column: x => x.Track_Id,
                        principalTable: "Track",
                        principalColumn: "Track_Id");
                });

            migrationBuilder.CreateTable(
                name: "Playlist_User",
                columns: table => new
                {
                    Playlist_User_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Playlist_Id = table.Column<int>(type: "integer", nullable: false),
                    User_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlist_User_pkey", x => x.Playlist_User_Id);
                    table.ForeignKey(
                        name: "Playlist_User_Playlist_Id_fkey",
                        column: x => x.Playlist_Id,
                        principalTable: "Playlists",
                        principalColumn: "PlayList_Id");
                    table.ForeignKey(
                        name: "Playlist_User_User_Id_fkey",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "User_Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags_Playlist",
                columns: table => new
                {
                    Tags_Playlist_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tags_Id = table.Column<int>(type: "integer", nullable: false),
                    Playlist_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_Playlist_pkey", x => x.Tags_Playlist_Id);
                    table.ForeignKey(
                        name: "Tags_Playlist_Playlist_Id_fkey",
                        column: x => x.Playlist_Id,
                        principalTable: "Playlists",
                        principalColumn: "PlayList_Id");
                    table.ForeignKey(
                        name: "Tags_Playlist_Tags_Id_fkey",
                        column: x => x.Tags_Id,
                        principalTable: "Tags",
                        principalColumn: "Tags_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_Artist_Id",
                table: "Album",
                column: "Artist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Genre_Album_Id",
                table: "Album_Genre",
                column: "Album_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Genre_Genre_Id",
                table: "Album_Genre",
                column: "Genre_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Track_Album_Id",
                table: "Album_Track",
                column: "Album_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Track_Track_Id",
                table: "Album_Track",
                column: "Track_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Artist_Artist_Id",
                table: "Genre_Artist",
                column: "Artist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Artist_Genre_Id",
                table: "Genre_Artist",
                column: "Genre_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Track_Genre_Id",
                table: "Genre_Track",
                column: "Genre_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Track_Track_Id",
                table: "Genre_Track",
                column: "Track_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayList_Track_Playlist_Id",
                table: "PlayList_Track",
                column: "Playlist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayList_Track_Track_Id",
                table: "PlayList_Track",
                column: "Track_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_User_Playlist_Id",
                table: "Playlist_User",
                column: "Playlist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_User_User_Id",
                table: "Playlist_User",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_User_Id",
                table: "Playlists",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Playlist_Playlist_Id",
                table: "Tags_Playlist",
                column: "Playlist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Playlist_Tags_Id",
                table: "Tags_Playlist",
                column: "Tags_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Track_Album_Id",
                table: "Track",
                column: "Album_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtist_Artist_Id",
                table: "TrackArtist",
                column: "Artist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtist_Track_Id",
                table: "TrackArtist",
                column: "Track_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_Id",
                table: "User",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Subscription_Id",
                table: "User",
                column: "Subscription_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Album_Genre");

            migrationBuilder.DropTable(
                name: "Album_Track");

            migrationBuilder.DropTable(
                name: "Genre_Artist");

            migrationBuilder.DropTable(
                name: "Genre_Track");

            migrationBuilder.DropTable(
                name: "PlayList_Track");

            migrationBuilder.DropTable(
                name: "Playlist_User");

            migrationBuilder.DropTable(
                name: "Tags_Playlist");

            migrationBuilder.DropTable(
                name: "TrackArtist");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Artist");
        }
    }
}
