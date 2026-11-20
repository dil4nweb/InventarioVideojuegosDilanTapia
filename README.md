# Inventario de Videojuegos

**Estudiante:** Dilan Tapia Mercado  
**Asignatura:** Programacion con C# y .NET  
**Tipo:** Aplicacion de escritorio

## Descripcion y problema

Esta aplicacion permite organizar un inventario personal de videojuegos. Resuelve la dificultad de llevar manualmente el registro de titulos, generos, plataformas, precios y existencias. Su objetivo es registrar, consultar, modificar y eliminar videojuegos de forma sencilla, con almacenamiento permanente.

## Funcionalidades

- Registrar videojuegos con nombre, genero, plataforma, precio y cantidad.
- Consultar y buscar por nombre, genero o plataforma.
- Seleccionar un videojuego de la tabla y modificar sus datos.
- Eliminar el videojuego seleccionado.
- Validar campos vacios, numeros negativos y valores fuera de rango.
- Conservar la informacion al cerrar y volver a abrir el programa.

## Tecnologias

- SDK de .NET **8.0.134** (objetivo del proyecto: **net8.0**).
- C# y Programacion Orientada a Objetos.
- Avalonia UI **11.3.12** (incluido Avalonia.Controls.DataGrid) con XAML (`.axaml`).
- ADO.NET con `Microsoft.Data.Sqlite` **8.0.11**.
- SQLite como base de datos local.
- No se utiliza Entity Framework ni ORM.

## Estructura y clases

```text
InventarioVideojuegosDilanTapia/
|-- InventarioVideojuegosDilanTapia.csproj
|-- Program.cs
|-- App.axaml
|-- App.axaml.cs
|-- MainWindow.axaml
|-- MainWindow.axaml.cs
|-- Modelos/
|   `-- Videojuego.cs
|-- Datos/
|   |-- ConexionSqlite.cs
|   `-- VideojuegoDAO.cs
|-- Servicios/
|   `-- VideojuegoServicio.cs
|-- evidencias/
|-- GUIA_EVIDENCIAS_Y_GITHUB.md
|-- README.md
`-- .gitignore
```

- `Videojuego`: clase de dominio, propiedades, constructor y datos del videojuego.
- `VideojuegoDAO`: acceso a datos con comandos SQL parametrizados (CRUD).
- `ConexionSqlite`: crea la conexion y la tabla cuando la aplicacion se inicia.
- `VideojuegoServicio`: encapsula las validaciones y coordina el DAO.
- `MainWindow.axaml`: interfaz XAML con formulario, botones, buscador y tabla.
- `MainWindow.axaml.cs`: eventos de botones y seleccion, conecta interfaz con servicio.
- `App` y `Program`: inicializan Avalonia.

**POO aplicada:** clases y objetos, propiedades, constructores, encapsulamiento mediante campos privados y separacion de responsabilidades entre modelo, servicio, datos y vista. No se agrega herencia artificial porque no se necesita para este inventario.

## Instalacion de dependencias

1. Instalar SDK .NET 8.0.134 (u otro SDK 8 compatible) en Windows.
2. Descargar o clonar el repositorio:

```powershell
git clone https://github.com/dil4nweb/InventarioVideojuegosDilanTapia.git
cd InventarioVideojuegosDilanTapia
```

3. Restaurar paquetes (internet requerido en la primera ejecucion):

```powershell
dotnet restore
```

## Ejecucion

Desde la carpeta que contiene el archivo `.csproj`:

```powershell
dotnet run
```

No se necesita crear manualmente la base de datos. Al iniciar, el programa crea automaticamente la carpeta y tabla si no existen.

## Base de datos SQLite

La base de datos se guarda en Windows en:

```text
%LOCALAPPDATA%\InventarioVideojuegosDilanTapia\videojuegos.db
```

Tabla `Videojuegos`: `Id` (entero, clave primaria), `Nombre` (texto), `Genero` (texto), `Plataforma` (texto), `Precio` (real), `Cantidad` (entero). La clase `ConexionSqlite` inicializa la tabla con `CREATE TABLE IF NOT EXISTS`. `VideojuegoDAO` ejecuta `SELECT`, `INSERT`, `UPDATE` y `DELETE` mediante `SqliteConnection` y `SqliteCommand`, usando parametros para entradas del usuario y `using` para liberar conexiones.