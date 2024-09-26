namespace speed;

using speed.Views;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Verificar si el token existe
		var token = Preferences.Get("jwt_token", string.Empty);

		if (!string.IsNullOrEmpty(token))
		{
			// Token existe, redirigir a la página principal
			MainPage = new NavigationPage(new MainPage()); // Asegúrate de tener creada esta página
		}
		else
		{
			// No hay token, redirigir a la página de login
			MainPage = new NavigationPage(new LoginPage()); // Asegúrate de tener creada esta página
		}
	}
}
