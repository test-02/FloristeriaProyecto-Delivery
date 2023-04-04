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
    public partial class PageCambiarPassword : ContentPage
    {
        ApiServiceAuthentication _userRepository = new ApiServiceAuthentication();
        public PageCambiarPassword()
        {
            InitializeComponent();
        }

        

        private async void btnCambiarPassword_Clicked(object sender, EventArgs e)
        {

            try
            {
                string password = txtPassword.Text;
                string confirmPass = txtConfirmar.Text;
                if (string.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Change Password", "Please type password", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(confirmPass))
                {
                    await DisplayAlert("Change Password", "Please type confirm password", "Ok");
                    return;
                }
                if (password != confirmPass)
                {
                    await DisplayAlert("Change Password", "Confirm password not match.", "Ok");
                    return;
                }
                string token = Preferences.Get("token", "");
                string newToken = await _userRepository.ChangePassword(token, password);
                if (!string.IsNullOrEmpty(newToken))
                {
                    await DisplayAlert("Change Password", "Password has been changed.", "Ok");
                    Preferences.Set("token", newToken);
                    //Preferences.Clear();
                    //await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Change Password", "Password Change failed.", "Ok");
                }
            }
            catch (Exception exception)
            {

            }

        }
    }
}