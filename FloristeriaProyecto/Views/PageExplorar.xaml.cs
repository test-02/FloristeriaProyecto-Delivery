using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageExplorar : ContentPage
    {

        public List<Categoria> oListaCategoria = new List<Categoria>();
        public ICommand CarouselItemTapped{ get; set; }

        public PageExplorar()
        {
            InitializeComponent();
            obtenerCategorias();
            CarouselViewCategorias_ItemTapped();
        }

        private async void obtenerCategorias()
        {
            Dictionary<string, Categoria> oObjeto = new Dictionary<string, Categoria>();
            oObjeto = await ApiServiceFirebase.ObtenerCategorias();

            List<Categoria> oListaTemp = new List<Categoria>();

            if (oObjeto.Count > 0)
            {
                foreach (KeyValuePair<string, Categoria> item in oObjeto)
                {
                    oListaTemp.Add(new Categoria()
                    {
                        idcategoria = item.Key,
                        nombre = item.Value.nombre,
                        imagen = item.Value.imagen,

                    });
                }
                oListaCategoria = oListaTemp;
            }

            CarouselViewCategorias.ItemsSource = oListaCategoria;
        }

        private void CarouselViewCategorias_ItemTapped()
        {
            CarouselItemTapped = new Xamarin.Forms.Command(async (selectItem) => {
                Categoria oCategoria = (Categoria)selectItem;
                await Navigation.PushAsync(new PageProductos(oCategoria.idcategoria));
            });
        }
    }
}