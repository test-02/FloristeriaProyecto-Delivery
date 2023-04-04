using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogin : ContentPage
    {
        ApiServiceAuthentication _userRepository = new ApiServiceAuthentication();

        public PageLogin()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.Properties.ContainsKey("email"))
            {
                txtEmail.Text = Application.Current.Properties["email"] as string;
            }

            if (Application.Current.Properties.ContainsKey("password"))
            {
                txtContrasena.Text = Application.Current.Properties["password"] as string;
            }
        }

        private async void BtnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            if (txtContrasena.Text.Trim().Equals("") || txtEmail.Text.Trim().Equals(""))
            {
                await DisplayAlert("Error", "Ingrese todos los datos", "Aceptar");
                return;
            }

            UserAuthentication oUsuario = new UserAuthentication()
            {
                email = txtEmail.Text,
                password = txtContrasena.Text,
                returnSecureToken = true
            };

            bool respuesta = await ApiServiceAuthentication.Login(oUsuario);
            if (respuesta)
            {

                Application.Current.MainPage = new PageInicio();
            }
            else
            {
                await DisplayAlert("Error", "Usuario o Password incorrectos", "OK");


            }
        }

        private void BtnRegistrarse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PageRegistro());
        }

        private void ForgotTap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Views.PageRestaurarContraseña());
        }

        private void checkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(checkBox.IsChecked==true)
            {
                persistenciaDatos();
            }
        }
        private async void persistenciaDatos()
        {
            string email = txtEmail.Text;
            string password = txtContrasena.Text;

            Application.Current.Properties["email"] = email;
            Application.Current.Properties["password"] = password;

            await Application.Current.SavePropertiesAsync();
        }
    }
}