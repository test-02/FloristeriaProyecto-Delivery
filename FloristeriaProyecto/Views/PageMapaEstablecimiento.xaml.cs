using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using System.Collections.ObjectModel;
using Plugin.Media.Abstractions;
using System.Data;
using Plugin.Geolocator;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMapaEstablecimiento : ContentPage
    {
        private double latitud = 15.496807;
        private double longitud = -88.036075;

        public PageMapaEstablecimiento()
        {
            InitializeComponent();
            
            txtLatitude.Text = Convert.ToString(latitud);
            txtLongitude.Text = Convert.ToString(longitud);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Chincheta();
        }

        private void Localizacion_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var centromapa = new Position(e.Position.Latitude, e.Position.Longitude);
            mapa.MoveToRegion(new MapSpan(centromapa, 1, 1));
        }

        private async void Chincheta()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var localizacion = await Geolocation.GetLocationAsync();

                    if (localizacion != null)
                    {
                        Position posicion = new Position(latitud, longitud);

                        Position posicion2 = new Position(localizacion.Latitude, localizacion.Longitude);

                        Pin pin = new Pin
                        {
                            Label = "Margaritas Floristeria",
                            Address = "Empresa",
                            Type = PinType.Place,
                            Position = posicion
                        };
                        mapa.Pins.Add(pin);

                        Pin pin2 = new Pin
                        {
                            Label = "Mi Ubicacion",
                            Address = "Lugar",
                            Type = PinType.Place,
                            Position = posicion2
                        };
                        mapa.Pins.Add(pin2);


                        mapa.MoveToRegion(MapSpan.FromCenterAndRadius(posicion, Distance.FromMeters(100)));
                    }
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

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "Aceptar");
        }

        private void btndelivery_Clicked(object sender, EventArgs e)
        {
            var conecetividad = Connectivity.NetworkAccess;

            if (conecetividad == NetworkAccess.Internet)
            {

                var location = new Xamarin.Essentials.Location(latitud, longitud);
                var options = new MapLaunchOptions
                {
                    Name = "Margaritas Floristeria",
                    NavigationMode = NavigationMode.Driving
                };


                Xamarin.Essentials.Map.OpenAsync(location, options);
            }
        }
    }
}