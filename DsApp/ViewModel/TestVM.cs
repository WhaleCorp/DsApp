using DsApp.Base;
using DsApp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DsApp.ViewModel
{
    public class TestVM : BaseModel
    {
        private string test;

        public TestVM()
        {
            GetCodeForAuthorizationAsync();
        }

        public string Code
        {
            get => test;
            set
            {
                test = value;
                OnPropertyChanged("Code");
            }
        }

        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async void GetCodeForAuthorizationAsync()
        {
            try
            {
                #if DEBUG
                    HttpClientHandler insecureHandler = GetInsecureHandler();
                    HttpClient client = new HttpClient(insecureHandler);
                    HttpResponseMessage result = await client.GetAsync("https://10.0.2.2:7296/Auth/GenerateCode");
                #else
                    HttpClient client = new HttpClient();
                #endif

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    this.Code = content;
                }



            }
            catch (Exception ex)
            { }
        }
    }
}
