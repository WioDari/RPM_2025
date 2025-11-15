--
-- PostgreSQL database dump
--

\restrict IKaOgonxLXJLdDV0vRoSiiRRgqIT1ZIx987DcUHE1h3mcUXDnxyxYDe8Jv5xkcn

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.6

-- Started on 2025-11-15 14:30:14

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 223 (class 1259 OID 17207)
-- Name: Albums; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Albums" (
    "Id" integer NOT NULL,
    "Title" text NOT NULL,
    "ArtistId" integer NOT NULL,
    "ReleaseYear" integer NOT NULL,
    "CoverPath" text,
    "TotalDuration" interval NOT NULL
);


--
-- TOC entry 228 (class 1259 OID 17240)
-- Name: AlbumsGenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AlbumsGenres" (
    "AlbumId" integer NOT NULL,
    "GenreId" integer NOT NULL
);


--
-- TOC entry 222 (class 1259 OID 17206)
-- Name: Albums_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Albums" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Albums_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 225 (class 1259 OID 17220)
-- Name: Artists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Artists" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "CountryId" integer NOT NULL,
    "Description" text NOT NULL,
    "ActiveStartYear" integer NOT NULL,
    "ActiveEndYear" integer,
    "PhotoPath" text
);


--
-- TOC entry 242 (class 1259 OID 17366)
-- Name: ArtistsGenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."ArtistsGenres" (
    "ArtistId" integer NOT NULL,
    "GenreId" integer NOT NULL
);


--
-- TOC entry 224 (class 1259 OID 17219)
-- Name: Artists_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Artists" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Artists_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 227 (class 1259 OID 17228)
-- Name: Countries; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Countries" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- TOC entry 226 (class 1259 OID 17227)
-- Name: Countries_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Countries" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Countries_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 220 (class 1259 OID 17184)
-- Name: Genres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Genres" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- TOC entry 219 (class 1259 OID 17183)
-- Name: Genres_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Genres" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Genres_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 230 (class 1259 OID 17261)
-- Name: Playlists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Playlists" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "CreatorUserId" integer NOT NULL,
    "DateCreated" date NOT NULL,
    "Likes" integer NOT NULL,
    "Description" text NOT NULL
);


--
-- TOC entry 234 (class 1259 OID 17291)
-- Name: PlaylistsTags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."PlaylistsTags" (
    "PlaylistId" integer NOT NULL,
    "TagId" integer NOT NULL
);


--
-- TOC entry 233 (class 1259 OID 17276)
-- Name: PlaylistsTracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."PlaylistsTracks" (
    "PlaylistId" integer NOT NULL,
    "TrackId" integer NOT NULL
);


--
-- TOC entry 229 (class 1259 OID 17260)
-- Name: Playlists_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Playlists" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Playlists_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 241 (class 1259 OID 17344)
-- Name: Subscriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Subscriptions" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- TOC entry 240 (class 1259 OID 17343)
-- Name: Subscriptions_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Subscriptions" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Subscriptions_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 232 (class 1259 OID 17269)
-- Name: Tags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Tags" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- TOC entry 231 (class 1259 OID 17268)
-- Name: Tags_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Tags" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Tags_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 218 (class 1259 OID 17176)
-- Name: Tracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Tracks" (
    "Id" integer NOT NULL,
    "TrackName" text NOT NULL,
    "AlbumId" integer NOT NULL,
    "Duration" interval NOT NULL,
    "ReleaseDate" date NOT NULL,
    "Bitrate" integer NOT NULL,
    "FilePath" text NOT NULL,
    "Raiting" numeric(3,2) NOT NULL,
    "PlayCount" integer NOT NULL
);


--
-- TOC entry 243 (class 1259 OID 17512)
-- Name: TracksArtists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."TracksArtists" (
    "TrackId" integer NOT NULL,
    "ArtistId" integer NOT NULL
);


--
-- TOC entry 221 (class 1259 OID 17191)
-- Name: TracksGenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."TracksGenres" (
    "TrackId" integer NOT NULL,
    "GenreId" integer NOT NULL
);


--
-- TOC entry 217 (class 1259 OID 17175)
-- Name: Tracks_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Tracks" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Tracks_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 238 (class 1259 OID 17316)
-- Name: UserRoles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UserRoles" (
    "Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- TOC entry 237 (class 1259 OID 17315)
-- Name: UserRoles_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."UserRoles" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."UserRoles_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 235 (class 1259 OID 17306)
-- Name: Users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "Login" text NOT NULL,
    "Password" text NOT NULL,
    "FullName" text NOT NULL,
    "Email" text NOT NULL,
    "RoleId" integer NOT NULL,
    "SubscriptionId" integer NOT NULL,
    "RegistrationDate" date NOT NULL,
    "LastLoginDateTime" timestamp without time zone
);


--
-- TOC entry 239 (class 1259 OID 17323)
-- Name: UsersPlaylists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UsersPlaylists" (
    "UserId" integer NOT NULL,
    "PlaylistId" integer NOT NULL
);


--
-- TOC entry 236 (class 1259 OID 17314)
-- Name: Users_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Users" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Users_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 5020 (class 0 OID 17207)
-- Dependencies: 223
-- Data for Name: Albums; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (1, 'As Alive As You Need Me To Be', 31, 2025, 'https://i.scdn.co/image/ab67616d0000b27317ff2581a6b924fb3a704395', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (2, 'Atomic Heart, Vol.5 (Original Game Soundtrack)', 8, 2025, 'https://i.scdn.co/image/ab67616d0000b2731a24aab10d2b27d63175c37a', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (3, 'Baby', 1, 2025, 'https://i.scdn.co/image/ab67616d0000b273fc679c0d97b173113141a42b', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (4, 'Barbara Barbara, we face a shining future', 42, 2016, 'https://i.scdn.co/image/ab67616d0000b273cf5cdbf0397a8a71c9fd0395', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (5, 'Bloodline (Bonus Tracks)', 43, 1992, 'https://i.scdn.co/image/ab67616d0000b273c74e21dfa7e9470c0c980f9f', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (6, 'Breed (feat. REEBZ)', 41, 2023, 'https://i.scdn.co/image/ab67616d0000b2737163994ea19cc74c0bbfb31e', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (7, 'Call of Duty®: Black Ops 6 - Zombies "The Tomb" (Original Soundtrack)', 16, 2025, 'https://i.scdn.co/image/ab67616d0000b2732db3154893852b63232e1466', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (8, 'CHOMPO III', 7, 2025, 'https://i.scdn.co/image/ab67616d0000b273aadd94c74f32483d503fb0ab', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (9, 'Cinematic Soundscape', 38, 2012, 'https://i.scdn.co/image/ab67616d0000b27381c4fc91dc2abd939c2c705f', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (10, 'Circle', 41, 2022, 'https://i.scdn.co/image/ab67616d0000b273f0f4f4f180ea0f32ac2c5f8f', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (11, 'Data Destruction', 3, 2025, 'https://i.scdn.co/image/ab67616d0000b273c6ad3e09b75355deee5b732d', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (12, 'Final Destination', 13, 2025, 'https://i.scdn.co/image/ab67616d0000b273857bb2eff1c145e26e9a2c1c', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (13, 'Formula Of Fear', 38, 2008, 'https://i.scdn.co/image/ab67616d0000b27359566bca173ee54657bd5f8d', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (14, 'Further Down The Spiral', 31, 1995, 'https://i.scdn.co/image/ab67616d0000b273551ecb57cda40ae833e3e5b1', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (15, 'Fuze', 24, 2025, 'https://i.scdn.co/image/ab67616d0000b27362d6852a9461f7c5579097c6', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (16, 'Ghost City Daze', 33, 2022, 'https://i.scdn.co/image/ab67616d0000b273ab65f724c2fdf067999d8d16', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (17, 'Ghosts V: Together', 31, 2020, 'https://i.scdn.co/image/ab67616d0000b27327c5a582320e08258ec13d0a', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (18, 'GOFASTER (YANA100)', 41, 2025, 'https://i.scdn.co/image/ab67616d0000b2735ff50f46b8b8720981957d51', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (19, 'Grave', 30, 2025, 'https://i.scdn.co/image/ab67616d0000b273659f2e9dc34715a487f073be', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (20, 'I See Red', 17, 2025, 'https://i.scdn.co/image/ab67616d0000b2737f34486112bf81e38838a429', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (21, 'Liquid (Bonus Track Version)', 43, 2000, 'https://i.scdn.co/image/ab67616d0000b273211808193df8743aedb1038b', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (22, 'Lvstlove', 5, 2025, 'https://i.scdn.co/image/ab67616d0000b2737348b951edbf4bb86854e806', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (23, 'Metalicious', 21, 2025, 'https://i.scdn.co/image/ab67616d0000b27368e7513b4ca1da14da0a81e6', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (24, 'Moments In Everglow', 18, 2025, 'https://i.scdn.co/image/ab67616d0000b27379edc5965038d4923dff96b0', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (25, 'Muleta', 25, 2025, 'https://i.scdn.co/image/ab67616d0000b273adebc14e80184ee7cc2e36b8', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (26, 'Never, Never, Land', 39, 2003, 'https://i.scdn.co/image/ab67616d0000b273894c4f974159696026573eb5', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (27, 'NO MERCY III', 19, 2025, 'https://i.scdn.co/image/ab67616d0000b273a10a1e401fc110168dfd02f3', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (28, 'Oblivion With Bells', 42, 2007, 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (29, 'Oblivion With Bells', 42, 2007, 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (30, 'Oblivion With Bells', 42, 2007, 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (31, 'ORDINARY LOSS', 22, 2025, 'https://i.scdn.co/image/ab67616d0000b27367bd88cd7a659c93b34b0688', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (32, 'Overload 2K', 30, 2025, 'https://i.scdn.co/image/ab67616d0000b273a24c69273fb3656b3ecb92a7', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (33, 'Repentance', 25, 2025, 'https://i.scdn.co/image/ab67616d0000b2730f423970a440915f6750e11e', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (34, 'Run', 20, 2025, 'https://i.scdn.co/image/ab67616d0000b273af743f04ff7cc6ae7300929f', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (35, 'Siren Of The Storm', 38, 2025, 'https://i.scdn.co/image/ab67616d0000b273628b577615d97f6837e30b73', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (36, 'SOMEWHEREIBELONG', 41, 2025, 'https://i.scdn.co/image/ab67616d0000b27328dc64c5aa8c59dd37a7d820', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (37, 'Subhuman', 43, 2007, 'https://i.scdn.co/image/ab67616d0000b273ce0298116be15cb81f91a1aa', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (38, 'The Day Is My Enemy (Expanded Edition)', 35, 2015, 'https://i.scdn.co/image/ab67616d0000b273872772e0b165e57a104fd37a', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (39, 'The Fragile', 31, 1999, 'https://i.scdn.co/image/ab67616d0000b273237a21c8ba1bfc2d85a83585', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (40, 'The Machine', 21, 2025, 'https://i.scdn.co/image/ab67616d0000b27328700d650196f5918a0578bf', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (41, 'The Masquerade (Zardonic & Toronto Is Broken Remix)', 27, 2025, 'https://i.scdn.co/image/ab67616d0000b273b9bb9b27f5aeb0e1a1475707', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (42, 'The Star', 25, 2025, 'https://i.scdn.co/image/ab67616d0000b2731e01b0b41fa3bf96bab342cb', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (43, 'Wall I Was', 28, 2025, 'https://i.scdn.co/image/ab67616d0000b273710d4624a5a1b87b70b56bb2', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (44, 'общедомовой', 2, 2025, 'https://i.scdn.co/image/ab67616d0000b273f11a5383c11039c003e9008b', '00:00:00');
INSERT INTO public."Albums" OVERRIDING SYSTEM VALUE VALUES (45, 'あなたを待って', 36, 2015, 'https://i.scdn.co/image/ab67616d0000b273fcc3cb5cc1166a95b86f5e6e', '00:00:00');


--
-- TOC entry 5025 (class 0 OID 17240)
-- Dependencies: 228
-- Data for Name: AlbumsGenres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."AlbumsGenres" VALUES (1, 66);
INSERT INTO public."AlbumsGenres" VALUES (1, 26);
INSERT INTO public."AlbumsGenres" VALUES (1, 43);
INSERT INTO public."AlbumsGenres" VALUES (2, 11);
INSERT INTO public."AlbumsGenres" VALUES (2, 31);
INSERT INTO public."AlbumsGenres" VALUES (3, 66);
INSERT INTO public."AlbumsGenres" VALUES (3, 26);
INSERT INTO public."AlbumsGenres" VALUES (3, 43);
INSERT INTO public."AlbumsGenres" VALUES (4, 11);
INSERT INTO public."AlbumsGenres" VALUES (4, 31);
INSERT INTO public."AlbumsGenres" VALUES (5, 23);
INSERT INTO public."AlbumsGenres" VALUES (5, 48);
INSERT INTO public."AlbumsGenres" VALUES (6, 11);
INSERT INTO public."AlbumsGenres" VALUES (6, 31);
INSERT INTO public."AlbumsGenres" VALUES (7, 66);
INSERT INTO public."AlbumsGenres" VALUES (7, 20);
INSERT INTO public."AlbumsGenres" VALUES (8, 12);
INSERT INTO public."AlbumsGenres" VALUES (8, 11);
INSERT INTO public."AlbumsGenres" VALUES (9, 23);
INSERT INTO public."AlbumsGenres" VALUES (9, 48);
INSERT INTO public."AlbumsGenres" VALUES (10, 67);
INSERT INTO public."AlbumsGenres" VALUES (10, 21);
INSERT INTO public."AlbumsGenres" VALUES (10, 4);
INSERT INTO public."AlbumsGenres" VALUES (11, 11);
INSERT INTO public."AlbumsGenres" VALUES (11, 12);
INSERT INTO public."AlbumsGenres" VALUES (11, 37);
INSERT INTO public."AlbumsGenres" VALUES (12, 11);
INSERT INTO public."AlbumsGenres" VALUES (12, 31);
INSERT INTO public."AlbumsGenres" VALUES (13, 67);
INSERT INTO public."AlbumsGenres" VALUES (13, 15);
INSERT INTO public."AlbumsGenres" VALUES (14, 45);
INSERT INTO public."AlbumsGenres" VALUES (14, 43);
INSERT INTO public."AlbumsGenres" VALUES (14, 44);
INSERT INTO public."AlbumsGenres" VALUES (14, 3);
INSERT INTO public."AlbumsGenres" VALUES (14, 58);
INSERT INTO public."AlbumsGenres" VALUES (15, 45);
INSERT INTO public."AlbumsGenres" VALUES (15, 43);
INSERT INTO public."AlbumsGenres" VALUES (15, 44);
INSERT INTO public."AlbumsGenres" VALUES (15, 3);
INSERT INTO public."AlbumsGenres" VALUES (15, 58);
INSERT INTO public."AlbumsGenres" VALUES (16, 66);
INSERT INTO public."AlbumsGenres" VALUES (16, 26);
INSERT INTO public."AlbumsGenres" VALUES (16, 43);
INSERT INTO public."AlbumsGenres" VALUES (17, 45);
INSERT INTO public."AlbumsGenres" VALUES (17, 43);
INSERT INTO public."AlbumsGenres" VALUES (17, 44);
INSERT INTO public."AlbumsGenres" VALUES (17, 3);
INSERT INTO public."AlbumsGenres" VALUES (17, 58);
INSERT INTO public."AlbumsGenres" VALUES (18, 12);
INSERT INTO public."AlbumsGenres" VALUES (18, 11);
INSERT INTO public."AlbumsGenres" VALUES (19, 23);
INSERT INTO public."AlbumsGenres" VALUES (19, 48);
INSERT INTO public."AlbumsGenres" VALUES (20, 23);
INSERT INTO public."AlbumsGenres" VALUES (20, 48);
INSERT INTO public."AlbumsGenres" VALUES (21, 25);
INSERT INTO public."AlbumsGenres" VALUES (21, 19);
INSERT INTO public."AlbumsGenres" VALUES (21, 59);
INSERT INTO public."AlbumsGenres" VALUES (21, 27);
INSERT INTO public."AlbumsGenres" VALUES (22, 13);
INSERT INTO public."AlbumsGenres" VALUES (24, 23);
INSERT INTO public."AlbumsGenres" VALUES (24, 24);
INSERT INTO public."AlbumsGenres" VALUES (25, 25);
INSERT INTO public."AlbumsGenres" VALUES (25, 27);
INSERT INTO public."AlbumsGenres" VALUES (25, 28);
INSERT INTO public."AlbumsGenres" VALUES (25, 30);
INSERT INTO public."AlbumsGenres" VALUES (26, 56);
INSERT INTO public."AlbumsGenres" VALUES (26, 43);
INSERT INTO public."AlbumsGenres" VALUES (26, 69);
INSERT INTO public."AlbumsGenres" VALUES (26, 45);
INSERT INTO public."AlbumsGenres" VALUES (26, 44);
INSERT INTO public."AlbumsGenres" VALUES (27, 25);
INSERT INTO public."AlbumsGenres" VALUES (27, 19);
INSERT INTO public."AlbumsGenres" VALUES (27, 59);
INSERT INTO public."AlbumsGenres" VALUES (28, 25);
INSERT INTO public."AlbumsGenres" VALUES (28, 19);
INSERT INTO public."AlbumsGenres" VALUES (28, 59);
INSERT INTO public."AlbumsGenres" VALUES (29, 25);
INSERT INTO public."AlbumsGenres" VALUES (29, 34);
INSERT INTO public."AlbumsGenres" VALUES (30, 12);
INSERT INTO public."AlbumsGenres" VALUES (30, 11);
INSERT INTO public."AlbumsGenres" VALUES (31, 13);
INSERT INTO public."AlbumsGenres" VALUES (31, 46);
INSERT INTO public."AlbumsGenres" VALUES (31, 12);
INSERT INTO public."AlbumsGenres" VALUES (31, 61);
INSERT INTO public."AlbumsGenres" VALUES (31, 23);
INSERT INTO public."AlbumsGenres" VALUES (32, 25);
INSERT INTO public."AlbumsGenres" VALUES (32, 19);
INSERT INTO public."AlbumsGenres" VALUES (32, 59);
INSERT INTO public."AlbumsGenres" VALUES (32, 27);
INSERT INTO public."AlbumsGenres" VALUES (33, 23);
INSERT INTO public."AlbumsGenres" VALUES (33, 48);
INSERT INTO public."AlbumsGenres" VALUES (33, 24);
INSERT INTO public."AlbumsGenres" VALUES (33, 14);
INSERT INTO public."AlbumsGenres" VALUES (34, 45);
INSERT INTO public."AlbumsGenres" VALUES (34, 43);
INSERT INTO public."AlbumsGenres" VALUES (34, 44);
INSERT INTO public."AlbumsGenres" VALUES (34, 3);
INSERT INTO public."AlbumsGenres" VALUES (34, 58);
INSERT INTO public."AlbumsGenres" VALUES (35, 23);
INSERT INTO public."AlbumsGenres" VALUES (35, 24);
INSERT INTO public."AlbumsGenres" VALUES (36, 29);
INSERT INTO public."AlbumsGenres" VALUES (36, 2);
INSERT INTO public."AlbumsGenres" VALUES (36, 62);
INSERT INTO public."AlbumsGenres" VALUES (38, 42);
INSERT INTO public."AlbumsGenres" VALUES (40, 52);
INSERT INTO public."AlbumsGenres" VALUES (41, 13);
INSERT INTO public."AlbumsGenres" VALUES (41, 46);
INSERT INTO public."AlbumsGenres" VALUES (41, 61);
INSERT INTO public."AlbumsGenres" VALUES (43, 23);
INSERT INTO public."AlbumsGenres" VALUES (43, 24);
INSERT INTO public."AlbumsGenres" VALUES (44, 50);
INSERT INTO public."AlbumsGenres" VALUES (45, 7);
INSERT INTO public."AlbumsGenres" VALUES (45, 35);
INSERT INTO public."AlbumsGenres" VALUES (45, 25);
INSERT INTO public."AlbumsGenres" VALUES (45, 8);


--
-- TOC entry 5022 (class 0 OID 17220)
-- Dependencies: 225
-- Data for Name: Artists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (2, 'AL-90', 1, 'Исполнитель AL-90 известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab67616d0000b2738fe600c89360e4a56ac26c7d');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (3, 'Skyth', 1, 'Исполнитель Skyth известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb9c0ccd821d055f7670190408');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (4, 'Sewerslvt', 1, 'Исполнитель Sewerslvt известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb8a1271f7f32e5202924ebff2');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (5, 'Cynthoni', 1, 'Исполнитель Cynthoni известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb068c68cd72c01daf38873103');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (6, 'Boom Kitty', 1, 'Исполнитель Boom Kitty известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb0537e4e52505684d70275b45');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (7, 'CHOMPO', 1, 'Исполнитель CHOMPO известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebab8bdcdcebb63c80fb868a75');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (8, 'Atomic Heart', 1, 'Исполнитель Atomic Heart известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb5b9d4a3ca752b33bf6d8a690');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (9, 'Geoffplaysguitar', 1, 'Исполнитель Geoffplaysguitar известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebc80b2654f9c8ff1eeac4a1cd');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (10, 'frenetic virtual orchestra', 1, 'Исполнитель frenetic virtual orchestra известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab67616d0000b273caf25a695ad72d5fa1641d01');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (11, 'Юлия Коган', 1, 'Исполнитель Юлия Коган известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab67616d0000b27359c1339003364d093ead463f');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (12, 'Solter', 1, 'Исполнитель Solter известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb20b33946a5d8579f780d6d7c');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (13, 'Kurai mizu', 1, 'Исполнитель Kurai mizu известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb52980e5c7d56a2b51fd5914d');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (14, 'Trivium', 1, 'Исполнитель Trivium известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebab21a41e346730ff64be53c2');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (15, 'Matthew K. Heafy', 1, 'Исполнитель Matthew K. Heafy известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb1d878a14af100015aae655c2');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (16, 'Kevin Sherwood', 1, 'Исполнитель Kevin Sherwood известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb82a01d3b0a1890a78e9e527a');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (17, 'Ladytron', 1, 'Исполнитель Ladytron известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebb948a9242e7ff017fb0d1173');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (18, 'Koven', 1, 'Исполнитель Koven известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb558339d8bae95cd33a20c4c7');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (19, 'Nedaj', 1, 'Исполнитель Nedaj известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb11586ee95f9e07f47252d1f6');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (20, 'GG Magree', 1, 'Исполнитель GG Magree известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebf784e061371be04d98b542e5');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (21, 'RIOT', 1, 'Исполнитель RIOT известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebd459730d3fb832bc9e18113e');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (22, 'HEALTH', 1, 'Исполнитель HEALTH известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb5b2bf033db0073228084edb4');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (23, 'ISOxo', 1, 'Исполнитель ISOxo известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb69dea5e9c932833f17b7ddce');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (24, 'Skrillex', 1, 'Исполнитель Skrillex известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebe32002317387b6d659308a94');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (25, 'Magnetude', 1, 'Исполнитель Magnetude известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebfca737f35ace839fea98adcc');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (26, 'Zardonic', 1, 'Исполнитель Zardonic известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebf2112a8bed841437334d935b');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (27, 'CANTERVICE', 1, 'Исполнитель CANTERVICE известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebdc3edcf7e8414d791d224ab4');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (28, 'Naked Flames', 1, 'Исполнитель Naked Flames известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb864a03d162414c8ff3eb7b21');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (29, 'WesGhost', 1, 'Исполнитель WesGhost известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb09935a526cc02e86cc127b5e');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (30, 'Kayzo', 1, 'Исполнитель Kayzo известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb5e72708b64f379843417ddee');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (31, 'Nine Inch Nails', 1, 'Исполнитель Nine Inch Nails известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb752fca27c5b839ea47d5100d');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (32, 'Dr. Nakano', 1, 'Исполнитель Dr. Nakano известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebf243cce1a787d3f6a84397f6');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (33, 'Hong Kong Express', 1, 'Исполнитель Hong Kong Express известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb26fbeeac4e8c45780b3b3a7d');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (34, 'Spor', 1, 'Исполнитель Spor известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb3200487ca708d8587c557783');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (35, 'The Prodigy', 1, 'Исполнитель The Prodigy известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb147841812056c247407811f3');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (36, '仮想夢プラザ', 1, 'Исполнитель 仮想夢プラザ известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebbea822341ca18435964b3a22');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (1, 'Habstrakt', 1, 'Исполнитель Habstrakt известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebd4f9e0206342d45ab98840c2');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (37, 'REEBZ', 1, 'Исполнитель REEBZ известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebe1ddd40d74ec59697d1ea2b9');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (38, 'Hybrid', 1, 'Исполнитель Hybrid известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb2d875cc2e54d7270f8d05f7e');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (39, 'UNKLE', 1, 'Исполнитель UNKLE известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebac89f1cb4f6881854f55f761');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (40, 'Sebotage', 1, 'Исполнитель Sebotage известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebe8500fa9b789bfc15faf1c26');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (41, 'Toronto Is Broken', 1, 'Исполнитель Toronto Is Broken известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5ebd7e5abc71f1fec13033a5baa');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (42, 'Underworld', 1, 'Исполнитель Underworld известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/ab6761610000e5eb5cfa742c32b2f8c23f94cf7e');
INSERT INTO public."Artists" OVERRIDING SYSTEM VALUE VALUES (43, 'Recoil', 1, 'Исполнитель Recoil известен своими треками на Spotify.', 2000, 2025, 'https://i.scdn.co/image/8a55f8b98045192cf6f96cdaf0a10a87a1333209');


--
-- TOC entry 5039 (class 0 OID 17366)
-- Dependencies: 242
-- Data for Name: ArtistsGenres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."ArtistsGenres" VALUES (1, 26);
INSERT INTO public."ArtistsGenres" VALUES (1, 39);
INSERT INTO public."ArtistsGenres" VALUES (1, 43);
INSERT INTO public."ArtistsGenres" VALUES (1, 65);
INSERT INTO public."ArtistsGenres" VALUES (2, 9);
INSERT INTO public."ArtistsGenres" VALUES (2, 10);
INSERT INTO public."ArtistsGenres" VALUES (2, 31);
INSERT INTO public."ArtistsGenres" VALUES (3, 5);
INSERT INTO public."ArtistsGenres" VALUES (3, 6);
INSERT INTO public."ArtistsGenres" VALUES (3, 22);
INSERT INTO public."ArtistsGenres" VALUES (3, 32);
INSERT INTO public."ArtistsGenres" VALUES (3, 47);
INSERT INTO public."ArtistsGenres" VALUES (4, 5);
INSERT INTO public."ArtistsGenres" VALUES (4, 6);
INSERT INTO public."ArtistsGenres" VALUES (4, 22);
INSERT INTO public."ArtistsGenres" VALUES (5, 20);
INSERT INTO public."ArtistsGenres" VALUES (5, 39);
INSERT INTO public."ArtistsGenres" VALUES (5, 65);
INSERT INTO public."ArtistsGenres" VALUES (6, 9);
INSERT INTO public."ArtistsGenres" VALUES (6, 10);
INSERT INTO public."ArtistsGenres" VALUES (6, 12);
INSERT INTO public."ArtistsGenres" VALUES (7, 5);
INSERT INTO public."ArtistsGenres" VALUES (7, 6);
INSERT INTO public."ArtistsGenres" VALUES (7, 22);
INSERT INTO public."ArtistsGenres" VALUES (8, 4);
INSERT INTO public."ArtistsGenres" VALUES (8, 21);
INSERT INTO public."ArtistsGenres" VALUES (8, 67);
INSERT INTO public."ArtistsGenres" VALUES (9, 9);
INSERT INTO public."ArtistsGenres" VALUES (9, 10);
INSERT INTO public."ArtistsGenres" VALUES (9, 12);
INSERT INTO public."ArtistsGenres" VALUES (9, 36);
INSERT INTO public."ArtistsGenres" VALUES (9, 63);
INSERT INTO public."ArtistsGenres" VALUES (10, 5);
INSERT INTO public."ArtistsGenres" VALUES (10, 6);
INSERT INTO public."ArtistsGenres" VALUES (10, 14);
INSERT INTO public."ArtistsGenres" VALUES (10, 22);
INSERT INTO public."ArtistsGenres" VALUES (10, 24);
INSERT INTO public."ArtistsGenres" VALUES (10, 25);
INSERT INTO public."ArtistsGenres" VALUES (10, 46);
INSERT INTO public."ArtistsGenres" VALUES (11, 15);
INSERT INTO public."ArtistsGenres" VALUES (11, 67);
INSERT INTO public."ArtistsGenres" VALUES (12, 67);
INSERT INTO public."ArtistsGenres" VALUES (13, 1);
INSERT INTO public."ArtistsGenres" VALUES (13, 43);
INSERT INTO public."ArtistsGenres" VALUES (13, 53);
INSERT INTO public."ArtistsGenres" VALUES (13, 57);
INSERT INTO public."ArtistsGenres" VALUES (13, 60);
INSERT INTO public."ArtistsGenres" VALUES (14, 19);
INSERT INTO public."ArtistsGenres" VALUES (14, 25);
INSERT INTO public."ArtistsGenres" VALUES (14, 27);
INSERT INTO public."ArtistsGenres" VALUES (14, 59);
INSERT INTO public."ArtistsGenres" VALUES (16, 13);
INSERT INTO public."ArtistsGenres" VALUES (18, 5);
INSERT INTO public."ArtistsGenres" VALUES (18, 6);
INSERT INTO public."ArtistsGenres" VALUES (18, 19);
INSERT INTO public."ArtistsGenres" VALUES (18, 22);
INSERT INTO public."ArtistsGenres" VALUES (18, 43);
INSERT INTO public."ArtistsGenres" VALUES (18, 53);
INSERT INTO public."ArtistsGenres" VALUES (19, 5);
INSERT INTO public."ArtistsGenres" VALUES (19, 6);
INSERT INTO public."ArtistsGenres" VALUES (19, 22);
INSERT INTO public."ArtistsGenres" VALUES (19, 24);
INSERT INTO public."ArtistsGenres" VALUES (20, 25);
INSERT INTO public."ArtistsGenres" VALUES (20, 27);
INSERT INTO public."ArtistsGenres" VALUES (20, 28);
INSERT INTO public."ArtistsGenres" VALUES (20, 30);
INSERT INTO public."ArtistsGenres" VALUES (21, 6);
INSERT INTO public."ArtistsGenres" VALUES (21, 27);
INSERT INTO public."ArtistsGenres" VALUES (21, 40);
INSERT INTO public."ArtistsGenres" VALUES (21, 64);
INSERT INTO public."ArtistsGenres" VALUES (22, 40);
INSERT INTO public."ArtistsGenres" VALUES (22, 43);
INSERT INTO public."ArtistsGenres" VALUES (22, 53);
INSERT INTO public."ArtistsGenres" VALUES (22, 55);
INSERT INTO public."ArtistsGenres" VALUES (22, 60);
INSERT INTO public."ArtistsGenres" VALUES (22, 68);
INSERT INTO public."ArtistsGenres" VALUES (23, 19);
INSERT INTO public."ArtistsGenres" VALUES (23, 25);
INSERT INTO public."ArtistsGenres" VALUES (23, 59);
INSERT INTO public."ArtistsGenres" VALUES (24, 6);
INSERT INTO public."ArtistsGenres" VALUES (24, 25);
INSERT INTO public."ArtistsGenres" VALUES (24, 33);
INSERT INTO public."ArtistsGenres" VALUES (25, 5);
INSERT INTO public."ArtistsGenres" VALUES (25, 6);
INSERT INTO public."ArtistsGenres" VALUES (25, 12);
INSERT INTO public."ArtistsGenres" VALUES (25, 13);
INSERT INTO public."ArtistsGenres" VALUES (25, 22);
INSERT INTO public."ArtistsGenres" VALUES (25, 46);
INSERT INTO public."ArtistsGenres" VALUES (25, 61);
INSERT INTO public."ArtistsGenres" VALUES (26, 5);
INSERT INTO public."ArtistsGenres" VALUES (26, 6);
INSERT INTO public."ArtistsGenres" VALUES (26, 14);
INSERT INTO public."ArtistsGenres" VALUES (26, 22);
INSERT INTO public."ArtistsGenres" VALUES (26, 24);
INSERT INTO public."ArtistsGenres" VALUES (26, 32);
INSERT INTO public."ArtistsGenres" VALUES (26, 47);
INSERT INTO public."ArtistsGenres" VALUES (27, 1);
INSERT INTO public."ArtistsGenres" VALUES (27, 16);
INSERT INTO public."ArtistsGenres" VALUES (27, 29);
INSERT INTO public."ArtistsGenres" VALUES (27, 62);
INSERT INTO public."ArtistsGenres" VALUES (29, 18);
INSERT INTO public."ArtistsGenres" VALUES (30, 38);
INSERT INTO public."ArtistsGenres" VALUES (30, 53);
INSERT INTO public."ArtistsGenres" VALUES (30, 54);
INSERT INTO public."ArtistsGenres" VALUES (31, 42);
INSERT INTO public."ArtistsGenres" VALUES (32, 17);
INSERT INTO public."ArtistsGenres" VALUES (32, 40);
INSERT INTO public."ArtistsGenres" VALUES (32, 68);
INSERT INTO public."ArtistsGenres" VALUES (37, 6);
INSERT INTO public."ArtistsGenres" VALUES (37, 51);
INSERT INTO public."ArtistsGenres" VALUES (38, 25);
INSERT INTO public."ArtistsGenres" VALUES (39, 13);
INSERT INTO public."ArtistsGenres" VALUES (39, 46);
INSERT INTO public."ArtistsGenres" VALUES (39, 61);
INSERT INTO public."ArtistsGenres" VALUES (40, 13);
INSERT INTO public."ArtistsGenres" VALUES (40, 41);
INSERT INTO public."ArtistsGenres" VALUES (40, 46);
INSERT INTO public."ArtistsGenres" VALUES (40, 61);
INSERT INTO public."ArtistsGenres" VALUES (42, 40);
INSERT INTO public."ArtistsGenres" VALUES (42, 49);
INSERT INTO public."ArtistsGenres" VALUES (43, 6);
INSERT INTO public."ArtistsGenres" VALUES (43, 8);
INSERT INTO public."ArtistsGenres" VALUES (43, 25);
INSERT INTO public."ArtistsGenres" VALUES (43, 35);
INSERT INTO public."ArtistsGenres" VALUES (43, 40);


--
-- TOC entry 5024 (class 0 OID 17228)
-- Dependencies: 227
-- Data for Name: Countries; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Countries" OVERRIDING SYSTEM VALUE VALUES (1, 'Unknown');


--
-- TOC entry 5017 (class 0 OID 17184)
-- Dependencies: 220
-- Data for Name: Genres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (1, 'alternative');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (2, 'alternative dance');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (3, 'alternative metal');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (4, 'ambient');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (5, 'and');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (6, 'bass');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (7, 'bass house');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (8, 'bassline');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (9, 'beat');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (10, 'big');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (11, 'big beat');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (12, 'breakbeat');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (13, 'breakcore');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (14, 'chillstep');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (15, 'chillwave');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (16, 'dance');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (17, 'darkwave');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (18, 'deathcore');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (19, 'deathstep');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (20, 'downtempo');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (21, 'drone');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (22, 'drum');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (23, 'drum and bass');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (24, 'drumstep');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (25, 'dubstep');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (26, 'ebm');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (27, 'edm');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (28, 'electro');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (29, 'electroclash');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (30, 'electronic');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (31, 'electronica');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (32, 'funk');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (33, 'future');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (34, 'future bass');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (35, 'g-house');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (36, 'hardcore');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (37, 'hardcore techno');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (38, 'heavy');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (39, 'hop');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (40, 'house');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (41, 'hyperpop');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (42, 'idm');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (43, 'industrial');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (44, 'industrial metal');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (45, 'industrial rock');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (46, 'jungle');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (47, 'liquid');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (48, 'liquid funk');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (49, 'lo-fi');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (50, 'lo-fi house');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (51, 'melodic');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (52, 'melodic bass');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (53, 'metal');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (54, 'metalcore');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (55, 'noise');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (56, 'noise rock');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (57, 'nu');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (58, 'nu metal');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (59, 'riddim');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (60, 'rock');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (61, 'speedcore');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (62, 'synthpop');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (63, 'techno');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (64, 'trap');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (65, 'trip');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (66, 'trip hop');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (67, 'vaporwave');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (68, 'witch');
INSERT INTO public."Genres" OVERRIDING SYSTEM VALUE VALUES (69, 'witch house');


--
-- TOC entry 5027 (class 0 OID 17261)
-- Dependencies: 230
-- Data for Name: Playlists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Playlists" OVERRIDING SYSTEM VALUE VALUES (1, 'Best of 2025', 4, '2025-05-08', 85456, '');
INSERT INTO public."Playlists" OVERRIDING SYSTEM VALUE VALUES (2, 'Death Notices Full OST', 1, '2023-10-29', 81189, '');
INSERT INTO public."Playlists" OVERRIDING SYSTEM VALUE VALUES (3, 'Means A Lot To Me', 2, '2024-10-20', 4863, '');


--
-- TOC entry 5031 (class 0 OID 17291)
-- Dependencies: 234
-- Data for Name: PlaylistsTags; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."PlaylistsTags" VALUES (1, 2);
INSERT INTO public."PlaylistsTags" VALUES (1, 5);
INSERT INTO public."PlaylistsTags" VALUES (1, 6);
INSERT INTO public."PlaylistsTags" VALUES (2, 1);
INSERT INTO public."PlaylistsTags" VALUES (2, 2);
INSERT INTO public."PlaylistsTags" VALUES (2, 6);
INSERT INTO public."PlaylistsTags" VALUES (3, 3);
INSERT INTO public."PlaylistsTags" VALUES (3, 2);
INSERT INTO public."PlaylistsTags" VALUES (3, 4);


--
-- TOC entry 5030 (class 0 OID 17276)
-- Dependencies: 233
-- Data for Name: PlaylistsTracks; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 5038 (class 0 OID 17344)
-- Dependencies: 241
-- Data for Name: Subscriptions; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Subscriptions" OVERRIDING SYSTEM VALUE VALUES (1, 'Free');
INSERT INTO public."Subscriptions" OVERRIDING SYSTEM VALUE VALUES (2, 'Premium');


--
-- TOC entry 5029 (class 0 OID 17269)
-- Dependencies: 232
-- Data for Name: Tags; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Tags" OVERRIDING SYSTEM VALUE VALUES (1, 'rock');
INSERT INTO public."Tags" OVERRIDING SYSTEM VALUE VALUES (2, 'chill');
INSERT INTO public."Tags" OVERRIDING SYSTEM VALUE VALUES (3, 'pop');
INSERT INTO public."Tags" OVERRIDING SYSTEM VALUE VALUES (4, 'workout');
INSERT INTO public."Tags" OVERRIDING SYSTEM VALUE VALUES (5, 'focus');
INSERT INTO public."Tags" OVERRIDING SYSTEM VALUE VALUES (6, 'study');


--
-- TOC entry 5015 (class 0 OID 17176)
-- Dependencies: 218
-- Data for Name: Tracks; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (1, 'As Alive As You Need Me To Be', 1, '00:03:53', '2025-07-17', 192, 'https://open.spotify.com/track/1xsEHo7mtGZLEG94vFX11z', 3.55, 71834289);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (2, 'At The Heart Of It All', 14, '00:07:14', '1995-01-01', 320, 'https://open.spotify.com/track/4JQEQOu8YKbdtMzVWADAAb', 3.40, 78182712);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (3, 'Baby', 3, '00:03:56', '2025-02-28', 280, 'https://open.spotify.com/track/1syQiwibqZKAUsgkLn8qtC', 4.94, 42250505);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (4, 'Beautiful Burnout', 30, '00:08:09', '2007-01-01', 280, 'https://open.spotify.com/track/1I61h0TNyzE86dx1tZzj3b', 3.53, 14678550);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (5, 'Beautiful Burnout', 29, '00:08:09', '2007-01-01', 280, 'https://open.spotify.com/track/1I61h0TNyzE86dx1tZzj3b', 3.53, 14678550);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (6, 'Beautiful Burnout', 28, '00:08:09', '2007-01-01', 280, 'https://open.spotify.com/track/1I61h0TNyzE86dx1tZzj3b', 3.53, 14678550);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (7, 'Breed', 6, '00:03:58', '2023-05-12', 280, 'https://open.spotify.com/track/5cQwBnsRxUhbKDK2C6fR4n', 3.17, 67607976);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (8, 'Circle (feat. Sebotage)', 10, '00:04:07', '2022-03-30', 320, 'https://open.spotify.com/track/2aTXg7Ch2EuKpa7m8Z8GSv', 3.25, 16908411);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (9, 'Data Destruction', 11, '00:03:20', '2025-01-30', 320, 'https://open.spotify.com/track/5UM2IIULlGyvYHm4SF3hHI', 3.57, 79887130);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (10, 'Dig', 7, '00:03:49', '2025-01-31', 192, 'https://open.spotify.com/track/7nVIE2M0t4sIh3ZkEqrjlY', 3.02, 30268024);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (11, 'Era', 24, '00:03:17', '2025-04-18', 280, 'https://open.spotify.com/track/1rxxl2R2tU1Uv1oP2bbeUn', 3.99, 54797402);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (12, 'Eye for an Eye', 26, '00:05:45', '2003-01-01', 280, 'https://open.spotify.com/track/6ReswWJnalmGTxyRSTmUkl', 4.83, 65817538);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (13, 'Final Destination', 12, '00:04:50', '2025-02-24', 256, 'https://open.spotify.com/track/7bqSNB1C3tWOUgOQTrODAG', 2.52, 31048640);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (14, 'Formula Of Fear', 13, '00:07:07', '2008-09-22', 256, 'https://open.spotify.com/track/0fj2n9wzkojYUy80rOFsLp', 4.64, 97311814);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (15, 'Freeze', 5, '00:07:28', '1992-01-01', 320, 'https://open.spotify.com/track/7KhV0aeGLhI2G1TC5bpgBe', 4.04, 86513228);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (16, 'Fuze', 15, '00:03:08', '2025-10-24', 256, 'https://open.spotify.com/track/5UZIVxzI4UyrSbg3ZLTGTH', 4.05, 62355472);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (17, 'Ghost City Daze', 16, '00:03:39', '2022-09-02', 280, 'https://open.spotify.com/track/5Npj2l7dvTvICHnh6xwNTU', 4.93, 14408959);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (18, 'Glam Bucket', 28, '00:05:45', '2007-01-01', 256, 'https://open.spotify.com/track/7k5rnk06tw2kUOj7AQXkha', 4.46, 36747640);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (19, 'Glam Bucket', 30, '00:05:45', '2007-01-01', 256, 'https://open.spotify.com/track/7k5rnk06tw2kUOj7AQXkha', 4.46, 36747640);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (20, 'Glam Bucket', 29, '00:05:45', '2007-01-01', 256, 'https://open.spotify.com/track/7k5rnk06tw2kUOj7AQXkha', 4.46, 36747640);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (21, 'GOFASTER', 18, '00:04:40', '2025-09-17', 256, 'https://open.spotify.com/track/0WpMAaQq61chei1VkqKGWv', 4.19, 4552008);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (22, 'Grave', 19, '00:02:39', '2025-01-31', 320, 'https://open.spotify.com/track/3TbzrpJIkSoicguKWNHcSh', 3.04, 77772777);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (23, 'I See Red', 20, '00:03:50', '2025-10-17', 192, 'https://open.spotify.com/track/51WgQlBgMnf1lDxBfZkJsa', 3.91, 79852612);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (24, 'If Rah', 4, '00:07:12', '2016-03-19', 256, 'https://open.spotify.com/track/15j4iuXi1OZfSBKmaVTeN7', 3.67, 33608103);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (25, 'Intruders', 37, '00:11:36', '2007-01-01', 320, 'https://open.spotify.com/track/0zU2OXXBlRj8xIB1mc0jPN', 3.32, 21712928);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (26, 'Lvstlove', 22, '00:06:06', '2025-01-03', 192, 'https://open.spotify.com/track/3INsVctSfXaU6rdPBSPalA', 2.98, 97856870);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (27, 'March of the Machine', 40, '00:04:07', '2025-08-01', 192, 'https://open.spotify.com/track/2ZhSXdApy452LqJFf7aEwz', 3.70, 60148993);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (28, 'Metalicious', 23, '00:06:00', '2025-06-27', 280, 'https://open.spotify.com/track/2PdmLVo9VOPhlySbMXH6M0', 3.92, 30476401);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (29, 'Muleta', 25, '00:04:38', '2025-04-11', 192, 'https://open.spotify.com/track/28zBEKDSGAny9y6KrBoCF4', 3.95, 59329189);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (30, 'Nasty - Spor Remix', 38, '00:05:09', '2015-11-10', 280, 'https://open.spotify.com/track/4SXdAtwtoVCZdUP9mfFFqG', 3.08, 47018173);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (31, 'Obelisk', 8, '00:02:45', '2025-02-27', 256, 'https://open.spotify.com/track/0UCcUFxxlHgaQGojhxzECF', 4.55, 14134899);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (32, 'ONYX', 27, '00:05:12', '2025-04-11', 280, 'https://open.spotify.com/track/0yuIvkFLeSxFBSyyJBiWVV', 4.60, 36864477);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (33, 'Orbit', 9, '00:05:25', '2012-09-07', 280, 'https://open.spotify.com/track/1bo3aqhoE0q808wF11ENrC', 3.70, 39317838);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (34, 'ORDINARY LOSS', 31, '00:03:53', '2025-09-11', 192, 'https://open.spotify.com/track/4qUrMbUVDxaMZy2zPB6mD7', 3.32, 3437809);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (35, 'Overload 2K', 32, '00:04:24', '2025-10-24', 192, 'https://open.spotify.com/track/2Y68mc2cWkIwBdqlY04J2e', 4.57, 26952175);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (36, 'Repentance', 33, '00:04:30', '2025-08-01', 280, 'https://open.spotify.com/track/12eQwZCKpzIpccXsH4xQZR', 3.36, 96111491);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (37, 'Run', 34, '00:03:23', '2025-08-13', 256, 'https://open.spotify.com/track/0Ov5YTilb4z4nvuTWeSsUu', 3.16, 42701725);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (38, 'Siren Of The Storm', 35, '00:09:15', '2025-01-19', 192, 'https://open.spotify.com/track/7HSZ0epFVfoDBFyMjCKMrP', 3.28, 64176479);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (39, 'SOMEWHEREIBELONG', 36, '00:03:47', '2025-05-02', 320, 'https://open.spotify.com/track/2bja1BK8CyrE5U1YLzPWtQ', 4.86, 39633329);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (40, 'The Big Come Down', 39, '00:04:12', '1999-09-21', 256, 'https://open.spotify.com/track/3cl42VFcRop14wAduvSeaH', 4.56, 80821880);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (41, 'The Great Below', 39, '00:05:17', '1999-09-21', 280, 'https://open.spotify.com/track/7LBWAib31Cthfz2KoI611Z', 3.07, 91263647);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (42, 'The Masquerade - Zardonic & Toronto Is Broken Remix', 41, '00:04:12', '2025-07-24', 192, 'https://open.spotify.com/track/787izJqCRpk9HOgho07jzb', 4.43, 68700455);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (43, 'The Star', 42, '00:04:57', '2025-05-23', 320, 'https://open.spotify.com/track/4EVYNBO90unYQMUkcgaAqS', 4.45, 80447698);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (44, 'To Heal', 29, '00:02:36', '2007-01-01', 320, 'https://open.spotify.com/track/5K0kPszkKvh6yHEI3wiUCZ', 4.22, 7016621);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (45, 'To Heal', 30, '00:02:36', '2007-01-01', 320, 'https://open.spotify.com/track/5K0kPszkKvh6yHEI3wiUCZ', 4.22, 7016621);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (46, 'To Heal', 28, '00:02:36', '2007-01-01', 320, 'https://open.spotify.com/track/5K0kPszkKvh6yHEI3wiUCZ', 4.22, 7016621);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (47, 'Together', 17, '00:10:03', '2020-03-27', 256, 'https://open.spotify.com/track/7iiu6ce9zGfbvs0yBvsZeT', 3.90, 31154483);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (48, 'Undisputed Altitude', 43, '00:11:00', '2025-03-14', 256, 'https://open.spotify.com/track/3K5td8Q9ezKgBjpiiuJDL9', 3.01, 32413023);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (49, 'Want', 21, '00:06:09', '2000-03-21', 192, 'https://open.spotify.com/track/6AFz1yXS7boPZlqJd2R7WS', 4.46, 19591519);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (50, 'В синем море, в белой пене (Оставайся, мальчик, с нами) [Geoffrey Day Remix]', 2, '00:05:10', '2025-01-29', 280, 'https://open.spotify.com/track/4zW0D21eynN6JcDwGR4f9F', 3.52, 71054588);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (51, 'общедомовой', 44, '00:05:07', '2025-02-14', 320, 'https://open.spotify.com/track/1QVlzjQU2RI4GpVen3x4Ro', 4.77, 17722035);
INSERT INTO public."Tracks" OVERRIDING SYSTEM VALUE VALUES (52, 'あなたを待って', 45, '00:31:34', '2015-05-16', 192, 'https://open.spotify.com/track/3GwYAO8fAxs5k0KiU2hpBP', 3.26, 32425960);


--
-- TOC entry 5040 (class 0 OID 17512)
-- Dependencies: 243
-- Data for Name: TracksArtists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."TracksArtists" VALUES (1, 43);
INSERT INTO public."TracksArtists" VALUES (2, 42);
INSERT INTO public."TracksArtists" VALUES (3, 43);
INSERT INTO public."TracksArtists" VALUES (4, 42);
INSERT INTO public."TracksArtists" VALUES (5, 41);
INSERT INTO public."TracksArtists" VALUES (5, 40);
INSERT INTO public."TracksArtists" VALUES (6, 42);
INSERT INTO public."TracksArtists" VALUES (7, 39);
INSERT INTO public."TracksArtists" VALUES (8, 38);
INSERT INTO public."TracksArtists" VALUES (9, 41);
INSERT INTO public."TracksArtists" VALUES (9, 37);
INSERT INTO public."TracksArtists" VALUES (10, 36);
INSERT INTO public."TracksArtists" VALUES (11, 35);
INSERT INTO public."TracksArtists" VALUES (11, 34);
INSERT INTO public."TracksArtists" VALUES (12, 42);
INSERT INTO public."TracksArtists" VALUES (13, 33);
INSERT INTO public."TracksArtists" VALUES (13, 32);
INSERT INTO public."TracksArtists" VALUES (14, 31);
INSERT INTO public."TracksArtists" VALUES (15, 31);
INSERT INTO public."TracksArtists" VALUES (16, 31);
INSERT INTO public."TracksArtists" VALUES (17, 43);
INSERT INTO public."TracksArtists" VALUES (18, 31);
INSERT INTO public."TracksArtists" VALUES (19, 38);
INSERT INTO public."TracksArtists" VALUES (20, 41);
INSERT INTO public."TracksArtists" VALUES (20, 40);
INSERT INTO public."TracksArtists" VALUES (20, 37);
INSERT INTO public."TracksArtists" VALUES (21, 41);
INSERT INTO public."TracksArtists" VALUES (21, 37);
INSERT INTO public."TracksArtists" VALUES (21, 40);
INSERT INTO public."TracksArtists" VALUES (22, 30);
INSERT INTO public."TracksArtists" VALUES (22, 29);
INSERT INTO public."TracksArtists" VALUES (23, 28);
INSERT INTO public."TracksArtists" VALUES (24, 27);
INSERT INTO public."TracksArtists" VALUES (24, 26);
INSERT INTO public."TracksArtists" VALUES (24, 41);
INSERT INTO public."TracksArtists" VALUES (25, 25);
INSERT INTO public."TracksArtists" VALUES (26, 24);
INSERT INTO public."TracksArtists" VALUES (26, 23);
INSERT INTO public."TracksArtists" VALUES (27, 22);
INSERT INTO public."TracksArtists" VALUES (28, 21);
INSERT INTO public."TracksArtists" VALUES (29, 21);
INSERT INTO public."TracksArtists" VALUES (30, 20);
INSERT INTO public."TracksArtists" VALUES (31, 38);
INSERT INTO public."TracksArtists" VALUES (32, 19);
INSERT INTO public."TracksArtists" VALUES (33, 30);
INSERT INTO public."TracksArtists" VALUES (34, 18);
INSERT INTO public."TracksArtists" VALUES (35, 31);
INSERT INTO public."TracksArtists" VALUES (36, 25);
INSERT INTO public."TracksArtists" VALUES (37, 17);
INSERT INTO public."TracksArtists" VALUES (38, 16);
INSERT INTO public."TracksArtists" VALUES (38, 15);
INSERT INTO public."TracksArtists" VALUES (38, 14);
INSERT INTO public."TracksArtists" VALUES (39, 13);
INSERT INTO public."TracksArtists" VALUES (39, 12);
INSERT INTO public."TracksArtists" VALUES (40, 11);
INSERT INTO public."TracksArtists" VALUES (40, 10);
INSERT INTO public."TracksArtists" VALUES (40, 9);
INSERT INTO public."TracksArtists" VALUES (40, 8);
INSERT INTO public."TracksArtists" VALUES (41, 7);
INSERT INTO public."TracksArtists" VALUES (41, 6);
INSERT INTO public."TracksArtists" VALUES (42, 5);
INSERT INTO public."TracksArtists" VALUES (42, 4);
INSERT INTO public."TracksArtists" VALUES (43, 3);
INSERT INTO public."TracksArtists" VALUES (43, 6);
INSERT INTO public."TracksArtists" VALUES (44, 25);
INSERT INTO public."TracksArtists" VALUES (45, 2);
INSERT INTO public."TracksArtists" VALUES (46, 1);


--
-- TOC entry 5018 (class 0 OID 17191)
-- Dependencies: 221
-- Data for Name: TracksGenres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."TracksGenres" VALUES (1, 66);
INSERT INTO public."TracksGenres" VALUES (1, 26);
INSERT INTO public."TracksGenres" VALUES (1, 43);
INSERT INTO public."TracksGenres" VALUES (2, 11);
INSERT INTO public."TracksGenres" VALUES (2, 31);
INSERT INTO public."TracksGenres" VALUES (3, 66);
INSERT INTO public."TracksGenres" VALUES (3, 26);
INSERT INTO public."TracksGenres" VALUES (3, 43);
INSERT INTO public."TracksGenres" VALUES (4, 11);
INSERT INTO public."TracksGenres" VALUES (4, 31);
INSERT INTO public."TracksGenres" VALUES (5, 23);
INSERT INTO public."TracksGenres" VALUES (5, 48);
INSERT INTO public."TracksGenres" VALUES (6, 11);
INSERT INTO public."TracksGenres" VALUES (6, 31);
INSERT INTO public."TracksGenres" VALUES (7, 66);
INSERT INTO public."TracksGenres" VALUES (7, 20);
INSERT INTO public."TracksGenres" VALUES (8, 12);
INSERT INTO public."TracksGenres" VALUES (8, 11);
INSERT INTO public."TracksGenres" VALUES (9, 23);
INSERT INTO public."TracksGenres" VALUES (9, 48);
INSERT INTO public."TracksGenres" VALUES (10, 67);
INSERT INTO public."TracksGenres" VALUES (10, 21);
INSERT INTO public."TracksGenres" VALUES (10, 4);
INSERT INTO public."TracksGenres" VALUES (11, 11);
INSERT INTO public."TracksGenres" VALUES (11, 12);
INSERT INTO public."TracksGenres" VALUES (11, 37);
INSERT INTO public."TracksGenres" VALUES (12, 11);
INSERT INTO public."TracksGenres" VALUES (12, 31);
INSERT INTO public."TracksGenres" VALUES (13, 67);
INSERT INTO public."TracksGenres" VALUES (13, 15);
INSERT INTO public."TracksGenres" VALUES (14, 45);
INSERT INTO public."TracksGenres" VALUES (14, 43);
INSERT INTO public."TracksGenres" VALUES (14, 44);
INSERT INTO public."TracksGenres" VALUES (14, 3);
INSERT INTO public."TracksGenres" VALUES (14, 58);
INSERT INTO public."TracksGenres" VALUES (15, 45);
INSERT INTO public."TracksGenres" VALUES (15, 43);
INSERT INTO public."TracksGenres" VALUES (15, 44);
INSERT INTO public."TracksGenres" VALUES (15, 3);
INSERT INTO public."TracksGenres" VALUES (15, 58);
INSERT INTO public."TracksGenres" VALUES (16, 45);
INSERT INTO public."TracksGenres" VALUES (16, 43);
INSERT INTO public."TracksGenres" VALUES (16, 44);
INSERT INTO public."TracksGenres" VALUES (16, 3);
INSERT INTO public."TracksGenres" VALUES (16, 58);
INSERT INTO public."TracksGenres" VALUES (17, 66);
INSERT INTO public."TracksGenres" VALUES (17, 26);
INSERT INTO public."TracksGenres" VALUES (17, 43);
INSERT INTO public."TracksGenres" VALUES (18, 45);
INSERT INTO public."TracksGenres" VALUES (18, 43);
INSERT INTO public."TracksGenres" VALUES (18, 44);
INSERT INTO public."TracksGenres" VALUES (18, 3);
INSERT INTO public."TracksGenres" VALUES (18, 58);
INSERT INTO public."TracksGenres" VALUES (19, 12);
INSERT INTO public."TracksGenres" VALUES (19, 11);
INSERT INTO public."TracksGenres" VALUES (20, 23);
INSERT INTO public."TracksGenres" VALUES (20, 48);
INSERT INTO public."TracksGenres" VALUES (21, 23);
INSERT INTO public."TracksGenres" VALUES (21, 48);
INSERT INTO public."TracksGenres" VALUES (22, 25);
INSERT INTO public."TracksGenres" VALUES (22, 19);
INSERT INTO public."TracksGenres" VALUES (22, 59);
INSERT INTO public."TracksGenres" VALUES (22, 27);
INSERT INTO public."TracksGenres" VALUES (23, 13);
INSERT INTO public."TracksGenres" VALUES (25, 23);
INSERT INTO public."TracksGenres" VALUES (25, 24);
INSERT INTO public."TracksGenres" VALUES (26, 25);
INSERT INTO public."TracksGenres" VALUES (26, 27);
INSERT INTO public."TracksGenres" VALUES (26, 28);
INSERT INTO public."TracksGenres" VALUES (26, 30);
INSERT INTO public."TracksGenres" VALUES (27, 56);
INSERT INTO public."TracksGenres" VALUES (27, 43);
INSERT INTO public."TracksGenres" VALUES (27, 69);
INSERT INTO public."TracksGenres" VALUES (27, 45);
INSERT INTO public."TracksGenres" VALUES (27, 44);
INSERT INTO public."TracksGenres" VALUES (28, 25);
INSERT INTO public."TracksGenres" VALUES (28, 19);
INSERT INTO public."TracksGenres" VALUES (28, 59);
INSERT INTO public."TracksGenres" VALUES (29, 25);
INSERT INTO public."TracksGenres" VALUES (29, 19);
INSERT INTO public."TracksGenres" VALUES (29, 59);
INSERT INTO public."TracksGenres" VALUES (30, 25);
INSERT INTO public."TracksGenres" VALUES (30, 34);
INSERT INTO public."TracksGenres" VALUES (31, 12);
INSERT INTO public."TracksGenres" VALUES (31, 11);
INSERT INTO public."TracksGenres" VALUES (32, 13);
INSERT INTO public."TracksGenres" VALUES (32, 46);
INSERT INTO public."TracksGenres" VALUES (32, 12);
INSERT INTO public."TracksGenres" VALUES (32, 61);
INSERT INTO public."TracksGenres" VALUES (32, 23);
INSERT INTO public."TracksGenres" VALUES (33, 25);
INSERT INTO public."TracksGenres" VALUES (33, 19);
INSERT INTO public."TracksGenres" VALUES (33, 59);
INSERT INTO public."TracksGenres" VALUES (33, 27);
INSERT INTO public."TracksGenres" VALUES (34, 23);
INSERT INTO public."TracksGenres" VALUES (34, 48);
INSERT INTO public."TracksGenres" VALUES (34, 24);
INSERT INTO public."TracksGenres" VALUES (34, 14);
INSERT INTO public."TracksGenres" VALUES (35, 45);
INSERT INTO public."TracksGenres" VALUES (35, 43);
INSERT INTO public."TracksGenres" VALUES (35, 44);
INSERT INTO public."TracksGenres" VALUES (35, 3);
INSERT INTO public."TracksGenres" VALUES (35, 58);
INSERT INTO public."TracksGenres" VALUES (36, 23);
INSERT INTO public."TracksGenres" VALUES (36, 24);
INSERT INTO public."TracksGenres" VALUES (37, 29);
INSERT INTO public."TracksGenres" VALUES (37, 2);
INSERT INTO public."TracksGenres" VALUES (37, 62);
INSERT INTO public."TracksGenres" VALUES (39, 42);
INSERT INTO public."TracksGenres" VALUES (41, 52);
INSERT INTO public."TracksGenres" VALUES (42, 13);
INSERT INTO public."TracksGenres" VALUES (42, 46);
INSERT INTO public."TracksGenres" VALUES (42, 61);
INSERT INTO public."TracksGenres" VALUES (44, 23);
INSERT INTO public."TracksGenres" VALUES (44, 24);
INSERT INTO public."TracksGenres" VALUES (45, 50);
INSERT INTO public."TracksGenres" VALUES (46, 7);
INSERT INTO public."TracksGenres" VALUES (46, 35);
INSERT INTO public."TracksGenres" VALUES (46, 25);
INSERT INTO public."TracksGenres" VALUES (46, 8);


--
-- TOC entry 5035 (class 0 OID 17316)
-- Dependencies: 238
-- Data for Name: UserRoles; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."UserRoles" OVERRIDING SYSTEM VALUE VALUES (1, 'Admin');
INSERT INTO public."UserRoles" OVERRIDING SYSTEM VALUE VALUES (2, 'Guest');
INSERT INTO public."UserRoles" OVERRIDING SYSTEM VALUE VALUES (3, 'Manager');
INSERT INTO public."UserRoles" OVERRIDING SYSTEM VALUE VALUES (4, 'User');


--
-- TOC entry 5032 (class 0 OID 17306)
-- Dependencies: 235
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (2, 'anjela.privalova', 'mZPXkIBf', 'Анжела Привалова', 'anjela.privalova@rambler.ru', 3, 2, '2025-10-20', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (3, 'antonina.pushmenkova', 'it3cySf5', 'Антонина Пушменкова', 'antonina.pushmenkova@mail.ru', 1, 2, '2025-10-29', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (4, 'apollinariya.allenova', 'sXFdIl0y', 'Аполлинария Алленова', 'apollinariya.allenova@outlook.com', 3, 2, '2025-10-29', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (5, 'asya.vedova', 'z6kkRwhf', 'Ася Шведова', 'asya.vedova@mail.ru', 4, 2, '2025-10-29', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (6, 'kseniya1978', 'bCWtEdaB', 'Ксения Хуторская', 'kseniya1978@rambler.ru', 4, 1, '2025-08-05', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (7, 'leontiy.kozlitin', 'RDJCXFyk', 'Леонтий Козлитин', 'leontiy.kozlitin@ya.ru', 3, 2, '2025-10-28', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (8, 'lyudmila.abakumova', 'E3rg2Dct', 'Людмила Абакумова', 'lyudmila.abakumova@hotmail.com', 4, 2, '2025-09-29', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (9, 'oksana95', 'c35tx4ov', 'Оксана Добронравова', 'oksana95@ya.ru', 2, 1, '2025-10-29', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (10, 'petr1968', 'K5PU52zo', 'Петр Крылаев', 'petr1968@ya.ru', 4, 1, '2025-10-29', '2025-10-29 00:00:00');
INSERT INTO public."Users" OVERRIDING SYSTEM VALUE VALUES (1, 'aleksey.novojilov', '1XF4kEOq', 'Алексей Новожилов', 'aleksey.novojilov@mail.ru', 1, 1, '2025-10-29', '2025-11-13 16:23:43.758856');


--
-- TOC entry 5036 (class 0 OID 17323)
-- Dependencies: 239
-- Data for Name: UsersPlaylists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."UsersPlaylists" VALUES (1, 1);
INSERT INTO public."UsersPlaylists" VALUES (2, 2);
INSERT INTO public."UsersPlaylists" VALUES (3, 1);
INSERT INTO public."UsersPlaylists" VALUES (4, 3);
INSERT INTO public."UsersPlaylists" VALUES (5, 2);
INSERT INTO public."UsersPlaylists" VALUES (6, 3);
INSERT INTO public."UsersPlaylists" VALUES (7, 2);
INSERT INTO public."UsersPlaylists" VALUES (8, 3);
INSERT INTO public."UsersPlaylists" VALUES (9, 2);
INSERT INTO public."UsersPlaylists" VALUES (10, 1);


--
-- TOC entry 5046 (class 0 OID 0)
-- Dependencies: 222
-- Name: Albums_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Albums_Id_seq"', 45, true);


--
-- TOC entry 5047 (class 0 OID 0)
-- Dependencies: 224
-- Name: Artists_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Artists_Id_seq"', 43, true);


--
-- TOC entry 5048 (class 0 OID 0)
-- Dependencies: 226
-- Name: Countries_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Countries_Id_seq"', 1, true);


--
-- TOC entry 5049 (class 0 OID 0)
-- Dependencies: 219
-- Name: Genres_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Genres_Id_seq"', 69, true);


--
-- TOC entry 5050 (class 0 OID 0)
-- Dependencies: 229
-- Name: Playlists_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Playlists_Id_seq"', 3, true);


--
-- TOC entry 5051 (class 0 OID 0)
-- Dependencies: 240
-- Name: Subscriptions_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Subscriptions_Id_seq"', 2, true);


--
-- TOC entry 5052 (class 0 OID 0)
-- Dependencies: 231
-- Name: Tags_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Tags_Id_seq"', 6, true);


--
-- TOC entry 5053 (class 0 OID 0)
-- Dependencies: 217
-- Name: Tracks_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Tracks_Id_seq"', 52, true);


--
-- TOC entry 5054 (class 0 OID 0)
-- Dependencies: 237
-- Name: UserRoles_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."UserRoles_Id_seq"', 4, true);


--
-- TOC entry 5055 (class 0 OID 0)
-- Dependencies: 236
-- Name: Users_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Users_Id_seq"', 10, true);


--
-- TOC entry 4822 (class 2606 OID 17213)
-- Name: Albums _albums__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Albums"
    ADD CONSTRAINT _albums__pk PRIMARY KEY ("Id");


--
-- TOC entry 4824 (class 2606 OID 17226)
-- Name: Artists _artists__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Artists"
    ADD CONSTRAINT _artists__pk PRIMARY KEY ("Id");


--
-- TOC entry 4826 (class 2606 OID 17234)
-- Name: Countries _countries__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Countries"
    ADD CONSTRAINT _countries__pk PRIMARY KEY ("Id");


--
-- TOC entry 4818 (class 2606 OID 17190)
-- Name: Genres _genres__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Genres"
    ADD CONSTRAINT _genres__pk PRIMARY KEY ("Id");


--
-- TOC entry 4830 (class 2606 OID 17267)
-- Name: Playlists _playlists__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Playlists"
    ADD CONSTRAINT _playlists__pk PRIMARY KEY ("Id");


--
-- TOC entry 4836 (class 2606 OID 17295)
-- Name: PlaylistsTags _playliststags__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PlaylistsTags"
    ADD CONSTRAINT _playliststags__pk PRIMARY KEY ("PlaylistId", "TagId");


--
-- TOC entry 4834 (class 2606 OID 17280)
-- Name: PlaylistsTracks _playliststracks__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PlaylistsTracks"
    ADD CONSTRAINT _playliststracks__pk PRIMARY KEY ("PlaylistId", "TrackId");


--
-- TOC entry 4844 (class 2606 OID 17350)
-- Name: Subscriptions _subscriptions__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Subscriptions"
    ADD CONSTRAINT _subscriptions__pk PRIMARY KEY ("Id");


--
-- TOC entry 4832 (class 2606 OID 17275)
-- Name: Tags _tags__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tags"
    ADD CONSTRAINT _tags__pk PRIMARY KEY ("Id");


--
-- TOC entry 4816 (class 2606 OID 17182)
-- Name: Tracks _tracks__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tracks"
    ADD CONSTRAINT _tracks__pk PRIMARY KEY ("Id");


--
-- TOC entry 4848 (class 2606 OID 17516)
-- Name: TracksArtists _tracksartists__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TracksArtists"
    ADD CONSTRAINT _tracksartists__pk PRIMARY KEY ("TrackId", "ArtistId");


--
-- TOC entry 4820 (class 2606 OID 17195)
-- Name: TracksGenres _tracksgenres__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TracksGenres"
    ADD CONSTRAINT _tracksgenres__pk PRIMARY KEY ("TrackId", "GenreId");


--
-- TOC entry 4840 (class 2606 OID 17322)
-- Name: UserRoles _userroles__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserRoles"
    ADD CONSTRAINT _userroles__pk PRIMARY KEY ("Id");


--
-- TOC entry 4838 (class 2606 OID 17312)
-- Name: Users _users__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT _users__pk PRIMARY KEY ("Id");


--
-- TOC entry 4842 (class 2606 OID 17327)
-- Name: UsersPlaylists _usersplaylists__pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UsersPlaylists"
    ADD CONSTRAINT _usersplaylists__pk PRIMARY KEY ("UserId", "PlaylistId");


--
-- TOC entry 4828 (class 2606 OID 17259)
-- Name: AlbumsGenres albumsgenres_pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AlbumsGenres"
    ADD CONSTRAINT albumsgenres_pk PRIMARY KEY ("AlbumId", "GenreId");


--
-- TOC entry 4846 (class 2606 OID 17531)
-- Name: ArtistsGenres artistsgenres_pk; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArtistsGenres"
    ADD CONSTRAINT artistsgenres_pk PRIMARY KEY ("ArtistId", "GenreId");


--
-- TOC entry 4865 (class 2606 OID 17369)
-- Name: ArtistsGenres _artistsgenres__artists_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArtistsGenres"
    ADD CONSTRAINT _artistsgenres__artists_fk FOREIGN KEY ("ArtistId") REFERENCES public."Artists"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4866 (class 2606 OID 17374)
-- Name: ArtistsGenres _artistsgenres__genres_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArtistsGenres"
    ADD CONSTRAINT _artistsgenres__genres_fk FOREIGN KEY ("GenreId") REFERENCES public."Genres"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4859 (class 2606 OID 17296)
-- Name: PlaylistsTags _playliststags__playlists_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PlaylistsTags"
    ADD CONSTRAINT _playliststags__playlists_fk FOREIGN KEY ("PlaylistId") REFERENCES public."Playlists"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4860 (class 2606 OID 17301)
-- Name: PlaylistsTags _playliststags__tags_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PlaylistsTags"
    ADD CONSTRAINT _playliststags__tags_fk FOREIGN KEY ("TagId") REFERENCES public."Tags"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4857 (class 2606 OID 17281)
-- Name: PlaylistsTracks _playliststracks__playlists_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PlaylistsTracks"
    ADD CONSTRAINT _playliststracks__playlists_fk FOREIGN KEY ("PlaylistId") REFERENCES public."Playlists"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4858 (class 2606 OID 17286)
-- Name: PlaylistsTracks _playliststracks__tracks_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PlaylistsTracks"
    ADD CONSTRAINT _playliststracks__tracks_fk FOREIGN KEY ("TrackId") REFERENCES public."Tracks"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4867 (class 2606 OID 17517)
-- Name: TracksArtists _tracksartists__artists_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TracksArtists"
    ADD CONSTRAINT _tracksartists__artists_fk FOREIGN KEY ("ArtistId") REFERENCES public."Artists"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4868 (class 2606 OID 17522)
-- Name: TracksArtists _tracksartists__tracks_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TracksArtists"
    ADD CONSTRAINT _tracksartists__tracks_fk FOREIGN KEY ("TrackId") REFERENCES public."Tracks"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4850 (class 2606 OID 17196)
-- Name: TracksGenres _tracksgenres__genres_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TracksGenres"
    ADD CONSTRAINT _tracksgenres__genres_fk FOREIGN KEY ("GenreId") REFERENCES public."Genres"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4851 (class 2606 OID 17201)
-- Name: TracksGenres _tracksgenres__tracks_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."TracksGenres"
    ADD CONSTRAINT _tracksgenres__tracks_fk FOREIGN KEY ("TrackId") REFERENCES public."Tracks"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4863 (class 2606 OID 17333)
-- Name: UsersPlaylists _usersplaylists__playlists_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UsersPlaylists"
    ADD CONSTRAINT _usersplaylists__playlists_fk FOREIGN KEY ("PlaylistId") REFERENCES public."Playlists"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4864 (class 2606 OID 17328)
-- Name: UsersPlaylists _usersplaylists__users_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UsersPlaylists"
    ADD CONSTRAINT _usersplaylists__users_fk FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4852 (class 2606 OID 17235)
-- Name: Albums albums_artists_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Albums"
    ADD CONSTRAINT albums_artists_fk FOREIGN KEY ("ArtistId") REFERENCES public."Artists"("Id") ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4854 (class 2606 OID 17243)
-- Name: AlbumsGenres albumsgenres_albums_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AlbumsGenres"
    ADD CONSTRAINT albumsgenres_albums_fk FOREIGN KEY ("AlbumId") REFERENCES public."Albums"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4855 (class 2606 OID 17248)
-- Name: AlbumsGenres albumsgenres_genres_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AlbumsGenres"
    ADD CONSTRAINT albumsgenres_genres_fk FOREIGN KEY ("GenreId") REFERENCES public."Genres"("Id") ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4853 (class 2606 OID 17253)
-- Name: Artists artists_countries_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Artists"
    ADD CONSTRAINT artists_countries_fk FOREIGN KEY ("CountryId") REFERENCES public."Countries"("Id") ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4856 (class 2606 OID 17361)
-- Name: Playlists playlists_users_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Playlists"
    ADD CONSTRAINT playlists_users_fk FOREIGN KEY ("CreatorUserId") REFERENCES public."Users"("Id") ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4849 (class 2606 OID 17214)
-- Name: Tracks tracks_albums_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tracks"
    ADD CONSTRAINT tracks_albums_fk FOREIGN KEY ("AlbumId") REFERENCES public."Albums"("Id") ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4861 (class 2606 OID 17351)
-- Name: Users users_subscriptions_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT users_subscriptions_fk FOREIGN KEY ("SubscriptionId") REFERENCES public."Subscriptions"("Id") ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4862 (class 2606 OID 17338)
-- Name: Users users_userroles_fk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT users_userroles_fk FOREIGN KEY ("RoleId") REFERENCES public."UserRoles"("Id") ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2025-11-15 14:30:14

--
-- PostgreSQL database dump complete
--

\unrestrict IKaOgonxLXJLdDV0vRoSiiRRgqIT1ZIx987DcUHE1h3mcUXDnxyxYDe8Jv5xkcn

