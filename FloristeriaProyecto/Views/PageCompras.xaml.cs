using FloristeriaProyecto.Modelo;
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
    public partial class PageCompras : ContentPage
    {
        public Compra Site;
        bool editando = false;

        public List<Compra> oListaCompra { get; set; }

        public PageCompras()
        {
            InitializeComponent();
            ObtenerCompra();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (editando)
            {
                editando = false;

                Site = null;
            }
        }

        private async void ObtenerCompra()
        {
            List<Compra> oObjecto = await ApiServiceFirebase.ObtenerCompra();

            if (oObjecto != null)
            {
                oListaCompra = oObjecto;
                ListViewCompra.ItemsSource = oObjecto;

                // await Application.Current.MainPage.DisplayAlert("Lista de Pedidos Llena", "Pedidos encontrados", "OK");
            }

            /*if (oObjecto.Count > 0)
            {
                double longitud = oObjecto[0].oDepacho.longitud;
                await Application.Current.MainPage.DisplayAlert("Alerta", "La longitud es " + longitud, "OK");
            }*/
            else
            {
                await Application.Current.MainPage.DisplayAlert("Lista de Pedidos vacía", "No se encontraron Pedidos", "OK");
            }

        }

        private void listSites_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Site = e.Item as Compra;
            }
            catch (Exception ex)
            {
                Message("Error:", ex.Message);
            }

        }

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }

        private async void ListViewCompra_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var items = e.Item as Compra;

            string result = await DisplayActionSheet("Escoga una opcion", "Cancelar", null, "Ir a la ubicacion del paquete", "Modo Delivery");

            if (result == "Ir a la ubicacion del paquete")
            {
                await Navigation.PushAsync(new Views.PageMapaEstablecimiento());
            }
            else if (result == "Modo Delivery")
            {
                await Navigation.PushAsync(new Views.PageMapaDelivery(items.oDepacho.latitud, items.oDepacho.longitud, items.oDepacho.personaContacto, items.oDepacho.celular));
            }
            else if (result == "Cancelar")
            {
                // Nada
            }
        }
    }
}