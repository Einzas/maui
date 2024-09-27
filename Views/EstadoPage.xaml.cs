using speed.Models;

namespace speed.Views;

public partial class EstadoPage : ContentPage
{
    public EstadoPage(SpeedData speedData)
    {
        InitializeComponent();
        this.LlenarDatos(speedData);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //await CheckPermissionsAsync();

        // Validar el token antes de proceder
        await ValidateTokenAsync();
    }

    private async Task ValidateTokenAsync()
    {
        var token = Preferences.Get("jwt_token", string.Empty);

        if (string.IsNullOrEmpty(token))
        {
            // Si no hay token, redirigir a la página de Login
            await DisplayAlert("Sesión expirada", "Debe iniciar sesión nuevamente.", "OK");
            Application.Current.MainPage = new NavigationPage(new LoginPage());

            return;
        }

        // Aquí podrías agregar la lógica para validar el token
        if (!IsValidToken(token))
        {
            await DisplayAlert("Sesión inválida", "El token ha expirado o es inválido.", "OK");
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    private bool IsValidToken(string token)
    {
        // Ejemplo simple para decodificar y validar el token (podrías usar una librería de JWT si es necesario)
        try
        {
            // Aquí puedes agregar una lógica para validar el token
            // Como por ejemplo, decodificarlo y revisar si está expirado
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            if (jwtToken == null)
                return false;

            // Obtener la fecha de expiración del token
            var expirationDate = jwtToken.ValidTo;

            // Verificar si el token ha expirado
            if (expirationDate < DateTime.UtcNow)
                return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al validar el token: {ex.Message}");
            return false;
        }

        return true;
    }

    //private async Task CheckPermissionsAsync()
    //{
    //    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

    //    if (status != PermissionStatus.Granted)
    //    {
    //        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

    //        if (status != PermissionStatus.Granted)
    //        {
    //            await DisplayAlert("Permisos requeridos", "La aplicación necesita permisos de ubicación para funcionar correctamente.", "OK");
    //            await CheckPermissionsAsync();
    //        }
    //    }
    //}

    private void OnScanBarcodeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new BarcodeScannerPage());
    }

    private void OnInicioClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

    private async void OnEstadoChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        string selectedEstado = picker.SelectedItem.ToString();

        switch (selectedEstado)
        {
            case "En Transito":
                // Abrir modal con campo para URL y botón para enviar
                await MostrarModalUrl();
                break;
            case "Entregado":
            case "Devolución":
                // Abrir modal para seleccionar imagen desde cámara o galería
                await MostrarModalImagen();
                break;
            case "Novedad":
                // Abrir modal con opciones adicionales y selección de imagen
                await MostrarModalNovedad();
                break;
            default:
                // No hacer nada en otros casos
                break;
        }
    }

    private async Task MostrarModalUrl()
    {
        string url = await DisplayPromptAsync("URL", "Ingrese la URL:");
        if (!string.IsNullOrEmpty(url))
        {
            // Realiza la acción con la URL proporcionada
            await DisplayAlert("Enviado", "URL enviada correctamente", "OK");
        }
    }
    private async Task MostrarModalImagen()
    {
        string action = await DisplayActionSheet("Seleccione una opción", "Cancelar", null, "Cámara", "Galería");

        if (action == "Cámara")
        {
            // Lógica para capturar imagen desde la cámara
            await DisplayAlert("Cámara", "Capturar imagen desde la cámara", "OK");
        }
        else if (action == "Galería")
        {
            // Lógica para seleccionar imagen desde la galería
            await DisplayAlert("Galería", "Seleccionar imagen desde la galería", "OK");
        }
    }
    private async Task MostrarModalNovedad()
    {
        // Opciones para el select adicional
        string opcionNovedad = await DisplayActionSheet("Razón de la novedad", "Cancelar", null,
            "Cliente No desea Recibir", "No Contesta", "Mal Direccionado");

        // Una vez seleccionada la opción, mostrar opción para imagen
        if (!string.IsNullOrEmpty(opcionNovedad))
        {
            await MostrarModalImagen();
        }
    }

    private void LlenarDatos(SpeedData speedData)
    {
        lblGuia.Text = string.IsNullOrEmpty(speedData.Guia) ? "No disponible" : speedData.Guia;
        lblCliente.Text = string.IsNullOrEmpty(speedData.NombreDestino) ? "No disponible" : speedData.NombreDestino;
        lblDireccion.Text = string.IsNullOrEmpty(speedData.DireccionDestino) ? "No disponible" : speedData.DireccionDestino;
        lblTelefono.Text = string.IsNullOrEmpty(speedData.TelefonoDestino) ? "No disponible" : speedData.TelefonoDestino;
        lblContiene.Text = string.IsNullOrEmpty(speedData.Contiene) ? "No disponible" : speedData.Contiene;

        if (speedData.Estado == "1") lblEstado.Text = "Nuevo";
        if (speedData.Estado == "2") lblEstado.Text = "Generado";
        if (speedData.Estado == "3") lblEstado.Text = "En Transito";
        if (speedData.Estado == "7") lblEstado.Text = "Entregado";
        if (speedData.Estado == "9") lblEstado.Text = "Devolución";
        if (speedData.Estado == "14") lblEstado.Text = "Novedad";
        // Si deseas mostrar el monto a recibir solo si hay recaudo
        if (speedData.Recaudo == "1" && !string.IsNullOrEmpty(speedData.MontoFactura))
        {
            lblMonto.Text = $"${speedData.MontoFactura}";
        }
        else
        {
            lblMonto.Text = "Sin monto a recaudar";
        }
    }

}