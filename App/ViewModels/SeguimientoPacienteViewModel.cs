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
    public class SeguimientoPacienteViewModel : BaseViewModel
    {
        private ResClient _resClient;
        private List<SeguimientoPacienteModel> DatosSeguimientoPaciente=new List<SeguimientoPacienteModel>();

        private bool _estadopaciente = false;
        private string _observaciones = string.Empty;
        public ICommand ButtonCommand { get; }
        public INavigation Navigation { get; set; }
        private ObservableCollection<SeguimientoPacienteModel> _listSeguimientoPaciente;
        public SeguimientoPacienteViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ButtonCommand = new Command(async () => await Guardar());
            _resClient = new ResClient();
            LoadData();
        }
        public ObservableCollection<SeguimientoPacienteModel> ListSeguimientoPaciente
        {
            get=> _listSeguimientoPaciente;
            private set
            {
                if (value!= _listSeguimientoPaciente)
                {
                    _listSeguimientoPaciente = value;
                    OnPropertyChanged(nameof(ListSeguimientoPaciente));
                }
            }

            //set { SetObservableProperty(ref _listSeguimientoPaciente, value); }
        }


        private ListModel _selectedSeguimientoPaciente;
        public ListModel SelectedSeguimientoPaciente
        {
            get =>  _selectedSeguimientoPaciente;
            set
            {
                if (value != null)
               // LoadMedico(value.id);
                SetObservableProperty(ref _selectedSeguimientoPaciente, value);
            }
        }




       


        public bool EstadoPaciente
        {
            get { return _estadopaciente; }
            set
            {
                if (_estadopaciente != value)
                {
                    _estadopaciente = value;
                    OnPropertyChanged(nameof(EstadoPaciente));
                }
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


        public async Task LoadData()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);

                IDictionary<string, string> data = new Dictionary<string, string>();
          //  data.Add("id", "0");
            var result = await _resClient.Get<List<SeguimientoPacienteModel>>(VariablesGlobales.URL + "SeguimientoPaciente/GetAllById", data);
            ListSeguimientoPaciente = new ObservableCollection<SeguimientoPacienteModel>(result);
                if (ListSeguimientoPaciente.Count==0)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, "No tiene datos para mostrar", VariablesGlobales.CERRAR);
                
                 }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, ex.Message, VariablesGlobales.CERRAR);

            }
        }



        async Task Guardar()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);
                var lista = new List<SeguimientoPacienteModel>();
                foreach (var item in ListSeguimientoPaciente)
                {
                    item.SepObservacion = string.IsNullOrEmpty(item.SepObservacion) ? string.Empty : item.SepObservacion;
                    lista.Add(item);
                }
                //validar si necesita agendar una cita
                decimal obterSi= lista.Where(x=>x.SepFinalizar).Count();
                decimal obtenerTotal=lista.Count();
                decimal obtenerResultado = (obterSi / obtenerTotal);
                decimal porcentaje = obtenerResultado * 100;

                //DatosSeguimientoPaciente.EspId = SelectedSeguimientoPaciente.id;
                //DatosSeguimientoPaciente.MesId = SelectedMedico.id;
                //DatosSeguimientoPaciente.CitFechaAtencion = FechaAtencion;
                //DatosSeguimientoPaciente.CitInicioAtencion = HoraInicio;
                //DatosSeguimientoPaciente.CitFinAtencion = HoraFin;
                //DatosSeguimientoPaciente.CitEstado = 1;
                //DatosSeguimientoPaciente.CitEstadoPaciente = EstadoPaciente;
                //DatosSeguimientoPaciente.CitObservaciones = Observaciones;
                //DatosSeguimientoPaciente.UsuId = Convert.ToInt64(Application.Current.Properties[VariablesGlobales.USUID]);
                var dataToken = await _resClient.Post<RespuestaModel<object>, List<SeguimientoPacienteModel>>(VariablesGlobales.URL + "SeguimientoPaciente/PostList", lista);

                if (dataToken.Codigo != System.Net.HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, dataToken.Message[0], VariablesGlobales.CERRAR);
                }

                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.EXITO, "Encuenta llenada correctamente", VariablesGlobales.CERRAR);
                if (porcentaje >= 70)
                {
                    await Navigation.PushAsync(new Master());
                }
                else
                {
                   var action= await Application.Current.MainPage.DisplayAlert("Usted debe volver agendar una nueva cita", "Crear una nueva Cita médica", "SI", "NO");
                    if (action)
                    {
                        await Navigation.PushAsync(new Cita());
                    }
                    else
                    {
                        await Navigation.PushAsync(new Master());
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, ex.Message, VariablesGlobales.CERRAR);

            }
        }

          
        
    }


}
