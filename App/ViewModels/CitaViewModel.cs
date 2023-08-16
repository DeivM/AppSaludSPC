using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.Views;
using App.WebServices;
using System;

using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class CitaViewModel : BaseViewModel
    {
        private ResClient _resClient;
        private CitaModel _datosCita;
        
        //private string _especialidade = string.Empty; mes_id   como se añade la especialidad y el doctor si solo tenemos mesid  
        private DateTime _fechaatencion = DateTime.MinValue;
        private TimeSpan _horainicioatencion = TimeSpan.Zero;
        private TimeSpan _horafinatencion = TimeSpan.Zero;
        private string _estadopaciente = string.Empty;
        private string _observaciones = string.Empty;
        public ICommand ButtonCommand { get; }
        public INavigation Navigation { get; set; }
        ILoginManager iml = null;
        public CitaModel DatosCita
        {
            get { return _datosCita; }
            set { _datosCita = value; OnPropertyChanged(); }
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
                   // OnPropertyChanged(nameof(Allowed));
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



        public DateTime FechaAtencion
        {
            get { return _fechaatencion; }
            set
            {
                if (_fechaatencion != value)
                {
                    _fechaatencion = value;
                    OnPropertyChanged(nameof(FechaAtencion));
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


        //dejamos esta linea de codigo de Email y contraseña
       // public bool Allowed => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Contrasena) && !string.IsNullOrEmpty(Nombres);

        public CitaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ButtonCommand = new Command(async () => await GUardar());
            _resClient = new ResClient();
        }

        async Task GUardar()
        {
            try
            {
                //DatosCita.MesId = ? 
                DatosCita.CitFechaAtencion = FechaAtencion;
                DatosCita.CitInicioAtencion = HoraInicio;
                DatosCita.CitFinAtencion = HoraFin;
                DatosCita.CitEstado = 1;
                DatosCita.CitObservaciones = Observaciones;
                

                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);

                var dataToken = await _resClient.Post<RespuestaModel<object>, CitaModel>(VariablesGlobales.URL + VariablesGlobales.APICITA+ "/cita", DatosCita); //que se pone en usuarioplan

                if (dataToken.Codigo != System.Net.HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, dataToken.Message[0], VariablesGlobales.CERRAR);
                }

                UserDialogs.Instance.HideLoading();
                await Navigation.PushAsync(new Master());
            }
            catch (Exception ex)
            {
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<CodeErrorResponse>(ex.Message);

                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, error.Message, VariablesGlobales.CERRAR);

            }
        }

          
        
    }


}
