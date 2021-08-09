using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://orgf3758661.crm8.dynamics.com/";
            //string url = "https://orgf3758661.api.crm8.dynamics.com/api/data/v9.2/"
            string username = "admin@raghav0308.onmicrosoft.com";
            string password = "pass@word1";

            string clientId = "910664b2-5b8d-4092-8bc7-7c8b90807fe8";
            //value: "AheB.t1YCTfr1bE619MEQ2eI1-I7rm--_2"
            //secret: 4ef4934c-2594-4f11-8c5d-58ad8dbf1156

            var userCredential = new UserCredential(username, password);

            string apiversion = "9.2";
            string webApiUrl = $"{url}/api/data/v{apiversion}/";

            Console.WriteLine("webApiUrl : " + webApiUrl);
            Console.WriteLine("username : " + username);
            Console.WriteLine("password : " + password);
            Console.WriteLine("clientId : " + clientId);


            //authentication
            var authParameters = AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(webApiUrl)).Result;
            var authContext = new AuthenticationContext(authParameters.Authority, false);
            var authResult = authContext.AcquireToken(url, clientId, userCredential);
            var authHeader = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(webApiUrl);
                client.DefaultRequestHeaders.Authorization = authHeader;

                var response = client.GetAsync("WhoAmI").Result;

                if (response.IsSuccessStatusCode)
                {
                    JObject body = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    Guid userId = (Guid)body["UserId"];
                    Console.WriteLine("Your userId is " + userId);
                }
                else
                {
                    Console.WriteLine("Request failed " + response.ReasonPhrase);


                }
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }


        }
    }
}
