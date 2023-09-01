
using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.ViewModels;
using App.WebServices;
using System;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecuperarContrasena : ContentPage
    {
        public RecuperarContrasena()
        {
            InitializeComponent();
            BindingContext = new RecuperarContrasenaViewModel(Navigation);
        }

        private async void btnRecuperarContrasena_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RecuperarContrasena());
        }
    }
}