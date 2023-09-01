using App.Helpers;
using App.Models;
using App.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            App.home = this;
            BindingContext = new HomeViewModel();
            usuario.Text = (string)Application.Current.Properties[VariablesGlobales.USUARIO];

            // this.Master = new Master();
            // this.Detail = new NavigationPage(new MasterDetails());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void AgendarCita_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cita());

        }

        private async void verMisCitas_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerCita());
        }

        private async void llenarEncuesta_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SeguimientoPaciente());
            
        }

        private void cerrarSesin_Tapped(object sender, EventArgs e)
        {
            App.Current.Logout();
        }
    }
}