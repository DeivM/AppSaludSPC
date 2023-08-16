using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.Views;
using App.WebServices;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ResClient _resClient;
        private UsuarioModel _datosUsuario;
        private ObservableCollection<ListModel> _lstHomeViewModel = new ObservableCollection<ListModel>();


        //  public ObservableCollection<AnimalGroup> _animals { get; private set; } = new ObservableCollection<AnimalGroup>();

        public ObservableCollection<HomeViewModelGroup> _alimentosG { get; private set; } = new ObservableCollection<HomeViewModelGroup>();
        public UsuarioModel DatosUsuario
        {
            get { return _datosUsuario; }
            set { _datosUsuario = value; OnPropertyChanged(); }
        }

        public ObservableCollection<HomeViewModelGroup> HomeViewModelG
        {
            get => _alimentosG;
            set
            {
                _alimentosG = value;
                RaisePropertyChanged();
            }
        }



        public List<HomeViewModelGroup> SelectedItem { get; set; }
        public ICommand NavigateCommand { get; }
        public INavigation Navigation { get; set; }

        public HomeViewModel()
        {
            _resClient = new ResClient();
            //Navigation = navigation;
           // NavigateCommand = new Command<Type>(async (pageType) => await GoToDetails(pageType));
            ObtenerDatos();
        }

        private async void ObtenerDatos()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);
                var result = await _resClient.Get<RespuestaModel<ObservableCollection<ComidaModel>>>(VariablesGlobales.URL + "Comida/GetListComidas");
                if (result != null && result.Data != null)
                {

                    foreach (var item in result.Data)
                    {
                        HomeViewModelG.Add(new HomeViewModelGroup( item.ComDiaNr.Value, item.detalle));
                       
                    }
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.ERROR, ex.Message, VariablesGlobales.CERRAR);
            }


        }
        async Task GoToDetails(Type pageType)
        {

            //if (SelectedItem != null)
            //{
                var page = (Page)Activator.CreateInstance(pageType);
                //DatosUsuario.MejId = SelectedItem.id;
              /*  page.BindingContext = new InformacionBasicaViewModel(Navigation)
                {
                    DatosUsuario = DatosUsuario
                };*/
                await Navigation.PushAsync(page);
            //}
        }
    }


    /*public class HomeViewModel
    {
        public long ComId { get; set; }
        public long PlaId { get; set; }
        public long AliId { get; set; }
        public short? ComDiaNr { get; set; }
        public string ComComida { get; set; }
        public string ComAlimentoNombre { get; set; }
        public bool? Calorias { get; set; }
    }*/
    public class HomeViewModelGroup : List<ComidaModel>
    {
        public long ComId { get; set; }
        public long PlaId { get; set; }
        public long AliId { get; set; }
        public short? ComDiaNr { get; set; }
        public string ComComida { get; set; }
        public string ComAlimentoNombre { get; set; }
        public bool? Calorias { get; set; }

        public HomeViewModelGroup(short _name, List<ComidaModel> animals) : base(animals)
        {
 
            ComDiaNr = _name;

        }
    }




}
