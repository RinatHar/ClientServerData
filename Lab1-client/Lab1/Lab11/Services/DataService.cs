using Lab1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    internal class DataService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<List<Data>> GetDataFromServerAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Data>>(data);
                }
            }
            catch { }

            return null;
        }

        public async Task<bool> AddDataToServerAsync(string url, string value)
        {
            var data = new DataDto { Value = value };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}
