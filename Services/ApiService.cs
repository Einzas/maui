using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using speed.Models;
using System.Net.Http.Json;
using Refit;
using Newtonsoft.Json;

namespace speed.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient) // Inyección de dependencias
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Opcional: establecer timeout
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new Login
                {
                    Email = email,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("https://new.imporsuitpro.com/Acceso/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    // Aquí puedes manejar el token o datos de sesión
                    return result;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Login fallido: {errorMessage}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Manejar problemas de conectividad, DNS, etc.
                Console.WriteLine($"Error de red: {httpEx.Message}");
                throw new Exception("Error al conectar con el servidor. Por favor, intente más tarde.");
            }
            catch (TaskCanceledException tcEx)
            {
                // Manejar timeout
                Console.WriteLine($"Error: Timeout {tcEx.Message}");
                throw new Exception("La solicitud tomó demasiado tiempo. Verifica tu conexión.");
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro error
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Ocurrió un error al realizar el login. Inténtalo de nuevo.");
            }
        }

        // Método para hacer la solicitud GET y obtener la información de la guía
        public async Task<SpeedData> GetGuiaInfoAsync(string codigo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://guias.imporsuitpro.com/Speed/buscar/{codigo}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(jsonResponse);

                    // Usar Newtonsoft.Json para deserializar
                    var result = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                    Console.WriteLine(result);

                    if (result.Status == 200)
                    {
                        return result.Data;
                    }
                    else
                    {
                        throw new Exception("Error: " + codigo);
                    }
                }
                else
                {
                    throw new Exception("Error en la solicitud");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la guía: {ex.Message}");
                throw;
            }
        }

    }

}

