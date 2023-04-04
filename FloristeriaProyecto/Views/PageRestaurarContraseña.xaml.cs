using FloristeriaProyecto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRestaurarContraseña : ContentPage
    {
        ApiServiceAuthentication _userRepository = new ApiServiceAuthentication();
        public PageRestaurarContraseña()
        {
            InitializeComponent();
        }

        private async void ButtonEnviarLink_Clicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Advertencia", "Favor ingresa tu email valido.", "Ok");
                return;
            }

            bool isSend = await _userRepository.ResetPassword(email);
            if (isSend)
            {
                await DisplayAlert("Cambiando Contraseña", "Hemos enviado un email a tu correo para restablecer tu contraseña.", "Ok");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Advertencia", "No se encontro la direccion de correo electronico.", "Ok");
            }

        }

        private void VolverTap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Views.PageLogin());

        }
    }
}