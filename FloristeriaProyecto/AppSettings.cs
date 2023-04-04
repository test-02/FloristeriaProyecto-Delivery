using AppNotas.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloristeriaProyecto
{
    public class AppSettings
    {
        public static string ApiFirebase = "https://floristeriamovil2-10357-default-rtdb.firebaseio.com/";
        public static string KeyAplication = "AIzaSyC6YhMrHw1E4Z2cqbRqjjGtkzmkUz_iDDA";
        //AIzaSyB02It8JWMbOwatyJGX9CUfmCQhCZLugDE


        public static ResponseAuthentication oAuthentication { get; set; }
        private static string ApiUrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";

        public static string ApiAuthentication(string tipo)
        {
            if (tipo == "LOGIN")
                return ApiUrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;
            else if (tipo == "SIGNIN")
                return ApiUrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
            else
                return "";
        }

    }
}
