SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

CREATE DATABASE integration_tests WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'en_US.UTF8';

ALTER DATABASE integration_tests OWNER TO postgres;

connect integration_tests

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;


CREATE SEQUENCE public.turmas_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
    CYCLE;

ALTER TABLE public.turmas_id_seq OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

CREATE TABLE public.turmas (
    id bigint DEFAULT nextval('public.turmas_id_seq'::regclass) NOT NULL,
    descricao character varying(100) NOT NULL,
    vagas integer NOT NULL
);

ALTER TABLE public.turmas OWNER TO postgres;

SELECT pg_catalog.setval('public.turmas_id_seq', 2, true);

ALTER TABLE ONLY public.turmas
    ADD CONSTRAINT turmas_pkey PRIMARY KEY (id);

CREATE UNIQUE INDEX turmas_idx ON public.turmas USING btree (id);


SET default_tablespace = '';

SET default_table_access_method = heap;

CREATE TABLE public.inscricoes (
   id character varying(40) NOT NULL,
   aluno character varying(40) NOT NULL,
   responsavel character varying(40) NOT NULL,
   turma bigint NOT NULL
);

ALTER TABLE public.inscricoes OWNER TO postgres;

ALTER TABLE ONLY public.inscricoes
    ADD CONSTRAINT inscricoes_pkey PRIMARY KEY (id);

CREATE UNIQUE INDEX inscricoes_idx ON public.inscricoes USING btree (id);

ALTER TABLE public.inscricoes
    ADD CONSTRAINT inscricoes_turma_fk FOREIGN KEY (turma) REFERENCES public.turmas(id);

GRANT ALL ON SCHEMA public TO PUBLIC;
