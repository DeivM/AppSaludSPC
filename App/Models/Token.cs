using App.Helpers;
using App.WebServices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Models
{
    public class Token
    {
        public static int CliId { get; set; }
        public static string usuUsuario { get; set; }
        public static string usuEmail { get; set; }
        public static string access_token { get; set; }
        public static string password { get; set; }


        private ResClient _resClient;
        public Token()
        {
            _resClient = new ResClient();
           

            if (Application.Current.Properties.ContainsKey("token"))
            {
                var val = Application.Current.Properties["token"];
                access_token = val.ToString();
            }
           
            if (Application.Current.Properties.ContainsKey("password"))
            {
                var val = Application.Current.Properties["password"];
                password = val.ToString();
            }
            if (Application.Current.Properties.ContainsKey("email"))
            {
                var val = Application.Current.Properties["email"];
                usuEmail = val.ToString();
            }
        }
        public static int GetIdUser()
        {
            if (Application.Current.Properties.ContainsKey(VariablesGlobales.USUID))
            {
                var val = Application.Current.Properties[VariablesGlobales.USUID];
                CliId = (int)val;
            }

            return CliId;
        }
        public static string GetUsuario()
        {
            var val = Application.Current.Properties[VariablesGlobales.USUARIO];
            usuUsuario = val.ToString();
            return usuUsuario;
        }
        public static string GetAccesToken()
        {
            var val = Application.Current.Properties["token"];
            access_token = val.ToString();
            return access_token;
        }
        public static  string GetEmail()
        {
            return usuEmail;
        }
        public static string GetPasword()
        {
            return password;
        }
       

        public async Task CheckTokenValidity()
        {
            
                TokenRequest token = new TokenRequest
                {
                    UsuEmail = GetEmail(),
                    UsuPassword = GetPasword()
                };
                var dataToken = await _resClient.PostToken<RespuestaModel<Token1>, TokenRequest>(VariablesGlobales.URL + "Cliente/Login", token);
                if (dataToken.Codigo == System.Net.HttpStatusCode.Unauthorized)
                {
                    App.Current.Logout();
                }
                else
                {
                    Application.Current.Properties[VariablesGlobales.USUARIO] = dataToken.Data.UsuId;
                    Application.Current.Properties["token"] = dataToken.Data.Access_token;
                    Application.Current.Properties["IsLoggedIn"] = true;
                    Application.Current.Properties["password"] = token.UsuPassword;
                    Application.Current.Properties["email"] = token.UsuEmail;
                    }
            

        }

    }
}
