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
       

    }
}