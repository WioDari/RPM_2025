using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Musick.Migrations
{
    /// <inheritdoc />
    public partial class DodatkoBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CounryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Countries_pkey", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenresID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenresName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Genres_pkey", x => x.GenresID);
                });

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Rols_pkey", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Subscriptions_pkey", x => x.SubscriptionID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameTag = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tags_pkey", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistsID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArtistName = table.Column<string>(type: "text", nullable: false),
                    CountryID = table.Column<int>(type: "integer", nullable: true),
                    YearsActive = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Artists_pkey", x => x.ArtistsID);
                    table.ForeignKey(
                        name: "Artists_CountryID_fkey",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserLogin = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    RoleID = table.Column<int>(type: "integer", nullable: false),
                    PlaylistID = table.Column<int>(type: "integer", nullable: true),
                    SubscriptionID = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsBan = table.Column<bool>(name: "IsBan?", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.UserID);
                    table.ForeignKey(
                        name: "Users_RoleID_fkey",
                        column: x => x.RoleID,
                        principalTable: "Rols",
                        principalColumn: "RoleID");
                    table.ForeignKey(
                        name: "Users_SubscriptionID_fkey",
                        column: x => x.SubscriptionID,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionID");
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumName = table.Column<string>(type: "text", nullable: false),
                    ArtistID = table.Column<int>(type: "integer", nullable: false),
                    RealiseDate = table.Column<int>(type: "integer", nullable: false),
                    AlbumCoverPath = table.Column<string>(type: "text", nullable: false),
                    TotalDuration = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Albums_pkey", x => x.AlbumID);
                    table.ForeignKey(
                        name: "Albums_ArtistID_fkey",
                        column: x => x.ArtistID,
                        principalTable: "Artists",
                        principalColumn: "ArtistsID");
                });

            migrationBuilder.CreateTable(
                name: "ArtistsGenres",
                columns: table => new
                {
                    ArtistID = table.Column<int>(type: "integer", nullable: false),
                    GenresID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("artistsgenres_pk", x => new { x.ArtistID, x.GenresID });
                    table.ForeignKey(
                        name: "ArtistsGenres_ArtistID_fkey",
                        column: x => x.ArtistID,
                        principalTable: "Artists",
                        principalColumn: "ArtistsID");
                    table.ForeignKey(
                        name: "ArtistsGenres_GenresID_fkey",
                        column: x => x.GenresID,
                        principalTable: "Genres",
                        principalColumn: "GenresID");
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaylistName = table.Column<string>(type: "text", nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    Likes = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlists_pkey", x => x.PlaylistID);
                    table.ForeignKey(
                        name: "playlists_users_fk",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "AlbumsGenres",
                columns: table => new
                {
                    AlbumID = table.Column<int>(type: "integer", nullable: false),
                    GenresID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("albumsgenres_pk", x => new { x.AlbumID, x.GenresID });
                    table.ForeignKey(
                        name: "AlbumsGenres_AlbumID_fkey",
                        column: x => x.AlbumID,
                        principalTable: "Albums",
                        principalColumn: "AlbumID");
                    table.ForeignKey(
                        name: "AlbumsGenres_GenresID_fkey",
                        column: x => x.GenresID,
                        principalTable: "Genres",
                        principalColumn: "GenresID");
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    TrackID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackName = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    RealiseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Bitrate = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false),
                    PlayCount = table.Column<int>(type: "integer", nullable: true),
                    AlbumID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tracks_pkey", x => x.TrackID);
                    table.ForeignKey(
                        name: "tracks_albums_fk",
                        column: x => x.AlbumID,
                        principalTable: "Albums",
                        principalColumn: "AlbumID");
                });

            migrationBuilder.CreateTable(
                name: "PlaylstTags",
                columns: table => new
                {
                    PlaylistID = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("playlsttags_pk", x => new { x.PlaylistID, x.TagId });
                    table.ForeignKey(
                        name: "PlaylstTags_PlaylistID_fkey",
                        column: x => x.PlaylistID,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistID");
                    table.ForeignKey(
                        name: "PlaylstTags_TagId_fkey",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId");
                });

            migrationBuilder.CreateTable(
                name: "UsersPlaylists",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    PlaylistID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usersplaylists_pk", x => new { x.UserID, x.PlaylistID });
                    table.ForeignKey(
                        name: "usersplaylists_playlists_fk",
                        column: x => x.PlaylistID,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistID");
                    table.ForeignKey(
                        name: "usersplaylists_users_fk",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "TracksArtists",
                columns: table => new
                {
                    TrackID = table.Column<int>(type: "integer", nullable: false),
                    ArtistID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tracksartists_pk", x => new { x.TrackID, x.ArtistID });
                    table.ForeignKey(
                        name: "TracksArtists_ArtistID_fkey",
                        column: x => x.ArtistID,
                        principalTable: "Artists",
                        principalColumn: "ArtistsID");
                    table.ForeignKey(
                        name: "TracksArtists_TrackID_fkey",
                        column: x => x.TrackID,
                        principalTable: "Tracks",
                        principalColumn: "TrackID");
                });

            migrationBuilder.CreateTable(
                name: "TracksGenres",
                columns: table => new
                {
                    TrackID = table.Column<int>(type: "integer", nullable: false),
                    GenresID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tracksgenres_pk", x => new { x.TrackID, x.GenresID });
                    table.ForeignKey(
                        name: "TracksGenres_GenresID_fkey",
                        column: x => x.GenresID,
                        principalTable: "Genres",
                        principalColumn: "GenresID");
                    table.ForeignKey(
                        name: "TracksGenres_TrackID_fkey",
                        column: x => x.TrackID,
                        principalTable: "Tracks",
                        principalColumn: "TrackID");
                });

            migrationBuilder.CreateTable(
                name: "TracksPlaylists",
                columns: table => new
                {
                    TrackID = table.Column<int>(type: "integer", nullable: false),
                    PlaylistID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tracksplaylists_pk", x => new { x.TrackID, x.PlaylistID });
                    table.ForeignKey(
                        name: "TracksPlaylists_PlaylistID_fkey",
                        column: x => x.PlaylistID,
                        principalTable: "Playlists",
                        principalColumn: "PlaylistID");
                    table.ForeignKey(
                        name: "TracksPlaylists_TrackID_fkey",
                        column: x => x.TrackID,
                        principalTable: "Tracks",
                        principalColumn: "TrackID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistID",
                table: "Albums",
                column: "ArtistID");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumsGenres_GenresID",
                table: "AlbumsGenres",
                column: "GenresID");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_CountryID",
                table: "Artists",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistsGenres_GenresID",
                table: "ArtistsGenres",
                column: "GenresID");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserID",
                table: "Playlists",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylstTags_TagId",
                table: "PlaylstTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumID",
                table: "Tracks",
                column: "AlbumID");

            migrationBuilder.CreateIndex(
                name: "IX_TracksArtists_ArtistID",
                table: "TracksArtists",
                column: "ArtistID");

            migrationBuilder.CreateIndex(
                name: "IX_TracksGenres_GenresID",
                table: "TracksGenres",
                column: "GenresID");

            migrationBuilder.CreateIndex(
                name: "IX_TracksPlaylists_PlaylistID",
                table: "TracksPlaylists",
                column: "PlaylistID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionID",
                table: "Users",
                column: "SubscriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersPlaylists_PlaylistID",
                table: "UsersPlaylists",
                column: "PlaylistID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumsGenres");

            migrationBuilder.DropTable(
                name: "ArtistsGenres");

            migrationBuilder.DropTable(
                name: "PlaylstTags");

            migrationBuilder.DropTable(
                name: "TracksArtists");

            migrationBuilder.DropTable(
                name: "TracksGenres");

            migrationBuilder.DropTable(
                name: "TracksPlaylists");

            migrationBuilder.DropTable(
                name: "UsersPlaylists");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Rols");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
