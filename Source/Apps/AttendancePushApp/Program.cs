using Models.Core.Employment;
using Models.Core.HR.Attendance;
using Models.Core.Operationals;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AttendancePushApp
{
    class Program
    {
        #region Fields

        static HttpClient _client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(10000) };
        static List<AccessEvent> _accessEvents = new List<AccessEvent>();
        static List<AccessPoint> _accessPoints = new List<AccessPoint>();

        #endregion

        static void Main(string[] args)
        {

            //PushDB<Department>("OperationalsDB", "Departments");
            //PushDB<Employee>("EmployeeDB", "Employees");

            PushAttendanceToAPI();
        }

        private static void PushAttendanceToAPI()
        {
            LoadJsonData();

            _client.BaseAddress = new Uri("https://localhost:5001/");

            //var token = GetTokenAsync().Result; //Get and set Authentication bearer token
            //_client.SetBearerToken(token);
            //Console.WriteLine(token);

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                foreach (var o in _accessPoints)
                {
                    Add(o);
                }
                foreach (var o in _accessEvents)
                {
                    Add(o);
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        private static void PushDB<T>(string dbName, string collectionName)
        {
            var str = System.IO.File.ReadAllText(@"C:\Temp\Attendance\" + collectionName + ".txt");
            var elements = JsonConvert.DeserializeObject<List<T>>(str);

            var client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase db = client.GetDatabase(dbName);
            var collection = db.GetCollection<T>(collectionName);
            try
            {
                foreach (var o in elements)
                {
                    collection.InsertOneAsync(o);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region Authentication

        private static async Task<string> GetTokenAsync()
        {
            var discoveryResponse = await DiscoveryClient.GetAsync("https://localhost:49333/");
            if (discoveryResponse.IsError)
            {
                Console.WriteLine("Error pushing attendance data to AttendanceApi: {0}", discoveryResponse.Error);
                return null;
            }

            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "AttendancePushApp", "Adpa@98765");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("AttendanceApi");
            if (tokenResponse.IsError)
            {
                Console.WriteLine("Error pushing attendance data to AttendanceApi -> Token end point error: {0}", tokenResponse.Error);
                return null;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            return tokenResponse.AccessToken;
        }

        #endregion

        #region Loading and pushing attendance data

        private static void LoadJsonData()
        {
            var accessEventsStr = System.IO.File.ReadAllText(@"C:\Temp\Attendance\AccessEvents.txt");
            _accessEvents = JsonConvert.DeserializeObject<List<AccessEvent>>(accessEventsStr);

            var accessPointsStr = System.IO.File.ReadAllText(@"C:\Temp\Attendance\AccessPoints.txt");
            _accessPoints = JsonConvert.DeserializeObject<List<AccessPoint>>(accessPointsStr);
        }

        static async void Add(AccessEvent obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("api/AccessEvents", httpContent);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        static async void Add(AccessPoint obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("api/AccessPoints", httpContent);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        #endregion
    }
}
