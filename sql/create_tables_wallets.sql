--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-07 20:20:21 WET

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
-- TOC entry 2 (class 3079 OID 16466)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 3381 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16583)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16588)
-- Name: wallet; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wallet (
    id_wallet uuid NOT NULL,
    id_user uuid NOT NULL,
    name character varying(50) NOT NULL,
    id_requisition character varying(255),
    agreement_days integer,
    id_account character varying,
    store_in_cloud boolean DEFAULT true NOT NULL
);


ALTER TABLE public.wallet OWNER TO postgres;

--
-- TOC entry 3374 (class 0 OID 16583)
-- Dependencies: 218
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20241204203219_InitialCreate', '8.0.11');


--
-- TOC entry 3375 (class 0 OID 16588)
-- Dependencies: 219
-- Data for Name: wallet; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.wallet (id_wallet, id_user, name, id_requisition, agreement_days, id_account, store_in_cloud) VALUES ('c5daf66b-ecfb-409b-9775-842b001991c2', '4458a5f9-9ea3-4d1c-9373-6de9f74185dc', 'teste', NULL, NULL, NULL, true);


--
-- TOC entry 3226 (class 2606 OID 16587)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3228 (class 2606 OID 16592)
-- Name: wallet pk_wallets; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wallet
    ADD CONSTRAINT pk_wallets PRIMARY KEY (id_wallet);


-- Completed on 2024-12-07 20:20:21 WET

--
-- PostgreSQL database dump complete
--

