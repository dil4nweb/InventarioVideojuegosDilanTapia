using Microsoft.Data.Sqlite;

namespace InventarioVideojuegosDilanTapia.Datos;

public static class ConexionSqlite
{
    // LocalApplicationData permite conservar la BD entre ejecuciones sin subir datos privados a GitHub.
    public static readonly string CarpetaDatos = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "InventarioVideojuegosDilanTapia");

    public static readonly string RutaBaseDatos = Path.Combine(CarpetaDatos, "videojuegos.db");

    public static SqliteConnection CrearConexion() =>
        new($"Data Source={RutaBaseDatos}");

    public static void Inicializar()
    {
        Directory.CreateDirectory(CarpetaDatos);
        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = """
            CREATE TABLE IF NOT EXISTS Videojuegos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Genero TEXT NOT NULL,
                Plataforma TEXT NOT NULL,
                Precio REAL NOT NULL CHECK (Precio >= 0),
                Cantidad INTEGER NOT NULL CHECK (Cantidad >= 0)
            );
            """;
        comando.ExecuteNonQuery();
    }
}
