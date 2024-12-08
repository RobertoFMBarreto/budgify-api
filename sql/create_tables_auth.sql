--
-- PostgreSQL database dump
--

-- Dumped from database version 17.2 (Debian 17.2-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-08 23:22:05 WET

SET statement_timeout = 0;

SET lock_timeout = 0;

SET idle_in_transaction_session_timeout = 0;

SET transaction_timeout = 0;

SET client_encoding = 'UTF8';

SET standard_conforming_strings = on;

SELECT pg_catalog.set_config ('search_path', '', false);

SET check_function_bodies = false;

SET xmloption = content;

SET client_min_messages = warning;

SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 16385)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;

--
-- TOC entry 3376 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner:
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16396)
-- Name: user_refresh_token; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_refresh_token (
    id_token uuid DEFAULT public.uuid_generate_v4 () NOT NULL,
    id_user uuid NOT NULL,
    token text NOT NULL,
    device text NOT NULL,
    creation_date timestamp without time zone DEFAULT now() NOT NULL,
    last_usage timestamp without time zone DEFAULT now() NOT NULL
);

ALTER TABLE public.user_refresh_token OWNER TO postgres;

--
-- TOC entry 3224 (class 2606 OID 16407)
-- Name: user_refresh_token user_refresh_token_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_refresh_token
ADD CONSTRAINT user_refresh_token_pkey PRIMARY KEY (id_token);

-- Completed on 2024-12-08 23:22:05 WET

--
-- PostgreSQL database dump complete
--