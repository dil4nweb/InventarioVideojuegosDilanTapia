namespace InventarioVideojuegosDilanTapia.Modelos;

public class Videojuego
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Genero { get; set; }
    public string Plataforma { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }

    public Videojuego(int id, string nombre, string genero, string plataforma, decimal precio, int cantidad)
    {
        Id = id;
        Nombre = nombre;
        Genero = genero;
        Plataforma = plataforma;
        Precio = precio;
        Cantidad = cantidad;
    }
}
