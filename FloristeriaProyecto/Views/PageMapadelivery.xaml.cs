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
    public partial class PageMapaDelivery : ContentPage
    {
        private double longitude, latitude;
        private string nombrecontacto, telefono;


        public PageMapaDelivery(double latitud, double longitud, string nombre, string celular)
        {
            InitializeComponent();

            txtNombre.Text = nombre.ToString();
            txtCelular.Text = celular.ToString();

            latitude = latitud;
            longitude = longitud;

            nombrecontacto = nombre;

        }

        public PageMapaDelivery()
        {
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
                        Position posicion = new Position(latitude, longitude);

                        Pin pin = new Pin
                        {
                            Label = nombrecontacto,
                            Address = telefono,
                            Type = PinType.Place,
                            Position = posicion
                        };
                        mapa.Pins.Add(pin);

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
            await DisplayAlert(title, message, "OK");
        }

        private void btndelivery_Clicked(object sender, EventArgs e)
        {
            var conecetividad = Connectivity.NetworkAccess;

            if (conecetividad == NetworkAccess.Internet)
            {

                var location = new Xamarin.Essentials.Location(latitude, longitude);
                var options = new MapLaunchOptions
                {
                    Name = txtNombre.ToString(),
                    NavigationMode = NavigationMode.Driving
                };


                Xamarin.Essentials.Map.OpenAsync(location, options);
            }
            
        }

        private void btnLlamar_Clicked(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open(telefono);
            }
            catch (ArgumentNullException anEx)
            {
                Message("Error", "Numero nulo o espacio en blanco");
            }
            catch (FeatureNotSupportedException ex)
            {
                Message("Error", "Phone Dialer no está soportado en este dispositivo");
            }
            catch (Exception ex)
            {
                Message("Error", "" + ex.Message);
            }
        }
    }
}