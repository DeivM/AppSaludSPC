using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.Views;
using App.WebServices;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class CitaViewModel : BaseViewModel
    {
        private ResClient _resClient;
        private CitaModel DatosCita=new CitaModel();


        private DateTime _fechaatencion = DateTime.Now;
        private TimeSpan _horainicioatencion = TimeSpan.Zero;
        private TimeSpan _horafinatencion = TimeSpan.Zero;
        private string _estadopaciente = string.Empty;
        private string _observaciones = string.Empty;
        public ICommand ButtonCommand { get; }
        public INavigation Navigation { get; set; }
        private ObservableCollection<ListModel> _listEspecialidad;//= new ObservableCollection<ListModel>();
        private ObservableCollection<ListModel> _listMedico;//= new ObservableCollection<ListModel>();
        private ObservableCollection<ListModel> _listHorario;


        public CitaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ButtonCommand = new Command(async () => await GUardar());
            _resClient = new ResClient();
            LoadDetalleDetalleRondasEmpleado();
        }
        public ObservableCollection<ListModel> ListEspecialidad
        {
            get=> _listEspecialidad;
            set { SetObservableProperty(ref _listEspecialidad, value); }
        }
        public ObservableCollection<ListModel> ListMedico
        {
            get => _listMedico;
            set => SetObservableProperty(ref _listMedico, value);
        }

        public ObservableCollection<ListModel> ListHorario
        {
            get => _listHorario;
            set => SetObservableProperty(ref _listHorario, value);
        }


        private ListModel _selectedEspecialidad;
        public ListModel SelectedEspecialidad
        {
            get =>  _selectedEspecialidad;
            set
            {
                if (value != null)
                LoadMedico(value.id);
                SetObservableProperty(ref _selectedEspecialidad, value);
            }
        }

        private ListModel _selectedMedico;
        public ListModel SelectedMedico
        {
            get => _selectedMedico;
            set
            {
                if (value != null)
                    LoadHorario(value.id);
                SetObservableProperty(ref _selectedMedico, value);
            }

            


            //set => SetObservableProperty(ref _selectedMedico, value);
        }

        public DateTime FechaAtencion
        {
            get { return _fechaatencion; }
            set
            {
                if (_fechaatencion != value)
                {
                    _fechaatencion = value;
                    OnPropertyChanged(nameof(FechaAtencion));
                    OnPropertyChanged(nameof(Allowed));
                    LoadHorario(_selectedMedico.id);
                }
            }
        }


       


        public string EstadoPaciente
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






        public TimeSpan HoraInicio
        {
            get { return _horainicioatencion; }
            set
            {
                if (_horainicioatencion != value)
                {
                    _horainicioatencion = value;
                    OnPropertyChanged(nameof(HoraInicio));
                }
            }
        }

        public TimeSpan HoraFin
        {
            get { return _horafinatencion; }
            set
            {
                if (_horafinatencion != value)
                {
                    _horafinatencion = value;
                    OnPropertyChanged(nameof(HoraFin));
                }
            }
        }
        private  bool ValidarFecha()
        {
            bool validar = true;
            if (FechaAtencion.Date<DateTime.Now.Date)
            {
                validar = false;
                Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, "No puede Seleccionar una fecha menor a la fecha actuaL", VariablesGlobales.CERRAR);
            }
            return validar;
        }

        public bool Allowed => ValidarFecha();

        //dejamos esta linea de codigo de Email y contraseña
        // public bool Allowed => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Contrasena) && !string.IsNullOrEmpty(Nombres);


        public async Task LoadDetalleDetalleRondasEmpleado()
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("id", "0");
            var result = await _resClient.Get<RespuestaModel<CitaData>>(VariablesGlobales.URL + "Cita/GetData", data);
            ListEspecialidad = new ObservableCollection<ListModel>(result.Data.MedicosEspecialidad);
        }

        public async Task LoadMedico(int id)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("id", id.ToString());
            var result = await _resClient.Get<List<ListModel>>(VariablesGlobales.URL + "Usuario/GetList", data);
            if (result.Count==0)
            {
              await  Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, "No tiene médicos asociados a la especialidad", VariablesGlobales.CERRAR);
            }
            ListMedico = new ObservableCollection<ListModel>(result);
        }

        public async Task LoadHorario(int id)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("id", id.ToString());
            data.Add("id1", FechaAtencion.ToString());
            data.Add("id2", DateTime.Now.TimeOfDay.ToString());
            var result = await _resClient.Get<List<ListModel>>(VariablesGlobales.URL + "Horario/GetList", data);
            if (result.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, "No tiene Horario disponibles para esas fechas", VariablesGlobales.CERRAR);
            }
            ListHorario = new ObservableCollection<ListModel>(result);
        }

        async Task GUardar()
        {
            try
            {
                DatosCita.EspId = SelectedEspecialidad.id;
                DatosCita.MesId = SelectedMedico.id;
                DatosCita.CitFechaAtencion = FechaAtencion;
                DatosCita.CitInicioAtencion = HoraInicio;
                DatosCita.CitFinAtencion = HoraFin;
                DatosCita.CitEstado = 1;
                DatosCita.CitEstadoPaciente = EstadoPaciente;
                DatosCita.CitObservaciones = Observaciones;
                DatosCita.UsuId = Convert.ToInt64(Application.Current.Properties[VariablesGlobales.USUID]);
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);

                var dataToken = await _resClient.Post<RespuestaModel<object>, CitaModel>(VariablesGlobales.URL + VariablesGlobales.APICITA, DatosCita);

                if (dataToken.Codigo != System.Net.HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, dataToken.Message[0], VariablesGlobales.CERRAR);
                }

                UserDialogs.Instance.HideLoading();
                await Navigation.PushAsync(new Master());
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
              //  var error = Newtonsoft.Json.JsonConvert.DeserializeObject<CodeErrorResponse>(ex.Message);
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, ex.Message, VariablesGlobales.CERRAR);

            }
        }

          
        
    }


}
