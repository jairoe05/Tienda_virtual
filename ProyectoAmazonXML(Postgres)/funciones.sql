CREATE OR REPLACE FUNCTION InsertarCarrito(num integer, deta xml) RETURNS integer AS $$
BEGIN
    INSERT INTO  carrito VALUES(num,deta);
 RETURN 1;
END;
$$ LANGUAGE plpgsql;


create or replace FUNCTION InsertarDetalleOrden(num integer, deta xml) RETURNS integer AS $$
begin
    insert into CarritoOrdenCompra values(num, deta);
    RETURN 1;
end;
$$ LANGUAGE plpgsql;

create or replace FUNCTION InsertarFactura(num integer, ced integer, fech date, deta xml,subto integer)RETURNS integer AS $$
begin
    insert into Factura values(num, ced, fech, deta, subto);
    RETURN 1;
end;
$$ LANGUAGE plpgsql;





create or replace function InsertarOrdenDeCompra(num integer, fech date, cod_distri integer, deta xml, estad character)
RETURNS integer AS $$
begin
   insert into Orden_Compra values(num, fech, cod_distri, deta, estad);
    RETURN 1;
end;
$$ LANGUAGE plpgsql;



create or replace function InsertarOrdenDeCompra(num integer, fech date, cod_distri integer, deta xml, estad character)
RETURNS integer AS $$
begin
    insert into Orden_Compra values(num, fech, cod_distri, deta, estad);
RETURN 1;
end;
$$ LANGUAGE plpgsql;



create or replace function InsertarPedido(num integer,ced integer, fech date, deta xml, estad character)
RETURNS integer AS $$
BEGIN
  INSERT INTO  pedido 	VALUES(num,ced,fech,deta,estad);
RETURN 1;
END;
$$ LANGUAGE plpgsql;