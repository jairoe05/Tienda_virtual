# Tienda Virtual: Proyecto Amazon

Tienda en línea académica, inspirada en Amazon, desarrollada con **ASP.NET Web Forms (C#) y PostgreSQL**. La información (productos, carrito, pedidos, facturas) se intercambia en formato **XML** entre la aplicación y la base de datos.

El sistema maneja **varios roles**, cada uno con su login y su menú:

| Rol | Qué puede hacer |
|---|---|
| **Usuario (cliente)** | Ver productos, agregar al carrito, hacer pedidos, ver sus pedidos y facturas, devolver productos |
| **Vendedor** | Gestionar pedidos y su detalle |
| **Distribuidor** | Ver órdenes, comprobar cantidades y actualizar el stock que suministra |
| **Gerente / Administrador** | Mantenimiento de usuarios, vendedores, distribuidores e inventario |

![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![ASP.NET Web Forms](https://img.shields.io/badge/ASP.NET-Web%20Forms-512BD4)
![PostgreSQL](https://img.shields.io/badge/DB-PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Spring.NET](https://img.shields.io/badge/IoC-Spring.NET-6DB33F)
![Bootstrap](https://img.shields.io/badge/UI-Bootstrap-7952B3?logo=bootstrap&logoColor=white)

## Proyectos del repositorio

| Carpeta | Descripción |
|---|---|
| `ProyectoAmazonXML(Postgres)/` | Primera versión: páginas Web Forms con acceso directo a PostgreSQL mediante Npgsql y XML. |
| `Spring amazonia Base Potgres/` | Versión refactorizada: la lógica está en clases por rol detrás de interfaces (`IUsuario`, `IVendedor`, `IDistribuidor`, `IAdministrador`, `IMantenimiento`, `Iconexion`) y se inyecta con **Spring.NET**. |

## Arquitectura (versión Spring.NET)

```
Páginas .aspx (Login*, Menu*, Mantenimientos)
        │  usan interfaces
        ▼
Metodos/  ── Usuario, Vendedor, Distribuidor, Administrador, Mantenimiento
        │  (inyectados con Spring.NET)
        ▼
conexion.cs ── Npgsql ──► PostgreSQL (baseAmazon)
                         intercambio de datos con XmlDocument
```

## Requisitos

- Windows + Visual Studio (el proyecto apunta a .NET Framework 4.0)
- IIS Express
- PostgreSQL con una base llamada `baseAmazon`
- Librerías incluidas en `bin/`: Npgsql 2.2, Spring.NET 1.1, Mono.Security

## Ejecución

1. Crea la base `baseAmazon` en PostgreSQL.
2. Abre la solución `ProyectoAmazonXML.sln` de la versión que quieras.
3. En `Web.config`, edita la cadena de conexión `AmazonConex` con tu servidor, usuario y contraseña.
4. Ejecuta con IIS Express (F5) y entra por `Login.aspx`.

## Tecnologías y conceptos aplicados

- ASP.NET Web Forms con Master Pages
- Inyección de dependencias e inversión de control con Spring.NET
- Programación orientada a interfaces
- Acceso a datos con ADO.NET / Npgsql
- Generación y lectura de XML con `System.Xml`
- Autenticación por rol y carrito de compras
