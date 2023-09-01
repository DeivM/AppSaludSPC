using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.Views;
using App.WebServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class VerCitaViewModel : BaseViewModel
    {
        private ResClient _resClient;

        public ICommand ButtonCommand { get; }
        public INavigation Navigation { get; set; }
        private ObservableCollection<CitaModel> _listCita;
        private string _observaciones = string.Empty;
        public VerCitaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ButtonCommand = new Command(async () => await AnularCita());

            _resClient = new ResClient();
            LoadData();
        }
        public ObservableCollection<CitaModel> ListCita
        {
            get => _listCita;
            private set
            {
                if (value != _listCita)
                {
                    _listCita = value;
                    OnPropertyChanged(nameof(ListCita));
                }
            }

        }


        private ListModel _selectedCita;
        public ListModel SelectedCita
        {
            get => _selectedCita;
            set
            {
                if (value != null)
                    SetObservableProperty(ref _selectedCita, value);
            }
        }


        public async Task LoadData()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);
                IDictionary<string, string> data = new Dictionary<string, string>();
                var result = await _resClient.Get<List<CitaModel>>(VariablesGlobales.URL + "Cita/GetAllById", data);
                ListCita = new ObservableCollection<CitaModel>(result);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, ex.Message, VariablesGlobales.CERRAR);

            }
        }

        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                if (_observaciones != value)
                {
                    _observaciones = value;
                    OnPropertyChanged(nameof(Observaciones));
                }
            }
        }

        async Task AnularCita()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);
                var lista = new List<CitaModel>();
                foreach (var item in ListCita)
                {
                    item.CitObservaciones = string.IsNullOrEmpty(item.CitObservaciones) ? string.Empty : item.CitObservaciones;
                    string a = string.IsNullOrEmpty(item.CitObservaciones) ? "1" : "0";
                    item.CitEstado = Convert.ToInt16(a);
                    lista.Add(item);
                }

                var dataToken = await _resClient.Post<RespuestaModel<object>, List<CitaModel>>(VariablesGlobales.URL + "Cita/PostList", lista);

                if (dataToken.Codigo != System.Net.HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, dataToken.Message[0], VariablesGlobales.CERRAR);
                }

                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.EXITO, "Cita anulada correctamente", VariablesGlobales.CERRAR);
              await  LoadData();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, ex.Message, VariablesGlobales.CERRAR);

            }
        }



    }


}
