CREATE DATABASE "Amazonia"
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'Spanish_Costa Rica.1252'
       LC_CTYPE = 'Spanish_Costa Rica.1252'
       CONNECTION LIMIT = -1;


create table public.cliente
(
Cedula integer primary key,
Pass character(15)not null,
Detalle XML
);--Nombre, Apellidos, Fecha_Nacimiento, Telefono, Numero_Tarjeta, Correo



create table vendedor
(
Id_Vendedor integer primary key,
Pass character(15)not null,
Detalle XML
);--Nombre, Apellidos, Fecha_Nacimiento, Telefono, Correo

create table distribuidor
(
Cod_Distribuidor integer primary key,
Id_Inventario integer not null,
Detalle XML
);--Nombre_Empresa, Telefono, Ubicacion, Correo

create table Inventario
(
Id_Inventario integer primary key,
Detalle XML
);--Codigo, Nombre, Categoria, Precio, Stock

--TERMINA SQL SERVER...                COMIENZA ORACLE

create table productosAmazon(
Codigo integer primary key,
Detalle XML,
Foto bytea
);--Codigo, Nombre, Categoria, Precio, Stock

create table productosDistribuidor(
Codigo integer primary key,
Detalle XML,
Foto bytea
);--Codigo, Nombre, Categoria, Precio, Stock

create table Pedido(
Numero_Pedido integer primary key,
Cedula_Cliente integer not null,
Fecha DATE not null,
Detalle XML not null,
Estado character(20) not null
);--Codigo, Nombre, Categoria, Precio, Cantidad   (sin stock)
alter table Pedido add constraint foren FOREIGN key(Cedula_Cliente) references Clientes(Cedula);

create table Orden_Compra(
Numero_Orden integer primary key,
Fecha date,
Codigo_Distribuidor integer not null,
Detalle XML not null,
Estado character(20) not null --Pendiente
);--Codigo, Nombre, Categoria, Precio, Cantidad    (sin stock)
alter table Orden_Compra add FOREIGN key(Codigo_Distribuidor) references Distribuidores(Cod_Distribuidor);

create table Factura(
Numero_Factura integer primary key,
Cedula_Cliente integer not null,
Fecha date not null,
Detalle xml not null,
Subtotal integer not null
);--Codigo, Nombre, Categoria, Precio, Cantidad    (sin stock)
alter table Factura add FOREIGN key(Cedula_Cliente) references Clientes(Cedula);

create table Carrito(
Numero_Carrito integer primary key,
Detalle XML not null
);--Codigo, Nombre, Categoria, Precio, Cantidad   (sin stock)

create table Clientes
(
Cedula integer primary key,
Pass character(15)not null,
Detalle XML
);--Nombre, Apellidos, Fecha_Nacimiento, Telefono, Numero_Tarjeta, Correo

create table Distribuidores
(
Cod_Distribuidor integer primary key,
Detalle XML
);--Nombre_Empresa, Telefono, Ubicacion, Correo