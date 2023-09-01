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
    public class RecuperarContrasenaViewModel : BaseViewModel
    {
        private ResClient _resClient;
        private UsuarioModel DatosUsuario= new UsuarioModel();

        private string _email = string.Empty;
        public ICommand ButtonCommand { get; }
        public INavigation Navigation { get; set; }

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
        



      


        public bool Allowed => !string.IsNullOrEmpty(Email) ;

        public RecuperarContrasenaViewModel(INavigation navigation)
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
                UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);

                var dataToken = await _resClient.Put<RespuestaModel<object>, UsuarioModel>(VariablesGlobales.URL + VariablesGlobales.APIUSUARIO+ "/UpdatePassword", DatosUsuario);
                    await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, "La contraseña se envió a su correo electrónico", VariablesGlobales.CERRAR);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert(VariablesGlobales.INFO, ex.Message, VariablesGlobales.CERRAR);

            }
        }

       
    }


}
