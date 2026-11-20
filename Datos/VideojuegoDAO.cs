using InventarioVideojuegosDilanTapia.Modelos;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace InventarioVideojuegosDilanTapia.Datos;

public class VideojuegoDAO
{
    public List<Videojuego> ObtenerTodos(string busqueda = "")
    {
        var resultado = new List<Videojuego>();
        using var conexion = ConexionSqlite.CrearConexion();
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = """
            SELECT Id, Nombre, Genero, Plataforma, Precio, Cantidad
            FROM Videojuegos
            WHERE Nombre LIKE @busqueda OR Genero LIKE @busqueda OR Plataforma LIKE @busqueda
            ORDER BY Id DESC;
            """;
        comando.Parameters.AddWithValue("@busqueda", $"%{busqueda}%");
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            resultado.Add(new Videojuego(
                lector.GetInt32(0), lector.GetString(1), lector.GetString(2),
                lector.GetString(3), Convert.ToDecimal(lector.GetDouble(4), CultureInfo.InvariantCulture),
                lector.GetInt32(5)));
        }
        return resultado;
    }

    public void Insertar(Videojuego videojuego)
    {
        using var conexion = ConexionSqlite.CrearConexion();
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = """
            INSERT INTO Videojuegos (Nombre, Genero, Plataforma, Precio, Cantidad)
            VALUES (@nombre, @genero, @plataforma, @precio, @cantidad);
            """;
        AgregarParametros(comando, videojuego);
        comando.ExecuteNonQuery();
    }

    public bool Actualizar(Videojuego videojuego)
    {
        using var conexion = ConexionSqlite.CrearConexion();
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = """
            UPDATE Videojuegos SET Nombre=@nombre, Genero=@genero,
                Plataforma=@plataforma, Precio=@precio, Cantidad=@cantidad
            WHERE Id=@id;
            """;
        AgregarParametros(comando, videojuego);
        comando.Parameters.AddWithValue("@id", videojuego.Id);
        return comando.ExecuteNonQuery() > 0;
    }

    public bool Eliminar(int id)
    {
        using var conexion = ConexionSqlite.CrearConexion();
        conexion.Open();
        // La eliminacion y el reinicio condicional del contador son atomicos.
        using var transaccion = conexion.BeginTransaction();
        using var comando = conexion.CreateCommand();
        comando.Transaction = transaccion;
        comando.CommandText = "DELETE FROM Videojuegos WHERE Id=@id;";
        comando.Parameters.AddWithValue("@id", id);
        bool eliminado = comando.ExecuteNonQuery() > 0;

        if (eliminado)
        {
            // Solo reiniciar cuando ya no queda ningun videojuego.
            // Nunca modificar los ID de registros que siguen guardados.
            using var reinicio = conexion.CreateCommand();
            reinicio.Transaction = transaccion;
            reinicio.CommandText = """
                DELETE FROM sqlite_sequence
                WHERE name = 'Videojuegos'
                  AND NOT EXISTS (SELECT 1 FROM Videojuegos);
                """;
            reinicio.ExecuteNonQuery();
        }

        transaccion.Commit();
        return eliminado;
    }

    private static void AgregarParametros(SqliteCommand comando, Videojuego videojuego)
    {
        comando.Parameters.AddWithValue("@nombre", videojuego.Nombre);
        comando.Parameters.AddWithValue("@genero", videojuego.Genero);
        comando.Parameters.AddWithValue("@plataforma", videojuego.Plataforma);
        comando.Parameters.AddWithValue("@precio", Convert.ToDouble(videojuego.Precio));
        comando.Parameters.AddWithValue("@cantidad", videojuego.Cantidad);
    }
}
