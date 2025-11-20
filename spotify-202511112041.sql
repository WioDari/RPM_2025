--
-- PostgreSQL database dump
--

\restrict oz6mgA6NlbswyFrXh5GVpqXbVglY0yVJfxyF4eEfkoJanxcu51CeETDYqnLtq2r

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

-- Started on 2025-11-11 20:41:26

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
-- TOC entry 242 (class 1259 OID 33280)
-- Name: albumgenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.albumgenres (
    albumid integer NOT NULL,
    genreid integer NOT NULL
);


--
-- TOC entry 236 (class 1259 OID 33181)
-- Name: albums; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.albums (
    albumid integer NOT NULL,
    albumname character varying NOT NULL,
    albumreleaseyear character varying(10) NOT NULL,
    coverpath character varying NOT NULL,
    albumduration character varying(10) NOT NULL
);


--
-- TOC entry 235 (class 1259 OID 33180)
-- Name: albums_albumid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.albums ALTER COLUMN albumid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.albums_albumid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 241 (class 1259 OID 33263)
-- Name: albumtracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.albumtracks (
    albumid integer NOT NULL,
    trackid integer NOT NULL
);


--
-- TOC entry 239 (class 1259 OID 33229)
-- Name: artistalbums; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.artistalbums (
    artistid integer NOT NULL,
    albumid integer NOT NULL
);


--
-- TOC entry 240 (class 1259 OID 33246)
-- Name: artistgenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.artistgenres (
    artistid integer NOT NULL,
    genreid integer NOT NULL
);


--
-- TOC entry 229 (class 1259 OID 33116)
-- Name: artists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.artists (
    artistid integer NOT NULL,
    artistname character varying(30) NOT NULL,
    countryid integer CONSTRAINT artists_countyid_not_null NOT NULL,
    activeyears character varying(10) NOT NULL,
    artistdescription character varying NOT NULL,
    photopath character varying NOT NULL
);


--
-- TOC entry 228 (class 1259 OID 33115)
-- Name: artists_artistid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.artists ALTER COLUMN artistid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.artists_artistid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 225 (class 1259 OID 33096)
-- Name: countries; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.countries (
    countryid integer NOT NULL,
    countryname character varying(30) NOT NULL
);


--
-- TOC entry 224 (class 1259 OID 33095)
-- Name: countries_countryid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.countries ALTER COLUMN countryid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.countries_countryid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 227 (class 1259 OID 33106)
-- Name: genres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.genres (
    genreid integer NOT NULL,
    genrename character varying(30) NOT NULL
);


--
-- TOC entry 226 (class 1259 OID 33105)
-- Name: genres_genreid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.genres ALTER COLUMN genreid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.genres_genreid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 232 (class 1259 OID 33151)
-- Name: playlists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.playlists (
    playlistid smallint NOT NULL,
    playlistname character varying NOT NULL,
    userid integer NOT NULL,
    playlistcreationdate date NOT NULL,
    likes integer NOT NULL,
    playlistdescription character varying
);


--
-- TOC entry 238 (class 1259 OID 33212)
-- Name: playlisttags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.playlisttags (
    playlistid smallint NOT NULL,
    tagid smallint NOT NULL
);


--
-- TOC entry 237 (class 1259 OID 33195)
-- Name: playlisttracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.playlisttracks (
    playlistid smallint NOT NULL,
    trackid integer NOT NULL
);


--
-- TOC entry 221 (class 1259 OID 33076)
-- Name: roles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.roles (
    roleid smallint NOT NULL,
    rolename character varying(20) NOT NULL
);


--
-- TOC entry 220 (class 1259 OID 33075)
-- Name: roles_roleid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.roles ALTER COLUMN roleid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.roles_roleid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 223 (class 1259 OID 33086)
-- Name: subscriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.subscriptions (
    subscriptionid smallint NOT NULL,
    subscriptionname character varying(20) NOT NULL
);


--
-- TOC entry 222 (class 1259 OID 33085)
-- Name: subscriptions_subscriptionid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.subscriptions ALTER COLUMN subscriptionid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.subscriptions_subscriptionid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 234 (class 1259 OID 33164)
-- Name: tags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tags (
    tagid smallint NOT NULL,
    tagname character varying(30) NOT NULL
);


--
-- TOC entry 233 (class 1259 OID 33163)
-- Name: tags_tagid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.tags ALTER COLUMN tagid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.tags_tagid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 243 (class 1259 OID 33297)
-- Name: trackartists; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.trackartists (
    trackid integer NOT NULL,
    artistid integer NOT NULL
);


--
-- TOC entry 244 (class 1259 OID 33314)
-- Name: trackgenres; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.trackgenres (
    trackid integer NOT NULL,
    genreid integer NOT NULL
);


--
-- TOC entry 231 (class 1259 OID 33134)
-- Name: tracks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tracks (
    trackid integer NOT NULL,
    trackname character varying NOT NULL,
    trackduration character varying(10) NOT NULL,
    trackreleasedate date NOT NULL,
    bitrate smallint NOT NULL,
    filepath character varying NOT NULL,
    rating numeric(3,2) NOT NULL,
    playcount integer NOT NULL
);


--
-- TOC entry 230 (class 1259 OID 33133)
-- Name: tracks_trackid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public.tracks ALTER COLUMN trackid ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.tracks_trackid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 219 (class 1259 OID 33060)
-- Name: users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.users (
    userid integer NOT NULL,
    login character varying(30) NOT NULL,
    password character varying(20) NOT NULL,
    email character varying(50) NOT NULL,
    roleid smallint NOT NULL,
    subscriptionid smallint NOT NULL,
    registrationdate date NOT NULL,
    lastlogindate date NOT NULL,
    fullname character varying(30) NOT NULL
);


--
-- TOC entry 5076 (class 0 OID 33280)
-- Dependencies: 242
-- Data for Name: albumgenres; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.albumgenres (albumid, genreid) FROM stdin;
1	2
1	26
1	27
1	32
1	28
3	4
3	15
3	22
3	36
4	20
4	5
5	16
5	26
5	45
6	30
6	13
8	42
9	5
9	6
10	30
10	13
12	40
13	5
13	6
14	2
14	26
14	27
14	32
14	28
15	17
15	18
15	19
15	15
16	8
16	46
17	2
17	26
17	27
17	32
17	28
18	30
18	13
19	10
19	17
19	33
19	15
20	1
20	35
20	39
21	16
21	26
21	45
22	29
22	34
22	37
23	10
23	33
23	15
24	7
24	14
24	30
24	13
25	14
25	13
26	11
26	45
27	6
27	13
27	29
27	34
27	37
28	20
28	5
29	26
29	27
29	28
29	47
29	44
30	10
30	17
30	33
30	15
31	14
31	13
32	21
32	15
33	5
33	6
34	30
34	13
35	16
35	26
36	6
36	23
36	5
37	2
37	26
37	27
37	32
37	28
38	10
38	33
38	15
40	14
40	13
41	37
42	41
43	3
43	12
43	46
\.


--
-- TOC entry 5070 (class 0 OID 33181)
-- Dependencies: 236
-- Data for Name: albums; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.albums (albumid, albumname, albumreleaseyear, coverpath, albumduration) FROM stdin;
1	As Alive As You Need Me To Be	2025	https://i.scdn.co/image/ab67616d0000b27317ff2581a6b924fb3a704395	0
2	Atomic Heart, Vol.5 (Original Game Soundtrack)	2025	https://i.scdn.co/image/ab67616d0000b2731a24aab10d2b27d63175c37a	0
3	Baby	2025	https://i.scdn.co/image/ab67616d0000b273fc679c0d97b173113141a42b	0
4	Barbara Barbara, we face a shining future	2016	https://i.scdn.co/image/ab67616d0000b273cf5cdbf0397a8a71c9fd0395	0
5	Bloodline (Bonus Tracks)	1992	https://i.scdn.co/image/ab67616d0000b273c74e21dfa7e9470c0c980f9f	0
6	Breed (feat. REEBZ)	2023	https://i.scdn.co/image/ab67616d0000b2737163994ea19cc74c0bbfb31e	0
7	Call of Duty®: Black Ops 6 - Zombies "The Tomb" (Original Soundtrack)	2025	https://i.scdn.co/image/ab67616d0000b2732db3154893852b63232e1466	0
8	CHOMPO III	2025	https://i.scdn.co/image/ab67616d0000b273aadd94c74f32483d503fb0ab	0
9	Cinematic Soundscape	2012	https://i.scdn.co/image/ab67616d0000b27381c4fc91dc2abd939c2c705f	0
10	Circle	2022	https://i.scdn.co/image/ab67616d0000b273f0f4f4f180ea0f32ac2c5f8f	0
11	Data Destruction	2025	https://i.scdn.co/image/ab67616d0000b273c6ad3e09b75355deee5b732d	0
12	Final Destination	2025	https://i.scdn.co/image/ab67616d0000b273857bb2eff1c145e26e9a2c1c	0
13	Formula Of Fear	2008	https://i.scdn.co/image/ab67616d0000b27359566bca173ee54657bd5f8d	0
14	Further Down The Spiral	1995	https://i.scdn.co/image/ab67616d0000b273551ecb57cda40ae833e3e5b1	0
15	Fuze	2025	https://i.scdn.co/image/ab67616d0000b27362d6852a9461f7c5579097c6	0
16	Ghost City Daze	2022	https://i.scdn.co/image/ab67616d0000b273ab65f724c2fdf067999d8d16	0
17	Ghosts V: Together	2020	https://i.scdn.co/image/ab67616d0000b27327c5a582320e08258ec13d0a	0
18	GOFASTER (YANA100)	2025	https://i.scdn.co/image/ab67616d0000b2735ff50f46b8b8720981957d51	0
19	Grave	2025	https://i.scdn.co/image/ab67616d0000b273659f2e9dc34715a487f073be	0
20	I See Red	2025	https://i.scdn.co/image/ab67616d0000b2737f34486112bf81e38838a429	0
21	Liquid (Bonus Track Version)	2000	https://i.scdn.co/image/ab67616d0000b273211808193df8743aedb1038b	0
22	Lvstlove	2025	https://i.scdn.co/image/ab67616d0000b2737348b951edbf4bb86854e806	0
23	Metalicious	2025	https://i.scdn.co/image/ab67616d0000b27368e7513b4ca1da14da0a81e6	0
24	Moments In Everglow	2025	https://i.scdn.co/image/ab67616d0000b27379edc5965038d4923dff96b0	0
25	Muleta	2025	https://i.scdn.co/image/ab67616d0000b273adebc14e80184ee7cc2e36b8	0
26	Never, Never, Land	2003	https://i.scdn.co/image/ab67616d0000b273894c4f974159696026573eb5	0
27	NO MERCY III	2025	https://i.scdn.co/image/ab67616d0000b273a10a1e401fc110168dfd02f3	0
28	Oblivion With Bells	2007	https://i.scdn.co/image/ab67616d0000b2736b09703b5a9e23a2c669012b	0
29	ORDINARY LOSS	2025	https://i.scdn.co/image/ab67616d0000b27367bd88cd7a659c93b34b0688	0
30	Overload 2K	2025	https://i.scdn.co/image/ab67616d0000b273a24c69273fb3656b3ecb92a7	0
31	Repentance	2025	https://i.scdn.co/image/ab67616d0000b2730f423970a440915f6750e11e	0
32	Run	2025	https://i.scdn.co/image/ab67616d0000b273af743f04ff7cc6ae7300929f	0
33	Siren Of The Storm	2025	https://i.scdn.co/image/ab67616d0000b273628b577615d97f6837e30b73	0
34	SOMEWHEREIBELONG	2025	https://i.scdn.co/image/ab67616d0000b27328dc64c5aa8c59dd37a7d820	0
35	Subhuman	2007	https://i.scdn.co/image/ab67616d0000b273ce0298116be15cb81f91a1aa	0
36	The Day Is My Enemy (Expanded Edition)	2015	https://i.scdn.co/image/ab67616d0000b273872772e0b165e57a104fd37a	0
37	The Fragile	1999	https://i.scdn.co/image/ab67616d0000b273237a21c8ba1bfc2d85a83585	0
38	The Machine	2025	https://i.scdn.co/image/ab67616d0000b27328700d650196f5918a0578bf	0
39	The Masquerade (Zardonic & Toronto Is Broken Remix)	2025	https://i.scdn.co/image/ab67616d0000b273b9bb9b27f5aeb0e1a1475707	0
40	The Star	2025	https://i.scdn.co/image/ab67616d0000b2731e01b0b41fa3bf96bab342cb	0
41	Wall I Was	2025	https://i.scdn.co/image/ab67616d0000b273710d4624a5a1b87b70b56bb2	0
42	общедомовой	2025	https://i.scdn.co/image/ab67616d0000b273f11a5383c11039c003e9008b	0
43	あなたを待って	2015	https://i.scdn.co/image/ab67616d0000b273fcc3cb5cc1166a95b86f5e6e	0
\.


--
-- TOC entry 5075 (class 0 OID 33263)
-- Dependencies: 241
-- Data for Name: albumtracks; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.albumtracks (albumid, trackid) FROM stdin;
1	2
2	45
3	4
4	21
5	14
6	6
7	9
8	28
9	30
10	7
11	8
12	12
13	13
14	3
15	15
16	16
17	42
18	18
19	19
20	20
21	44
22	23
23	25
24	10
25	26
26	11
27	29
28	5
28	17
28	41
29	31
30	32
31	33
32	34
33	35
34	36
35	22
36	27
37	37
37	38
38	24
39	39
40	40
41	43
42	46
43	47
\.


--
-- TOC entry 5073 (class 0 OID 33229)
-- Dependencies: 239
-- Data for Name: artistalbums; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.artistalbums (artistid, albumid) FROM stdin;
1	13
2	36
3	43
4	2
5	1
6	3
7	28
8	37
9	6
10	3
11	41
12	31
13	6
14	13
15	20
16	11
17	13
18	3
19	15
19	14
20	27
21	1
22	39
23	23
24	26
25	19
26	5
27	25
28	2
29	22
30	14
31	19
32	24
33	6
34	3
35	1
36	9
37	13
38	23
39	17
40	19
41	16
42	42
43	8
\.


--
-- TOC entry 5074 (class 0 OID 33246)
-- Dependencies: 240
-- Data for Name: artistgenres; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.artistgenres (artistid, genreid) FROM stdin;
1	45
1	16
1	26
2	5
2	20
3	13
3	30
4	13
5	45
5	11
6	6
6	5
7	13
8	46
8	12
8	3
9	5
9	6
9	23
10	13
10	7
10	14
10	29
10	15
11	46
11	8
12	46
13	28
13	26
13	27
13	2
13	32
14	15
14	10
14	33
14	17
16	37
18	27
18	10
18	13
18	26
19	13
19	14
20	15
20	17
20	18
20	19
21	36
21	48
21	17
22	44
22	26
22	47
22	28
22	27
23	15
23	10
23	33
24	15
24	21
25	37
25	29
25	6
25	34
25	13
26	13
26	30
26	14
26	7
27	39
27	1
27	35
29	38
30	43
30	31
30	24
31	40
32	47
32	9
37	42
38	15
39	37
39	29
39	34
40	37
40	29
40	34
40	25
42	41
43	36
43	22
43	15
43	4
\.


--
-- TOC entry 5063 (class 0 OID 33116)
-- Dependencies: 229
-- Data for Name: artists; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.artists (artistid, artistname, countryid, activeyears, artistdescription, photopath) FROM stdin;
1	Recoil	1	2000–2025	Исполнитель Recoil известен своими треками на Spotify.	https://i.scdn.co/image/8a55f8b98045192cf6f96cdaf0a10a87a1333209
2	Underworld	1	2000–2025	Исполнитель Underworld известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb5cfa742c32b2f8c23f94cf7e
3	Toronto Is Broken	1	2000–2025	Исполнитель Toronto Is Broken известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebd7e5abc71f1fec13033a5baa
4	Sebotage	1	2000–2025	Исполнитель Sebotage известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebe8500fa9b789bfc15faf1c26
5	UNKLE	1	2000–2025	Исполнитель UNKLE известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebac89f1cb4f6881854f55f761
6	Hybrid	1	2000–2025	Исполнитель Hybrid известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb2d875cc2e54d7270f8d05f7e
7	REEBZ	1	2000–2025	Исполнитель REEBZ известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebe1ddd40d74ec59697d1ea2b9
8	仮想夢プラザ	1	2000–2025	Исполнитель 仮想夢プラザ известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebbea822341ca18435964b3a22
9	The Prodigy	1	2000–2025	Исполнитель The Prodigy известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb147841812056c247407811f3
10	Spor	1	2000–2025	Исполнитель Spor известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb3200487ca708d8587c557783
11	Hong Kong Express	1	2000–2025	Исполнитель Hong Kong Express известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb26fbeeac4e8c45780b3b3a7d
12	Dr. Nakano	1	2000–2025	Исполнитель Dr. Nakano известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebf243cce1a787d3f6a84397f6
13	Nine Inch Nails	1	2000–2025	Исполнитель Nine Inch Nails известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb752fca27c5b839ea47d5100d
14	Kayzo	1	2000–2025	Исполнитель Kayzo известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb5e72708b64f379843417ddee
15	WesGhost	1	2000–2025	Исполнитель WesGhost известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb09935a526cc02e86cc127b5e
16	Naked Flames	1	2000–2025	Исполнитель Naked Flames известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb864a03d162414c8ff3eb7b21
17	CANTERVICE	1	2000–2025	Исполнитель CANTERVICE известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebdc3edcf7e8414d791d224ab4
18	Zardonic	1	2000–2025	Исполнитель Zardonic известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebf2112a8bed841437334d935b
19	Magnetude	1	2000–2025	Исполнитель Magnetude известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebfca737f35ace839fea98adcc
20	Skrillex	1	2000–2025	Исполнитель Skrillex известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebe32002317387b6d659308a94
21	ISOxo	1	2000–2025	Исполнитель ISOxo известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb69dea5e9c932833f17b7ddce
22	HEALTH	1	2000–2025	Исполнитель HEALTH известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb5b2bf033db0073228084edb4
23	RIOT	1	2000–2025	Исполнитель RIOT известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebd459730d3fb832bc9e18113e
24	GG Magree	1	2000–2025	Исполнитель GG Magree известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebf784e061371be04d98b542e5
25	Nedaj	1	2000–2025	Исполнитель Nedaj известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb11586ee95f9e07f47252d1f6
26	Koven	1	2000–2025	Исполнитель Koven известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb558339d8bae95cd33a20c4c7
27	Ladytron	1	2000–2025	Исполнитель Ladytron известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebb948a9242e7ff017fb0d1173
28	Kevin Sherwood	1	2000–2025	Исполнитель Kevin Sherwood известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb82a01d3b0a1890a78e9e527a
29	Matthew K. Heafy	1	2000–2025	Исполнитель Matthew K. Heafy известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb1d878a14af100015aae655c2
30	Trivium	1	2000–2025	Исполнитель Trivium известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebab21a41e346730ff64be53c2
31	Kurai mizu	1	2000–2025	Исполнитель Kurai mizu известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb52980e5c7d56a2b51fd5914d
32	Solter	1	2000–2025	Исполнитель Solter известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb20b33946a5d8579f780d6d7c
33	Юлия Коган	1	2000–2025	Исполнитель Юлия Коган известен своими треками на Spotify.	https://i.scdn.co/image/ab67616d0000b27359c1339003364d093ead463f
34	frenetic virtual orchestra	1	2000–2025	Исполнитель frenetic virtual orchestra известен своими треками на Spotify.	https://i.scdn.co/image/ab67616d0000b273caf25a695ad72d5fa1641d01
35	Geoffplaysguitar	1	2000–2025	Исполнитель Geoffplaysguitar известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebc80b2654f9c8ff1eeac4a1cd
36	Atomic Heart	1	2000–2025	Исполнитель Atomic Heart известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb5b9d4a3ca752b33bf6d8a690
37	CHOMPO	1	2000–2025	Исполнитель CHOMPO известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebab8bdcdcebb63c80fb868a75
38	Boom Kitty	1	2000–2025	Исполнитель Boom Kitty известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb0537e4e52505684d70275b45
39	Cynthoni	1	2000–2025	Исполнитель Cynthoni известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb068c68cd72c01daf38873103
40	Sewerslvt	1	2000–2025	Исполнитель Sewerslvt известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb8a1271f7f32e5202924ebff2
41	Skyth	1	2000–2025	Исполнитель Skyth известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5eb9c0ccd821d055f7670190408
42	AL-90	1	2000–2025	Исполнитель AL-90 известен своими треками на Spotify.	https://i.scdn.co/image/ab67616d0000b2738fe600c89360e4a56ac26c7d
43	Habstrakt	1	2000–2025	Исполнитель Habstrakt известен своими треками на Spotify.	https://i.scdn.co/image/ab6761610000e5ebd4f9e0206342d45ab98840c2
\.


--
-- TOC entry 5059 (class 0 OID 33096)
-- Dependencies: 225
-- Data for Name: countries; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.countries (countryid, countryname) FROM stdin;
1	Неизвестно
\.


--
-- TOC entry 5061 (class 0 OID 33106)
-- Dependencies: 227
-- Data for Name: genres; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.genres (genreid, genrename) FROM stdin;
1	 alternative dance
2	 alternative metal
3	 ambient
4	 bassline
5	 big beat
6	 breakbeat
7	 chillstep
8	 chillwave
9	 darkwave
10	 deathstep
11	 downtempo
12	 drone
13	 drum and bass
14	 drumstep
15	 dubstep
16	 ebm
18	 electro
19	 electronic
20	 electronica
21	 future bass
22	 g-house
23	 hardcore techno
24	 heavy metal
25	 hyperpop
26	 industrial
27	 industrial metal
28	 industrial rock
29	 jungle
30	 liquid funk
31	 metal
32	 nu metal
33	 riddim
34	 speedcore
35	 synthpop
36	bass house
37	breakcore
38	deathcore
39	electroclash
40	idm
41	lo-fi house
42	melodic bass
43	metalcore
44	noise rock
45	trip hop
46	vaporwave
47	witch house
17	 edm
48	trap
\.


--
-- TOC entry 5066 (class 0 OID 33151)
-- Dependencies: 232
-- Data for Name: playlists; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.playlists (playlistid, playlistname, userid, playlistcreationdate, likes, playlistdescription) FROM stdin;
1	Means A Lot To Me	4	2024-10-20	4863	\N
2	Death Notices Full OST	9	2023-10-29	81189	\N
3	Best of 2025	1	2025-05-08	85456	\N
\.


--
-- TOC entry 5072 (class 0 OID 33212)
-- Dependencies: 238
-- Data for Name: playlisttags; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.playlisttags (playlistid, tagid) FROM stdin;
1	1
1	2
1	3
2	1
2	4
2	2
3	5
3	6
3	2
\.


--
-- TOC entry 5071 (class 0 OID 33195)
-- Dependencies: 237
-- Data for Name: playlisttracks; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.playlisttracks (playlistid, trackid) FROM stdin;
\.


--
-- TOC entry 5055 (class 0 OID 33076)
-- Dependencies: 221
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.roles (roleid, rolename) FROM stdin;
1	Пользователь
2	Менеджер
3	Администратор
\.


--
-- TOC entry 5057 (class 0 OID 33086)
-- Dependencies: 223
-- Data for Name: subscriptions; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.subscriptions (subscriptionid, subscriptionname) FROM stdin;
1	Бесплатная
2	Премиум
\.


--
-- TOC entry 5068 (class 0 OID 33164)
-- Dependencies: 234
-- Data for Name: tags; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.tags (tagid, tagname) FROM stdin;
1	study
2	 chill
3	 focus
4	 rock
5	workout
6	 pop
\.


--
-- TOC entry 5077 (class 0 OID 33297)
-- Dependencies: 243
-- Data for Name: trackartists; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.trackartists (trackid, artistid) FROM stdin;
2	36
3	38
4	38
5	12
6	34
6	35
7	21
7	29
8	7
9	7
9	4
10	4
11	40
12	32
12	10
13	3
14	30
15	15
15	18
16	42
16	17
17	37
18	39
18	24
18	43
19	22
19	11
20	6
21	6
22	6
23	14
24	28
25	26
26	31
27	27
27	19
28	19
29	16
30	25
31	13
32	13
33	13
34	13
35	13
36	1
37	23
38	23
39	20
39	41
39	9
40	3
41	3
42	3
43	3
44	2
45	2
45	5
46	33
47	8
\.


--
-- TOC entry 5078 (class 0 OID 33314)
-- Dependencies: 244
-- Data for Name: trackgenres; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.trackgenres (trackid, genreid) FROM stdin;
2	2
2	26
2	27
2	32
2	28
3	2
3	26
3	27
3	32
3	28
4	4
4	15
4	22
4	36
5	20
5	5
6	30
6	13
7	30
7	13
10	7
10	14
10	30
10	13
11	11
11	45
12	40
13	5
13	6
14	16
14	26
14	45
15	17
15	18
15	19
15	15
16	8
16	46
17	20
17	5
18	30
18	13
19	9
19	17
19	33
19	15
20	1
20	35
20	39
21	20
21	5
22	16
22	26
22	45
23	29
23	34
23	37
24	9
24	33
24	15
25	9
25	33
25	15
26	14
26	13
27	6
27	23
27	5
28	42
29	6
29	13
29	29
29	34
29	37
30	5
30	6
31	26
31	27
31	28
31	47
31	44
32	9
32	17
32	33
32	15
33	14
33	13
34	21
34	15
35	5
35	6
36	30
36	13
37	2
37	26
37	27
37	32
37	28
38	2
38	26
38	27
38	32
38	28
40	14
40	13
41	20
41	5
42	2
42	26
42	27
42	32
42	28
43	37
44	17
44	26
44	45
46	41
47	3
47	12
47	46
\.


--
-- TOC entry 5065 (class 0 OID 33134)
-- Dependencies: 231
-- Data for Name: tracks; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.tracks (trackid, trackname, trackduration, trackreleasedate, bitrate, filepath, rating, playcount) FROM stdin;
2	As Alive As You Need Me To Be	369	2025-02-14	192	https://open.spotify.com/track/1xsEHo7mtGZLEG94vFX11z	3.53	19591519
3	At The Heart Of It All	489	1995-01-01	320	https://open.spotify.com/track/4JQEQOu8YKbdtMzVWADAAb	3.32	14678550
4	Baby	696	2025-10-24	280	https://open.spotify.com/track/1syQiwibqZKAUsgkLn8qtC	4.46	21712928
5	Beautiful Burnout	345	2007-01-01	280	https://open.spotify.com/track/1I61h0TNyzE86dx1tZzj3b	3.25	36747640
6	Breed	247	2025-04-11	280	https://open.spotify.com/track/5cQwBnsRxUhbKDK2C6fR4n	4.22	16908411
7	Circle (feat. Sebotage)	156	2022-03-30	320	https://open.spotify.com/track/2aTXg7Ch2EuKpa7m8Z8GSv	4.83	7016621
8	Data Destruction	345	2025-06-27	320	https://open.spotify.com/track/5UM2IIULlGyvYHm4SF3hHI	4.64	65817538
9	Dig	427	2020-03-27	192	https://open.spotify.com/track/7nVIE2M0t4sIh3ZkEqrjlY	3.17	97311814
10	Era	238	2025-07-17	280	https://open.spotify.com/track/1rxxl2R2tU1Uv1oP2bbeUn	3.26	67607976
11	Eye for an Eye	1894	2003-01-01	280	https://open.spotify.com/track/6ReswWJnalmGTxyRSTmUkl	3.08	32425960
12	Final Destination	309	1999-09-21	256	https://open.spotify.com/track/7bqSNB1C3tWOUgOQTrODAG	3.67	47018173
13	Formula Of Fear	432	2008-09-22	256	https://open.spotify.com/track/0fj2n9wzkojYUy80rOFsLp	4.93	33608103
14	Freeze	219	1992-01-01	320	https://open.spotify.com/track/7KhV0aeGLhI2G1TC5bpgBe	3.07	14408959
15	Fuze	317	2000-03-21	256	https://open.spotify.com/track/5UZIVxzI4UyrSbg3ZLTGTH	4.56	91263647
16	Ghost City Daze	252	2022-09-02	280	https://open.spotify.com/track/5Npj2l7dvTvICHnh6xwNTU	3.40	80821880
17	Glam Bucket	434	2007-01-01	256	https://open.spotify.com/track/7k5rnk06tw2kUOj7AQXkha	4.04	78182712
18	GOFASTER	448	2025-03-14	256	https://open.spotify.com/track/0WpMAaQq61chei1VkqKGWv	3.90	86513228
19	Grave	603	2025-02-27	320	https://open.spotify.com/track/3TbzrpJIkSoicguKWNHcSh	3.70	31154483
20	I See Red	325	2015-05-16	192	https://open.spotify.com/track/51WgQlBgMnf1lDxBfZkJsa	4.19	39317838
21	If Rah	280	2025-10-17	256	https://open.spotify.com/track/15j4iuXi1OZfSBKmaVTeN7	4.86	4552008
22	Intruders	227	2007-01-01	320	https://open.spotify.com/track/0zU2OXXBlRj8xIB1mc0jPN	3.04	39633329
23	Lvstlove	159	2025-01-03	192	https://open.spotify.com/track/3INsVctSfXaU6rdPBSPalA	3.01	77772777
24	March of the Machine	660	2025-08-01	192	https://open.spotify.com/track/2ZhSXdApy452LqJFf7aEwz	4.43	32413023
25	Metalicious	252	2025-10-24	280	https://open.spotify.com/track/2PdmLVo9VOPhlySbMXH6M0	4.45	68700455
26	Muleta	297	2025-04-11	192	https://open.spotify.com/track/28zBEKDSGAny9y6KrBoCF4	4.05	80447698
27	Nasty - Spor Remix	188	2015-11-10	280	https://open.spotify.com/track/4SXdAtwtoVCZdUP9mfFFqG	3.32	62355472
28	Obelisk	233	2025-05-23	256	https://open.spotify.com/track/0UCcUFxxlHgaQGojhxzECF	4.46	3437809
29	ONYX	247	2025-04-11	280	https://open.spotify.com/track/0yuIvkFLeSxFBSyyJBiWVV	3.70	60148993
30	Orbit	360	2012-09-07	280	https://open.spotify.com/track/1bo3aqhoE0q808wF11ENrC	3.92	30476401
31	ORDINARY LOSS	203	2025-09-11	192	https://open.spotify.com/track/4qUrMbUVDxaMZy2zPB6mD7	3.16	42701725
32	Overload 2K	555	2008-09-22	192	https://open.spotify.com/track/2Y68mc2cWkIwBdqlY04J2e	3.28	64176479
33	Repentance	312	2025-08-01	280	https://open.spotify.com/track/12eQwZCKpzIpccXsH4xQZR	4.60	36864477
34	Run	264	2025-04-11	256	https://open.spotify.com/track/0Ov5YTilb4z4nvuTWeSsUu	4.57	26952175
35	Siren Of The Storm	197	2025-09-17	192	https://open.spotify.com/track/7HSZ0epFVfoDBFyMjCKMrP	3.99	54797402
36	SOMEWHEREIBELONG	233	2025-05-02	320	https://open.spotify.com/track/2bja1BK8CyrE5U1YLzPWtQ	3.55	71834289
37	The Big Come Down	270	2025-01-19	256	https://open.spotify.com/track/3cl42VFcRop14wAduvSeaH	3.36	96111491
38	The Great Below	230	2025-04-18	280	https://open.spotify.com/track/7LBWAib31Cthfz2KoI611Z	3.91	79852612
39	The Masquerade - Zardonic & Toronto Is Broken Remix	229	1999-09-21	192	https://open.spotify.com/track/787izJqCRpk9HOgho07jzb	3.02	30268024
40	The Star	290	2016-03-19	320	https://open.spotify.com/track/4EVYNBO90unYQMUkcgaAqS	2.52	31048640
41	To Heal	310	2007-01-01	320	https://open.spotify.com/track/5K0kPszkKvh6yHEI3wiUCZ	3.52	71054588
42	Together	165	2025-07-24	256	https://open.spotify.com/track/7iiu6ce9zGfbvs0yBvsZeT	4.55	14134899
43	Undisputed Altitude	366	2023-05-12	256	https://open.spotify.com/track/3K5td8Q9ezKgBjpiiuJDL9	2.98	97856870
44	Want	200	2000-03-21	192	https://open.spotify.com/track/6AFz1yXS7boPZlqJd2R7WS	3.57	79887130
45	В синем море, в белой пене (Оставайся, мальчик, с нами) [Geoffrey Day Remix]	278	2025-02-24	280	https://open.spotify.com/track/4zW0D21eynN6JcDwGR4f9F	3.95	59329189
46	общедомовой	307	2025-09-11	320	https://open.spotify.com/track/1QVlzjQU2RI4GpVen3x4Ro	4.77	17722035
47	あなたを待って	236	2025-08-13	192	https://open.spotify.com/track/3GwYAO8fAxs5k0KiU2hpBP	4.94	42250505
\.


--
-- TOC entry 5053 (class 0 OID 33060)
-- Dependencies: 219
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.users (userid, login, password, email, roleid, subscriptionid, registrationdate, lastlogindate, fullname) FROM stdin;
1	apollinariya.allenova	sXFdIl0y	apollinariya.allenova@outlook.com	2	2	2025-10-29	2025-10-29	Аполлинария Алленова
2	leontiy.kozlitin	RDJCXFyk	leontiy.kozlitin@ya.ru	2	2	2025-10-28	2025-10-29	Леонтий Козлитин
3	lyudmila.abakumova	E3rg2Dct	lyudmila.abakumova@hotmail.com	1	2	2025-09-29	2025-10-29	Людмила Абакумова
4	anjela.privalova	mZPXkIBf	anjela.privalova@rambler.ru	2	2	2025-10-20	2025-10-29	Анжела Привалова
5	kseniya1978	bCWtEdaB	kseniya1978@rambler.ru	1	1	2025-08-05	2025-10-29	Ксения Хуторская
6	oksana95	c35tx4ov	oksana95@ya.ru	1	1	2025-10-29	2025-10-29	Оксана Добронравова
7	antonina.pushmenkova	it3cySf5	antonina.pushmenkova@mail.ru	3	2	2025-10-29	2025-10-29	Антонина Пушменкова
8	asya.vedova	z6kkRwhf	asya.vedova@mail.ru	1	2	2025-10-29	2025-10-29	Ася Шведова
9	aleksey.novojilov	1XF4kEOq	aleksey.novojilov@mail.ru	3	1	2025-10-29	2025-10-29	Алексей Новожилов
10	petr1968	K5PU52zo	petr1968@ya.ru	1	1	2025-10-29	2025-10-29	Петр Крылаев
\.


--
-- TOC entry 5084 (class 0 OID 0)
-- Dependencies: 235
-- Name: albums_albumid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.albums_albumid_seq', 43, true);


--
-- TOC entry 5085 (class 0 OID 0)
-- Dependencies: 228
-- Name: artists_artistid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.artists_artistid_seq', 43, true);


--
-- TOC entry 5086 (class 0 OID 0)
-- Dependencies: 224
-- Name: countries_countryid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.countries_countryid_seq', 1, true);


--
-- TOC entry 5087 (class 0 OID 0)
-- Dependencies: 226
-- Name: genres_genreid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.genres_genreid_seq', 48, true);


--
-- TOC entry 5088 (class 0 OID 0)
-- Dependencies: 220
-- Name: roles_roleid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.roles_roleid_seq', 3, true);


--
-- TOC entry 5089 (class 0 OID 0)
-- Dependencies: 222
-- Name: subscriptions_subscriptionid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.subscriptions_subscriptionid_seq', 2, true);


--
-- TOC entry 5090 (class 0 OID 0)
-- Dependencies: 233
-- Name: tags_tagid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.tags_tagid_seq', 6, true);


--
-- TOC entry 5091 (class 0 OID 0)
-- Dependencies: 230
-- Name: tracks_trackid_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.tracks_trackid_seq', 47, true);


--
-- TOC entry 4881 (class 2606 OID 33286)
-- Name: albumgenres albumgenres_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albumgenres
    ADD CONSTRAINT albumgenres_pkey PRIMARY KEY (albumid, genreid);


--
-- TOC entry 4867 (class 2606 OID 33194)
-- Name: albums albums_coverpath_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albums
    ADD CONSTRAINT albums_coverpath_key UNIQUE (coverpath);


--
-- TOC entry 4869 (class 2606 OID 33192)
-- Name: albums albums_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albums
    ADD CONSTRAINT albums_pkey PRIMARY KEY (albumid);


--
-- TOC entry 4879 (class 2606 OID 33269)
-- Name: albumtracks albumtracks_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albumtracks
    ADD CONSTRAINT albumtracks_pkey PRIMARY KEY (albumid, trackid);


--
-- TOC entry 4875 (class 2606 OID 33235)
-- Name: artistalbums artistalbums_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artistalbums
    ADD CONSTRAINT artistalbums_pkey PRIMARY KEY (artistid, albumid);


--
-- TOC entry 4877 (class 2606 OID 33252)
-- Name: artistgenres artistgenres_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artistgenres
    ADD CONSTRAINT artistgenres_pkey PRIMARY KEY (artistid, genreid);


--
-- TOC entry 4851 (class 2606 OID 33130)
-- Name: artists artists_artistname_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artists
    ADD CONSTRAINT artists_artistname_key UNIQUE (artistname);


--
-- TOC entry 4853 (class 2606 OID 33132)
-- Name: artists artists_photopath_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artists
    ADD CONSTRAINT artists_photopath_key UNIQUE (photopath);


--
-- TOC entry 4855 (class 2606 OID 33128)
-- Name: artists artists_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artists
    ADD CONSTRAINT artists_pkey PRIMARY KEY (artistid);


--
-- TOC entry 4843 (class 2606 OID 33104)
-- Name: countries countries_countryname_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_countryname_key UNIQUE (countryname);


--
-- TOC entry 4845 (class 2606 OID 33102)
-- Name: countries countries_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.countries
    ADD CONSTRAINT countries_pkey PRIMARY KEY (countryid);


--
-- TOC entry 4847 (class 2606 OID 33114)
-- Name: genres genres_genrename_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.genres
    ADD CONSTRAINT genres_genrename_key UNIQUE (genrename);


--
-- TOC entry 4849 (class 2606 OID 33112)
-- Name: genres genres_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.genres
    ADD CONSTRAINT genres_pkey PRIMARY KEY (genreid);


--
-- TOC entry 4861 (class 2606 OID 33162)
-- Name: playlists playlists_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_pkey PRIMARY KEY (playlistid);


--
-- TOC entry 4873 (class 2606 OID 33218)
-- Name: playlisttags playlisttags_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlisttags
    ADD CONSTRAINT playlisttags_pkey PRIMARY KEY (playlistid, tagid);


--
-- TOC entry 4871 (class 2606 OID 33201)
-- Name: playlisttracks playlisttracks_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlisttracks
    ADD CONSTRAINT playlisttracks_pkey PRIMARY KEY (playlistid, trackid);


--
-- TOC entry 4835 (class 2606 OID 33082)
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (roleid);


--
-- TOC entry 4837 (class 2606 OID 33084)
-- Name: roles roles_rolename_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_rolename_key UNIQUE (rolename);


--
-- TOC entry 4839 (class 2606 OID 33092)
-- Name: subscriptions subscriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.subscriptions
    ADD CONSTRAINT subscriptions_pkey PRIMARY KEY (subscriptionid);


--
-- TOC entry 4841 (class 2606 OID 33094)
-- Name: subscriptions subscriptions_subscriptionname_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.subscriptions
    ADD CONSTRAINT subscriptions_subscriptionname_key UNIQUE (subscriptionname);


--
-- TOC entry 4863 (class 2606 OID 33170)
-- Name: tags tags_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tags
    ADD CONSTRAINT tags_pkey PRIMARY KEY (tagid);


--
-- TOC entry 4865 (class 2606 OID 33172)
-- Name: tags tags_tagname_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tags
    ADD CONSTRAINT tags_tagname_key UNIQUE (tagname);


--
-- TOC entry 4883 (class 2606 OID 33303)
-- Name: trackartists trackartists_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trackartists
    ADD CONSTRAINT trackartists_pkey PRIMARY KEY (trackid, artistid);


--
-- TOC entry 4885 (class 2606 OID 33320)
-- Name: trackgenres trackgenres_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trackgenres
    ADD CONSTRAINT trackgenres_pkey PRIMARY KEY (trackid, genreid);


--
-- TOC entry 4857 (class 2606 OID 33150)
-- Name: tracks tracks_filepath_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tracks
    ADD CONSTRAINT tracks_filepath_key UNIQUE (filepath);


--
-- TOC entry 4859 (class 2606 OID 33148)
-- Name: tracks tracks_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tracks
    ADD CONSTRAINT tracks_pkey PRIMARY KEY (trackid);


--
-- TOC entry 4831 (class 2606 OID 33074)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- TOC entry 4833 (class 2606 OID 33072)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (userid);


--
-- TOC entry 4900 (class 2606 OID 33287)
-- Name: albumgenres albumgenres_albumid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albumgenres
    ADD CONSTRAINT albumgenres_albumid_fkey FOREIGN KEY (albumid) REFERENCES public.albums(albumid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4901 (class 2606 OID 33292)
-- Name: albumgenres albumgenres_genreid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albumgenres
    ADD CONSTRAINT albumgenres_genreid_fkey FOREIGN KEY (genreid) REFERENCES public.genres(genreid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4898 (class 2606 OID 33270)
-- Name: albumtracks albumtracks_albumid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albumtracks
    ADD CONSTRAINT albumtracks_albumid_fkey FOREIGN KEY (albumid) REFERENCES public.albums(albumid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4899 (class 2606 OID 33275)
-- Name: albumtracks albumtracks_trackid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.albumtracks
    ADD CONSTRAINT albumtracks_trackid_fkey FOREIGN KEY (trackid) REFERENCES public.tracks(trackid) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4894 (class 2606 OID 33241)
-- Name: artistalbums artistalbums_albumid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artistalbums
    ADD CONSTRAINT artistalbums_albumid_fkey FOREIGN KEY (albumid) REFERENCES public.albums(albumid) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4895 (class 2606 OID 33236)
-- Name: artistalbums artistalbums_artistid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artistalbums
    ADD CONSTRAINT artistalbums_artistid_fkey FOREIGN KEY (artistid) REFERENCES public.artists(artistid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4896 (class 2606 OID 33253)
-- Name: artistgenres artistgenres_artistid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artistgenres
    ADD CONSTRAINT artistgenres_artistid_fkey FOREIGN KEY (artistid) REFERENCES public.artists(artistid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4897 (class 2606 OID 33258)
-- Name: artistgenres artistgenres_genreid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artistgenres
    ADD CONSTRAINT artistgenres_genreid_fkey FOREIGN KEY (genreid) REFERENCES public.genres(genreid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4888 (class 2606 OID 41287)
-- Name: artists artists_countryid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.artists
    ADD CONSTRAINT artists_countryid_fkey FOREIGN KEY (countryid) REFERENCES public.countries(countryid);


--
-- TOC entry 4889 (class 2606 OID 33346)
-- Name: playlists playlists_userid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_userid_fkey FOREIGN KEY (userid) REFERENCES public.users(userid);


--
-- TOC entry 4892 (class 2606 OID 33219)
-- Name: playlisttags playlisttags_playlistid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlisttags
    ADD CONSTRAINT playlisttags_playlistid_fkey FOREIGN KEY (playlistid) REFERENCES public.playlists(playlistid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4893 (class 2606 OID 33224)
-- Name: playlisttags playlisttags_tagid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlisttags
    ADD CONSTRAINT playlisttags_tagid_fkey FOREIGN KEY (tagid) REFERENCES public.tags(tagid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4890 (class 2606 OID 33202)
-- Name: playlisttracks playlisttracks_playlistid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlisttracks
    ADD CONSTRAINT playlisttracks_playlistid_fkey FOREIGN KEY (playlistid) REFERENCES public.playlists(playlistid) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4891 (class 2606 OID 33207)
-- Name: playlisttracks playlisttracks_trackid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.playlisttracks
    ADD CONSTRAINT playlisttracks_trackid_fkey FOREIGN KEY (trackid) REFERENCES public.tracks(trackid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4902 (class 2606 OID 33309)
-- Name: trackartists trackartists_artistid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trackartists
    ADD CONSTRAINT trackartists_artistid_fkey FOREIGN KEY (artistid) REFERENCES public.artists(artistid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4903 (class 2606 OID 33304)
-- Name: trackartists trackartists_trackid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trackartists
    ADD CONSTRAINT trackartists_trackid_fkey FOREIGN KEY (trackid) REFERENCES public.tracks(trackid) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4904 (class 2606 OID 33326)
-- Name: trackgenres trackgenres_genreid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trackgenres
    ADD CONSTRAINT trackgenres_genreid_fkey FOREIGN KEY (genreid) REFERENCES public.genres(genreid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4905 (class 2606 OID 33321)
-- Name: trackgenres trackgenres_trackid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.trackgenres
    ADD CONSTRAINT trackgenres_trackid_fkey FOREIGN KEY (trackid) REFERENCES public.tracks(trackid) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- TOC entry 4886 (class 2606 OID 33331)
-- Name: users users_roleid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_roleid_fkey FOREIGN KEY (roleid) REFERENCES public.roles(roleid) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4887 (class 2606 OID 33336)
-- Name: users users_subscriptionid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_subscriptionid_fkey FOREIGN KEY (subscriptionid) REFERENCES public.subscriptions(subscriptionid) ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2025-11-11 20:41:27

--
-- PostgreSQL database dump complete
--

\unrestrict oz6mgA6NlbswyFrXh5GVpqXbVglY0yVJfxyF4eEfkoJanxcu51CeETDYqnLtq2r

