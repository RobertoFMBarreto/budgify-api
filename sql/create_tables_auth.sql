--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-07 21:59:27 WET

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
-- TOC entry 2 (class 3079 OID 16389)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 3378 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16448)
-- Name: user_refresh_token; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_refresh_token (
    id_token uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    id_user uuid NOT NULL,
    token text NOT NULL,
    device text NOT NULL,
    creation_date timestamp without time zone DEFAULT now() NOT NULL,
    last_usage timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.user_refresh_token OWNER TO postgres;

--
-- TOC entry 3372 (class 0 OID 16448)
-- Dependencies: 218
-- Data for Name: user_refresh_token; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.user_refresh_token (id_token, id_user, token, device, creation_date, last_usage) VALUES ('3846a2b0-ed37-425a-9a38-9a5b425e11f2', '4458a5f9-9ea3-4d1c-9373-6de9f74185dc', 'ZjA9VjkKAGjf/WYUbN7VkqsDgezZTUWT2DnCfYFm+Lkzu1nrIRgH8Q/sx4zYKN1mJJgsA0BSq8LYz74L0e6NTyDxqzAC49htBN5dSdS+ulSSeot0/Lhmrhgs+cLhixqR7cETGBfBg8SK1umWN+4rPZKR1yvmIou53eQWSNRMyz/lwNrz0slvWCHW06NH7YDbuwcS8oUkmLfAmSyRLCfaW0I2ADLi6eo/C7mgpHGou/dzdofag9wvBc4xks51mb5GMtrTM9Mbrm/mhsdStO8LnY5Aj49FqomQJFTN4+FbMRcaSUkhNNsZHf4gtVgMgJNoKra582k3LC0IT7syRnJ6lM+x89iOOSF+dTqzxwdVAoDaFd/FxIvoKwZGo/jWAnQ6RRqqQIbi4UxegMHkU4V8ZpFVAvS8fGEii9drYljqL6qieycNrqNS+C09cmwwIZb4+n7Cqv2EwEcZ6+dAcBDesRipbYKPURtPeukIkzXTeKhu6Xm+lH5NJn8MkIfO9Dh85eZdm7VSc24gDI9UUDc+2nv7WBBmNOVGEXx35MQl581Sa11PDV96TtkGKOPoVGXJ8qX909aK6ui4lN+j4KWrPg==', 'insomnia/10.1.1 / ', '2024-12-01 17:17:47.50956', '2024-12-01 17:17:47.50956');
INSERT INTO public.user_refresh_token (id_token, id_user, token, device, creation_date, last_usage) VALUES ('590d365e-5dc9-4a91-a4c5-5cb36cd2484c', '4458a5f9-9ea3-4d1c-9373-6de9f74185dc', 'fn3Yq0mMAs+oiJXlqxLyzdk6Ec7ntQHM4qxZNuf1Gry65V19wYnxzgbQ45v3iJ0btfmg06ZjjB5adkZqNPlrbGinbVV8lrt/2dcPpQXMHwrGrvfSf7crlnlLKboWF6EYCp30TYxf9qWvF0CoVXk87bkE7ftAPcSgT8Mmd3aYSFW95YCg6Wld1TBy08TZj43AiSq019M3z15KnF0/QXhS90Kc5rKPD5Deenw2cNpiA5/vdKkpnhPvdVnhEImBhKPKr9s4CbPlWNRbBnIUnoFc2/uJZX/OapDkRy9pqomr2W7caNlGtLR/niwVpE24OnBmCoTO0O0rihLMi8a7BtOqCi/Ss6nsuoV7JYs4J5KNPSUm8q2SvRLDVAOLk6SVKvLznbgXwlOmNL+KbF6MoxeDLbRTirlDThgAC9aIufS4rPlSzqlk+d0QGsjQptxWRBWuTtY5GTR2P8lnaxVv0Ll9vrFHN+/H9xiOTyZ7vIIVZTC046+T+zdrZaWUXSbTxuLvfZ1+kDTvn5CWwkXzdZNZp/hOhQ00Uy6p8wTPYU+v6v6gkiWEBjfyq223CNEovU2VNE6FYQAD3vhlCE/E+lz/iQ==', 'insomnia/10.2.0 / 127.0.0.1', '2024-12-04 19:39:20.328055', '2024-12-04 19:39:20.328055');


--
-- TOC entry 3224 (class 2606 OID 16457)
-- Name: user_refresh_token device_unique_refresh; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_refresh_token
    ADD CONSTRAINT device_unique_refresh UNIQUE (device) INCLUDE (device);


--
-- TOC entry 3226 (class 2606 OID 16455)
-- Name: user_refresh_token user_refresh_token_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_refresh_token
    ADD CONSTRAINT user_refresh_token_pkey PRIMARY KEY (id_token);


-- Completed on 2024-12-07 21:59:28 WET

--
-- PostgreSQL database dump complete
--

