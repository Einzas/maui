using speed.Views;
namespace speed;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await CheckPermissionsAsync();
	}
	private async void OnScanBarcodeClicked(object sender, EventArgs e)
	{
		Application.Current.MainPage = new NavigationPage(new BarcodeScannerPage());
	}

	private async void Logout(object sender, EventArgs e)
	{
		// Eliminar el token almacenado
		Preferences.Remove("jwt_token");

		// Establecer LoginPage como la nueva MainPage
		Application.Current.MainPage = new NavigationPage(new LoginPage());
	}
	private async Task CheckPermissionsAsync()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.Camera>();
		}

		if (status != PermissionStatus.Granted)
		{
			await DisplayAlert("Permiso denegado", "No se ha concedido permiso para usar la cámara.", "OK");
			return;
		}
	}

}

