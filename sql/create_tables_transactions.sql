--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-07 20:19:53 WET

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
-- TOC entry 2 (class 3079 OID 16487)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 3432 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16482)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16498)
-- Name: category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category (
    id_category uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    name character varying(50) NOT NULL,
    id_user uuid
);


ALTER TABLE public.category OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16523)
-- Name: reocurring; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reocurring (
    id_reocurring uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    id_wallet uuid NOT NULL,
    id_category uuid,
    id_subcategory uuid,
    description text NOT NULL,
    amount real NOT NULL,
    day_of_week integer,
    start_date timestamp with time zone NOT NULL,
    is_yearly boolean DEFAULT false NOT NULL,
    is_monthly boolean DEFAULT false NOT NULL,
    is_weekly boolean DEFAULT false NOT NULL,
    is_active boolean DEFAULT true NOT NULL
);


ALTER TABLE public.reocurring OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16512)
-- Name: subcategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subcategory (
    id_subcategory uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    id_category uuid NOT NULL,
    name character varying(50) NOT NULL,
    id_user uuid
);


ALTER TABLE public.subcategory OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16504)
-- Name: transaction_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transaction_group (
    id_transaction_group uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    description text NOT NULL,
    start_date timestamp(6) with time zone NOT NULL,
    end_date timestamp(6) with time zone NOT NULL,
    planned_amount real NOT NULL,
    id_user uuid NOT NULL
);


ALTER TABLE public.transaction_group OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16545)
-- Name: transactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transactions (
    id_transaction uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    id_wallet uuid NOT NULL,
    id_category uuid,
    id_subcategory uuid,
    id_transaction_group uuid,
    id_reocurring uuid,
    date timestamp(6) with time zone NOT NULL,
    description text NOT NULL,
    amount real NOT NULL,
    is_planned boolean DEFAULT false NOT NULL,
    latitude real,
    longitude real
);


ALTER TABLE public.transactions OWNER TO postgres;

--
-- TOC entry 3421 (class 0 OID 16482)
-- Dependencies: 218
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20241204191448_InitialCreate', '8.0.11');


--
-- TOC entry 3422 (class 0 OID 16498)
-- Dependencies: 219
-- Data for Name: category; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3425 (class 0 OID 16523)
-- Dependencies: 222
-- Data for Name: reocurring; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3424 (class 0 OID 16512)
-- Dependencies: 221
-- Data for Name: subcategory; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3423 (class 0 OID 16504)
-- Dependencies: 220
-- Data for Name: transaction_group; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.transaction_group (id_transaction_group, description, start_date, end_date, planned_amount, id_user) VALUES ('5a1dfdd4-e7fd-4098-ba00-eac009f97026', 'string', '2024-12-06 10:20:21.788+00', '2024-12-06 20:20:21.788+00', 100, '4458a5f9-9ea3-4d1c-9373-6de9f74185dc');


--
-- TOC entry 3426 (class 0 OID 16545)
-- Dependencies: 223
-- Data for Name: transactions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2928820c-a8a7-4e88-aaee-cc695d387205', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-07 14:24:04.618+00', 'description', 500, false, 0, 0);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('fbba0a16-387d-4a85-9a98-a7bf8bac3e75', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, '5a1dfdd4-e7fd-4098-ba00-eac009f97026', NULL, '2024-12-06 14:24:04.618+00', 'description', 500, false, 0, 0);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c508582a-2bbf-47f4-800b-20f7c4e58648', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-07 00:00:00+00', 'Trf MB WAY 966824733 0384728/67', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4f26936b-b47e-4739-8591-42e7d97447ba', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-07 00:00:00+00', 'COMPRA MCDONALDS VI 0384728/66', -7.6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0421a40a-3844-46a4-b7ba-4f4dc52f875a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-06 00:00:00+00', 'COMPRA WWW AMAZON   0384728/00', -15.34, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5128f741-c84d-4a76-9436-0a6835723a55', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-05 00:00:00+00', 'Trf MB WAY 961958947 0384728/65', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('75c3ec5b-17d6-48f9-b010-dc37529a3712', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-04 00:00:00+00', 'SEPA DD-90037171982-FIDELIDADE', -25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a34a33d5-6631-4790-a468-2f36a06f7304', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-03 00:00:00+00', 'COMPRA POSICO 11969 0384728/64', -35, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9d326dc1-69e6-447e-b825-02a3594c49e6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-12-03 00:00:00+00', 'Trf MB WAY 961958947 0384728/64', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5803b6d4-d1b8-47d8-9ffb-2e2e4924bb77', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-30 00:00:00+00', 'COMPRA CONTINENTE B 0384728/63', -7.4, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5826a7cd-335c-47f8-b6fd-043c9d06485c', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-29 00:00:00+00', 'COMPRA GALP P11773  0384728/62', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('70fc2d7f-6824-44b6-bb06-f730d87d2b2f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-29 00:00:00+00', 'SEPA DD-600388     -CROSSFIT V', -59, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d53bd564-cbb6-491a-817a-a05fd766c76d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-28 00:00:00+00', 'COMPRA CONTINENTE V 0384728/61', -8.12, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c04085c1-e202-473c-8630-9dec03945acf', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-28 00:00:00+00', 'Com trf MB WAY', -0.04, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('fc8713c3-148d-4c96-b650-0a9415357914', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-28 00:00:00+00', 'Trf MB WAY 961958947 0384728/62', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('67ae6615-3617-41c3-ab3d-abe94357ec3a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-28 00:00:00+00', 'COMPRA MCDONALDS ES 0384728/60', -11.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('705777f8-b831-45db-9fec-f40a5e3d1902', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-27 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/59', -3.94, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('799f7b4d-ca1d-4acb-a137-a10d31e736fc', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-27 00:00:00+00', 'Com trf MB WAY', -0.01, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f38023dd-ab13-4d20-9e53-885a828af74d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-27 00:00:00+00', 'Trf MB WAY 961958947 0384728/61', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4c5b0991-be29-422f-8ca8-d458c5c90305', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-27 00:00:00+00', 'COMPRA MBWAY   FATU 0384728/60', -17.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('452250ee-ae2d-4c1b-b238-3217afb9e3be', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-27 00:00:00+00', 'COMPRA IPCA         0384728/59', -105, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('dffce47c-5291-40ad-9639-91e4e8980dd5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-27 00:00:00+00', 'Trf MB WAY 961958947 0384728/58', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ada9d806-198e-41cb-afa3-9fc71ac8f50f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-26 00:00:00+00', 'Com trf MB WAY', -0.04, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8de29524-8025-4562-bd69-6db87670e074', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-26 00:00:00+00', 'Trf MB WAY 961958947 0384728/57', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('36f27891-8036-4da5-b9ef-854fbeef7901', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-26 00:00:00+00', 'COMPRA PRIMARK BRAG 0384728/00', -11.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f7a978ef-1fa2-4d24-8378-0b1cc11c8d90', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-25 00:00:00+00', 'COMPRA TEIXEIRA   L 0384728/56', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('fafcbafc-798c-48e0-91a7-165f2369a0f5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-25 00:00:00+00', 'COMPRA GOOGLE GOOGL 0384728/00', -3.99, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('00019c73-cd6a-4777-94df-05c6f339fc36', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-25 00:00:00+00', 'COMPRA BK27203 VIAN 0384728/55', -2.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5006463f-c909-4a2b-abf1-5c0e678cc029', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-24 00:00:00+00', 'COMPRA PARQUE CAFET 0384728/53', -7.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('da9e3d6b-73d4-4531-b1a4-2f2657922724', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-23 00:00:00+00', 'Trf MB WAY 961958947 0384728/53', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('21a7029e-596a-48ea-a707-7ac532599f2c', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-22 00:00:00+00', 'Trf MB WAY 963176075 0384728/52', -16.6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('23dc0ea2-ca45-4614-9d7c-80b1a6fe29d6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-22 00:00:00+00', 'COMPRA GALP P11773  0384728/51', -35, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9179a89b-4075-4e9e-8bc3-0bc7b2ea8bbb', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-21 00:00:00+00', 'Trf MB WAY 961958947 0384728/51', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('36e4eab8-a774-44fc-a3d1-66bb36a2b534', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-20 00:00:00+00', 'COMPRA MERCADONA    0384728/50', -5.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9009116c-2ed0-46fe-8346-83c1e148a708', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-20 00:00:00+00', 'Trf MB WAY 961958947 0384728/49', -13, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('913c8ba3-6217-4164-ab3e-f24995d2c814', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-19 00:00:00+00', 'COMPRA POSICO 11969 0384728/48', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ac172e64-05b5-4484-a81c-576641d26e43', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-19 00:00:00+00', 'COMPRA ARCOS DE VAL 0384728/46', -2.44, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5b36cb44-3cac-4166-af77-23ea81b01dcb', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-18 00:00:00+00', 'Trf MB WAY 961958947 0384728/47', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('3a15283e-dd29-413e-83c5-f8bcb14d20bb', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-18 00:00:00+00', 'Trf MB WAY 961958947 0384728/46', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('739feeeb-eb71-4e02-90e7-63285162232f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-18 00:00:00+00', 'COMPRA NORMAL BRAGA 0384728/00', -5.6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b0e41a47-e264-4a7f-a201-0540c0ada353', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-18 00:00:00+00', 'COMPRA Spotify P318 0384728/00', -10.49, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b78b462e-f387-470c-bfee-7d68bc2dc4c6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-18 00:00:00+00', 'COMPRA IKEA BRAGA C 0384728/00', -10.37, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a5e78f89-8e26-4373-a1b9-8896287f3ef2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-18 00:00:00+00', 'COMPRA PRIMARK BRAG 0384728/00', -27, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ac3ef011-168a-408e-8deb-583c2faef9d3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-17 00:00:00+00', 'COMPRA GALP P11773  0384728/45', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('22fe4e1e-5fb9-42f7-b5c9-e75b50d01a77', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-17 00:00:00+00', 'COMPRA BERTRAND EST 0384728/42', -11.45, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('aac28259-5af8-4cec-83a1-a5321899e135', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-17 00:00:00+00', 'COMPRA Revolut  139 0384728/00', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c9841b13-f7b3-4006-889d-218cc17efc20', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-16 00:00:00+00', 'COMPRA BRAGA PARQUE 0384728/41', -4.2, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('fbf07005-0488-48bc-a68c-1f9aaa089bb5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-16 00:00:00+00', 'COMPRA MBWAY   CINE 0384728/39', -3.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('596ed53d-665b-4cd9-b7c1-9ba1b86785aa', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-16 00:00:00+00', 'Trf MB WAY 961958947 0384728/38', -7.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8e8c4e05-feb7-40c9-af30-64cac5d53deb', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-16 00:00:00+00', 'COMPRA ANTONIO R O  0384728/37', -30, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('80bc15eb-7d4e-4cfd-a589-944ca84989f9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-15 00:00:00+00', 'COMPRA PANS   COMPA 0384728/36', -19.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4ecab5fe-2363-4485-86de-a31d999704b3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-15 00:00:00+00', 'COMPRA MCDONALDS ES 0384728/35', -9.2, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('dddf53d1-fa42-42a4-8ffd-f71e2461ce4d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-15 00:00:00+00', 'COMPRA CONTINENTE V 0384728/34', -2.09, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c66a7ede-ce3c-4f50-8434-7f72fd27be5e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-15 00:00:00+00', 'CTT CORREIOS P 21154/217065511', -1.47, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ca75e5e9-3f32-4225-85f5-7ae02cfcb1c3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-14 00:00:00+00', 'TRANSF SEPA -MEDIS COMPANHIA P', 12, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e89418cc-e7c2-495c-8eee-59fe790dd854', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-13 00:00:00+00', 'Com trf MB WAY', -0.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('aec8ba12-3827-4043-aee0-14389a92b509', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-13 00:00:00+00', 'Trf MB WAY 961958947 0384728/33', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c9825a12-b780-4bc5-b40c-5fcb8dd336b9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-13 00:00:00+00', 'COMPRA KIWOKO LIMA  0384728/00', -12.58, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b96e8cb5-87ce-4151-9744-4fc23e4bdc32', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-13 00:00:00+00', 'TRANSF SEPA -MEDIS COMPANHIA P', 12, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('16a0a574-a091-4407-ab18-91e1e335b638', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-12 00:00:00+00', 'COMPRA O POTE RESTA 0384728/32', -11.8, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0e648fca-6184-4355-b0cb-454250c9b7d4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-12 00:00:00+00', 'Com trf MB WAY', -0.62, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e5be8f98-e6ba-41b9-96a9-3afcab479905', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-12 00:00:00+00', 'Trf MB WAY 966824733 0384728/32', -300, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('60c9cc82-b7ba-443b-a7c9-a2cde585ca9a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-12 00:00:00+00', 'COMPRA GALP P11773  0384728/31', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b75aba53-9336-437b-bab9-3609c9bc9702', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-12 00:00:00+00', 'COMPRA PRIMARK BRAG 0384728/00', -44, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b8897965-50bc-4db8-bc80-5bba9089cb50', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-12 00:00:00+00', 'TRANSF SEPA -LINOVT ENGENHARIA', 1156.48, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a33b4d17-b43f-4357-ba71-106d18440995', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-11 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/30', -4.28, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('61f447dc-cedf-4d70-8eb6-edae2c98d1d6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-11 00:00:00+00', 'Trf MB WAY 966824733 0384728/29', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9a64d0e1-cc74-4c71-b80a-438b0f5fbeea', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-10 00:00:00+00', 'COMPRA MERCADONA    0384728/28', -14.8, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('90734619-a1de-4d99-8dff-c09f5d3d08fe', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-10 00:00:00+00', 'TRF MBW 003501430002036450028', 2.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('92c0b0d9-1a8c-45cf-b1d2-a067c2ef9d79', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-10 00:00:00+00', 'COMPRA A11          0384728/27', -2.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('df27307a-ca12-42d9-a3a3-32db5224fa1d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-10 00:00:00+00', 'TRF MBW 003501430002036450028', 7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('877bdfa7-0a61-48f9-8d73-9d0e58d838f4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-10 00:00:00+00', 'COMPRA PINGO DOCE B 0384728/25', -6.38, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('3250d615-f2f1-4b4c-af61-529b3ade0d92', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'COMPRA CONTINENTE B 0384728/24', -5.68, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('47de31df-7399-43e9-99cf-daf35cd4c453', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/23', -20.26, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8b0ffa32-a02b-4d3b-888b-133548bcd445', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'COMPRA CONTINENTE B 0384728/22', -15.53, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e3ed3616-1dbe-4409-b3b6-e78488df7769', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'Trf MB WAY 961958947 0384728/22', -22.72, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('67021cac-d2a9-44e5-b6ae-1b3eb4a61405', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'COMPRA GALP P11773  0384728/21', -30, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1bd6e375-9bb5-40b9-9185-c9f234f9dccb', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'COMPRA CLINICA MEDI 0384728/20', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ae459a5e-97c5-4efc-bbc9-4e358786f8a7', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'Disponibilizacao de um cartao de debito', -19.76, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a5bcfcf2-92d9-45b9-b26f-61289dd7aa4f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'COMPRA BK27203 VIAN 0384728/19', -26.95, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8e0df20c-b009-44e2-a7a0-0d3e537ea8c9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-09 00:00:00+00', 'TRF MBW 003501430002036450028', 13.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ff171367-5a30-4885-9d8e-ec164a35fdcc', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-08 00:00:00+00', 'Trf MB WAY 961958947 0384728/19', -12, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('32e4e43e-8411-4ed8-a200-ad99e8543545', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-07 00:00:00+00', 'COMPRA CONTINENTE   0384728/18', -8.4, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8c5150b2-9581-4f97-8704-161f665c0f13', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-07 00:00:00+00', 'COMPRA GALP P11773  0384728/17', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ce0efdc5-e3c0-404c-b4f9-449f9cfcc7c2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-07 00:00:00+00', 'TRANSF SEPA -LINOVT ENGENHARIA', 266.4, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('6c5ae583-99e9-47c0-a380-1ef424dc3d11', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-06 00:00:00+00', 'Trf MB WAY 961958947 0384728/17', -9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1357de59-37fa-42da-9aa9-b27a97890b51', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-05 00:00:00+00', 'COMPRA GALP P11773  0384728/16', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('cc466771-ea38-4e72-afe9-4f16b64f1b28', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-04 00:00:00+00', 'Com trf MB WAY', -0.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2f7e47c2-0611-4fd2-a0e6-c4b600bdd97e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-04 00:00:00+00', 'Trf MB WAY 961958947 0384728/16', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4578f95b-82eb-48cc-abb5-dd5be30ea3b6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'COMPRA CONTINENTE B 0384728/15', -23.67, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('78e35a62-5c5d-4f33-864e-6665a3029715', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'COMPRA ELECLERC VIA 0384728/14', -5.95, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('39ac8bae-59fb-49d6-8019-5aa83773145e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'COMPRA JOM VIANA CA 0384728/13', -6.23, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f2e42de2-467f-4737-bc3d-26c42650992a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'COMPRA CONTINENTE V 0384728/12', -0.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d8614c5b-edc9-4850-946e-ba923b14af6c', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'TRF MBW 003300004528070007805', 5.15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5b6e79c2-27cc-4fc9-9db3-a5b613e91d3a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'TRF MBW 001800034385862002090', 5.15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('37db2da4-c696-43a4-995b-2f6f828aa887', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-03 00:00:00+00', 'COMPRA MINIPRECO BA 0384728/11', -20.57, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('55ee8ef5-0863-49df-98c3-9308fa87ed39', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-02 00:00:00+00', 'TRF MBW 003300004528070007805', 2.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4cea1106-e095-4979-945d-79fa5f2e0b61', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-02 00:00:00+00', 'TRF MBW 001800034385862002090', 2.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2a1a9d53-b436-449a-8a38-e00cc562b926', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-02 00:00:00+00', 'Trf MB WAY 915415403 0384728/11', -2.23, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('55f07e6f-2efb-4569-9223-a6928918b079', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-02 00:00:00+00', 'COMPRA CERQUEIRASS  0384728/10', -31.6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('aa3c8e41-4874-485a-bcfe-249df2c3e9b0', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-02 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/09', -11.28, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('58d38bf4-4b52-4a48-9e69-4ab653ed7eed', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-01 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/08', -13.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5299f44e-caff-449e-887e-7e370a603322', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-01 00:00:00+00', 'COMPRA CONTINENTE B 0384728/07', -26.38, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('12e698a9-0432-458e-8ff5-a59980f417f3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-01 00:00:00+00', 'COMPRA MERCADONA    0384728/06', -15.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('3a7ef116-7348-4d86-9b40-bfd12f3f8687', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-01 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/05', -9.93, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5583afe5-9792-4fce-8545-70b63c972655', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-11-01 00:00:00+00', 'A.D.A.M. - AGU 21947/302242049', -108.25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ed45eef4-2c35-4d54-9d06-c4d974a81f4f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-31 00:00:00+00', 'Trf MB WAY 961958947 0384728/06', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c0551baa-018c-4852-8c04-b076e7d33004', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-31 00:00:00+00', 'Trf MB WAY 961958947 0384728/05', -25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2e2fa04b-3bf5-4c0f-85c4-6dddb128b55f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-31 00:00:00+00', 'TRF MBW 003501430002036450028', 145, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e80fa5d6-1783-4bb3-87a4-a4ffc4f21a77', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-31 00:00:00+00', 'SEPA DD-600388     -CROSSFIT V', -59, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d126f290-f50b-4ecf-bce8-0e0e2b2f82fe', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-29 00:00:00+00', 'COMPRA GALP P11773  0384728/04', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8b717ba0-b79e-4c3d-9e23-4099a15ed011', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-29 00:00:00+00', 'SEPA DD-90037171982-FIDELIDADE', -25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('94dd8bfc-db91-4353-a2d5-36bb3effb4f6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-28 00:00:00+00', 'Trf MB WAY 961958947 0384728/04', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f11c25ce-5f6d-40cb-b917-25fce37d3f63', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-28 00:00:00+00', 'COMPRA LIDL AGRADEC 0384728/03', -17.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0981a540-e456-4467-acf0-75e349d5ac47', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-26 23:00:00+00', 'COMPRA SCALA CAFFE  0384728/02', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1090db21-a310-4313-bd5f-63eb6a9cffe2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-26 23:00:00+00', 'COMPRA MERCADONA    0384728/02', -20.3, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('cb126feb-0e9d-4f76-b690-fbc1a8896a10', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-26 23:00:00+00', 'Trf MB WAY 961958947 0384728/01', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f1c337bf-0c11-407a-9ae0-e028f2e37ac6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-25 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/00', -6.34, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8c7cfb71-027d-4fd8-afae-41031ce0d1a4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-25 23:00:00+00', 'COMPRA CONTINENTE B 0384728/99', -9.97, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c9fd133d-9ff0-4aa5-af49-a3389dc70e7d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-25 23:00:00+00', 'COMPRA MERCADONA    0384728/98', -0.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('76537355-1a70-45cf-969d-ce766da064cf', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-25 23:00:00+00', 'COMPRA MERCADONA    0384728/97', -0.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0f2d7fcc-b9a5-49b9-a192-65f391ba1b0a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-25 23:00:00+00', 'COMPRA MERCADONA    0384728/96', -20.64, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('892df73a-8f7e-42f6-aabc-5c46d61cf027', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-25 23:00:00+00', 'LEVANT Intermarche  0384728/95', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c6ddacd0-865d-402d-88fb-8c6eb5cd3d55', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-24 23:00:00+00', 'COMPRA GALP P11773  0384728/94', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('714acda7-523b-4931-be32-337d9ad0398b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-24 23:00:00+00', 'COMPRA GOOGLE GOOGL 0384728/00', -3.99, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9b82ba43-3374-41a8-bc71-389fe5c281c8', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-23 23:00:00+00', 'Trf MB WAY 961958947 0384728/93', -2, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('65847885-fb0e-4395-be7b-a45c9b2b17a5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-23 23:00:00+00', 'COMPRA WWW VIVAWALL 0384728/92', -12.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b06ecc30-c9b4-4f48-a5f5-ca419e2425c5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-21 23:00:00+00', 'COMPRA GALP P11773  0384728/91', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('124639f5-8e2a-4187-bef0-3036558083a4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-20 23:00:00+00', 'COMPRA PRIMARK BRAG 0384728/00', -2, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ea3ce9a2-948e-48a4-9e11-fcc8073729c8', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-20 23:00:00+00', 'COMPRA NORMAL BRAGA 0384728/00', -14.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('47d7620e-2fde-416d-b2b3-a56b9485f81e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-19 23:00:00+00', 'COMPRA ELECLERC VIA 0384728/90', -13.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0604ac91-cf91-4f25-ac86-474d1cf689f1', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'TRF MBW 003501430002036450028', 15.53, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('6d2e7d8f-34f1-4ea0-853c-dc06298b205e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/89', -31.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('77decd5e-4531-4a34-b858-79ed4b891023', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'COMPRA MAREC ESPACO 0384728/88', -4.49, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('7b7d6caf-e24a-4ca7-8898-5115d104a3da', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'Com trf MB WAY', -0.27, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c1476038-b009-425c-a781-865fb24106c0', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'Trf MB WAY 961958947 0384728/86', -129.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2228fdc0-10e2-47a4-a4cc-c68d22e372da', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'TRF MBW 003501430002036450028', 7.28, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4ab1e40b-e9c7-4724-8237-37a03918125f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'TRF MBW 003501430002036450028', 3.22, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('6c60fc42-11ca-4524-9ef9-612d762293ae', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'COMPRA PINGO DOCE B 0384728/85', -14.55, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d55e315d-161b-46c4-809b-127c8336db4e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'TRF MBW 019300001050479346390', 4.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ae501388-5209-4af7-beef-5f09b23f7d75', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'COMPRA DECATHLON BR 0384728/84', -4.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('de252c1b-c975-4d3b-8383-2ff357334bbd', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-18 23:00:00+00', 'COMPRA MERCADONA    0384728/83', -6.43, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5b5a5bdb-0832-4141-bf42-d06c183eed46', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-17 23:00:00+00', 'COMPRA GALP P11773  0384728/82', -35, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('14ea6570-49e6-4095-bd83-03297085aabd', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-17 23:00:00+00', 'COMPRA Spotify P30B 0384728/00', -10.49, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f3914fc0-1ae7-4381-95ca-9d30a8091ec3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-16 23:00:00+00', 'CTT CORREIOS P 21154/217065511', -3.89, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1dfeaef2-6288-4b62-9205-ae124ae2e76e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-15 23:00:00+00', 'TRF MBW 003501430002036450028', 3, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5a448434-ec6b-487b-9a78-414e22475b61', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-14 23:00:00+00', 'COMPRA MBWAY   FATU 0384728/81', -17.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f769a249-5011-4d50-b0e2-64cf7b8b10a4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-14 23:00:00+00', 'COMPRA Nike Porto 2 0384728/00', -77.34, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('3e42a1ed-ac74-4ad3-9843-b945782c74fe', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-13 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/79', -8.06, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1bd0a3d4-30c2-4ea4-bc48-90139638eea4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-13 23:00:00+00', 'COMPRA PRIMARK BRAG 0384728/00', -40.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8ec73fff-db8e-4f82-833c-28beac0b8de1', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-13 23:00:00+00', 'COMPRA IKEA BRAGA C 0384728/00', -2.75, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8b8bff9c-c558-492f-a0be-14a078df0554', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'TRF MBW 003501430002036450028', 5.57, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('64bd3150-c57e-48d1-8d23-013c9cffa332', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'Trf MB WAY 961958947 0384728/80', -1.4, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5c784cb8-f3b8-47d4-b5a3-c85e4e76d8e5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'Trf MB WAY 961958947 0384728/79', -15.15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2331a249-502e-4887-b0e1-2a0353a29357', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/78', -2.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b8a32800-ba5e-4e36-97f1-03c775b0ebd2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'Trf MB WAY 961958947 0384728/78', -3.54, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9204177f-5159-4736-95d7-d9fba3f46ac3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'COMPRA GALP P11773  0384728/77', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ab083e33-3be7-4e5a-81b7-f7237981e8f2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'TRF MBW 003501430002036450028', 38.67, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c264dcd7-dbe2-4152-9924-312b4ea8b2c0', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'TRF MBW 003501430002036450028', 8.39, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f37b7abe-1132-4f9b-bb1b-335980013658', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-12 23:00:00+00', 'COMPRA MERCADONA    0384728/75', -16.78, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('957f9bbd-aecf-4e42-b1ab-b86502d03e1e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a966ae17-2680-4cde-be7e-5403d7212a80', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('07fad5ec-8c88-4fe2-9491-1c7eccc57ecf', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 1.95, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('7b063255-931b-4ea1-9cb8-861465812760', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'COMPRA MCDONALDS VI 0384728/74', -3.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('34aec787-0c8f-426d-8046-d4fd11716a3e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 22.85, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('cddffc62-4879-4e11-89f0-c50299252fdf', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'COMPRA KARUTA SUSHI 0384728/73', -45.7, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e4ef7343-f3c0-4953-a973-bf92f65086ae', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'COMPRA CLINICA MEDI 0384728/72', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('63557259-f724-416f-b094-ebd6ac66165e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'Trf MB WAY 961958947 0384728/76', -2.25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b6d89736-8afc-41fd-a397-6217e0a07237', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 11.47, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('38fee4dd-f714-4726-96e4-0e1734c15ccd', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'COMPRA PINGO DOCE B 0384728/70', -22.94, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ca6d2678-c0ab-4b80-b2e8-8212e145fd0a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 20.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('64d49264-bcba-4d4b-82a9-9f205cad0b83', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'TRF MBW 003501430002036450028', 12.2, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('efcde05f-ecbb-49a1-b7f6-0df948cafa89', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-11 23:00:00+00', 'COMPRA MCDONALDS VI 0384728/68', -24.4, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('6ed7669e-2d0c-478b-b4a9-30758083db62', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-09 23:00:00+00', 'TRF MBW 003501430002036450028', 8, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5605a0b3-0914-45b1-b2ff-cf095f96edd9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-09 23:00:00+00', 'TRF MBW 003501430002036450028', 101, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('37dccbf0-663a-4989-91bf-cc620287f842', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-09 23:00:00+00', 'COMPRA GALP P11773  0384728/67', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('91e274b8-6047-4d84-a30c-8106565f7a9b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-08 23:00:00+00', 'Trf MB WAY 961958947 0384728/75', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('6af55868-d23c-482e-a0e3-ab19447a78b8', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-08 23:00:00+00', 'Trf MB WAY 961958947 0384728/74', -12, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('3c9231d0-3ea5-404d-8518-d09e1f5bcd73', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-07 23:00:00+00', 'ORDENADOS   -LINOVT ENGENHARIA', 1156.48, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8d7d090e-45eb-4b23-978e-461f059e1869', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-05 23:00:00+00', 'COMPRA CONTINENTE V 0384728/66', -6.49, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('066f80e6-1cac-4e24-979a-521eb7086398', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-05 23:00:00+00', 'COMPRA MERCADONA    0384728/65', -5.78, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e525228c-4905-4488-8a11-3e0b8c322684', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-05 23:00:00+00', 'COMPRA CONTINENTE V 0384728/64', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f84df09e-da49-4089-8522-728b33fe6956', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-04 23:00:00+00', 'COMPRA MERCADONA    0384728/63', -9.19, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c9eb2b2b-e1a3-407e-b72d-e9be9911841b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-04 23:00:00+00', 'COMPRA GALP P11773  0384728/62', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d05a9ca9-1413-47d4-a663-fa470be46cd9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-03 23:00:00+00', 'Trf MB WAY 961958947 0384728/73', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0e19addf-171f-4473-b5dd-35bb2e7e0442', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-03 23:00:00+00', 'Trf MB WAY 961958947 0384728/72', -12, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5007b56f-27f9-49e1-975c-19ed33412798', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-03 23:00:00+00', 'Trf MB WAY 961958947 0384728/71', -23, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c29b3d6e-83bf-4950-8b40-c7e0bf06ce06', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-02 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/61', -9.72, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('898df904-7d97-4348-9b49-b58e9d3ea583', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-10-01 23:00:00+00', 'Trf MB WAY 961958947 0384728/70', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1c461a54-4c37-4fa5-bf43-9b8ed91d41f3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-30 23:00:00+00', 'Trf MB WAY 961958947 0384728/69', -13, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a2723670-48b6-407a-9d25-91d1ccbc6b48', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-30 23:00:00+00', 'SEPA DD-600388     -CROSSFIT V', -59, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f56ed94e-00b6-4944-a3c9-3f0c1ce30199', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-29 23:00:00+00', 'TRF MBW 003501430002036450028', 13, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('3024a5d0-5394-49fe-9e21-a1fa74e118f9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-29 23:00:00+00', 'Com trf MB WAY', -0.03, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8cf982a2-6681-4b31-acdc-03f921394f4b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-29 23:00:00+00', 'Trf MB WAY 961958947 0384728/68', -13, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4360b9ca-1965-4188-bdb1-ce8a13a18306', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-29 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/60', -4.34, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('275f5eb3-54c4-4d67-a2d0-d3f600577986', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-29 23:00:00+00', 'SEPA DD-90037171982-FIDELIDADE', -25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8fe3c6f1-2fd6-4069-bd82-4d283f41cd89', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'Com trf MB WAY', -0.16, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b5433d5b-3aa1-4b58-b3f3-f42443873a02', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'Trf MB WAY 961958947 0384728/67', -74, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d8f96081-ad14-43fb-9652-6b40ca4f2470', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'COMPRA CONTINENTE V 0384728/59', -8.92, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b1de973e-6576-4687-b32d-0e921bd624d9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'Com trf MB WAY', -0.01, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ce916f97-d743-4d62-94f3-2c3423149e45', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'Trf MB WAY 961958947 0384728/66', -5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e54e1d00-f12c-4b63-90fa-c78e0ff6eb71', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'Com trf MB WAY', -0.04, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b3f98351-1efb-455d-9e52-92052f93e592', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-28 23:00:00+00', 'Trf MB WAY 961958947 0384728/65', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f9ea69ee-f810-4934-a71b-f8e12765748a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-27 23:00:00+00', 'COMPRA GALP P11773  0384728/58', -37, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('4292618d-8884-43a3-a6f4-09069bea73f3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-27 23:00:00+00', 'COMPRA PANS   COMPA 0384728/57', -19.9, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0629a70c-d54a-41dc-a2cd-1622522bf018', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-27 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/56', -32.91, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('7e791fa7-a39f-4a5d-bc05-80441e499c1b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-27 23:00:00+00', 'LEVANT Intermarche  0384728/55', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('555fa4eb-3d77-4723-be48-caa57e7a5202', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'COMPRA CONTINENTE V 0384728/54', -3.96, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d68870c3-c5a3-4d54-8167-8ee00a1ad740', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'CTT CORREIOS P 21154/216633940', -5.8, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('dd4e1c40-bdb4-485e-a851-763cf9a214be', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'Com trf MB WAY', -0.01, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('6717bfbb-91e5-4cc9-9f61-33436f555ac4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'Trf MB WAY 961958947 0384728/64', -3, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('aef5df71-2310-4762-8390-c4fb0ae98535', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'Com trf MB WAY', -0.03, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d8fd9080-f1c9-48e6-ab0f-9ffe27d136e1', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'Trf MB WAY 961958947 0384728/63', -16, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c0af5b62-8f8d-4dc2-8048-fe99ad09a03c', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/53', -5.22, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2e56a4c3-2762-414a-8c9a-de26d240aa3c', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-25 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/52', -20.99, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('98a4c19b-96e6-42df-bf16-c5dc1fec8359', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-24 23:00:00+00', 'Com trf MB WAY', -0.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d1d47bde-661a-472c-9160-c85417fd337d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-24 23:00:00+00', 'Trf MB WAY 961958947 0384728/62', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1f283031-6540-4347-ba71-b298833739b4', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-24 23:00:00+00', 'COMPRA GOOGLE GOOGL 0384728/00', -3.99, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('7715b6cc-8d4b-43ba-a02c-f832e3c89f7b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-23 23:00:00+00', 'Com trf MB WAY', -0.1, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('204062d9-e071-4071-b954-5676b25d7d05', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-23 23:00:00+00', 'Trf MB WAY 961958947 0384728/61', -50, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c7ec1de4-6660-4afb-b2c6-023b45039c22', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'Com trf MB WAY', -0.01, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('69992741-3048-4f8d-9e27-6ef9af930e00', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'Trf MB WAY 961958947 0384728/60', -6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('71bf8d61-1336-48d7-af79-ad351d99693d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'COMPRA MERCADONA    0384728/50', -77.94, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('076ca5ee-e4da-4057-8895-238b2b0927f2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'Com trf MB WAY', -0.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('82a655cc-ed9a-463d-8153-9024e55bdd67', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'Trf MB WAY 961958947 0384728/59', -25, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('825d36db-1545-4450-bf24-b16c9bbd449f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'Com trf MB WAY', -0.02, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('7cb1fed4-02c5-49f3-913d-9cf2fea29de0', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-22 23:00:00+00', 'Trf MB WAY 961958947 0384728/58', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ec60b616-82c7-4679-b9c3-1735590d62cb', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-21 23:00:00+00', 'COMPRA BK27203 VIAN 0384728/49', -30.39, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9014576b-9858-42aa-a1c1-da857418a551', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'COMPRA CONTINENTE B 0384728/48', -32.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('614acb78-4bf6-4ee0-b743-1846dc43bb16', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'Com trf MB WAY', -0.02, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('9af9951b-c8c7-4764-a604-4f5a6f6e70b1', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'Com trf MB WAY', -0.02, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f608159e-cbf0-4d4a-931d-ce69bac7074d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'Trf MB WAY 961958947 0384728/57', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('13fef3dd-c6f7-4b74-8e71-09dffc9cd1ce', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'Trf MB WAY 961958947 0384728/55', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e20f2c4d-c894-4900-8aee-c7e5de7ad1cf', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'Com trf MB WAY', -0.01, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('39988bfa-4b14-4071-ac24-0c8ab3876983', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-20 23:00:00+00', 'Trf MB WAY 961958947 0384728/56', -6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('badac371-01e5-424c-a6ee-a73f53347aa9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-19 23:00:00+00', 'Com trf MB WAY', -0.03, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('295b53cb-d8a6-4932-9681-81e31db0bcb2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-19 23:00:00+00', 'Trf MB WAY 961958947 0384728/54', -15, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8b73371f-c76a-41a2-a504-bcb95fb208de', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-18 23:00:00+00', 'Com trf MB WAY', -0.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('78da401b-7a6e-433a-8c8a-8dad9a2fe92a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-18 23:00:00+00', 'Trf MB WAY 961958947 0384728/53', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a3621ad3-efa7-4b7c-83b1-c14ca179ca5a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-18 23:00:00+00', 'COMPRA TEIXEIRA   L 0384728/47', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('339e2844-0286-46c6-9aef-445e4fcfdd11', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-17 23:00:00+00', 'COMPRA SCALA CAFFE  0384728/46', -10.85, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('ef79b464-9a52-4e4a-ba0d-bc0e2e8141b3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-17 23:00:00+00', 'COMPRA CONTINENTE   0384728/45', -13.35, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0c68da13-2493-4912-897f-6031e5a5db65', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-17 23:00:00+00', 'Com trf MB WAY', -0.06, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('0f1d80ed-f54d-4f2c-b79f-6c1c78428c9d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-17 23:00:00+00', 'Trf MB WAY 961958947 0384728/52', -30, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f7ecd757-18de-4d3c-b986-937334aeef77', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-17 23:00:00+00', 'COMPRA Spotify P2FF 0384728/00', -10.49, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d1f50661-26bf-4c28-8f4f-e458769e6a5b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-16 23:00:00+00', 'Com trf MB WAY', -0.02, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('487b855f-587c-4462-8f87-b0fe503668f3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-16 23:00:00+00', 'Trf MB WAY 961958947 0384728/51', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5d242bfa-3d95-42ff-8fbc-07e0604477a9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-15 23:00:00+00', 'COMPRA LIDL AGRADEC 0384728/43', -62.99, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('2bd22270-efde-4d24-b98a-42cc104e5ee3', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-15 23:00:00+00', 'Com trf MB WAY', -0.04, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('f4d28739-7eff-4d06-b35f-383d7b09ae12', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-15 23:00:00+00', 'Trf MB WAY 961958947 0384728/50', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5ce57f30-21e0-464d-ace5-f2fc7aca0c9d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-15 23:00:00+00', 'COMPRA BK27203 VIAN 0384728/42', -28.45, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8e63d739-1ca8-45af-b739-f5d0c824b65e', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Com trf MB WAY', -0.01, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('31610acb-32f4-4311-966a-78c6146395b6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Trf MB WAY 961958947 0384728/49', -6, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('109a9b3e-9ad2-4905-9fd3-ea7f45728ea0', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Com trf MB WAY', -0.02, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8961046c-2196-4f0c-b16e-34070c8ff8fd', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Trf MB WAY 961958947 0384728/48', -10, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('e83562d5-09f9-412e-a6e6-3bfe58ba3728', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Com trf MB WAY', -0.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('23f8ef69-67b9-427e-bec6-052c923c205b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Trf MB WAY 934579260 0384728/47', -25.45, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5c633f3d-d8e9-40ab-aa32-9f91a97b8893', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Com trf MB WAY', -0.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('aa5b6318-9fbf-419f-8ccf-e04ae0ca0dac', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Com trf MB WAY', -0.08, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('d5be4034-0ead-4922-8f8c-b8abccc90249', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Trf MB WAY 961958947 0384728/46', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c16778d6-45a8-4f07-a070-19bbb117c9a6', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-14 23:00:00+00', 'Trf MB WAY 919443157 0384728/45', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('cb7660a6-7665-4e7f-990b-8483db7c4c50', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-13 23:00:00+00', 'Com trf MB WAY', -0.05, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('b16b4c8c-86ef-49bd-b7d6-df2e396e6067', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-13 23:00:00+00', 'Trf MB WAY 932257120 0384728/44', -25.65, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('010e0b36-fa50-4d76-9480-96cb03d16220', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-13 23:00:00+00', 'COMPRA BK PORTIMAO  0384728/41', -10.55, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('00178a91-2713-47cc-9f29-344025fc00ff', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-13 23:00:00+00', 'Com trf MB WAY', -0.04, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('bc884ae2-f9d9-439f-af53-41acc2cadab9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-13 23:00:00+00', 'Trf MB WAY 961958947 0384728/43', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('fb3a109c-3381-4bdc-95cd-b6f47e4327b5', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-12 23:00:00+00', 'Trf MB WAY 915187762 0384728/42', -9.32, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('a05fde04-ca58-464f-b0cc-8e311d6d931f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-12 23:00:00+00', 'Trf MB WAY 961958947 0384728/41', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1064a7e9-e547-41b6-b17a-9c6ed27884dc', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-11 23:00:00+00', 'COMPRA SCALA CAFFE  0384728/40', -13.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('035d3f2e-33ff-4c5a-bd57-b61f7003bfa2', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-11 23:00:00+00', 'Trf MB WAY 961958947 0384728/40', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('5e58c4a8-0816-4178-9de5-f83c9f229343', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-11 23:00:00+00', 'COMPRA CONTINENTE V 0384728/39', -0.1, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('491a0d78-0083-4869-a077-83469d492a6f', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-11 23:00:00+00', 'COMPRA CONTINENTE V 0384728/38', -8.96, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('08dd9faa-2fa8-4464-bc6f-188d9eb6984d', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-11 23:00:00+00', 'Trf MB WAY 961958947 0384728/38', -30, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('8af39c43-de3b-4578-9e91-450a249c450c', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-11 23:00:00+00', 'COMPRA GALP P11773  0384728/37', -40, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('1721eb98-e6cc-4be0-9c6c-79145e88d5d9', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-10 23:00:00+00', 'COMPRA RYANAIR224DU 0384728/00', -45.99, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c4750912-8faf-4183-a6f6-93e2af5b13af', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-08 23:00:00+00', 'COMPRA RNE          0384728/36', -16.5, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c4cbf277-dc61-47db-8dcf-3c3073f0637a', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-08 23:00:00+00', 'Trf MB WAY 961958947 0384728/35', -20, false, NULL, NULL);
INSERT INTO public.transactions (id_transaction, id_wallet, id_category, id_subcategory, id_transaction_group, id_reocurring, date, description, amount, is_planned, latitude, longitude) VALUES ('c5be6b8b-f83b-4d5f-bad1-18c1e8e8ab0b', 'c5daf66b-ecfb-409b-9775-842b001991c2', NULL, NULL, NULL, NULL, '2024-09-08 23:00:00+00', 'COMPRA STEAMGAMES C 0384728/00', -13.47, false, NULL, NULL);


--
-- TOC entry 3251 (class 2606 OID 16486)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3253 (class 2606 OID 16503)
-- Name: category category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT category_pkey PRIMARY KEY (id_category);


--
-- TOC entry 3262 (class 2606 OID 16534)
-- Name: reocurring reocurring_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reocurring
    ADD CONSTRAINT reocurring_pkey PRIMARY KEY (id_reocurring);


--
-- TOC entry 3258 (class 2606 OID 16517)
-- Name: subcategory subcategory_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategory
    ADD CONSTRAINT subcategory_pkey PRIMARY KEY (id_subcategory);


--
-- TOC entry 3255 (class 2606 OID 16511)
-- Name: transaction_group transaction_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transaction_group
    ADD CONSTRAINT transaction_group_pkey PRIMARY KEY (id_transaction_group);


--
-- TOC entry 3268 (class 2606 OID 16553)
-- Name: transactions transactions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT transactions_pkey PRIMARY KEY (id_transaction);


--
-- TOC entry 3259 (class 1259 OID 16574)
-- Name: IX_reocurring_id_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_reocurring_id_category" ON public.reocurring USING btree (id_category);


--
-- TOC entry 3260 (class 1259 OID 16575)
-- Name: IX_reocurring_id_subcategory; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_reocurring_id_subcategory" ON public.reocurring USING btree (id_subcategory);


--
-- TOC entry 3256 (class 1259 OID 16576)
-- Name: IX_subcategory_id_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_subcategory_id_category" ON public.subcategory USING btree (id_category);


--
-- TOC entry 3263 (class 1259 OID 16577)
-- Name: IX_transactions_id_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_category" ON public.transactions USING btree (id_category);


--
-- TOC entry 3264 (class 1259 OID 16578)
-- Name: IX_transactions_id_reocurring; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_reocurring" ON public.transactions USING btree (id_reocurring);


--
-- TOC entry 3265 (class 1259 OID 16579)
-- Name: IX_transactions_id_subcategory; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_subcategory" ON public.transactions USING btree (id_subcategory);


--
-- TOC entry 3266 (class 1259 OID 16580)
-- Name: IX_transactions_id_transaction_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_transaction_group" ON public.transactions USING btree (id_transaction_group);


--
-- TOC entry 3269 (class 2606 OID 16518)
-- Name: subcategory FKCategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategory
    ADD CONSTRAINT "FKCategory" FOREIGN KEY (id_category) REFERENCES public.category(id_category);


--
-- TOC entry 3270 (class 2606 OID 16535)
-- Name: reocurring FKCategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reocurring
    ADD CONSTRAINT "FKCategory" FOREIGN KEY (id_category) REFERENCES public.category(id_category);


--
-- TOC entry 3272 (class 2606 OID 16554)
-- Name: transactions FKReocurring; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT "FKReocurring" FOREIGN KEY (id_reocurring) REFERENCES public.reocurring(id_reocurring);


--
-- TOC entry 3271 (class 2606 OID 16540)
-- Name: reocurring FKSubcategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reocurring
    ADD CONSTRAINT "FKSubcategory" FOREIGN KEY (id_subcategory) REFERENCES public.subcategory(id_subcategory);


--
-- TOC entry 3273 (class 2606 OID 16559)
-- Name: transactions FKSubcategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT "FKSubcategory" FOREIGN KEY (id_subcategory) REFERENCES public.subcategory(id_subcategory);


--
-- TOC entry 3274 (class 2606 OID 16564)
-- Name: transactions FKTransactiongroup; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT "FKTransactiongroup" FOREIGN KEY (id_transaction_group) REFERENCES public.transaction_group(id_transaction_group);


--
-- TOC entry 3275 (class 2606 OID 16569)
-- Name: transactions FkCategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
    ADD CONSTRAINT "FkCategory" FOREIGN KEY (id_category) REFERENCES public.category(id_category);


-- Completed on 2024-12-07 20:19:54 WET

--
-- PostgreSQL database dump complete
--

