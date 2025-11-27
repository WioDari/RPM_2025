using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicWpf.Migrations
{
    /// <inheritdoc />
    public partial class Music : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtistName = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    YearsActive = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoPath = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Artist_pkey", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenreName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genre_pkey", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_pkey", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subscription_pkey", x => x.SubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagsName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_pkey", x => x.TagsId);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    TracksId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackName = table.Column<string>(type: "text", nullable: false),
                    AlbumInfo = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Bitrate = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    AlbumCoverPath = table.Column<string>(type: "text", nullable: false),
                    AlbumCoverBinary = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false),
                    PlayCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tracks_pkey", x => x.TracksId);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumTitle = table.Column<string>(type: "text", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    CoverPath = table.Column<string>(type: "text", nullable: false),
                    TotalDuration = table.Column<int>(type: "integer", nullable: false),
                    ArtistId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Album_pkey", x => x.AlbumId);
                    table.ForeignKey(
                        name: "Album_ArtistId_fkey",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "ArtistId");
                });

            migrationBuilder.CreateTable(
                name: "GenreArtist",
                columns: table => new
                {
                    GenreArtistId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    ArtistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GenreArtist_pkey", x => x.GenreArtistId);
                    table.ForeignKey(
                        name: "GenreArtist_ArtistId_fkey",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "ArtistId");
                    table.ForeignKey(
                        name: "GenreArtist_GenreId_fkey",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "GenreId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserLogin = table.Column<string>(type: "text", nullable: false),
                    UserPassword = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastLogin = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.UserId);
                    table.ForeignKey(
                        name: "User_RoleId_fkey",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "User_SubscriptionId_fkey",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "SubscriptionId");
                });

            migrationBuilder.CreateTable(
                name: "ArtistTracks",
                columns: table => new
                {
                    ArtistTracksId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtistId = table.Column<int>(type: "integer", nullable: false),
                    TrackId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ArtistTracks_pkey", x => x.ArtistTracksId);
                    table.ForeignKey(
                        name: "ArtistTracks_ArtistId_fkey",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "ArtistId");
                    table.ForeignKey(
                        name: "ArtistTracks_TrackId_fkey",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "TracksId");
                });

            migrationBuilder.CreateTable(
                name: "TracksGenres",
                columns: table => new
                {
                    TracksGenresId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TracksId = table.Column<int>(type: "integer", nullable: false),
                    GenresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TracksGenres_pkey", x => x.TracksGenresId);
                    table.ForeignKey(
                        name: "TracksGenres_GenresId_fkey",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "GenreId");
                    table.ForeignKey(
                        name: "TracksGenres_TracksId_fkey",
                        column: x => x.TracksId,
                        principalTable: "Tracks",
                        principalColumn: "TracksId");
                });

            migrationBuilder.CreateTable(
                name: "AlbumGenres",
                columns: table => new
                {
                    AlbumGenresId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AlbumGenres_pkey", x => x.AlbumGenresId);
                    table.ForeignKey(
                        name: "AlbumGenres_AlbumId_fkey",
                        column: x => x.AlbumId,
                        principalTable: "Album",
                        principalColumn: "AlbumId");
                    table.ForeignKey(
                        name: "AlbumGenres_GenreId_fkey",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "GenreId");
                });

            migrationBuilder.CreateTable(
                name: "AlbumTracks",
                columns: table => new
                {
                    AlbumTracksId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumId = table.Column<int>(type: "integer", nullable: false),
                    TracksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AlbumTracks_pkey", x => x.AlbumTracksId);
                    table.ForeignKey(
                        name: "AlbumTracks_AlbumId_fkey",
                        column: x => x.AlbumId,
                        principalTable: "Album",
                        principalColumn: "AlbumId");
                    table.ForeignKey(
                        name: "AlbumTracks_TracksId_fkey",
                        column: x => x.TracksId,
                        principalTable: "Tracks",
                        principalColumn: "TracksId");
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaylistName = table.Column<string>(type: "text", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlist_pkey", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "Playlist_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistsUser",
                columns: table => new
                {
                    PlaylistsUserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaylistsId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaylistsUser_pkey", x => x.PlaylistsUserId);
                    table.ForeignKey(
                        name: "PlaylistsUser_PlaylistsId_fkey",
                        column: x => x.PlaylistsId,
                        principalTable: "Playlist",
                        principalColumn: "PlaylistId");
                    table.ForeignKey(
                        name: "PlaylistsUser_UserId_fkey",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTags",
                columns: table => new
                {
                    PlaylistTagsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagsId = table.Column<int>(type: "integer", nullable: false),
                    PlaylistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaylistTags_pkey", x => x.PlaylistTagsId);
                    table.ForeignKey(
                        name: "PlaylistTags_PlaylistId_fkey",
                        column: x => x.PlaylistId,
                        principalTable: "Playlist",
                        principalColumn: "PlaylistId");
                    table.ForeignKey(
                        name: "PlaylistTags_TagsId_fkey",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "TagsId");
                });

            migrationBuilder.CreateTable(
                name: "TracksPlaylist",
                columns: table => new
                {
                    TracksPlaylistId = table.Column<int>(type: "integer", nullable: false),
                    TracksId = table.Column<int>(type: "integer", nullable: true),
                    PlaylistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TracksPlaylist_pkey", x => x.TracksPlaylistId);
                    table.ForeignKey(
                        name: "TracksPlaylist_PlaylistId_fkey",
                        column: x => x.PlaylistId,
                        principalTable: "Playlist",
                        principalColumn: "PlaylistId");
                    table.ForeignKey(
                        name: "TracksPlaylist_TracksId_fkey",
                        column: x => x.TracksId,
                        principalTable: "Tracks",
                        principalColumn: "TracksId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_ArtistId",
                table: "Album",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumGenres_AlbumId",
                table: "AlbumGenres",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumGenres_GenreId",
                table: "AlbumGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumTracks_AlbumId",
                table: "AlbumTracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumTracks_TracksId",
                table: "AlbumTracks",
                column: "TracksId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTracks_ArtistId",
                table: "ArtistTracks",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTracks_TrackId",
                table: "ArtistTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreArtist_ArtistId",
                table: "GenreArtist",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreArtist_GenreId",
                table: "GenreArtist",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_UserId",
                table: "Playlist",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsUser_PlaylistsId",
                table: "PlaylistsUser",
                column: "PlaylistsId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistsUser_UserId",
                table: "PlaylistsUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_PlaylistId",
                table: "PlaylistTags",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_TagsId",
                table: "PlaylistTags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_TracksGenres_GenresId",
                table: "TracksGenres",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_TracksGenres_TracksId",
                table: "TracksGenres",
                column: "TracksId");

            migrationBuilder.CreateIndex(
                name: "IX_TracksPlaylist_PlaylistId",
                table: "TracksPlaylist",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_TracksPlaylist_TracksId",
                table: "TracksPlaylist",
                column: "TracksId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_SubscriptionId",
                table: "User",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumGenres");

            migrationBuilder.DropTable(
                name: "AlbumTracks");

            migrationBuilder.DropTable(
                name: "ArtistTracks");

            migrationBuilder.DropTable(
                name: "GenreArtist");

            migrationBuilder.DropTable(
                name: "PlaylistsUser");

            migrationBuilder.DropTable(
                name: "PlaylistTags");

            migrationBuilder.DropTable(
                name: "TracksGenres");

            migrationBuilder.DropTable(
                name: "TracksPlaylist");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
