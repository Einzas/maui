using Microsoft.Maui.Controls;
using speed.ViewModels;

namespace speed.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(); // Establecer el ViewModel
        }
    }
}
