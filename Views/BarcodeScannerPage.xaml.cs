using speed.Services;
using ZXing.Net.Maui;

namespace speed.Views
{

    public partial class BarcodeScannerPage : ContentPage
    {
        private readonly ApiService _apiService;
        public BarcodeScannerPage()
        {
            InitializeComponent(); // Mueve esto al inicio
            _apiService = new ApiService(new HttpClient());

            try
            {
                barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
                {
                    Formats = ZXing.Net.Maui.BarcodeFormat.Code128,
                    AutoRotate = true,
                    Multiple = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar el lector de códigos: {ex.Message}");
            }
        }

        private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            try
            {
                var first = e.Results?.FirstOrDefault();
                if (first != null)
                {
                    string codigoGuia = first.Value;

                    // Obtener la información del código de barras
                    var guiaInfo = await _apiService.GetGuiaInfoAsync(codigoGuia);

                    // Actualiza la interfaz de usuario en el hilo principal
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        Application.Current.MainPage = new NavigationPage(new EstadoPage(guiaInfo));
                    });
                }
            }
            catch (HttpRequestException httpEx)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Error de Red", $"No se pudo conectar con el servidor: {httpEx.Message}", "OK");
                });
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}\n{ex.InnerException?.Message}", "OK");
                });
            }
        }

        private void OnInicioClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

    }
}
