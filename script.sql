CREATE UNLOGGED TABLE public.cliente (
                                id int4 NOT NULL,
                                limite int4 NOT NULL,
                                CONSTRAINT cliente_pk PRIMARY KEY (id)
);

CREATE UNLOGGED TABLE public.transacao (
                                  id int4 NOT NULL GENERATED ALWAYS AS IDENTITY,
                                  cliente_id int4 NOT NULL,
                                  valor int4 NOT NULL,
                                  tipo_transacao bpchar(1) NOT NULL,
                                  descricao varchar(10) NOT NULL,
                                  realizada_em timestamptz NOT NULL DEFAULT now(),
                                  saldo int4 NULL,
                                  CONSTRAINT transacao_pk PRIMARY KEY (id),
                                  CONSTRAINT transacao_fk FOREIGN KEY (cliente_id) REFERENCES public.cliente(id)
);
CREATE INDEX transacao_cliente_id_idx ON public.transacao USING btree (cliente_id);

CREATE OR REPLACE FUNCTION atualiza_saldo()
RETURNS TRIGGER AS $$
DECLARE
saldo_anterior INTEGER;
BEGIN
    saldo_anterior := COALESCE((SELECT saldo FROM transacao WHERE cliente_id = NEW.cliente_id ORDER BY id DESC LIMIT 1), 0);

    if new.tipo_transacao = 'c' then
    	NEW.saldo := COALESCE(saldo_anterior + new.valor, 0);
else
    	NEW.saldo := COALESCE(saldo_anterior - new.valor, 0);
end if;

RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER TBI_atualiza_saldo_cliente
    BEFORE INSERT ON transacao
    FOR EACH ROW
    EXECUTE FUNCTION atualiza_saldo();

INSERT INTO public.cliente(id, limite)
VALUES
    (1, 1000 * 100),
    (2, 800 * 100),
    (3, 10000 * 100),
    (4, 100000 * 100),
    (5, 5000 * 100);