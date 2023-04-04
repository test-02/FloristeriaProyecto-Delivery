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
    public partial class PageMapa : ContentPage
    {
        private double longitude, latitude;

        public PageMapa(double longitud, double latitud)
        {
            InitializeComponent();

            txtLongitude.Text = Convert.ToString(longitud);
            txtLatitude.Text = Convert.ToString(latitud);

            longitude = longitud;
            latitude = latitud;
        }

        public PageMapa()
        {
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Chincheta();

            /*var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {

                var localizacion = CrossGeolocator.Current;

                if (localizacion != null)
                {
                    localizacion.PositionChanged += Localizacion_PositionChanged;

                    if (!localizacion.IsListening)
                    {
                        await localizacion.StartListeningAsync(TimeSpan.FromSeconds(10), 100);
                    }

                    var posicion = await localizacion.GetPositionAsync();
                    var centromapa = new Position(posicion.Latitude, posicion.Longitude);
                    mapa.MoveToRegion(new MapSpan(centromapa, 1, 1));
                }
                else
                {
                    var posicion = await localizacion.GetLastKnownLocationAsync();
                    var centromapa = new Position(posicion.Latitude, posicion.Longitude);
                    mapa.MoveToRegion(new MapSpan(centromapa, 1, 1));
                }
            }*/

        }

        private void Localizacion_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var centromapa = new Position(e.Position.Latitude, e.Position.Longitude);
            mapa.MoveToRegion(new MapSpan(centromapa, 1, 1));
        }
        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }


        /*private async void getLatitudeAndLongitude()
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
        /*

       
        /*private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            var status = await DisplayAlert("Aviso", $"¿Desea Guardar su ubicacion Actual?", "SI", "NO");

            if (status)
            {

                if (string.IsNullOrEmpty(txtLatitude.Text) || string.IsNullOrEmpty(txtLongitude.Text))
                {
                    Message("Aviso", "Aun no se obtiene la ubicacion");
                    getLatitudeAndLongitude();
                    return;
                }


                Usuario oUsuario = new Usuario()
                {
                    Nombres = oGlobalUsuario.Nombres,
                    Apellidos = oGlobalUsuario.Apellidos,
                    Documento = oGlobalUsuario.Documento,
                    Image = oGlobalUsuario.Image,
                    Clave = oGlobalUsuario.Clave,
                    Email = oGlobalUsuario.Email,
                    latitud = double.Parse(txtLatitude.Text),
                    longitud = double.Parse(txtLongitude.Text)

                };

                bool respuesta = await ApiServiceFirebase.GuardarCambiosUsuario(oUsuario);

                if (respuesta)
                {
                    await DisplayAlert("Mensaje", "Se guardaron los cambios", "OK");
                }
                else
                {
                    await DisplayAlert("Mensaje", "Hubo un error al guardar", "OK");
                }
            }

        }*/

        private async void Nada()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var localizacion = await Geolocation.GetLocationAsync();

                    if (localizacion != null)
                    {
                        var pin = new Pin()
                        {
                            Type = PinType.SearchResult,
                            Position = new Position(latitude, longitude),

                        };

                        mapa.Pins.Add(pin);
                        mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromMeters(100)));
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
                            Label = "Mi Ubicacion",
                            Address = "",
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
    }
}