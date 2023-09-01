using Acr.UserDialogs;
using App.Helpers;
using App.Models;
using App.Views;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class MasDatosViewModel  : BaseViewModel
    {

        private UsuarioModel _datosUsuario;
        private string _edad = string.Empty;
        public ICommand NavigateCommand { get; }
        public INavigation Navigation { get; set; }

        public UsuarioModel DatosUsuario
        {
            get { return _datosUsuario; }
          //  set { _datosUsuario = value; OnPropertyChanged(); }
        }

        public string MasDatos
        {
            get { return _edad; }
            set
            {
                if (_edad != value)
                {
                    _edad = value;
                    OnPropertyChanged(nameof(MasDatos));
                    OnPropertyChanged(nameof(Allowed));
                   
                    
                }
            }
        }

        public bool Allowed => !string.IsNullOrEmpty(MasDatos) && Convert.ToInt16(_edad) > 14;

        public MasDatosViewModel(INavigation navigation)
        {
            Navigation = navigation;
            NavigateCommand = new Command<Type>(async (pageType) => await GoToDetails(pageType));
           
        }


        async Task GoToDetails(Type pageType)
        {

            if (MasDatos != null)
            {
                var page = (Page)Activator.CreateInstance(pageType);
                //DatosUsuario.UsuMasDatos = Convert.ToInt16( MasDatos);
             /*    page.BindingContext = new EstaturaViewModel(Navigation)
                 {
                     DatosUsuario = DatosUsuario
                 };*/
                await Navigation.PushAsync(page);
            }
        }
    }
}
