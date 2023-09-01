using App.Helpers;
using App.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    public partial class App : Application, ILoginManager
    {
        public static Home home { get; set; }
        public static App Current;
        public static double ScreenHeight;
        public static double ScreenWidth;
        public App()
        {
            InitializeComponent();
            Current = this;
            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;
            if (isLoggedIn)
            {
                //MainPage = new Home();
                MainPage = new NavigationPage(new Home());
                //{
                //    BarBackgroundColor = Color.Red // Establece el color de la barra de navegación aquí
                //};
            }
            else
            {
                MainPage = new NavigationPage(new PaginaInicio());
                //{
                //    BarBackgroundColor = Color.Red // Establece el color de la barra de navegación aquí
                //};


                // MainPage = new PaginaInicio();
                //  MainPage = new NavigationPage(new Login(this));
                // MainPage = new Ubicacion();

            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void Logout()
        {
            Properties["IsLoggedIn"] = false;
            Properties["usuario"] = string.Empty;
            Properties["token"] = string.Empty;
            Properties["expires"] = null;
            Properties["currentTime"] = null;
            Properties["password"] = string.Empty;
            Properties["email"] = string.Empty;
            MainPage = new Login(this);
        }

        public void ShowMainPage()
        {
            MainPage = new NavigationPage( new Home());
        }

        public void CreaCuenta()
        {

            MainPage = new CrearCuenta();
        }

        public void Rcuperar()
        {

            MainPage = new RecuperarContrasena();
        }


    }
}
