
using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.ViewModels;
using App.WebServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetails : ContentPage
    {
        private ResClient _resClient;
        public MasterDetails()
        {
            InitializeComponent();
            _resClient = new ResClient();
            //ObtenerDatosCategoria();
            //  this.BindingContext = this;
            BindingContext = new MainViewModel();
            BindingContext = new MainPageViewModel(Navigation);
        }

        private async void lsvView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
           // CategoriaModel club = (CategoriaModel)lv.SelectedItem;
          // await Navigation.PushAsync(new CabeceraPedido(club.CatId));


            //App.home.IsPresented = false;
            //await App.home.Navigation.PushAsync(new CabeceraPedido(club.CatId));
        }

        private async void ObtenerDatosCategoria()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);
                var result = await _resClient.Get<RespuestaModel<List<FichaInscripcionModel>>>(VariablesGlobales.URL + "FichaInscripcion/GetList");
                if (result != null && result.Data != null)
                {
                    //CvCandidatas.ItemsSource = result.Data;
                }
                UserDialogs.Instance.HideLoading();
               // imgPrueba.Source = "data:image/jpeg;base64,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";
            }
            catch (Exception ex)
            {

                UserDialogs.Instance.HideLoading();
                await DisplayAlert(VariablesGlobales.ERROR, ex.Message, VariablesGlobales.OK);
            }
           
        }
        private Timer timer;

        public ObservableCollection<Walkthrough> WalkthroughItems { get => Load(); }


        protected override void OnAppearing()
        {
            timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds) { AutoReset = true, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            timer?.Dispose();
            base.OnDisappearing();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*Device.BeginInvokeOnMainThread(() =>
            {

                if (cvWalkthrough.Position == 2)
                {
                    cvWalkthrough.Position = 0;
                    return;
                }

                cvWalkthrough.Position += 1;
            });*/
        }

        private ObservableCollection<Walkthrough> Load()
        {
            return new ObservableCollection<Walkthrough>(new[]
            {
                new Walkthrough
                {
                    Heading ="Mountains",
                    Caption = "Explore the world from your own point of view. Visit mountains and enjoy the freshness of life.",
                    Image = "mountains.png"
                },

                new Walkthrough
                {
                    Heading ="Cities",
                    Caption = "Experience the blue and grey sights of high rise buildings around the world",
                    Image = "Cities.png"
                },

                new Walkthrough
                {
                    Heading ="Ancient",
                    Caption = "Understand the history and heritage of different cultures around the world.",
                    Image = "Ancient.png"
                }
            });
        }
    }

    public class Walkthrough
    {
        public string Heading { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; }
    }
    


}