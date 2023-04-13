using Plugin.LocalNotifications;
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
    public partial class PageInicio : Shell
    {
        public PageInicio()
        {
            InitializeComponent();

            CrossLocalNotifications.Current.Show("*Aviso*", "Tienes pedidos pendientes, Revisalos.", 0, DateTime.Now.AddSeconds(1));
        }
    }
}