using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotas.Modelo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using System.IO;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRegistro : ContentPage
    {

        ApiServiceAuthentication _userRepository = new ApiServiceAuthentication();
        byte[] Image;
        MediaFile FileFoto = null;
        
        public PageRegistro()
        {
            InitializeComponent();
            getLatitudeAndLongitude();
        }
        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtLatitud.Text) || string.IsNullOrEmpty(txtLongitud.Text))
            {
                Message("Aviso", "Aun no se obtiene la ubicacion");
                getLatitudeAndLongitude();
                return;
            }

            if (Image == null)
            {
                Message("Aviso", "Aun no se a agregado una foto.");
                return;
            }

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                Message("Aviso", "Debe escribir su nombre");
                return;
            }

            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                Message("Aviso", "Debe escribir su apellido");
                return;
            }

            if (string.IsNullOrEmpty(txtDocumento.Text))
            {
                Message("Aviso", "Debe escribir su ID");
                return;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                Message("Aviso", "Debe escribir un Email");
                return;
            }

            if (string.IsNullOrEmpty(txtContrasena.Text))
            {
                Message("Aviso", "Debe escribir una Contraseña");
                return;
            }

            if (txtContrasena.Text.Length < 5)
            {
                Message("Aviso", "Debe escribir una Contraseña mayor a 5 digitos");

                return;
            }
           

            Usuario oUsuario = new Usuario()
            {
                Nombres = txtNombre.Text,
                Apellidos = txtApellido.Text,
                Documento = txtDocumento.Text,
                Email = txtEmail.Text,
                Image = Image,
                Clave = txtContrasena.Text,
                latitud = double.Parse(txtLatitud.Text),
                longitud = double.Parse(txtLongitud.Text)
            };

            bool respuesta = await ApiServiceAuthentication.RegistrarUsuario(oUsuario);

            if (respuesta)
            {
                await DisplayAlert("Correcto", "Usuario registrado", "Aceptar");
                await Navigation.PopModalAsync();



            }
            else
            {
                await DisplayAlert("Oops", "No se pudo registrar", "Aceptar");
            }
            
        }

        private void TapBackArrow_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

       

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {

            bool response = await Application.Current.MainPage.DisplayAlert("Advertencia", "Realizar la opción mendiante: ", "Camara", "Galeria");

            if (response)
                GetImageFromCamera();
            else
                GetImageFromGallery();


        }

        private async void GetImageFromCamera()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    FileFoto = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                        SaveToAlbum = true
                    });

                    if (FileFoto == null)
                        return;

                    Imagen.Source = ImageSource.FromStream(() => { return FileFoto.GetStream(); });
                    Image = File.ReadAllBytes(FileFoto.Path);
                }
                catch (Exception)
                {
                    Message("Advertencia", "Se produjo un error al tomar la fotografia.");
                }
            }
            else
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }


        }

        private async void GetImageFromGallery()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    var FileFoto = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                    });
                    if (FileFoto == null)
                        return;

                    Imagen.Source = ImageSource.FromStream(() => { return FileFoto.GetStream(); });
                    Image = File.ReadAllBytes(FileFoto.Path);
                }
                else
                {
                    Message("Advertencia", "Se produjo un error al seleccionar la imagen");
                }
            }
            catch (Exception)
            {
                Message("Advertencia", "Se produjo un error al seleccionar la imagen");
            }

        }



        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();

                    stream.CopyTo(memory);

                    return memory.ToArray();
                }
            }

            return null;
        }

        private async void getLatitudeAndLongitude()
        {
            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var localizacion = await Geolocation.GetLocationAsync();
                    txtLatitud.Text = Math.Round(localizacion.Latitude, 5) + "";
                    txtLongitud.Text = Math.Round(localizacion.Longitude, 5) + "";
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

    }
}