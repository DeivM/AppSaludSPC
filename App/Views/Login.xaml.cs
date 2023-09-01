
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
    public partial class Login : ContentPage
    {
        private ResClient _resClient;
        ILoginManager iml = null;

        public Login()
        {
            InitializeComponent();
            _resClient = new ResClient();
            BindingContext = new MainPageViewModel(Navigation);
        }

        public Login(ILoginManager ilm)
        {
            InitializeComponent();
            _resClient = new ResClient();
            iml = ilm;
            BindingContext = new MainPageViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            txtEmail.Focus();
        }
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPass.Text))
                {
                    UserDialogs.Instance.ShowLoading(VariablesGlobales.ESPEREMOMENTO);
                    TokenRequest token = new TokenRequest
                    {
                        UsuEmail = txtEmail.Text,
                        UsuPassword = txtPass.Text
                    };
                    var dataToken = await _resClient.PostToken<RespuestaModel<Token1>, TokenRequest>(VariablesGlobales.URL + "Login/Login", token);
                    if (dataToken.Codigo == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await DisplayAlert(VariablesGlobales.ERROR, dataToken.Message[0], VariablesGlobales.OK);
                    }
                    else
                    {

                        Application.Current.Properties[VariablesGlobales.USUID] = dataToken.Data.UsuId;
                        Application.Current.Properties[VariablesGlobales.USUARIO] = dataToken.Data.UsuNombre;
                        Application.Current.Properties["token"] = dataToken.Data.Access_token;
                        Application.Current.Properties["IsLoggedIn"] = true;
                        Application.Current.Properties["password"] = txtPass.Text;
                        Application.Current.Properties["email"] = txtEmail.Text;
                        iml.ShowMainPage();
                    }
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    await DisplayAlert(VariablesGlobales.INFO, VariablesGlobales.CORREOCONTRASEVACIO, VariablesGlobales.OK);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert(VariablesGlobales.ERROR, ex.Message.ToString(), VariablesGlobales.OK);
            }
            

        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
          await  Navigation.PushModalAsync(new CrearCuenta());

          //  await Navigation.PushAsync(new NavigationPage(new CrearCuenta()));

        }

        private async void btnRecuperarContrasena_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RecuperarContrasena());
        }
    }
}