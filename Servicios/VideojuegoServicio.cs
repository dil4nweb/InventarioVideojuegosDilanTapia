using System.Globalization;
using InventarioVideojuegosDilanTapia.Datos;
using InventarioVideojuegosDilanTapia.Modelos;

namespace InventarioVideojuegosDilanTapia.Servicios;

public class VideojuegoServicio
{
    private readonly VideojuegoDAO _dao = new();

    public List<Videojuego> Consultar(string filtro = "") => _dao.ObtenerTodos(filtro.Trim());

    public Videojuego ValidarYCrear(int id, string? nombre, string? genero,
        string? plataforma, string? precioTexto, string? cantidadTexto)
    {
        nombre = nombre?.Trim();
        genero = genero?.Trim();
        plataforma = plataforma?.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(genero) ||
            string.IsNullOrWhiteSpace(plataforma))
            throw new ArgumentException("Nombre, género y plataforma son obligatorios.");
        if (nombre.Length > 120 || genero.Length > 60 || plataforma.Length > 60)
            throw new ArgumentException("El nombre admite hasta 120 caracteres; género y plataforma, hasta 60.");

        // Se acepta coma o punto como separador decimal, sin separadores de miles.
        var normalizado = (precioTexto ?? "").Trim().Replace(',', '.');
        if (!decimal.TryParse(normalizado, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var precio) || precio < 0 || precio > 100000000)
            throw new ArgumentException("El precio debe ser un número entre 0 y 100000000 (ejemplo: 59900.50).");
        if (!int.TryParse(cantidadTexto, out var cantidad) || cantidad < 0 || cantidad > 1000000)
            throw new ArgumentException("La cantidad debe ser un entero entre 0 y 1000000.");

        return new Videojuego(id, nombre, genero, plataforma, precio, cantidad);
    }

    public void Registrar(Videojuego videojuego) => _dao.Insertar(videojuego);
    public bool Modificar(Videojuego videojuego) => _dao.Actualizar(videojuego);
    public bool Eliminar(int id) => _dao.Eliminar(id);
}
