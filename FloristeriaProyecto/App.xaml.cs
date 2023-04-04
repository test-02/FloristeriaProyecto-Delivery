
using FloristeriaProyecto.Views;
using Rating;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FloristeriaProyecto
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
       
        }

        protected override void OnStart()
        {
            Iniciar();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            Debug.WriteLine("OnResume");

        }

        public async void Iniciar()
        {
            if ((Preferences.Get("Remember", true) == true))
            {
                MainPage = new PageLogin();
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(PageInicio)}");
            }
        }
    }
}
