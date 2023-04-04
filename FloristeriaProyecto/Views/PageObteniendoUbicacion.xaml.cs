using FloristeriaProyecto.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageObteniendoUbicacion : ContentPage
    {
        Ubicacion ubicacion = null;

        public ObservableCollection<Bolsa> oListaGlobalBolsa = new ObservableCollection<Bolsa>();
        public PageObteniendoUbicacion(Ubicacion ubicacion)
        {
            InitializeComponent();
            getLatitudeAndLongitude();
           // oListaGlobalBolsa = oListaBolsa;
            this.ubicacion = ubicacion;

        }

     /*   public PageObteniendoUbicacion(Ubicacion ubicacion)
        {
            this.ubicacion = ubicacion;
        }*/

        private async void getLatitudeAndLongitude()
        {
            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var localizacion = await Geolocation.GetLocationAsync();
                    txtLatitude.Text = Math.Round(localizacion.Latitude, 5) + "";
                    txtLongitude.Text = Math.Round(localizacion.Longitude, 5) + "";
                }
                else
                {
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception e)
            {

                if (e.Message.Equals("Location services are not enabled on device."))
                {

                    Message("Error", "Servicio de localizacion no encendido");
                }
                else
                {
                    Message("Error", e.Message);
                }

            }
        }
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                Message("Advertencia", "Actualmente no cuenta con acceso a internet");
                return;
            }

            if (string.IsNullOrEmpty(txtLatitude.Text) || string.IsNullOrEmpty(txtLongitude.Text))
            {
                Message("Aviso", "Aun no se obtiene la ubicacion");
                getLatitudeAndLongitude();
                return;
            }


            Ubicacion oUbicacion = new Ubicacion()
            {
                personaContacto = txtPersonaContacto.Text,
                latitud = double.Parse(txtLatitude.Text),
                longitud = double.Parse(txtLongitude.Text)
            };

            Compra oCompra = new Compra()
            {
                fechaCompra = DateTime.Now.ToString("dd/MM/yyyy"),
                tipoEntrega = "ubicacion",
                oListaBolsa = oListaGlobalBolsa,
               // oUbicacion = oUbicacion,
                oTienda = null,
                oDepacho = null
            };

            await Navigation.PushAsync(new PagePago(oCompra));



        }

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }
    }
}