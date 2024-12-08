--
-- PostgreSQL database dump
--

-- Dumped from database version 17.2 (Debian 17.2-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-08 23:23:06 WET

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
-- TOC entry 3432 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner:
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16396)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);

ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16399)
-- Name: category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category (
    id_category uuid DEFAULT public.uuid_generate_v4 () NOT NULL,
    name character varying(50) NOT NULL,
    id_user uuid
);

ALTER TABLE public.category OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16403)
-- Name: reocurring; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reocurring (
    id_reocurring uuid DEFAULT public.uuid_generate_v4 () NOT NULL,
    id_wallet uuid NOT NULL,
    id_category uuid,
    id_subcategory uuid,
    description text NOT NULL,
    amount real NOT NULL,
    day_of_week integer,
    start_date timestamp
    with
        time zone NOT NULL,
        is_yearly boolean DEFAULT false NOT NULL,
        is_monthly boolean DEFAULT false NOT NULL,
        is_weekly boolean DEFAULT false NOT NULL,
        is_active boolean DEFAULT true NOT NULL
);

ALTER TABLE public.reocurring OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16413)
-- Name: subcategory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subcategory (
    id_subcategory uuid DEFAULT public.uuid_generate_v4 () NOT NULL,
    id_category uuid NOT NULL,
    name character varying(50) NOT NULL,
    id_user uuid
);

ALTER TABLE public.subcategory OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16417)
-- Name: transaction_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transaction_group (
    id_transaction_group uuid DEFAULT public.uuid_generate_v4 () NOT NULL,
    description text NOT NULL,
    start_date timestamp(6)
    with
        time zone NOT NULL,
        end_date timestamp(6)
    with
        time zone NOT NULL,
        planned_amount real NOT NULL,
        id_user uuid NOT NULL
);

ALTER TABLE public.transaction_group OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16423)
-- Name: transactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transactions (
    id_transaction uuid DEFAULT public.uuid_generate_v4 () NOT NULL,
    id_wallet uuid NOT NULL,
    id_category uuid,
    id_subcategory uuid,
    id_transaction_group uuid,
    id_reocurring uuid,
    date timestamp(6)
    with
        time zone NOT NULL,
        description text NOT NULL,
        amount real NOT NULL,
        is_planned boolean DEFAULT false NOT NULL,
        latitude real,
        longitude real
);

ALTER TABLE public.transactions OWNER TO postgres;

--
-- TOC entry 3421 (class 0 OID 16396)
-- Dependencies: 218
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20241204191448_InitialCreate', '8.0.11');

--
-- TOC entry 3251 (class 2606 OID 16431)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");

--
-- TOC entry 3253 (class 2606 OID 16433)
-- Name: category category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
ADD CONSTRAINT category_pkey PRIMARY KEY (id_category);

--
-- TOC entry 3257 (class 2606 OID 16435)
-- Name: reocurring reocurring_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reocurring
ADD CONSTRAINT reocurring_pkey PRIMARY KEY (id_reocurring);

--
-- TOC entry 3260 (class 2606 OID 16437)
-- Name: subcategory subcategory_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategory
ADD CONSTRAINT subcategory_pkey PRIMARY KEY (id_subcategory);

--
-- TOC entry 3262 (class 2606 OID 16439)
-- Name: transaction_group transaction_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transaction_group
ADD CONSTRAINT transaction_group_pkey PRIMARY KEY (id_transaction_group);

--
-- TOC entry 3268 (class 2606 OID 16441)
-- Name: transactions transactions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
ADD CONSTRAINT transactions_pkey PRIMARY KEY (id_transaction);

--
-- TOC entry 3254 (class 1259 OID 16442)
-- Name: IX_reocurring_id_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_reocurring_id_category" ON public.reocurring USING btree (id_category);

--
-- TOC entry 3255 (class 1259 OID 16443)
-- Name: IX_reocurring_id_subcategory; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_reocurring_id_subcategory" ON public.reocurring USING btree (id_subcategory);

--
-- TOC entry 3258 (class 1259 OID 16444)
-- Name: IX_subcategory_id_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_subcategory_id_category" ON public.subcategory USING btree (id_category);

--
-- TOC entry 3263 (class 1259 OID 16445)
-- Name: IX_transactions_id_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_category" ON public.transactions USING btree (id_category);

--
-- TOC entry 3264 (class 1259 OID 16446)
-- Name: IX_transactions_id_reocurring; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_reocurring" ON public.transactions USING btree (id_reocurring);

--
-- TOC entry 3265 (class 1259 OID 16447)
-- Name: IX_transactions_id_subcategory; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_subcategory" ON public.transactions USING btree (id_subcategory);

--
-- TOC entry 3266 (class 1259 OID 16448)
-- Name: IX_transactions_id_transaction_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_transactions_id_transaction_group" ON public.transactions USING btree (id_transaction_group);

--
-- TOC entry 3271 (class 2606 OID 16449)
-- Name: subcategory FKCategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategory
ADD CONSTRAINT "FKCategory" FOREIGN KEY (id_category) REFERENCES public.category (id_category);

--
-- TOC entry 3269 (class 2606 OID 16454)
-- Name: reocurring FKCategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reocurring
ADD CONSTRAINT "FKCategory" FOREIGN KEY (id_category) REFERENCES public.category (id_category);

--
-- TOC entry 3272 (class 2606 OID 16459)
-- Name: transactions FKReocurring; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
ADD CONSTRAINT "FKReocurring" FOREIGN KEY (id_reocurring) REFERENCES public.reocurring (id_reocurring);

--
-- TOC entry 3270 (class 2606 OID 16464)
-- Name: reocurring FKSubcategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reocurring
ADD CONSTRAINT "FKSubcategory" FOREIGN KEY (id_subcategory) REFERENCES public.subcategory (id_subcategory);

--
-- TOC entry 3273 (class 2606 OID 16469)
-- Name: transactions FKSubcategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
ADD CONSTRAINT "FKSubcategory" FOREIGN KEY (id_subcategory) REFERENCES public.subcategory (id_subcategory);

--
-- TOC entry 3274 (class 2606 OID 16474)
-- Name: transactions FKTransactiongroup; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
ADD CONSTRAINT "FKTransactiongroup" FOREIGN KEY (id_transaction_group) REFERENCES public.transaction_group (id_transaction_group);

--
-- TOC entry 3275 (class 2606 OID 16479)
-- Name: transactions FkCategory; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transactions
ADD CONSTRAINT "FkCategory" FOREIGN KEY (id_category) REFERENCES public.category (id_category);

-- Completed on 2024-12-08 23:23:06 WET

--
-- PostgreSQL database dump complete
--