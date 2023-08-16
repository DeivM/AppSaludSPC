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
    public class CrearCuentaViewModel : BaseViewModel
    {
        private ResClient _resClient;
        private UsuarioModel _datosUsuario;
        private string _nombres = string.Empty;
        private string _apellidos = string.Empty;
        private string _cedula = string.Empty;
        private string _direccion = string.Empty;
        private string _telefono = string.Empty;
        private string _sexo = string.Empty;
        private DateTime _fechanacimiento = DateTime.MinValue;
        private string _email = string.Empty;
        private string _contrasena = string.Empty;
        public ICommand ButtonCommand { get; }
        public INavigation Navigation { get; set; }
        ILoginManager iml = null;
        public UsuarioModel DatosUsuario
        {
            get { return _datosUsuario; }
            set { _datosUsuario = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }
        public string Contrasena
        {
            get { return _contrasena; }
            set
            {
                if (_contrasena != value)
                {
                    _contrasena = value;
                    OnPropertyChanged(nameof(Contrasena));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }



        public DateTime FechaNacimiento
        {
            get { return _fechanacimiento; }
            set
            {
                if (_fechanacimiento != value)
                {
                    _fechanacimiento = value;
                    OnPropertyChanged(nameof(FechaNacimiento));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }


        public string Sexo
        {
            get { return _sexo; }
            set
            {
                if (_sexo != value)
                {
                    _sexo = value;
                    OnPropertyChanged(nameof(Sexo));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }

        public string Telefono
        {
            get { return _telefono; }
            set
            {
                if (_telefono != value)
                {
                    _telefono = value;
                    OnPropertyChanged(nameof(Telefono));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }

        public string Direccion
        {
            get { return _direccion; }
            set
            {
                if (_direccion != value)
                {
                    _direccion = value;
                    OnPropertyChanged(nameof(Direccion));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }

        public string Cedula
        {
            get { return _cedula; }
            set
            {
                if (_cedula != value)
                {
                    _cedula = value;
                    OnPropertyChanged(nameof(Cedula));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }

        public string Apellidos
        {
            get { return _apellidos; }
            set
            {
                if (_apellidos != value)
                {
                    _apellidos = value;
                    OnPropertyChanged(nameof(Apellidos));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }
        public string Nombres
        {
            get { return _nombres; }
            set
            {
                if (_nombres != value)
                {
                    _nombres = value;
                    OnPropertyChanged(nameof(Nombres));
                    OnPropertyChanged(nameof(Allowed));
                }
            }
        }

        public bool Allowed => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Contrasena) && !string.IsNullOrEmpty(Nombres);

        public CrearCuentaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ButtonCommand = new Command(async () => await GUardar());
            _resClient = new ResClient();
        }

        async Task GUardar()
        {
            try
            {
                DatosUsuario.UsuEmail=Email;
                DatosUsuario.UsuPassword=Contrasena;
                DatosUsuario.UsuNombres = Nombres;
                DatosUsuario.UsuEstado = 1;
                DatosUsuario.PerId = 4;
                DatosUsuario.UsuApellidos = Apellidos;
                DatosUsuario.UsuCedula = Cedula;
                DatosUsuario.UsuDireccion = Direccion;
                DatosUsuario.UsuTelefono = Telefono;
                DatosUsuario.UsuSexo = Sexo;
                DatosUsuario.UsuFechaNacimiento = FechaNacimiento;

                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);

                var dataToken = await _resClient.Post<RespuestaModel<object>, UsuarioModel>(VariablesGlobales.URL + VariablesGlobales.APIUSUARIO+ "/UsuarioPlan", DatosUsuario);

                if (dataToken.Codigo != System.Net.HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, dataToken.Message[0], VariablesGlobales.CERRAR);
                }
                //Login();
                UsuarioModel token = new UsuarioModel
                {
                    UsuEmail = DatosUsuario.UsuEmail,
                    UsuPassword = DatosUsuario.UsuPassword
                };
                var dataToken1 = await _resClient.PostToken<RespuestaModel<Token1>, UsuarioModel>(VariablesGlobales.URL + "Usuario/Login", token);
                if (dataToken.Codigo == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.ERROR, dataToken.Message[0], VariablesGlobales.OK);
                }
                else
                {

                    Application.Current.Properties[VariablesGlobales.USUID] = dataToken1.Data.UsuId;
                    Application.Current.Properties[VariablesGlobales.USUARIO] = dataToken1.Data.UsuNombre;
                    Application.Current.Properties["token"] = dataToken1.Data.Access_token;
                    Application.Current.Properties["IsLoggedIn"] = true;
                    Application.Current.Properties["password"] = DatosUsuario.UsuEmail;
                    Application.Current.Properties["email"] = DatosUsuario.UsuPassword;

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

        private async void Login()
        {
            TokenRequest token = new TokenRequest
            {
                UsuEmail = DatosUsuario.UsuEmail,
                UsuPassword = DatosUsuario.UsuPassword
            };
            var dataToken = await _resClient.PostToken<RespuestaModel<Token1>, TokenRequest>(VariablesGlobales.URL + "Login/Login", token);
            if (dataToken.Codigo == System.Net.HttpStatusCode.Unauthorized)
            {
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.ERROR, dataToken.Message[0], VariablesGlobales.OK);
            }
            else
            {

                Application.Current.Properties[VariablesGlobales.USUID] = dataToken.Data.UsuId;
                Application.Current.Properties[VariablesGlobales.USUARIO] = dataToken.Data.UsuNombre ;
                Application.Current.Properties["token"] = dataToken.Data.Access_token;
                Application.Current.Properties["IsLoggedIn"] = true;
                Application.Current.Properties["password"] = DatosUsuario.UsuEmail;
                Application.Current.Properties["email"] = DatosUsuario.UsuPassword;
              
            }
            
        }
    }


}
