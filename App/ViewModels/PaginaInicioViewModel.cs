using App.Models;
using App.Views;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class PaginaInicioViewModel  : BaseViewModel
    {
        public ICommand IniciarSessionCommand {  get; }
        public ICommand CrearCentaCommand { get; }
        public PaginaInicioViewModel()
        {
            //IniciarSessionCommand = new Command(async () => await IniciarCuenta());

           // CrearCentaCommand=new Command(CrearCuenta);
        }

      /*  private async Task IniciarCuenta()
        {
            await Navigation.PushAsync(new Login());
        }

        private void CrearCuenta()
        {
            await Navigation.PushAsync(new ForgotPassword());
        }*/


    }
}
