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
    public partial class PageCalificar : ContentPage
    {
        public PageCalificar()
        {
            InitializeComponent();
        }

        private void btnenviar_Clicked(object sender, EventArgs e)
        {
            int Rating = rating.SelectedStarValue;
            DisplayAlert("Gracias Por Calificarnos", Rating.ToString(), "Ok");
            IsEnabled = false;


        }
    }
}