using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePerfil : ContentPage
    {
        public Usuario Usuarios;
        Double latitud;
        Double longitud;

        byte[] imagenNueva;
        MediaFile FileFoto = null;

        static byte[] imagenOriginal;

        public PagePerfil()
        {
            InitializeComponent();
            obtenerUsuario();
        }

        Usuario oGlobalUsuario;

        private async void BtnGuardarCambios_Clicked(object sender, EventArgs e)
        {

            if (imagenNueva == imagenOriginal)
            {
                imagenNueva = imagenOriginal;
            }
            else
            {
                imagenNueva = imagenNueva;
            }

            if (txtNombre.Text.Trim() == "" || txtApellido.Text.Trim() == "" || txtDocumento.Text.Trim() == "")
            {
                await DisplayAlert("Mensaje", "Debe completar todos los campos", "OK");
                return;
            }

            var pregunta = await DisplayAlert("Aviso", "¿Seguro que desea realizar los cambios?", "Si", "Cancelar");
            
            if (pregunta == true)
            {
                Usuario oUsuario = new Usuario()
                {
                    Nombres = txtNombre.Text,
                    Apellidos = txtApellido.Text,
                    Documento = txtDocumento.Text,
                    Image = imagenNueva,
                    Clave = oGlobalUsuario.Clave,
                    Email = oGlobalUsuario.Email,

                    latitud = oGlobalUsuario.latitud,
                    longitud = oGlobalUsuario.longitud

                };

                bool respuesta = await ApiServiceFirebase.GuardarCambiosUsuario(oUsuario);

                if (respuesta)
                {
                    await DisplayAlert("Mensaje", "Se guardaron los cambios con exito", "OK");
                }
                else
                {
                    await DisplayAlert("Mensaje", "Hubo un error al guardar los cambios", "OK");
                }
            }
            else
            {
                //false conditon
            }
        }

        private async void obtenerUsuario()
        {

            oGlobalUsuario = await ApiServiceFirebase.ObtenerUsuario();

            if (oGlobalUsuario != null)
            {
                imagenOriginal = oGlobalUsuario.Image;
                imagenNueva = oGlobalUsuario.Image;
                Imagen.Source = GetImageResourseFromBytes(oGlobalUsuario.Image);

                txtNombre.Text = oGlobalUsuario.Nombres;
                txtApellido.Text = oGlobalUsuario.Apellidos;
                txtDocumento.Text = oGlobalUsuario.Documento;
                txtEmail.Text = oGlobalUsuario.Email;

                latitud = oGlobalUsuario.latitud;
                longitud = oGlobalUsuario.longitud;

            }
        }

        private ImageSource GetImageResourseFromBytes(byte[] bytes)
        {
            ImageSource retSource = null;

            if (bytes != null)
            {
                byte[] imageAsBytes = (byte[])bytes;
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }

            return retSource;
        }

        private void BtnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new PageLogin();
        }

        private async void btnModificarFoto_Clicked(object sender, EventArgs e)
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
                    imagenNueva = File.ReadAllBytes(FileFoto.Path);
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
                    imagenNueva = File.ReadAllBytes(FileFoto.Path);
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
        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
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

        private async void VerUbicacionTap_Tapped(object sender, EventArgs e)
        {
            var status = await DisplayAlert("Aviso", $"¿Desea ir a la ubicacion indicada?", "SI", "NO");

            if (status)
            {
                await Navigation.PushModalAsync(new PageMapa(longitud, latitud));
            }

        }
    }
}