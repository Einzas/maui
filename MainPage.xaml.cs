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

			return true;
		}
		catch (Exception)
		{
			// Si ocurre algún error, el token no es válido
			return false;
		}
	}
	private void OnScanBarcodeClicked(object sender, EventArgs e)
	{
		Application.Current.MainPage = new NavigationPage(new BarcodeScannerPage());
	}

	private void Logout(object sender, EventArgs e)
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

