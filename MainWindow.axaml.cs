using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Controls.Selection;
using Avalonia.Threading;
using InventarioVideojuegosDilanTapia.Datos;
using InventarioVideojuegosDilanTapia.Modelos;
using InventarioVideojuegosDilanTapia.Servicios;

namespace InventarioVideojuegosDilanTapia;

public partial class MainWindow : Window
{
    private readonly VideojuegoServicio _servicio = new();
    private int _idSeleccionado;
    private bool _actualizando;

    public MainWindow()
    {
        InitializeComponent();
        try
        {
            ConexionSqlite.Inicializar();
            Refrescar();
            MostrarEstado("Base de datos lista. Puedes registrar videojuegos.");
        }
        catch (Exception ex)
        {
            MostrarEstado("No se pudo iniciar la base de datos: " + ex.Message);
        }
    }

    private void Refrescar()
    {
        _actualizando = true;
        try
        {
            var registros = _servicio.Consultar(BuscarCaja.Text ?? "");
            TablaVideojuegos.ItemsSource = null;
            TablaVideojuegos.ItemsSource = registros;
            Dispatcher.UIThread.Post(() => TablaVideojuegos.InvalidateVisual(), DispatcherPriority.Background);
            TotalTexto.Text = $"Registros encontrados: {registros.Count}";
        }
        finally { _actualizando = false; }
    }

    private void Guardar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var juego = _servicio.ValidarYCrear(_idSeleccionado, NombreCaja.Text,
                GeneroCaja.Text, PlataformaCaja.Text, PrecioCaja.Text, CantidadCaja.Text);
            if (_idSeleccionado == 0)
            {
                _servicio.Registrar(juego);
                MostrarEstado("Videojuego registrado correctamente.");
            }
            else
            {
                if (!_servicio.Modificar(juego))
                    throw new InvalidOperationException("El videojuego seleccionado ya no existe.");
                MostrarEstado("Videojuego actualizado correctamente.");
            }
            LimpiarFormulario();
            BuscarCaja.Text = "";
            Refrescar();
        }
        catch (Exception ex) { MostrarEstado("Error: " + ex.Message); }
    }

    private void Eliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_idSeleccionado == 0)
        {
            MostrarEstado("Selecciona un videojuego de la tabla antes de eliminar.");
            return;
        }
        try
        {
            if (!_servicio.Eliminar(_idSeleccionado))
                throw new InvalidOperationException("El videojuego ya no existe.");
            LimpiarFormulario();
            Refrescar();
            MostrarEstado("Videojuego eliminado correctamente.");
        }
        catch (Exception ex) { MostrarEstado("Error: " + ex.Message); }
    }

    private void Tabla_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_actualizando || TablaVideojuegos.SelectedItem is not Videojuego juego) return;
        _idSeleccionado = juego.Id;
        IdTexto.Text = $"Editando registro #{juego.Id}";
        NombreCaja.Text = juego.Nombre;
        GeneroCaja.Text = juego.Genero;
        PlataformaCaja.Text = juego.Plataforma;
        PrecioCaja.Text = juego.Precio.ToString(System.Globalization.CultureInfo.InvariantCulture);
        CantidadCaja.Text = juego.Cantidad.ToString();
        GuardarBoton.Content = "Guardar cambios";
        MostrarEstado("Registro seleccionado. Puedes modificar sus campos o eliminarlo.");
    }

    private void Limpiar_Click(object? sender, RoutedEventArgs e)
    {
        LimpiarFormulario();
        MostrarEstado("Formulario limpio. Listo para un nuevo registro.");
    }

    private void LimpiarFormulario()
    {
        _idSeleccionado = 0;
        TablaVideojuegos.SelectedItem = null;
        IdTexto.Text = "Nuevo registro";
        NombreCaja.Text = "";
        GeneroCaja.Text = "";
        PlataformaCaja.Text = "";
        PrecioCaja.Text = "";
        CantidadCaja.Text = "";
        GuardarBoton.Content = "Registrar videojuego";
    }

    private void Buscar_TextChanged(object? sender, TextChangedEventArgs e)
    {
        try { Refrescar(); }
        catch (Exception ex) { MostrarEstado("Error al buscar: " + ex.Message); }
    }

    private void Actualizar_Click(object? sender, RoutedEventArgs e)
    {
        try { Refrescar(); MostrarEstado("Lista actualizada."); }
        catch (Exception ex) { MostrarEstado("Error al consultar: " + ex.Message); }
    }

    private void MostrarEstado(string mensaje) => EstadoTexto.Text = mensaje;
}
