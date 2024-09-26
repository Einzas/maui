using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace speed.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string email;
        private string password;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        private async Task ExecuteLoginCommand()
        {
            try
            {
                var loginData = new
                {
                    correo = Email,
                    contrasena = Password
                };

                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync("https://new.imporsuitpro.com/Acceso/login", content);

                    // Verifica si la petición fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserializa la respuesta del servidor
                        var result = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

                        // Guardar token en Preferences
                        Preferences.Set("jwt_token", result.Token);

                        // Almacenar información del usuario
                        Preferences.Set("user_name", result.Data.NombreUsers);

                        // Mostrar mensaje de éxito
                        await Application.Current.MainPage.DisplayAlert("Login Success", "Inicio de sesión exitoso", "OK");

                        // Redirigir a la página principal o la vista deseada después del login
                        Application.Current.MainPage = new MainPage(); // Asegúrate de tener esta página creada
                    }
                    else
                    {
                        // Si la petición no fue exitosa, muestra el código de error en un mensaje emergente
                        await Application.Current.MainPage.DisplayAlert("Login Failed", $"Error: {response.StatusCode}", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja errores de conexión u otros errores y muestra un mensaje emergente
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo conectar al servidor. " + ex.Message, "OK");
            }
        }
    }
}
