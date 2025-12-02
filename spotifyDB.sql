--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17rc1

-- Started on 2025-12-02 17:27:02

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

--
-- TOC entry 3522 (class 1262 OID 5)
-- Name: postgres; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE postgres WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


\connect postgres

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

--
-- TOC entry 3523 (class 0 OID 0)
-- Dependencies: 3522
-- Name: DATABASE postgres; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON DATABASE postgres IS 'default administrative connection database';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 232 (class 1259 OID 16492)
-- Name: Albums; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Albums" (
    album_id integer NOT NULL,
    "albumName" text NOT NULL,
    artist integer,
    "releaseYear" integer NOT NULL,
    "coverPath" text NOT NULL,
    "totalDuration" interval
);


--
-- TOC entry 231 (class 1259 OID 16491)
-- Name: Albums_album_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Albums_album_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3524 (class 0 OID 0)
-- Dependencies: 231
-- Name: Albums_album_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Albums_album_id_seq" OWNED BY public."Albums".album_id;


--
-- TOC entry 228 (class 1259 OID 16444)
-- Name: Artists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Artists" (
    artist_id integer NOT NULL,
    "artistName" text NOT NULL,
    "artistCountry" integer,
    "activeYears" text NOT NULL,
    "artistDescription" text NOT NULL,
    "photoPath" text NOT NULL
);


--
-- TOC entry 227 (class 1259 OID 16443)
-- Name: Artists_artist_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Artists_artist_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3525 (class 0 OID 0)
-- Dependencies: 227
-- Name: Artists_artist_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Artists_artist_id_seq" OWNED BY public."Artists".artist_id;


--
-- TOC entry 236 (class 1259 OID 16523)
-- Name: Genres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Genres" (
    genre_id integer NOT NULL,
    "genreName" text
);


--
-- TOC entry 235 (class 1259 OID 16522)
-- Name: Genres_genre_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Genres_genre_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3526 (class 0 OID 0)
-- Dependencies: 235
-- Name: Genres_genre_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Genres_genre_id_seq" OWNED BY public."Genres".genre_id;


--
-- TOC entry 226 (class 1259 OID 16435)
-- Name: Tags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Tags" (
    tag_id integer NOT NULL,
    "tagName" text NOT NULL
);


--
-- TOC entry 225 (class 1259 OID 16434)
-- Name: Tags_tag_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Tags_tag_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3527 (class 0 OID 0)
-- Dependencies: 225
-- Name: Tags_tag_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Tags_tag_id_seq" OWNED BY public."Tags".tag_id;


--
-- TOC entry 234 (class 1259 OID 16506)
-- Name: Tracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Tracks" (
    track_id integer NOT NULL,
    "trackName" text NOT NULL,
    artist integer,
    info text,
    "trackDuration" interval,
    "releaseDate" date,
    bitrate integer NOT NULL,
    "trackPath" text NOT NULL,
    "albumcoverPath" text NOT NULL,
    "trackRating" numeric(3,2) DEFAULT 0,
    "totalPlays" integer DEFAULT 0
);


--
-- TOC entry 233 (class 1259 OID 16505)
-- Name: Tracks_track_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Tracks_track_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3528 (class 0 OID 0)
-- Dependencies: 233
-- Name: Tracks_track_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Tracks_track_id_seq" OWNED BY public."Tracks".track_id;


--
-- TOC entry 238 (class 1259 OID 16544)
-- Name: albumGenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."albumGenres" (
    "Album" integer,
    "AGenre" integer
);


--
-- TOC entry 243 (class 1259 OID 16622)
-- Name: albumTracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."albumTracks" (
    "TAlbum" integer,
    "ATrack" integer
);


--
-- TOC entry 239 (class 1259 OID 16557)
-- Name: artistGenre; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."artistGenre" (
    "Artist" integer,
    "AGenre" integer
);


--
-- TOC entry 224 (class 1259 OID 16426)
-- Name: countries; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.countries (
    country_id integer NOT NULL,
    "countryName" text NOT NULL
);


--
-- TOC entry 223 (class 1259 OID 16425)
-- Name: countries_country_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.countries_country_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3529 (class 0 OID 0)
-- Dependencies: 223
-- Name: countries_country_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.countries_country_id_seq OWNED BY public.countries.country_id;


--
-- TOC entry 230 (class 1259 OID 16475)
-- Name: playLists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."playLists" (
    playlist_id integer NOT NULL,
    "playlistName" text NOT NULL,
    "userCreator" integer,
    "creationDate" date NOT NULL,
    likes integer DEFAULT 0,
    "playlistDescription" text
);


--
-- TOC entry 229 (class 1259 OID 16474)
-- Name: playLists_playlist_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."playLists_playlist_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3530 (class 0 OID 0)
-- Dependencies: 229
-- Name: playLists_playlist_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."playLists_playlist_id_seq" OWNED BY public."playLists".playlist_id;


--
-- TOC entry 241 (class 1259 OID 16583)
-- Name: playistTags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."playistTags" (
    "Tag" integer,
    "playList" integer
);


--
-- TOC entry 220 (class 1259 OID 16398)
-- Name: subscriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.subscriptions (
    sub_id integer NOT NULL,
    sub_name text NOT NULL
);


--
-- TOC entry 219 (class 1259 OID 16397)
-- Name: subscriptions_sub_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.subscriptions_sub_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3531 (class 0 OID 0)
-- Dependencies: 219
-- Name: subscriptions_sub_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.subscriptions_sub_id_seq OWNED BY public.subscriptions.sub_id;


--
-- TOC entry 240 (class 1259 OID 16570)
-- Name: trackArtists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."trackArtists" (
    "relatedTrack" integer,
    "relatedArtist" integer
);


--
-- TOC entry 237 (class 1259 OID 16531)
-- Name: trackGenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."trackGenres" (
    "Track" integer,
    "TGenre" integer
);


--
-- TOC entry 242 (class 1259 OID 16596)
-- Name: trackPlaylists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."trackPlaylists" (
    track integer,
    playlist integer
);


--
-- TOC entry 244 (class 1259 OID 16635)
-- Name: userPlaylists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."userPlaylists" (
    "User" integer,
    "UPlaylist" integer
);


--
-- TOC entry 218 (class 1259 OID 16389)
-- Name: userRoles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."userRoles" (
    role_id integer NOT NULL,
    role_name text NOT NULL
);


--
-- TOC entry 217 (class 1259 OID 16388)
-- Name: userRoles_role_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."userRoles_role_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3532 (class 0 OID 0)
-- Dependencies: 217
-- Name: userRoles_role_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."userRoles_role_id_seq" OWNED BY public."userRoles".role_id;


--
-- TOC entry 222 (class 1259 OID 16407)
-- Name: users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    "userLogin" character varying(50) NOT NULL,
    "userPassword" character varying(10) NOT NULL,
    fio text NOT NULL,
    "userEMail" character varying(50),
    role integer,
    "subscriptionType" integer,
    "registrationDate" date NOT NULL,
    "lastLogin" date NOT NULL
);


--
-- TOC entry 221 (class 1259 OID 16406)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 3533 (class 0 OID 0)
-- Dependencies: 221
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- TOC entry 3295 (class 2604 OID 16495)
-- Name: Albums album_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Albums" ALTER COLUMN album_id SET DEFAULT nextval('public."Albums_album_id_seq"'::regclass);


--
-- TOC entry 3292 (class 2604 OID 16447)
-- Name: Artists artist_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Artists" ALTER COLUMN artist_id SET DEFAULT nextval('public."Artists_artist_id_seq"'::regclass);


--
-- TOC entry 3299 (class 2604 OID 16526)
-- Name: Genres genre_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Genres" ALTER COLUMN genre_id SET DEFAULT nextval('public."Genres_genre_id_seq"'::regclass);


--
-- TOC entry 3291 (class 2604 OID 16438)
-- Name: Tags tag_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tags" ALTER COLUMN tag_id SET DEFAULT nextval('public."Tags_tag_id_seq"'::regclass);


--
-- TOC entry 3296 (class 2604 OID 16509)
-- Name: Tracks track_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tracks" ALTER COLUMN track_id SET DEFAULT nextval('public."Tracks_track_id_seq"'::regclass);


--
-- TOC entry 3290 (class 2604 OID 16429)
-- Name: countries country_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.countries ALTER COLUMN country_id SET DEFAULT nextval('public.countries_country_id_seq'::regclass);


--
-- TOC entry 3293 (class 2604 OID 16478)
-- Name: playLists playlist_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."playLists" ALTER COLUMN playlist_id SET DEFAULT nextval('public."playLists_playlist_id_seq"'::regclass);


--
-- TOC entry 3288 (class 2604 OID 16401)
-- Name: subscriptions sub_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.subscriptions ALTER COLUMN sub_id SET DEFAULT nextval('public.subscriptions_sub_id_seq'::regclass);


--
-- TOC entry 3287 (class 2604 OID 16392)
-- Name: userRoles role_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."userRoles" ALTER COLUMN role_id SET DEFAULT nextval('public."userRoles_role_id_seq"'::regclass);


--
-- TOC entry 3289 (class 2604 OID 16410)
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- TOC entry 3504 (class 0 OID 16492)
-- Dependencies: 232
-- Data for Name: Albums; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Albums" VALUES (1, 'Liquid (Bonus Track Version)', 1, 2000, 'https://i.scdn.co/image/ab67616d0000b273211808193df8743aedb1038b', '00:00:00');
INSERT INTO public."Albums" VALUES (2, 'Oblivion With Bells', 2, 2007, 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', '00:00:00');
INSERT INTO public."Albums" VALUES (3, 'Subhuman', 1, 2007, 'https://i.scdn.co/image/ab67616d0000b273ce0298116be15cb81f91a1aa', '00:00:00');
INSERT INTO public."Albums" VALUES (4, 'Circle', 3, 2022, 'https://i.scdn.co/image/ab67616d0000b273f0f4f4f180ea0f32ac2c5f8f', '00:00:00');
INSERT INTO public."Albums" VALUES (5, 'Never, Never, Land', 5, 2003, 'https://i.scdn.co/image/ab67616d0000b273894c4f974159696026573eb5', '00:00:00');
INSERT INTO public."Albums" VALUES (6, 'Formula Of Fear', 6, 2008, 'https://i.scdn.co/image/ab67616d0000b27359566bca173ee54657bd5f8d', '00:00:00');
INSERT INTO public."Albums" VALUES (7, 'Breed (feat. REEBZ)', 3, 2023, 'https://i.scdn.co/image/ab67616d0000b2737163994ea19cc74c0bbfb31e', '00:00:00');
INSERT INTO public."Albums" VALUES (8, 'あなたを待って', 8, 2015, 'https://i.scdn.co/image/ab67616d0000b273fcc3cb5cc1166a95b86f5e6e', '00:00:00');
INSERT INTO public."Albums" VALUES (9, 'The Day Is My Enemy (Expanded Edition)', 9, 2015, 'https://i.scdn.co/image/ab67616d0000b273872772e0b165e57a104fd37a', '00:00:00');
INSERT INTO public."Albums" VALUES (10, 'Barbara Barbara, we face a shining future', 2, 2016, 'https://i.scdn.co/image/ab67616d0000b273cf5cdbf0397a8a71c9fd0395', '00:00:00');
INSERT INTO public."Albums" VALUES (11, 'Ghost City Daze', 11, 2022, 'https://i.scdn.co/image/ab67616d0000b273ab65f724c2fdf067999d8d16', '00:00:00');
INSERT INTO public."Albums" VALUES (12, 'The Fragile', 13, 1999, 'https://i.scdn.co/image/ab67616d0000b273237a21c8ba1bfc2d85a83585', '00:00:00');
INSERT INTO public."Albums" VALUES (13, 'Further Down The Spiral', 13, 1995, 'https://i.scdn.co/image/ab67616d0000b273551ecb57cda40ae833e3e5b1', '00:00:00');
INSERT INTO public."Albums" VALUES (14, 'Bloodline (Bonus Tracks)', 1, 1992, 'https://i.scdn.co/image/ab67616d0000b273c74e21dfa7e9470c0c980f9f', '00:00:00');
INSERT INTO public."Albums" VALUES (15, 'Ghosts V: Together', 13, 2020, 'https://i.scdn.co/image/ab67616d0000b27327c5a582320e08258ec13d0a', '00:00:00');
INSERT INTO public."Albums" VALUES (16, 'Cinematic Soundscape', 6, 2012, 'https://i.scdn.co/image/ab67616d0000b27381c4fc91dc2abd939c2c705f', '00:00:00');
INSERT INTO public."Albums" VALUES (17, 'GOFASTER (YANA100)', 3, 2025, 'https://i.scdn.co/image/ab67616d0000b2735ff50f46b8b8720981957d51', '00:00:00');
INSERT INTO public."Albums" VALUES (18, 'SOMEWHEREIBELONG', 3, 2025, 'https://i.scdn.co/image/ab67616d0000b27328dc64c5aa8c59dd37a7d820', '00:00:00');
INSERT INTO public."Albums" VALUES (19, 'Grave', 14, 2025, 'https://i.scdn.co/image/ab67616d0000b273659f2e9dc34715a487f073be', '00:00:00');
INSERT INTO public."Albums" VALUES (20, 'Wall I Was', 16, 2025, 'https://i.scdn.co/image/ab67616d0000b273710d4624a5a1b87b70b56bb2', '00:00:00');
INSERT INTO public."Albums" VALUES (21, 'The Masquerade (Zardonic & Toronto Is Broken Remix)', 17, 2025, 'https://i.scdn.co/image/ab67616d0000b273b9bb9b27f5aeb0e1a1475707', '00:00:00');
INSERT INTO public."Albums" VALUES (22, 'The Star', 19, 2025, 'https://i.scdn.co/image/ab67616d0000b2731e01b0b41fa3bf96bab342cb', '00:00:00');
INSERT INTO public."Albums" VALUES (23, 'Fuze', 20, 2025, 'https://i.scdn.co/image/ab67616d0000b27362d6852a9461f7c5579097c6', '00:00:00');
INSERT INTO public."Albums" VALUES (24, 'ORDINARY LOSS', 22, 2025, 'https://i.scdn.co/image/ab67616d0000b27367bd88cd7a659c93b34b0688', '00:00:00');
INSERT INTO public."Albums" VALUES (25, 'The Machine', 23, 2025, 'https://i.scdn.co/image/ab67616d0000b27328700d650196f5918a0578bf', '00:00:00');
INSERT INTO public."Albums" VALUES (26, 'Metalicious', 23, 2025, 'https://i.scdn.co/image/ab67616d0000b27368e7513b4ca1da14da0a81e6', '00:00:00');
INSERT INTO public."Albums" VALUES (27, 'Run', 24, 2025, 'https://i.scdn.co/image/ab67616d0000b273af743f04ff7cc6ae7300929f', '00:00:00');
INSERT INTO public."Albums" VALUES (28, 'Siren Of The Storm', 6, 2025, 'https://i.scdn.co/image/ab67616d0000b273628b577615d97f6837e30b73', '00:00:00');
INSERT INTO public."Albums" VALUES (29, 'NO MERCY III', 25, 2025, 'https://i.scdn.co/image/ab67616d0000b273a10a1e401fc110168dfd02f3', '00:00:00');
INSERT INTO public."Albums" VALUES (30, 'Overload 2K', 14, 2025, 'https://i.scdn.co/image/ab67616d0000b273a24c69273fb3656b3ecb92a7', '00:00:00');
INSERT INTO public."Albums" VALUES (31, 'Moments In Everglow', 26, 2025, 'https://i.scdn.co/image/ab67616d0000b27379edc5965038d4923dff96b0', '00:00:00');
INSERT INTO public."Albums" VALUES (32, 'As Alive As You Need Me To Be', 13, 2025, 'https://i.scdn.co/image/ab67616d0000b27317ff2581a6b924fb3a704395', '00:00:00');
INSERT INTO public."Albums" VALUES (33, 'Repentance', 19, 2025, 'https://i.scdn.co/image/ab67616d0000b2730f423970a440915f6750e11e', '00:00:00');
INSERT INTO public."Albums" VALUES (34, 'I See Red', 27, 2025, 'https://i.scdn.co/image/ab67616d0000b2737f34486112bf81e38838a429', '00:00:00');
INSERT INTO public."Albums" VALUES (35, 'Call of Duty®: Black Ops 6 - Zombies "The Tomb" (Original Soundtrack)', 28, 2025, 'https://i.scdn.co/image/ab67616d0000b2732db3154893852b63232e1466', '00:00:00');
INSERT INTO public."Albums" VALUES (36, 'Final Destination', 31, 2025, 'https://i.scdn.co/image/ab67616d0000b273857bb2eff1c145e26e9a2c1c', '00:00:00');
INSERT INTO public."Albums" VALUES (37, 'Atomic Heart, Vol.5 (Original Game Soundtrack)', 36, 2025, 'https://i.scdn.co/image/ab67616d0000b2731a24aab10d2b27d63175c37a', '00:00:00');
INSERT INTO public."Albums" VALUES (38, 'CHOMPO III', 37, 2025, 'https://i.scdn.co/image/ab67616d0000b273aadd94c74f32483d503fb0ab', '00:00:00');
INSERT INTO public."Albums" VALUES (39, 'Lvstlove', 39, 2025, 'https://i.scdn.co/image/ab67616d0000b2737348b951edbf4bb86854e806', '00:00:00');
INSERT INTO public."Albums" VALUES (40, 'Data Destruction', 41, 2025, 'https://i.scdn.co/image/ab67616d0000b273c6ad3e09b75355deee5b732d', '00:00:00');
INSERT INTO public."Albums" VALUES (41, 'Muleta', 19, 2025, 'https://i.scdn.co/image/ab67616d0000b273adebc14e80184ee7cc2e36b8', '00:00:00');
INSERT INTO public."Albums" VALUES (42, 'общедомовой', 42, 2025, 'https://i.scdn.co/image/ab67616d0000b273f11a5383c11039c003e9008b', '00:00:00');
INSERT INTO public."Albums" VALUES (43, 'Baby', 43, 2025, 'https://i.scdn.co/image/ab67616d0000b273fc679c0d97b173113141a42b', '00:00:00');


--
-- TOC entry 3500 (class 0 OID 16444)
-- Dependencies: 228
-- Data for Name: Artists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Artists" VALUES (1, 'Recoil', NULL, '2000–2025', 'Исполнитель Recoil известен своими треками на Spotify.', 'https://i.scdn.co/image/8a55f8b98045192cf6f96cdaf0a10a87a1333209');
INSERT INTO public."Artists" VALUES (2, 'Underworld', NULL, '2000–2025', 'Исполнитель Underworld известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb5cfa742c32b2f8c23f94cf7e');
INSERT INTO public."Artists" VALUES (3, 'Toronto Is Broken', NULL, '2000–2025', 'Исполнитель Toronto Is Broken известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebd7e5abc71f1fec13033a5baa');
INSERT INTO public."Artists" VALUES (4, 'Sebotage', NULL, '2000–2025', 'Исполнитель Sebotage известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebe8500fa9b789bfc15faf1c26');
INSERT INTO public."Artists" VALUES (5, 'UNKLE', NULL, '2000–2025', 'Исполнитель UNKLE известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebac89f1cb4f6881854f55f761');
INSERT INTO public."Artists" VALUES (6, 'Hybrid', NULL, '2000–2025', 'Исполнитель Hybrid известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb2d875cc2e54d7270f8d05f7e');
INSERT INTO public."Artists" VALUES (7, 'REEBZ', NULL, '2000–2025', 'Исполнитель REEBZ известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebe1ddd40d74ec59697d1ea2b9');
INSERT INTO public."Artists" VALUES (8, '仮想夢プラザ', NULL, '2000–2025', 'Исполнитель 仮想夢プラザ известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebbea822341ca18435964b3a22');
INSERT INTO public."Artists" VALUES (9, 'The Prodigy', NULL, '2000–2025', 'Исполнитель The Prodigy известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb147841812056c247407811f3');
INSERT INTO public."Artists" VALUES (10, 'Spor', NULL, '2000–2025', 'Исполнитель Spor известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb3200487ca708d8587c557783');
INSERT INTO public."Artists" VALUES (11, 'Hong Kong Express', NULL, '2000–2025', 'Исполнитель Hong Kong Express известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb26fbeeac4e8c45780b3b3a7d');
INSERT INTO public."Artists" VALUES (12, 'Dr. Nakano', NULL, '2000–2025', 'Исполнитель Dr. Nakano известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebf243cce1a787d3f6a84397f6');
INSERT INTO public."Artists" VALUES (13, 'Nine Inch Nails', NULL, '2000–2025', 'Исполнитель Nine Inch Nails известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb752fca27c5b839ea47d5100d');
INSERT INTO public."Artists" VALUES (14, 'Kayzo', NULL, '2000–2025', 'Исполнитель Kayzo известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb5e72708b64f379843417ddee');
INSERT INTO public."Artists" VALUES (15, 'WesGhost', NULL, '2000–2025', 'Исполнитель WesGhost известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb09935a526cc02e86cc127b5e');
INSERT INTO public."Artists" VALUES (16, 'Naked Flames', NULL, '2000–2025', 'Исполнитель Naked Flames известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb864a03d162414c8ff3eb7b21');
INSERT INTO public."Artists" VALUES (17, 'CANTERVICE', NULL, '2000–2025', 'Исполнитель CANTERVICE известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebdc3edcf7e8414d791d224ab4');
INSERT INTO public."Artists" VALUES (18, 'Zardonic', NULL, '2000–2025', 'Исполнитель Zardonic известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebf2112a8bed841437334d935b');
INSERT INTO public."Artists" VALUES (19, 'Magnetude', NULL, '2000–2025', 'Исполнитель Magnetude известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebfca737f35ace839fea98adcc');
INSERT INTO public."Artists" VALUES (20, 'Skrillex', NULL, '2000–2025', 'Исполнитель Skrillex известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebe32002317387b6d659308a94');
INSERT INTO public."Artists" VALUES (21, 'ISOxo', NULL, '2000–2025', 'Исполнитель ISOxo известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb69dea5e9c932833f17b7ddce');
INSERT INTO public."Artists" VALUES (22, 'HEALTH', NULL, '2000–2025', 'Исполнитель HEALTH известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb5b2bf033db0073228084edb4');
INSERT INTO public."Artists" VALUES (23, 'RIOT', NULL, '2000–2025', 'Исполнитель RIOT известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebd459730d3fb832bc9e18113e');
INSERT INTO public."Artists" VALUES (24, 'GG Magree', NULL, '2000–2025', 'Исполнитель GG Magree известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebf784e061371be04d98b542e5');
INSERT INTO public."Artists" VALUES (25, 'Nedaj', NULL, '2000–2025', 'Исполнитель Nedaj известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb11586ee95f9e07f47252d1f6');
INSERT INTO public."Artists" VALUES (26, 'Koven', NULL, '2000–2025', 'Исполнитель Koven известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb558339d8bae95cd33a20c4c7');
INSERT INTO public."Artists" VALUES (27, 'Ladytron', NULL, '2000–2025', 'Исполнитель Ladytron известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebb948a9242e7ff017fb0d1173');
INSERT INTO public."Artists" VALUES (28, 'Kevin Sherwood', NULL, '2000–2025', 'Исполнитель Kevin Sherwood известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb82a01d3b0a1890a78e9e527a');
INSERT INTO public."Artists" VALUES (29, 'Matthew K. Heafy', NULL, '2000–2025', 'Исполнитель Matthew K. Heafy известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb1d878a14af100015aae655c2');
INSERT INTO public."Artists" VALUES (30, 'Trivium', NULL, '2000–2025', 'Исполнитель Trivium известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebab21a41e346730ff64be53c2');
INSERT INTO public."Artists" VALUES (31, 'Kurai mizu', NULL, '2000–2025', 'Исполнитель Kurai mizu известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb52980e5c7d56a2b51fd5914d');
INSERT INTO public."Artists" VALUES (32, 'Solter', NULL, '2000–2025', 'Исполнитель Solter известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb20b33946a5d8579f780d6d7c');
INSERT INTO public."Artists" VALUES (33, 'Юлия Коган', NULL, '2000–2025', 'Исполнитель Юлия Коган известен своими треками на Spotify.', 'https://i.scdn.co/image/ab67616d0000b27359c1339003364d093ead463f');
INSERT INTO public."Artists" VALUES (34, 'frenetic virtual orchestra', NULL, '2000–2025', 'Исполнитель frenetic virtual orchestra известен своими треками на Spotify.', 'https://i.scdn.co/image/ab67616d0000b273caf25a695ad72d5fa1641d01');
INSERT INTO public."Artists" VALUES (35, 'Geoffplaysguitar', NULL, '2000–2025', 'Исполнитель Geoffplaysguitar известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebc80b2654f9c8ff1eeac4a1cd');
INSERT INTO public."Artists" VALUES (36, 'Atomic Heart', NULL, '2000–2025', 'Исполнитель Atomic Heart известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb5b9d4a3ca752b33bf6d8a690');
INSERT INTO public."Artists" VALUES (37, 'CHOMPO', NULL, '2000–2025', 'Исполнитель CHOMPO известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebab8bdcdcebb63c80fb868a75');
INSERT INTO public."Artists" VALUES (38, 'Boom Kitty', NULL, '2000–2025', 'Исполнитель Boom Kitty известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb0537e4e52505684d70275b45');
INSERT INTO public."Artists" VALUES (39, 'Cynthoni', NULL, '2000–2025', 'Исполнитель Cynthoni известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb068c68cd72c01daf38873103');
INSERT INTO public."Artists" VALUES (40, 'Sewerslvt', NULL, '2000–2025', 'Исполнитель Sewerslvt известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb8a1271f7f32e5202924ebff2');
INSERT INTO public."Artists" VALUES (41, 'Skyth', NULL, '2000–2025', 'Исполнитель Skyth известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5eb9c0ccd821d055f7670190408');
INSERT INTO public."Artists" VALUES (42, 'AL-90', NULL, '2000–2025', 'Исполнитель AL-90 известен своими треками на Spotify.', 'https://i.scdn.co/image/ab67616d0000b2738fe600c89360e4a56ac26c7d');
INSERT INTO public."Artists" VALUES (43, 'Habstrakt', NULL, '2000–2025', 'Исполнитель Habstrakt известен своими треками на Spotify.', 'https://i.scdn.co/image/ab6761610000e5ebd4f9e0206342d45ab98840c2');


--
-- TOC entry 3508 (class 0 OID 16523)
-- Dependencies: 236
-- Data for Name: Genres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Genres" VALUES (1, 'trip hop');
INSERT INTO public."Genres" VALUES (2, 'ebm');
INSERT INTO public."Genres" VALUES (3, 'industrial');
INSERT INTO public."Genres" VALUES (4, 'big beat');
INSERT INTO public."Genres" VALUES (5, 'electronica');
INSERT INTO public."Genres" VALUES (6, 'drum and bass');
INSERT INTO public."Genres" VALUES (7, 'liquid funk');
INSERT INTO public."Genres" VALUES (8, 'downtempo');
INSERT INTO public."Genres" VALUES (9, 'breakbeat');
INSERT INTO public."Genres" VALUES (10, 'vaporwave');
INSERT INTO public."Genres" VALUES (11, 'drone');
INSERT INTO public."Genres" VALUES (12, 'ambient');
INSERT INTO public."Genres" VALUES (13, 'hardcore techno');
INSERT INTO public."Genres" VALUES (14, 'chillwave');
INSERT INTO public."Genres" VALUES (15, 'industrial rock');
INSERT INTO public."Genres" VALUES (16, 'industrial metal');
INSERT INTO public."Genres" VALUES (17, 'alternative metal');
INSERT INTO public."Genres" VALUES (18, 'nu metal');
INSERT INTO public."Genres" VALUES (19, 'dubstep');
INSERT INTO public."Genres" VALUES (20, 'deathstep');
INSERT INTO public."Genres" VALUES (21, 'riddim');
INSERT INTO public."Genres" VALUES (22, 'edm');
INSERT INTO public."Genres" VALUES (23, 'breakcore');
INSERT INTO public."Genres" VALUES (24, 'drumstep');
INSERT INTO public."Genres" VALUES (25, 'electro');
INSERT INTO public."Genres" VALUES (26, 'electronic');
INSERT INTO public."Genres" VALUES (27, 'noise rock');
INSERT INTO public."Genres" VALUES (28, 'witch house');
INSERT INTO public."Genres" VALUES (29, 'future bass');
INSERT INTO public."Genres" VALUES (30, 'jungle');
INSERT INTO public."Genres" VALUES (31, 'speedcore');
INSERT INTO public."Genres" VALUES (32, 'chillstep');
INSERT INTO public."Genres" VALUES (33, 'electroclash');
INSERT INTO public."Genres" VALUES (34, 'alternative dance');
INSERT INTO public."Genres" VALUES (35, 'synthpop');
INSERT INTO public."Genres" VALUES (36, 'idm');
INSERT INTO public."Genres" VALUES (37, 'darkwave');
INSERT INTO public."Genres" VALUES (38, 'melodic bass');
INSERT INTO public."Genres" VALUES (39, 'bass house');
INSERT INTO public."Genres" VALUES (40, 'g-house');
INSERT INTO public."Genres" VALUES (41, 'bassline');
INSERT INTO public."Genres" VALUES (42, 'lo-fi house');
INSERT INTO public."Genres" VALUES (43, 'metalcore');
INSERT INTO public."Genres" VALUES (44, 'metal');
INSERT INTO public."Genres" VALUES (45, 'heavy metal');
INSERT INTO public."Genres" VALUES (46, 'deathcore');
INSERT INTO public."Genres" VALUES (47, 'hyperpop');


--
-- TOC entry 3498 (class 0 OID 16435)
-- Dependencies: 226
-- Data for Name: Tags; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Tags" VALUES (1, 'focus');
INSERT INTO public."Tags" VALUES (2, 'workout');
INSERT INTO public."Tags" VALUES (3, 'study');
INSERT INTO public."Tags" VALUES (4, 'chill');
INSERT INTO public."Tags" VALUES (5, 'pop');
INSERT INTO public."Tags" VALUES (6, 'rock');


--
-- TOC entry 3506 (class 0 OID 16506)
-- Dependencies: 234
-- Data for Name: Tracks; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Tracks" VALUES (1, 'Want', 1, 'Liquid (Bonus Track Version) (2000)', '00:06:09', '2000-03-21', 192, 'https://open.spotify.com/track/6AFz1yXS7boPZlqJd2R7WS', 'https://i.scdn.co/image/ab67616d0000b273211808193df8743aedb1038b', 3.53, 19591519);
INSERT INTO public."Tracks" VALUES (2, 'Beautiful Burnout', 2, 'Oblivion With Bells (2007)', '00:08:09', '2007-01-01', 280, 'https://open.spotify.com/track/1I61h0TNyzE86dx1tZzj3b', 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', 3.32, 14678550);
INSERT INTO public."Tracks" VALUES (3, 'Intruders', 1, 'Subhuman (2007)', '00:11:36', '2007-01-01', 320, 'https://open.spotify.com/track/0zU2OXXBlRj8xIB1mc0jPN', 'https://i.scdn.co/image/ab67616d0000b273ce0298116be15cb81f91a1aa', 4.46, 21712928);
INSERT INTO public."Tracks" VALUES (4, 'Glam Bucket', 2, 'Oblivion With Bells (2007)', '00:05:45', '2007-01-01', 256, 'https://open.spotify.com/track/7k5rnk06tw2kUOj7AQXkha', 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', 3.25, 36747640);
INSERT INTO public."Tracks" VALUES (5, 'Circle (feat. Sebotage)', 3, 'Circle (2022)', '00:04:07', '2022-03-30', 320, 'https://open.spotify.com/track/2aTXg7Ch2EuKpa7m8Z8GSv', 'https://i.scdn.co/image/ab67616d0000b273f0f4f4f180ea0f32ac2c5f8f', 4.22, 16908411);
INSERT INTO public."Tracks" VALUES (6, 'To Heal', 2, 'Oblivion With Bells (2007)', '00:02:36', '2007-01-01', 320, 'https://open.spotify.com/track/5K0kPszkKvh6yHEI3wiUCZ', 'https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b', 4.83, 7016621);
INSERT INTO public."Tracks" VALUES (7, 'Eye for an Eye', 5, 'Never, Never, Land (2003)', '00:05:45', '2003-01-01', 280, 'https://open.spotify.com/track/6ReswWJnalmGTxyRSTmUkl', 'https://i.scdn.co/image/ab67616d0000b273894c4f974159696026573eb5', 4.64, 65817538);
INSERT INTO public."Tracks" VALUES (8, 'Formula Of Fear', 6, 'Formula Of Fear (2008)', '00:07:07', '2008-09-22', 256, 'https://open.spotify.com/track/0fj2n9wzkojYUy80rOFsLp', 'https://i.scdn.co/image/ab67616d0000b27359566bca173ee54657bd5f8d', 3.17, 97311814);
INSERT INTO public."Tracks" VALUES (9, 'Breed', 3, 'Breed (feat. REEBZ) (2023)', '00:03:58', '2023-05-12', 280, 'https://open.spotify.com/track/5cQwBnsRxUhbKDK2C6fR4n', 'https://i.scdn.co/image/ab67616d0000b2737163994ea19cc74c0bbfb31e', 3.26, 67607976);
INSERT INTO public."Tracks" VALUES (10, 'あなたを待って', 8, 'あなたを待って (2015)', '00:31:34', '2015-05-16', 192, 'https://open.spotify.com/track/3GwYAO8fAxs5k0KiU2hpBP', 'https://i.scdn.co/image/ab67616d0000b273fcc3cb5cc1166a95b86f5e6e', 3.08, 32425960);
INSERT INTO public."Tracks" VALUES (11, 'Nasty - Spor Remix', 9, 'The Day Is My Enemy (Expanded Edition) (2015)', '00:05:09', '2015-11-10', 280, 'https://open.spotify.com/track/4SXdAtwtoVCZdUP9mfFFqG', 'https://i.scdn.co/image/ab67616d0000b273872772e0b165e57a104fd37a', 3.67, 47018173);
INSERT INTO public."Tracks" VALUES (12, 'If Rah', 2, 'Barbara Barbara, we face a shining future (2016)', '00:07:12', '2016-03-19', 256, 'https://open.spotify.com/track/15j4iuXi1OZfSBKmaVTeN7', 'https://i.scdn.co/image/ab67616d0000b273cf5cdbf0397a8a71c9fd0395', 4.93, 33608103);
INSERT INTO public."Tracks" VALUES (13, 'Ghost City Daze', 11, 'Ghost City Daze (2022)', '00:03:39', '2022-09-02', 280, 'https://open.spotify.com/track/5Npj2l7dvTvICHnh6xwNTU', 'https://i.scdn.co/image/ab67616d0000b273ab65f724c2fdf067999d8d16', 3.07, 14408959);
INSERT INTO public."Tracks" VALUES (14, 'The Great Below', 13, 'The Fragile (1999)', '00:05:17', '1999-09-21', 280, 'https://open.spotify.com/track/7LBWAib31Cthfz2KoI611Z', 'https://i.scdn.co/image/ab67616d0000b273237a21c8ba1bfc2d85a83585', 4.56, 91263647);
INSERT INTO public."Tracks" VALUES (15, 'The Big Come Down', 13, 'The Fragile (1999)', '00:04:12', '1999-09-21', 256, 'https://open.spotify.com/track/3cl42VFcRop14wAduvSeaH', 'https://i.scdn.co/image/ab67616d0000b273237a21c8ba1bfc2d85a83585', 3.40, 80821880);
INSERT INTO public."Tracks" VALUES (16, 'At The Heart Of It All', 13, 'Further Down The Spiral (1995)', '00:07:14', '1995-01-01', 320, 'https://open.spotify.com/track/4JQEQOu8YKbdtMzVWADAAb', 'https://i.scdn.co/image/ab67616d0000b273551ecb57cda40ae833e3e5b1', 4.04, 78182712);
INSERT INTO public."Tracks" VALUES (17, 'Freeze', 1, 'Bloodline (Bonus Tracks) (1992)', '00:07:28', '1992-01-01', 320, 'https://open.spotify.com/track/7KhV0aeGLhI2G1TC5bpgBe', 'https://i.scdn.co/image/ab67616d0000b273c74e21dfa7e9470c0c980f9f', 3.90, 86513228);
INSERT INTO public."Tracks" VALUES (18, 'Together', 13, 'Ghosts V: Together (2020)', '00:10:03', '2020-03-27', 256, 'https://open.spotify.com/track/7iiu6ce9zGfbvs0yBvsZeT', 'https://i.scdn.co/image/ab67616d0000b27327c5a582320e08258ec13d0a', 3.70, 31154483);
INSERT INTO public."Tracks" VALUES (19, 'Orbit', 6, 'Cinematic Soundscape (2012)', '00:05:25', '2012-09-07', 280, 'https://open.spotify.com/track/1bo3aqhoE0q808wF11ENrC', 'https://i.scdn.co/image/ab67616d0000b27381c4fc91dc2abd939c2c705f', 4.19, 39317838);
INSERT INTO public."Tracks" VALUES (20, 'GOFASTER', 3, 'GOFASTER (YANA100) (2025)', '00:04:40', '2025-09-17', 256, 'https://open.spotify.com/track/0WpMAaQq61chei1VkqKGWv', 'https://i.scdn.co/image/ab67616d0000b2735ff50f46b8b8720981957d51', 4.86, 4552008);
INSERT INTO public."Tracks" VALUES (21, 'SOMEWHEREIBELONG', 3, 'SOMEWHEREIBELONG (2025)', '00:03:47', '2025-05-02', 320, 'https://open.spotify.com/track/2bja1BK8CyrE5U1YLzPWtQ', 'https://i.scdn.co/image/ab67616d0000b27328dc64c5aa8c59dd37a7d820', 3.04, 39633329);
INSERT INTO public."Tracks" VALUES (22, 'Grave', 14, 'Grave (2025)', '00:02:39', '2025-01-31', 320, 'https://open.spotify.com/track/3TbzrpJIkSoicguKWNHcSh', 'https://i.scdn.co/image/ab67616d0000b273659f2e9dc34715a487f073be', 3.01, 77772777);
INSERT INTO public."Tracks" VALUES (23, 'Undisputed Altitude', 16, 'Wall I Was (2025)', '00:11:00', '2025-03-14', 256, 'https://open.spotify.com/track/3K5td8Q9ezKgBjpiiuJDL9', 'https://i.scdn.co/image/ab67616d0000b273710d4624a5a1b87b70b56bb2', 4.43, 32413023);
INSERT INTO public."Tracks" VALUES (24, 'The Masquerade - Zardonic & Toronto Is Broken Remix', 17, 'The Masquerade (Zardonic & Toronto Is Broken Remix) (2025)', '00:04:12', '2025-07-24', 192, 'https://open.spotify.com/track/787izJqCRpk9HOgho07jzb', 'https://i.scdn.co/image/ab67616d0000b273b9bb9b27f5aeb0e1a1475707', 4.45, 68700455);
INSERT INTO public."Tracks" VALUES (25, 'The Star', 19, 'The Star (2025)', '00:04:57', '2025-05-23', 320, 'https://open.spotify.com/track/4EVYNBO90unYQMUkcgaAqS', 'https://i.scdn.co/image/ab67616d0000b2731e01b0b41fa3bf96bab342cb', 4.05, 80447698);
INSERT INTO public."Tracks" VALUES (26, 'Fuze', 20, 'Fuze (2025)', '00:03:08', '2025-10-24', 256, 'https://open.spotify.com/track/5UZIVxzI4UyrSbg3ZLTGTH', 'https://i.scdn.co/image/ab67616d0000b27362d6852a9461f7c5579097c6', 3.32, 62355472);
INSERT INTO public."Tracks" VALUES (27, 'ORDINARY LOSS', 22, 'ORDINARY LOSS (2025)', '00:03:53', '2025-09-11', 192, 'https://open.spotify.com/track/4qUrMbUVDxaMZy2zPB6mD7', 'https://i.scdn.co/image/ab67616d0000b27367bd88cd7a659c93b34b0688', 4.46, 3437809);
INSERT INTO public."Tracks" VALUES (28, 'March of the Machine', 23, 'The Machine (2025)', '00:04:07', '2025-08-01', 192, 'https://open.spotify.com/track/2ZhSXdApy452LqJFf7aEwz', 'https://i.scdn.co/image/ab67616d0000b27328700d650196f5918a0578bf', 3.70, 60148993);
INSERT INTO public."Tracks" VALUES (29, 'Metalicious', 23, 'Metalicious (2025)', '00:06:00', '2025-06-27', 280, 'https://open.spotify.com/track/2PdmLVo9VOPhlySbMXH6M0', 'https://i.scdn.co/image/ab67616d0000b27368e7513b4ca1da14da0a81e6', 3.92, 30476401);
INSERT INTO public."Tracks" VALUES (30, 'Run', 24, 'Run (2025)', '00:03:23', '2025-08-13', 256, 'https://open.spotify.com/track/0Ov5YTilb4z4nvuTWeSsUu', 'https://i.scdn.co/image/ab67616d0000b273af743f04ff7cc6ae7300929f', 3.16, 42701725);
INSERT INTO public."Tracks" VALUES (31, 'Siren Of The Storm', 6, 'Siren Of The Storm (2025)', '00:09:15', '2025-01-19', 192, 'https://open.spotify.com/track/7HSZ0epFVfoDBFyMjCKMrP', 'https://i.scdn.co/image/ab67616d0000b273628b577615d97f6837e30b73', 3.28, 64176479);
INSERT INTO public."Tracks" VALUES (32, 'ONYX', 25, 'NO MERCY III (2025)', '00:05:12', '2025-04-11', 280, 'https://open.spotify.com/track/0yuIvkFLeSxFBSyyJBiWVV', 'https://i.scdn.co/image/ab67616d0000b273a10a1e401fc110168dfd02f3', 4.60, 36864477);
INSERT INTO public."Tracks" VALUES (33, 'Overload 2K', 14, 'Overload 2K (2025)', '00:04:24', '2025-10-24', 192, 'https://open.spotify.com/track/2Y68mc2cWkIwBdqlY04J2e', 'https://i.scdn.co/image/ab67616d0000b273a24c69273fb3656b3ecb92a7', 4.57, 26952175);
INSERT INTO public."Tracks" VALUES (34, 'Era', 26, 'Moments In Everglow (2025)', '00:03:17', '2025-04-18', 280, 'https://open.spotify.com/track/1rxxl2R2tU1Uv1oP2bbeUn', 'https://i.scdn.co/image/ab67616d0000b27379edc5965038d4923dff96b0', 3.99, 54797402);
INSERT INTO public."Tracks" VALUES (35, 'As Alive As You Need Me To Be', 13, 'As Alive As You Need Me To Be (2025)', '00:03:53', '2025-07-17', 192, 'https://open.spotify.com/track/1xsEHo7mtGZLEG94vFX11z', 'https://i.scdn.co/image/ab67616d0000b27317ff2581a6b924fb3a704395', 3.55, 71834289);
INSERT INTO public."Tracks" VALUES (36, 'Repentance', 19, 'Repentance (2025)', '00:04:30', '2025-08-01', 280, 'https://open.spotify.com/track/12eQwZCKpzIpccXsH4xQZR', 'https://i.scdn.co/image/ab67616d0000b2730f423970a440915f6750e11e', 3.36, 96111491);
INSERT INTO public."Tracks" VALUES (37, 'I See Red', 27, 'I See Red (2025)', '00:03:50', '2025-10-17', 192, 'https://open.spotify.com/track/51WgQlBgMnf1lDxBfZkJsa', 'https://i.scdn.co/image/ab67616d0000b2737f34486112bf81e38838a429', 3.91, 79852612);
INSERT INTO public."Tracks" VALUES (38, 'Dig', 28, 'Call of Duty®: Black Ops 6 - Zombies "The Tomb" (Original Soundtrack) (2025)', '00:03:49', '2025-01-31', 192, 'https://open.spotify.com/track/7nVIE2M0t4sIh3ZkEqrjlY', 'https://i.scdn.co/image/ab67616d0000b2732db3154893852b63232e1466', 3.02, 30268024);
INSERT INTO public."Tracks" VALUES (39, 'Final Destination', 31, 'Final Destination (2025)', '00:04:50', '2025-02-24', 256, 'https://open.spotify.com/track/7bqSNB1C3tWOUgOQTrODAG', 'https://i.scdn.co/image/ab67616d0000b273857bb2eff1c145e26e9a2c1c', 2.52, 31048640);
INSERT INTO public."Tracks" VALUES (40, 'В синем море, в белой пене (Оставайся, мальчик, с нами) [Geoffrey Day Remix]', 33, 'Atomic Heart, Vol.5 (Original Game Soundtrack) (2025)', '00:05:10', '2025-01-29', 280, 'https://open.spotify.com/track/4zW0D21eynN6JcDwGR4f9F', 'https://i.scdn.co/image/ab67616d0000b2731a24aab10d2b27d63175c37a', 3.52, 71054588);
INSERT INTO public."Tracks" VALUES (41, 'Obelisk', 37, 'CHOMPO III (2025)', '00:02:45', '2025-02-27', 256, 'https://open.spotify.com/track/0UCcUFxxlHgaQGojhxzECF', 'https://i.scdn.co/image/ab67616d0000b273aadd94c74f32483d503fb0ab', 4.55, 14134899);
INSERT INTO public."Tracks" VALUES (42, 'Lvstlove', 39, 'Lvstlove (2025)', '00:06:06', '2025-01-03', 192, 'https://open.spotify.com/track/3INsVctSfXaU6rdPBSPalA', 'https://i.scdn.co/image/ab67616d0000b2737348b951edbf4bb86854e806', 2.98, 97856870);
INSERT INTO public."Tracks" VALUES (43, 'Data Destruction', 41, 'Data Destruction (2025)', '00:03:20', '2025-01-30', 320, 'https://open.spotify.com/track/5UM2IIULlGyvYHm4SF3hHI', 'https://i.scdn.co/image/ab67616d0000b273c6ad3e09b75355deee5b732d', 3.57, 79887130);
INSERT INTO public."Tracks" VALUES (44, 'Muleta', 19, 'Muleta (2025)', '00:04:38', '2025-04-11', 192, 'https://open.spotify.com/track/28zBEKDSGAny9y6KrBoCF4', 'https://i.scdn.co/image/ab67616d0000b273adebc14e80184ee7cc2e36b8', 3.95, 59329189);
INSERT INTO public."Tracks" VALUES (45, 'общедомовой', 42, 'общедомовой (2025)', '00:05:07', '2025-02-14', 320, 'https://open.spotify.com/track/1QVlzjQU2RI4GpVen3x4Ro', 'https://i.scdn.co/image/ab67616d0000b273f11a5383c11039c003e9008b', 4.77, 17722035);
INSERT INTO public."Tracks" VALUES (46, 'Baby', 43, 'Baby (2025)', '00:03:56', '2025-02-28', 280, 'https://open.spotify.com/track/1syQiwibqZKAUsgkLn8qtC', 'https://i.scdn.co/image/ab67616d0000b273fc679c0d97b173113141a42b', 4.94, 42250505);


--
-- TOC entry 3510 (class 0 OID 16544)
-- Dependencies: 238
-- Data for Name: albumGenres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."albumGenres" VALUES (1, 1);
INSERT INTO public."albumGenres" VALUES (1, 2);
INSERT INTO public."albumGenres" VALUES (1, 3);
INSERT INTO public."albumGenres" VALUES (2, 4);
INSERT INTO public."albumGenres" VALUES (2, 5);
INSERT INTO public."albumGenres" VALUES (3, 1);
INSERT INTO public."albumGenres" VALUES (3, 2);
INSERT INTO public."albumGenres" VALUES (3, 3);
INSERT INTO public."albumGenres" VALUES (4, 6);
INSERT INTO public."albumGenres" VALUES (4, 7);
INSERT INTO public."albumGenres" VALUES (5, 1);
INSERT INTO public."albumGenres" VALUES (5, 8);
INSERT INTO public."albumGenres" VALUES (6, 9);
INSERT INTO public."albumGenres" VALUES (6, 4);
INSERT INTO public."albumGenres" VALUES (7, 6);
INSERT INTO public."albumGenres" VALUES (7, 7);
INSERT INTO public."albumGenres" VALUES (8, 10);
INSERT INTO public."albumGenres" VALUES (8, 11);
INSERT INTO public."albumGenres" VALUES (8, 12);
INSERT INTO public."albumGenres" VALUES (9, 4);
INSERT INTO public."albumGenres" VALUES (9, 9);
INSERT INTO public."albumGenres" VALUES (9, 13);
INSERT INTO public."albumGenres" VALUES (10, 4);
INSERT INTO public."albumGenres" VALUES (10, 5);
INSERT INTO public."albumGenres" VALUES (11, 10);
INSERT INTO public."albumGenres" VALUES (11, 14);
INSERT INTO public."albumGenres" VALUES (12, 15);
INSERT INTO public."albumGenres" VALUES (12, 3);
INSERT INTO public."albumGenres" VALUES (12, 16);
INSERT INTO public."albumGenres" VALUES (12, 17);
INSERT INTO public."albumGenres" VALUES (12, 18);
INSERT INTO public."albumGenres" VALUES (13, 15);
INSERT INTO public."albumGenres" VALUES (13, 3);
INSERT INTO public."albumGenres" VALUES (13, 16);
INSERT INTO public."albumGenres" VALUES (13, 17);
INSERT INTO public."albumGenres" VALUES (13, 18);
INSERT INTO public."albumGenres" VALUES (14, 1);
INSERT INTO public."albumGenres" VALUES (14, 2);
INSERT INTO public."albumGenres" VALUES (14, 3);
INSERT INTO public."albumGenres" VALUES (15, 15);
INSERT INTO public."albumGenres" VALUES (15, 3);
INSERT INTO public."albumGenres" VALUES (15, 16);
INSERT INTO public."albumGenres" VALUES (15, 17);
INSERT INTO public."albumGenres" VALUES (15, 18);
INSERT INTO public."albumGenres" VALUES (16, 9);
INSERT INTO public."albumGenres" VALUES (16, 4);
INSERT INTO public."albumGenres" VALUES (17, 6);
INSERT INTO public."albumGenres" VALUES (17, 7);
INSERT INTO public."albumGenres" VALUES (18, 6);
INSERT INTO public."albumGenres" VALUES (18, 7);
INSERT INTO public."albumGenres" VALUES (19, 19);
INSERT INTO public."albumGenres" VALUES (19, 20);
INSERT INTO public."albumGenres" VALUES (19, 21);
INSERT INTO public."albumGenres" VALUES (19, 22);
INSERT INTO public."albumGenres" VALUES (20, 23);
INSERT INTO public."albumGenres" VALUES (22, 6);
INSERT INTO public."albumGenres" VALUES (22, 24);
INSERT INTO public."albumGenres" VALUES (23, 19);
INSERT INTO public."albumGenres" VALUES (23, 22);
INSERT INTO public."albumGenres" VALUES (23, 25);
INSERT INTO public."albumGenres" VALUES (23, 26);
INSERT INTO public."albumGenres" VALUES (24, 27);
INSERT INTO public."albumGenres" VALUES (24, 3);
INSERT INTO public."albumGenres" VALUES (24, 28);
INSERT INTO public."albumGenres" VALUES (24, 15);
INSERT INTO public."albumGenres" VALUES (24, 16);
INSERT INTO public."albumGenres" VALUES (25, 19);
INSERT INTO public."albumGenres" VALUES (25, 20);
INSERT INTO public."albumGenres" VALUES (25, 21);
INSERT INTO public."albumGenres" VALUES (26, 19);
INSERT INTO public."albumGenres" VALUES (26, 20);
INSERT INTO public."albumGenres" VALUES (26, 21);
INSERT INTO public."albumGenres" VALUES (27, 19);
INSERT INTO public."albumGenres" VALUES (27, 29);
INSERT INTO public."albumGenres" VALUES (28, 9);
INSERT INTO public."albumGenres" VALUES (28, 4);
INSERT INTO public."albumGenres" VALUES (29, 23);
INSERT INTO public."albumGenres" VALUES (29, 30);
INSERT INTO public."albumGenres" VALUES (29, 9);
INSERT INTO public."albumGenres" VALUES (29, 31);
INSERT INTO public."albumGenres" VALUES (29, 6);
INSERT INTO public."albumGenres" VALUES (30, 19);
INSERT INTO public."albumGenres" VALUES (30, 20);
INSERT INTO public."albumGenres" VALUES (30, 21);
INSERT INTO public."albumGenres" VALUES (30, 22);
INSERT INTO public."albumGenres" VALUES (31, 6);
INSERT INTO public."albumGenres" VALUES (31, 7);
INSERT INTO public."albumGenres" VALUES (31, 24);
INSERT INTO public."albumGenres" VALUES (31, 32);
INSERT INTO public."albumGenres" VALUES (32, 15);
INSERT INTO public."albumGenres" VALUES (32, 3);
INSERT INTO public."albumGenres" VALUES (32, 16);
INSERT INTO public."albumGenres" VALUES (32, 17);
INSERT INTO public."albumGenres" VALUES (32, 18);
INSERT INTO public."albumGenres" VALUES (33, 6);
INSERT INTO public."albumGenres" VALUES (33, 24);
INSERT INTO public."albumGenres" VALUES (34, 33);
INSERT INTO public."albumGenres" VALUES (34, 34);
INSERT INTO public."albumGenres" VALUES (34, 35);
INSERT INTO public."albumGenres" VALUES (36, 36);
INSERT INTO public."albumGenres" VALUES (38, 38);
INSERT INTO public."albumGenres" VALUES (39, 23);
INSERT INTO public."albumGenres" VALUES (39, 30);
INSERT INTO public."albumGenres" VALUES (39, 31);
INSERT INTO public."albumGenres" VALUES (41, 6);
INSERT INTO public."albumGenres" VALUES (41, 24);
INSERT INTO public."albumGenres" VALUES (42, 42);
INSERT INTO public."albumGenres" VALUES (43, 39);
INSERT INTO public."albumGenres" VALUES (43, 40);
INSERT INTO public."albumGenres" VALUES (43, 19);
INSERT INTO public."albumGenres" VALUES (43, 41);


--
-- TOC entry 3515 (class 0 OID 16622)
-- Dependencies: 243
-- Data for Name: albumTracks; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."albumTracks" VALUES (1, 1);
INSERT INTO public."albumTracks" VALUES (3, 3);
INSERT INTO public."albumTracks" VALUES (2, 2);
INSERT INTO public."albumTracks" VALUES (2, 4);
INSERT INTO public."albumTracks" VALUES (2, 6);
INSERT INTO public."albumTracks" VALUES (4, 5);
INSERT INTO public."albumTracks" VALUES (5, 7);
INSERT INTO public."albumTracks" VALUES (6, 8);
INSERT INTO public."albumTracks" VALUES (7, 9);
INSERT INTO public."albumTracks" VALUES (8, 10);
INSERT INTO public."albumTracks" VALUES (9, 11);
INSERT INTO public."albumTracks" VALUES (10, 12);
INSERT INTO public."albumTracks" VALUES (11, 13);
INSERT INTO public."albumTracks" VALUES (12, 14);
INSERT INTO public."albumTracks" VALUES (12, 15);
INSERT INTO public."albumTracks" VALUES (13, 16);
INSERT INTO public."albumTracks" VALUES (14, 17);
INSERT INTO public."albumTracks" VALUES (15, 18);
INSERT INTO public."albumTracks" VALUES (16, 19);
INSERT INTO public."albumTracks" VALUES (17, 20);
INSERT INTO public."albumTracks" VALUES (18, 21);
INSERT INTO public."albumTracks" VALUES (19, 22);
INSERT INTO public."albumTracks" VALUES (20, 23);
INSERT INTO public."albumTracks" VALUES (21, 24);
INSERT INTO public."albumTracks" VALUES (22, 25);
INSERT INTO public."albumTracks" VALUES (23, 26);
INSERT INTO public."albumTracks" VALUES (24, 27);
INSERT INTO public."albumTracks" VALUES (25, 28);
INSERT INTO public."albumTracks" VALUES (26, 29);
INSERT INTO public."albumTracks" VALUES (27, 30);
INSERT INTO public."albumTracks" VALUES (28, 31);
INSERT INTO public."albumTracks" VALUES (29, 32);
INSERT INTO public."albumTracks" VALUES (30, 33);
INSERT INTO public."albumTracks" VALUES (31, 34);
INSERT INTO public."albumTracks" VALUES (32, 35);
INSERT INTO public."albumTracks" VALUES (33, 36);
INSERT INTO public."albumTracks" VALUES (34, 37);
INSERT INTO public."albumTracks" VALUES (35, 38);
INSERT INTO public."albumTracks" VALUES (36, 39);
INSERT INTO public."albumTracks" VALUES (37, 40);
INSERT INTO public."albumTracks" VALUES (38, 41);
INSERT INTO public."albumTracks" VALUES (39, 42);
INSERT INTO public."albumTracks" VALUES (40, 43);
INSERT INTO public."albumTracks" VALUES (41, 44);
INSERT INTO public."albumTracks" VALUES (42, 45);
INSERT INTO public."albumTracks" VALUES (43, 46);


--
-- TOC entry 3511 (class 0 OID 16557)
-- Dependencies: 239
-- Data for Name: artistGenre; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."artistGenre" VALUES (1, 1);
INSERT INTO public."artistGenre" VALUES (1, 2);
INSERT INTO public."artistGenre" VALUES (1, 3);
INSERT INTO public."artistGenre" VALUES (2, 4);
INSERT INTO public."artistGenre" VALUES (2, 5);
INSERT INTO public."artistGenre" VALUES (3, 6);
INSERT INTO public."artistGenre" VALUES (3, 7);
INSERT INTO public."artistGenre" VALUES (4, 6);
INSERT INTO public."artistGenre" VALUES (5, 1);
INSERT INTO public."artistGenre" VALUES (5, 8);
INSERT INTO public."artistGenre" VALUES (6, 9);
INSERT INTO public."artistGenre" VALUES (6, 4);
INSERT INTO public."artistGenre" VALUES (7, 6);
INSERT INTO public."artistGenre" VALUES (8, 10);
INSERT INTO public."artistGenre" VALUES (8, 11);
INSERT INTO public."artistGenre" VALUES (8, 12);
INSERT INTO public."artistGenre" VALUES (9, 4);
INSERT INTO public."artistGenre" VALUES (9, 9);
INSERT INTO public."artistGenre" VALUES (9, 13);
INSERT INTO public."artistGenre" VALUES (10, 6);
INSERT INTO public."artistGenre" VALUES (10, 32);
INSERT INTO public."artistGenre" VALUES (10, 24);
INSERT INTO public."artistGenre" VALUES (10, 30);
INSERT INTO public."artistGenre" VALUES (10, 19);
INSERT INTO public."artistGenre" VALUES (11, 10);
INSERT INTO public."artistGenre" VALUES (11, 14);
INSERT INTO public."artistGenre" VALUES (12, 10);
INSERT INTO public."artistGenre" VALUES (13, 15);
INSERT INTO public."artistGenre" VALUES (13, 3);
INSERT INTO public."artistGenre" VALUES (13, 16);
INSERT INTO public."artistGenre" VALUES (13, 17);
INSERT INTO public."artistGenre" VALUES (13, 18);
INSERT INTO public."artistGenre" VALUES (14, 19);
INSERT INTO public."artistGenre" VALUES (14, 20);
INSERT INTO public."artistGenre" VALUES (14, 21);
INSERT INTO public."artistGenre" VALUES (14, 22);
INSERT INTO public."artistGenre" VALUES (16, 23);
INSERT INTO public."artistGenre" VALUES (18, 16);
INSERT INTO public."artistGenre" VALUES (18, 20);
INSERT INTO public."artistGenre" VALUES (18, 6);
INSERT INTO public."artistGenre" VALUES (18, 3);
INSERT INTO public."artistGenre" VALUES (19, 6);
INSERT INTO public."artistGenre" VALUES (19, 24);
INSERT INTO public."artistGenre" VALUES (20, 19);
INSERT INTO public."artistGenre" VALUES (20, 22);
INSERT INTO public."artistGenre" VALUES (20, 25);
INSERT INTO public."artistGenre" VALUES (20, 26);
INSERT INTO public."artistGenre" VALUES (21, 39);
INSERT INTO public."artistGenre" VALUES (21, 22);
INSERT INTO public."artistGenre" VALUES (22, 27);
INSERT INTO public."artistGenre" VALUES (22, 3);
INSERT INTO public."artistGenre" VALUES (22, 28);
INSERT INTO public."artistGenre" VALUES (22, 15);
INSERT INTO public."artistGenre" VALUES (22, 16);
INSERT INTO public."artistGenre" VALUES (23, 19);
INSERT INTO public."artistGenre" VALUES (23, 20);
INSERT INTO public."artistGenre" VALUES (23, 21);
INSERT INTO public."artistGenre" VALUES (24, 19);
INSERT INTO public."artistGenre" VALUES (24, 29);
INSERT INTO public."artistGenre" VALUES (25, 23);
INSERT INTO public."artistGenre" VALUES (25, 30);
INSERT INTO public."artistGenre" VALUES (25, 9);
INSERT INTO public."artistGenre" VALUES (25, 31);
INSERT INTO public."artistGenre" VALUES (25, 6);
INSERT INTO public."artistGenre" VALUES (26, 6);
INSERT INTO public."artistGenre" VALUES (26, 7);
INSERT INTO public."artistGenre" VALUES (26, 24);
INSERT INTO public."artistGenre" VALUES (26, 32);
INSERT INTO public."artistGenre" VALUES (27, 33);
INSERT INTO public."artistGenre" VALUES (27, 34);
INSERT INTO public."artistGenre" VALUES (27, 35);
INSERT INTO public."artistGenre" VALUES (29, 46);
INSERT INTO public."artistGenre" VALUES (30, 43);
INSERT INTO public."artistGenre" VALUES (30, 44);
INSERT INTO public."artistGenre" VALUES (30, 45);
INSERT INTO public."artistGenre" VALUES (31, 36);
INSERT INTO public."artistGenre" VALUES (32, 28);
INSERT INTO public."artistGenre" VALUES (32, 37);
INSERT INTO public."artistGenre" VALUES (37, 38);
INSERT INTO public."artistGenre" VALUES (38, 19);
INSERT INTO public."artistGenre" VALUES (39, 23);
INSERT INTO public."artistGenre" VALUES (39, 30);
INSERT INTO public."artistGenre" VALUES (39, 31);
INSERT INTO public."artistGenre" VALUES (40, 23);
INSERT INTO public."artistGenre" VALUES (40, 30);
INSERT INTO public."artistGenre" VALUES (40, 31);
INSERT INTO public."artistGenre" VALUES (40, 47);
INSERT INTO public."artistGenre" VALUES (42, 42);
INSERT INTO public."artistGenre" VALUES (43, 39);
INSERT INTO public."artistGenre" VALUES (43, 40);
INSERT INTO public."artistGenre" VALUES (43, 19);
INSERT INTO public."artistGenre" VALUES (43, 41);


--
-- TOC entry 3496 (class 0 OID 16426)
-- Dependencies: 224
-- Data for Name: countries; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3502 (class 0 OID 16475)
-- Dependencies: 230
-- Data for Name: playLists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."playLists" VALUES (1, 'Means A Lot To Me', 4, '2024-10-20', 4863, '');
INSERT INTO public."playLists" VALUES (2, 'Death Notices Full OST', 9, '2023-10-29', 81189, '');
INSERT INTO public."playLists" VALUES (3, 'Best of 2025', 1, '2025-05-08', 85456, '');


--
-- TOC entry 3513 (class 0 OID 16583)
-- Dependencies: 241
-- Data for Name: playistTags; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."playistTags" VALUES (1, 1);
INSERT INTO public."playistTags" VALUES (3, 1);
INSERT INTO public."playistTags" VALUES (4, 1);
INSERT INTO public."playistTags" VALUES (3, 2);
INSERT INTO public."playistTags" VALUES (6, 2);
INSERT INTO public."playistTags" VALUES (4, 2);
INSERT INTO public."playistTags" VALUES (2, 3);
INSERT INTO public."playistTags" VALUES (5, 3);
INSERT INTO public."playistTags" VALUES (4, 3);


--
-- TOC entry 3492 (class 0 OID 16398)
-- Dependencies: 220
-- Data for Name: subscriptions; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.subscriptions VALUES (1, 'Premium');
INSERT INTO public.subscriptions VALUES (2, 'Free');


--
-- TOC entry 3512 (class 0 OID 16570)
-- Dependencies: 240
-- Data for Name: trackArtists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."trackArtists" VALUES (1, 1);
INSERT INTO public."trackArtists" VALUES (2, 2);
INSERT INTO public."trackArtists" VALUES (3, 1);
INSERT INTO public."trackArtists" VALUES (4, 2);
INSERT INTO public."trackArtists" VALUES (5, 3);
INSERT INTO public."trackArtists" VALUES (5, 4);
INSERT INTO public."trackArtists" VALUES (6, 2);
INSERT INTO public."trackArtists" VALUES (7, 5);
INSERT INTO public."trackArtists" VALUES (8, 6);
INSERT INTO public."trackArtists" VALUES (9, 3);
INSERT INTO public."trackArtists" VALUES (9, 7);
INSERT INTO public."trackArtists" VALUES (10, 8);
INSERT INTO public."trackArtists" VALUES (11, 9);
INSERT INTO public."trackArtists" VALUES (11, 10);
INSERT INTO public."trackArtists" VALUES (12, 2);
INSERT INTO public."trackArtists" VALUES (13, 11);
INSERT INTO public."trackArtists" VALUES (13, 12);
INSERT INTO public."trackArtists" VALUES (14, 13);
INSERT INTO public."trackArtists" VALUES (15, 13);
INSERT INTO public."trackArtists" VALUES (16, 13);
INSERT INTO public."trackArtists" VALUES (17, 1);
INSERT INTO public."trackArtists" VALUES (18, 13);
INSERT INTO public."trackArtists" VALUES (19, 6);
INSERT INTO public."trackArtists" VALUES (20, 3);
INSERT INTO public."trackArtists" VALUES (20, 4);
INSERT INTO public."trackArtists" VALUES (20, 7);
INSERT INTO public."trackArtists" VALUES (21, 3);
INSERT INTO public."trackArtists" VALUES (21, 7);
INSERT INTO public."trackArtists" VALUES (21, 4);
INSERT INTO public."trackArtists" VALUES (22, 14);
INSERT INTO public."trackArtists" VALUES (22, 15);
INSERT INTO public."trackArtists" VALUES (23, 16);
INSERT INTO public."trackArtists" VALUES (24, 17);
INSERT INTO public."trackArtists" VALUES (24, 18);
INSERT INTO public."trackArtists" VALUES (24, 3);
INSERT INTO public."trackArtists" VALUES (25, 19);
INSERT INTO public."trackArtists" VALUES (26, 20);
INSERT INTO public."trackArtists" VALUES (26, 21);
INSERT INTO public."trackArtists" VALUES (27, 22);
INSERT INTO public."trackArtists" VALUES (28, 23);
INSERT INTO public."trackArtists" VALUES (29, 23);
INSERT INTO public."trackArtists" VALUES (30, 24);
INSERT INTO public."trackArtists" VALUES (31, 6);
INSERT INTO public."trackArtists" VALUES (32, 25);
INSERT INTO public."trackArtists" VALUES (33, 14);
INSERT INTO public."trackArtists" VALUES (34, 26);
INSERT INTO public."trackArtists" VALUES (35, 13);
INSERT INTO public."trackArtists" VALUES (36, 19);
INSERT INTO public."trackArtists" VALUES (37, 27);
INSERT INTO public."trackArtists" VALUES (38, 28);
INSERT INTO public."trackArtists" VALUES (38, 29);
INSERT INTO public."trackArtists" VALUES (38, 30);
INSERT INTO public."trackArtists" VALUES (39, 31);
INSERT INTO public."trackArtists" VALUES (39, 32);
INSERT INTO public."trackArtists" VALUES (40, 33);
INSERT INTO public."trackArtists" VALUES (40, 34);
INSERT INTO public."trackArtists" VALUES (40, 35);
INSERT INTO public."trackArtists" VALUES (40, 36);
INSERT INTO public."trackArtists" VALUES (41, 37);
INSERT INTO public."trackArtists" VALUES (41, 38);
INSERT INTO public."trackArtists" VALUES (42, 39);
INSERT INTO public."trackArtists" VALUES (42, 40);
INSERT INTO public."trackArtists" VALUES (43, 41);
INSERT INTO public."trackArtists" VALUES (43, 38);
INSERT INTO public."trackArtists" VALUES (44, 19);
INSERT INTO public."trackArtists" VALUES (45, 42);
INSERT INTO public."trackArtists" VALUES (46, 43);


--
-- TOC entry 3509 (class 0 OID 16531)
-- Dependencies: 237
-- Data for Name: trackGenres; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."trackGenres" VALUES (1, 1);
INSERT INTO public."trackGenres" VALUES (1, 2);
INSERT INTO public."trackGenres" VALUES (1, 3);
INSERT INTO public."trackGenres" VALUES (2, 4);
INSERT INTO public."trackGenres" VALUES (2, 5);
INSERT INTO public."trackGenres" VALUES (3, 1);
INSERT INTO public."trackGenres" VALUES (3, 2);
INSERT INTO public."trackGenres" VALUES (3, 3);
INSERT INTO public."trackGenres" VALUES (4, 4);
INSERT INTO public."trackGenres" VALUES (4, 5);
INSERT INTO public."trackGenres" VALUES (5, 6);
INSERT INTO public."trackGenres" VALUES (5, 7);
INSERT INTO public."trackGenres" VALUES (6, 4);
INSERT INTO public."trackGenres" VALUES (6, 5);
INSERT INTO public."trackGenres" VALUES (7, 1);
INSERT INTO public."trackGenres" VALUES (7, 8);
INSERT INTO public."trackGenres" VALUES (8, 9);
INSERT INTO public."trackGenres" VALUES (8, 4);
INSERT INTO public."trackGenres" VALUES (9, 6);
INSERT INTO public."trackGenres" VALUES (9, 7);
INSERT INTO public."trackGenres" VALUES (10, 10);
INSERT INTO public."trackGenres" VALUES (10, 11);
INSERT INTO public."trackGenres" VALUES (10, 12);
INSERT INTO public."trackGenres" VALUES (11, 4);
INSERT INTO public."trackGenres" VALUES (11, 9);
INSERT INTO public."trackGenres" VALUES (11, 13);
INSERT INTO public."trackGenres" VALUES (12, 4);
INSERT INTO public."trackGenres" VALUES (12, 5);
INSERT INTO public."trackGenres" VALUES (13, 10);
INSERT INTO public."trackGenres" VALUES (13, 14);
INSERT INTO public."trackGenres" VALUES (14, 15);
INSERT INTO public."trackGenres" VALUES (14, 3);
INSERT INTO public."trackGenres" VALUES (14, 16);
INSERT INTO public."trackGenres" VALUES (14, 17);
INSERT INTO public."trackGenres" VALUES (14, 18);
INSERT INTO public."trackGenres" VALUES (15, 15);
INSERT INTO public."trackGenres" VALUES (15, 3);
INSERT INTO public."trackGenres" VALUES (15, 16);
INSERT INTO public."trackGenres" VALUES (15, 17);
INSERT INTO public."trackGenres" VALUES (15, 18);
INSERT INTO public."trackGenres" VALUES (16, 15);
INSERT INTO public."trackGenres" VALUES (16, 3);
INSERT INTO public."trackGenres" VALUES (16, 16);
INSERT INTO public."trackGenres" VALUES (16, 17);
INSERT INTO public."trackGenres" VALUES (16, 18);
INSERT INTO public."trackGenres" VALUES (17, 1);
INSERT INTO public."trackGenres" VALUES (17, 2);
INSERT INTO public."trackGenres" VALUES (17, 3);
INSERT INTO public."trackGenres" VALUES (18, 15);
INSERT INTO public."trackGenres" VALUES (18, 3);
INSERT INTO public."trackGenres" VALUES (18, 16);
INSERT INTO public."trackGenres" VALUES (18, 17);
INSERT INTO public."trackGenres" VALUES (18, 18);
INSERT INTO public."trackGenres" VALUES (19, 9);
INSERT INTO public."trackGenres" VALUES (19, 4);
INSERT INTO public."trackGenres" VALUES (20, 6);
INSERT INTO public."trackGenres" VALUES (20, 7);
INSERT INTO public."trackGenres" VALUES (21, 6);
INSERT INTO public."trackGenres" VALUES (21, 7);
INSERT INTO public."trackGenres" VALUES (22, 19);
INSERT INTO public."trackGenres" VALUES (22, 20);
INSERT INTO public."trackGenres" VALUES (22, 21);
INSERT INTO public."trackGenres" VALUES (22, 22);
INSERT INTO public."trackGenres" VALUES (23, 23);
INSERT INTO public."trackGenres" VALUES (25, 6);
INSERT INTO public."trackGenres" VALUES (25, 24);
INSERT INTO public."trackGenres" VALUES (26, 19);
INSERT INTO public."trackGenres" VALUES (26, 22);
INSERT INTO public."trackGenres" VALUES (26, 25);
INSERT INTO public."trackGenres" VALUES (26, 26);
INSERT INTO public."trackGenres" VALUES (27, 27);
INSERT INTO public."trackGenres" VALUES (27, 3);
INSERT INTO public."trackGenres" VALUES (27, 28);
INSERT INTO public."trackGenres" VALUES (27, 15);
INSERT INTO public."trackGenres" VALUES (27, 16);
INSERT INTO public."trackGenres" VALUES (28, 19);
INSERT INTO public."trackGenres" VALUES (28, 20);
INSERT INTO public."trackGenres" VALUES (28, 21);
INSERT INTO public."trackGenres" VALUES (29, 19);
INSERT INTO public."trackGenres" VALUES (29, 20);
INSERT INTO public."trackGenres" VALUES (29, 21);
INSERT INTO public."trackGenres" VALUES (30, 19);
INSERT INTO public."trackGenres" VALUES (30, 29);
INSERT INTO public."trackGenres" VALUES (31, 9);
INSERT INTO public."trackGenres" VALUES (31, 4);
INSERT INTO public."trackGenres" VALUES (32, 23);
INSERT INTO public."trackGenres" VALUES (32, 30);
INSERT INTO public."trackGenres" VALUES (32, 9);
INSERT INTO public."trackGenres" VALUES (32, 31);
INSERT INTO public."trackGenres" VALUES (32, 6);
INSERT INTO public."trackGenres" VALUES (33, 19);
INSERT INTO public."trackGenres" VALUES (33, 20);
INSERT INTO public."trackGenres" VALUES (33, 21);
INSERT INTO public."trackGenres" VALUES (33, 22);
INSERT INTO public."trackGenres" VALUES (34, 6);
INSERT INTO public."trackGenres" VALUES (34, 7);
INSERT INTO public."trackGenres" VALUES (34, 24);
INSERT INTO public."trackGenres" VALUES (34, 32);
INSERT INTO public."trackGenres" VALUES (35, 15);
INSERT INTO public."trackGenres" VALUES (35, 3);
INSERT INTO public."trackGenres" VALUES (35, 16);
INSERT INTO public."trackGenres" VALUES (35, 17);
INSERT INTO public."trackGenres" VALUES (35, 18);
INSERT INTO public."trackGenres" VALUES (36, 6);
INSERT INTO public."trackGenres" VALUES (36, 24);
INSERT INTO public."trackGenres" VALUES (37, 33);
INSERT INTO public."trackGenres" VALUES (37, 34);
INSERT INTO public."trackGenres" VALUES (37, 35);
INSERT INTO public."trackGenres" VALUES (39, 36);
INSERT INTO public."trackGenres" VALUES (41, 38);
INSERT INTO public."trackGenres" VALUES (42, 23);
INSERT INTO public."trackGenres" VALUES (42, 30);
INSERT INTO public."trackGenres" VALUES (42, 31);
INSERT INTO public."trackGenres" VALUES (44, 6);
INSERT INTO public."trackGenres" VALUES (44, 24);
INSERT INTO public."trackGenres" VALUES (45, 42);
INSERT INTO public."trackGenres" VALUES (46, 39);
INSERT INTO public."trackGenres" VALUES (46, 40);
INSERT INTO public."trackGenres" VALUES (46, 19);
INSERT INTO public."trackGenres" VALUES (46, 41);


--
-- TOC entry 3514 (class 0 OID 16596)
-- Dependencies: 242
-- Data for Name: trackPlaylists; Type: TABLE DATA; Schema: public; Owner: -
--



--
-- TOC entry 3516 (class 0 OID 16635)
-- Dependencies: 244
-- Data for Name: userPlaylists; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."userPlaylists" VALUES (4, 1);
INSERT INTO public."userPlaylists" VALUES (9, 2);
INSERT INTO public."userPlaylists" VALUES (1, 3);
INSERT INTO public."userPlaylists" VALUES (2, 2);
INSERT INTO public."userPlaylists" VALUES (3, 3);
INSERT INTO public."userPlaylists" VALUES (5, 2);
INSERT INTO public."userPlaylists" VALUES (6, 1);
INSERT INTO public."userPlaylists" VALUES (7, 2);
INSERT INTO public."userPlaylists" VALUES (8, 1);
INSERT INTO public."userPlaylists" VALUES (10, 3);


--
-- TOC entry 3490 (class 0 OID 16389)
-- Dependencies: 218
-- Data for Name: userRoles; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."userRoles" VALUES (1, 'Admin');
INSERT INTO public."userRoles" VALUES (2, 'Manager');
INSERT INTO public."userRoles" VALUES (3, 'User');
INSERT INTO public."userRoles" VALUES (4, 'Guest');


--
-- TOC entry 3494 (class 0 OID 16407)
-- Dependencies: 222
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public.users VALUES (1, 'apollinariya.alienova', 'xKrillby', 'Апполинария Алуенова', 'apollinariya.alienova@outlook.com', 2, 1, '2025-10-29', '2025-10-29');
INSERT INTO public.users VALUES (2, 'koorthy.kozilina', 'RDJCXyA', 'Леонтий Козлитин', 'koorthy.kozilina@ya.ru', 2, 1, '2025-10-28', '2025-10-29');
INSERT INTO public.users VALUES (3, 'lyudmin.a.bakumova', 'E3rg2bG', 'Людмила Абакумова', 'lyudmin.a.bakumova@hotmail.com', 3, 1, '2025-09-29', '2025-10-29');
INSERT INTO public.users VALUES (4, 'anjela.privalova', 'mZPXABf', 'Анжела Привалова', 'anjela.privalova@rambler.ru', 2, 1, '2025-10-20', '2025-10-29');
INSERT INTO public.users VALUES (5, 'kseniya1978', 'bcVNEGaB', 'Ксения Хуторская', 'kseniya1978@rambler.ru', 3, 2, '2025-08-05', '2025-10-29');
INSERT INTO public.users VALUES (6, 'oksana95', 'c35tx4ov', 'Оксана Добронравова', 'oksana95@ya.ru', 4, 2, '2025-10-29', '2025-10-29');
INSERT INTO public.users VALUES (7, 'antonina.pushmenkova', 'it3cy55', 'Антонина Пушменкова', 'antonina.pushmenkova@mail.ru', 1, 1, '2025-10-29', '2025-10-29');
INSERT INTO public.users VALUES (8, 'asya.vedova', 'z6kkfwhf', 'Ася Шведова', 'asya.vedova@mail.ru', 3, 1, '2025-10-29', '2025-10-29');
INSERT INTO public.users VALUES (9, 'aleksey.novojilov', 'LXF4kEQq', 'Алексей Новожилов', 'aleksey.novojilov@mail.ru', 1, 2, '2025-10-29', '2025-10-29');
INSERT INTO public.users VALUES (10, 'petr1968', 'KSPU52zo', 'Петр Крылаев', 'petr1968@ya.ru', 3, 2, '2025-10-29', '2025-10-29');


--
-- TOC entry 3534 (class 0 OID 0)
-- Dependencies: 231
-- Name: Albums_album_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Albums_album_id_seq"', 1, false);


--
-- TOC entry 3535 (class 0 OID 0)
-- Dependencies: 227
-- Name: Artists_artist_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Artists_artist_id_seq"', 1, false);


--
-- TOC entry 3536 (class 0 OID 0)
-- Dependencies: 235
-- Name: Genres_genre_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Genres_genre_id_seq"', 1, false);


--
-- TOC entry 3537 (class 0 OID 0)
-- Dependencies: 225
-- Name: Tags_tag_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Tags_tag_id_seq"', 6, true);


--
-- TOC entry 3538 (class 0 OID 0)
-- Dependencies: 233
-- Name: Tracks_track_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Tracks_track_id_seq"', 1, false);


--
-- TOC entry 3539 (class 0 OID 0)
-- Dependencies: 223
-- Name: countries_country_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.countries_country_id_seq', 1, false);


--
-- TOC entry 3540 (class 0 OID 0)
-- Dependencies: 229
-- Name: playLists_playlist_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."playLists_playlist_id_seq"', 1, false);


--
-- TOC entry 3541 (class 0 OID 0)
-- Dependencies: 219
-- Name: subscriptions_sub_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.subscriptions_sub_id_seq', 2, true);


--
-- TOC entry 3542 (class 0 OID 0)
-- Dependencies: 217
-- Name: userRoles_role_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."userRoles_role_id_seq"', 4, true);


--
-- TOC entry 3543 (class 0 OID 0)
-- Dependencies: 221
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.users_user_id_seq', 1, false);


--
-- TOC entry 3317 (class 2606 OID 16499)
-- Name: Albums Albums_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Albums"
    ADD CONSTRAINT "Albums_pkey" PRIMARY KEY (album_id);


--
-- TOC entry 3311 (class 2606 OID 16451)
-- Name: Artists Artists_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Artists"
    ADD CONSTRAINT "Artists_pkey" PRIMARY KEY (artist_id);


--
-- TOC entry 3321 (class 2606 OID 16530)
-- Name: Genres Genres_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Genres"
    ADD CONSTRAINT "Genres_pkey" PRIMARY KEY (genre_id);


--
-- TOC entry 3309 (class 2606 OID 16442)
-- Name: Tags Tags_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tags"
    ADD CONSTRAINT "Tags_pkey" PRIMARY KEY (tag_id);


--
-- TOC entry 3319 (class 2606 OID 16515)
-- Name: Tracks Tracks_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tracks"
    ADD CONSTRAINT "Tracks_pkey" PRIMARY KEY (track_id);


--
-- TOC entry 3307 (class 2606 OID 16433)
-- Name: countries countries_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_pkey PRIMARY KEY (country_id);


--
-- TOC entry 3313 (class 2606 OID 16483)
-- Name: playLists playLists_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."playLists"
    ADD CONSTRAINT "playLists_pkey" PRIMARY KEY (playlist_id);


--
-- TOC entry 3315 (class 2606 OID 16485)
-- Name: playLists playLists_playlistName_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."playLists"
    ADD CONSTRAINT "playLists_playlistName_key" UNIQUE ("playlistName");


--
-- TOC entry 3303 (class 2606 OID 16405)
-- Name: subscriptions subscriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.subscriptions
    ADD CONSTRAINT subscriptions_pkey PRIMARY KEY (sub_id);


--
-- TOC entry 3301 (class 2606 OID 16396)
-- Name: userRoles userRoles_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."userRoles"
    ADD CONSTRAINT "userRoles_pkey" PRIMARY KEY (role_id);


--
-- TOC entry 3305 (class 2606 OID 16414)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 3326 (class 2606 OID 16500)
-- Name: Albums Albums_artist_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Albums"
    ADD CONSTRAINT "Albums_artist_fkey" FOREIGN KEY (artist) REFERENCES public."Artists"(artist_id);


--
-- TOC entry 3324 (class 2606 OID 16452)
-- Name: Artists Artists_artistCountry_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Artists"
    ADD CONSTRAINT "Artists_artistCountry_fkey" FOREIGN KEY ("artistCountry") REFERENCES public.countries(country_id);


--
-- TOC entry 3327 (class 2606 OID 16516)
-- Name: Tracks Tracks_artist_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tracks"
    ADD CONSTRAINT "Tracks_artist_fkey" FOREIGN KEY (artist) REFERENCES public."Artists"(artist_id);


--
-- TOC entry 3330 (class 2606 OID 16552)
-- Name: albumGenres albumGenres_AGenre_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."albumGenres"
    ADD CONSTRAINT "albumGenres_AGenre_fkey" FOREIGN KEY ("AGenre") REFERENCES public."Genres"(genre_id);


--
-- TOC entry 3331 (class 2606 OID 16547)
-- Name: albumGenres albumGenres_Album_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."albumGenres"
    ADD CONSTRAINT "albumGenres_Album_fkey" FOREIGN KEY ("Album") REFERENCES public."Albums"(album_id);


--
-- TOC entry 3340 (class 2606 OID 16630)
-- Name: albumTracks albumTracks_ATrack_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."albumTracks"
    ADD CONSTRAINT "albumTracks_ATrack_fkey" FOREIGN KEY ("ATrack") REFERENCES public."Tracks"(track_id);


--
-- TOC entry 3341 (class 2606 OID 16625)
-- Name: albumTracks albumTracks_TAlbum_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."albumTracks"
    ADD CONSTRAINT "albumTracks_TAlbum_fkey" FOREIGN KEY ("TAlbum") REFERENCES public."Albums"(album_id);


--
-- TOC entry 3332 (class 2606 OID 16565)
-- Name: artistGenre artistGenre_AGenre_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."artistGenre"
    ADD CONSTRAINT "artistGenre_AGenre_fkey" FOREIGN KEY ("AGenre") REFERENCES public."Genres"(genre_id);


--
-- TOC entry 3333 (class 2606 OID 16560)
-- Name: artistGenre artistGenre_Artist_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."artistGenre"
    ADD CONSTRAINT "artistGenre_Artist_fkey" FOREIGN KEY ("Artist") REFERENCES public."Artists"(artist_id);


--
-- TOC entry 3325 (class 2606 OID 16486)
-- Name: playLists playLists_userCreator_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."playLists"
    ADD CONSTRAINT "playLists_userCreator_fkey" FOREIGN KEY ("userCreator") REFERENCES public.users(user_id);


--
-- TOC entry 3336 (class 2606 OID 16586)
-- Name: playistTags playistTags_Tag_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."playistTags"
    ADD CONSTRAINT "playistTags_Tag_fkey" FOREIGN KEY ("Tag") REFERENCES public."Tags"(tag_id);


--
-- TOC entry 3337 (class 2606 OID 16591)
-- Name: playistTags playistTags_playList_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."playistTags"
    ADD CONSTRAINT "playistTags_playList_fkey" FOREIGN KEY ("playList") REFERENCES public."playLists"(playlist_id);


--
-- TOC entry 3334 (class 2606 OID 16578)
-- Name: trackArtists trackArtists_relatedArtist_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."trackArtists"
    ADD CONSTRAINT "trackArtists_relatedArtist_fkey" FOREIGN KEY ("relatedArtist") REFERENCES public."Artists"(artist_id);


--
-- TOC entry 3335 (class 2606 OID 16573)
-- Name: trackArtists trackArtists_relatedTrack_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."trackArtists"
    ADD CONSTRAINT "trackArtists_relatedTrack_fkey" FOREIGN KEY ("relatedTrack") REFERENCES public."Tracks"(track_id);


--
-- TOC entry 3328 (class 2606 OID 16539)
-- Name: trackGenres trackGenres_TGenre_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."trackGenres"
    ADD CONSTRAINT "trackGenres_TGenre_fkey" FOREIGN KEY ("TGenre") REFERENCES public."Genres"(genre_id);


--
-- TOC entry 3329 (class 2606 OID 16534)
-- Name: trackGenres trackGenres_Track_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."trackGenres"
    ADD CONSTRAINT "trackGenres_Track_fkey" FOREIGN KEY ("Track") REFERENCES public."Tracks"(track_id);


--
-- TOC entry 3338 (class 2606 OID 16604)
-- Name: trackPlaylists trackPlaylists_playlist_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."trackPlaylists"
    ADD CONSTRAINT "trackPlaylists_playlist_fkey" FOREIGN KEY (playlist) REFERENCES public."playLists"(playlist_id);


--
-- TOC entry 3339 (class 2606 OID 16599)
-- Name: trackPlaylists trackPlaylists_track_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."trackPlaylists"
    ADD CONSTRAINT "trackPlaylists_track_fkey" FOREIGN KEY (track) REFERENCES public."Tracks"(track_id);


--
-- TOC entry 3342 (class 2606 OID 16643)
-- Name: userPlaylists userPlaylists_UPlaylist_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."userPlaylists"
    ADD CONSTRAINT "userPlaylists_UPlaylist_fkey" FOREIGN KEY ("UPlaylist") REFERENCES public."playLists"(playlist_id);


--
-- TOC entry 3343 (class 2606 OID 16638)
-- Name: userPlaylists userPlaylists_User_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."userPlaylists"
    ADD CONSTRAINT "userPlaylists_User_fkey" FOREIGN KEY ("User") REFERENCES public.users(user_id);


--
-- TOC entry 3322 (class 2606 OID 16415)
-- Name: users users_role_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_role_fkey FOREIGN KEY (role) REFERENCES public."userRoles"(role_id);


--
-- TOC entry 3323 (class 2606 OID 16420)
-- Name: users users_subscriptionType_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT "users_subscriptionType_fkey" FOREIGN KEY ("subscriptionType") REFERENCES public.subscriptions(sub_id);


-- Completed on 2025-12-02 17:27:02

--
-- PostgreSQL database dump complete
--

