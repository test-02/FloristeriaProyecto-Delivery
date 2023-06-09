﻿using AppNotas.Modelo;
using Firebase.Auth;
using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Views;
using Newtonsoft.Json;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;

namespace FloristeriaProyecto.Service
{
    public class ApiServiceFirebase
    {
        public bool IsEnabled { get; }

        public ApiServiceFirebase()
        {
            IsEnabled = true;
        }
        public static async Task<bool> RegistrarUsuario(Usuario oUsuario, ResponseAuthentication oResponse)
        {
            try
            {

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var formatoapi = string.Concat(AppSettings.ApiFirebase, "{0}/{1}.json?auth={2}");

                var response = await client.PutAsync(
                    string.Format(formatoapi, "usuarios", oResponse.LocalId, oResponse.IdToken),
                    content);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {//enviar mensaje verificacion
                    string WebAPIkey = "AIzaSyC6YhMrHw1E4Z2cqbRqjjGtkzmkUz_iDDA";
                    var action = await App.Current.MainPage.DisplayAlert("Alerta", "Su correo electrónico no está activado, ¿quiere enviar enlace de Verificación?!", "Yes", "No");

                    if (action)
                    {
                        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));

                        await authProvider.SendEmailVerificationAsync(oResponse.IdToken);
                    }
                    //persistencia de datos
                    var serializedcontnet = JsonConvert.SerializeObject(content);
                    Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }
        }


        public static async Task<Dictionary<string, Categoria>> ObtenerCategorias()
        {
            Dictionary<string, Categoria> oObject = new Dictionary<string, Categoria>();
            try
            {
                HttpClient client = new HttpClient();
                string apiurlformat = string.Concat(AppSettings.ApiFirebase, "dbalmacen/categoria.json?auth={0}");
                var response = await client.GetAsync(string.Format(apiurlformat, AppSettings.oAuthentication.IdToken));

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        oObject = JsonConvert.DeserializeObject<Dictionary<string, Categoria>>(jsonstring);
                    }
                    return oObject;
                }
                else
                {
                    return oObject;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return oObject;
            }

        }

        public static async Task<Usuario> ObtenerUsuario()
        {
            Usuario oObject = new Usuario();
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/{0}.json?auth={1}");
                var response = await client.GetAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        oObject = JsonConvert.DeserializeObject<Usuario>(jsonstring);
                    }
                    return oObject;
                }
                else
                {
                    return oObject;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return oObject;
            }

        }

        public static async Task<bool> GuardarCambiosUsuario(Usuario oUsuario)
        {
            Usuario oObject = new Usuario();
            try
            {
                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");


                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/{0}.json?auth={1}");
                var response = await client.PutAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        public static async Task<bool> EliminarBolsa()
        {
            Usuario oObject = new Usuario();
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "bolsa/{0}.json?auth={1}");
                var response = await client.DeleteAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }
        }

        public static async Task<List<Departamento>> ObtenerDepartamentos()
        {
            Dictionary<string, Departamento> oObject = new Dictionary<string, Departamento>();
            List<Departamento> oListaDepartamento = new List<Departamento>();
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "ubigeo/departamento.json?auth={0}");
                var response = await client.GetAsync(string.Format(apiformat, AppSettings.oAuthentication.IdToken));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        oObject = JsonConvert.DeserializeObject<Dictionary<string, Departamento>>(jsonstring);
                        foreach (KeyValuePair<string, Departamento> item in oObject)
                        {
                            oListaDepartamento.Add(new Departamento()
                            {
                                nombredepartamento = item.Value.nombredepartamento,
                            });
                        }
                    }

                    return oListaDepartamento;
                }
                else
                {
                    oListaDepartamento = null;
                    return oListaDepartamento;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                oListaDepartamento = null;
                return oListaDepartamento;
            }
        }

        public static async Task<List<Tienda>> ObtenerTiendas()
        {
            Dictionary<string, Tienda> oObject = new Dictionary<string, Tienda>();
            List<Tienda> oListaTienda = new List<Tienda>();
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "tiendas.json?auth={0}");
                var response = await client.GetAsync(string.Format(apiformat, AppSettings.oAuthentication.IdToken));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        oObject = JsonConvert.DeserializeObject<Dictionary<string, Tienda>>(jsonstring);
                        foreach (KeyValuePair<string, Tienda> item in oObject)
                        {
                            oListaTienda.Add(new Tienda()
                            {
                                nombretienda = item.Value.nombretienda,
                                ubicacion = item.Value.ubicacion,
                                direccion1 = item.Value.direccion1,
                                direccion2 = item.Value.direccion2,
                                titulo = string.Format("{0} - {1}", item.Value.nombretienda, item.Value.ubicacion)
                            });
                        }
                    }

                    return oListaTienda;
                }
                else
                {
                    oListaTienda = null;
                    return oListaTienda;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                oListaTienda = null;
                return oListaTienda;
            }
        }

        public static async Task<bool> RegistrarCompra(Compra oCompra)
        {
            try
            {

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oCompra);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var formatoapi = string.Concat(AppSettings.ApiFirebase, "{0}/{1}.json?auth={2}");

                var response = await client.PostAsync(string.Format(formatoapi, "compra", AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    bool respuesta = await EliminarBolsa();
                    return respuesta;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }


        public static async Task<List<Compra>> ObtenerCompra()
        {
            Dictionary<string, Compra> oObject = new Dictionary<string, Compra>();
            List<Compra> oListaCompra = new List<Compra>();
            try
            {
                HttpClient client = new HttpClient();
                string apiformat = string.Concat(AppSettings.ApiFirebase, "compra/{0}.json?auth={1}");
                var response = await client.GetAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken));
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        oObject = JsonConvert.DeserializeObject<Dictionary<string, Compra>>(jsonstring);
                        foreach (KeyValuePair<string, Compra> item in oObject)
                        {
                            oListaCompra.Add(new Compra()
                            {
                                tipoEntrega = item.Value.tipoEntrega,
                                fechaCompra = item.Value.fechaCompra,
                                oDepacho = item.Value.oDepacho,
                                oDetallePago = item.Value.oDetallePago,
                                oListaBolsa = item.Value.oListaBolsa,
                                oTienda = item.Value.oTienda,
                            });
                        }
                    }

                    return oListaCompra;
                }
                else
                {
                    oListaCompra = null;
                    return oListaCompra;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                oListaCompra = null;
                return oListaCompra;
            }
        }

    }
}
