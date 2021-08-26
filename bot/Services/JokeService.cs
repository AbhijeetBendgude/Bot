using bot.Services.Abstractions;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace bot.Services
{
    public class JokeService : IJokeService
    {
        public async Task<string> GetJoke()
        {
            string joke = ConstantStrings.no_joke;

            using (HttpClient client = new()) 
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await client.GetAsync(ConstantStrings.joke_api_url);

                if (response.IsSuccessStatusCode) 
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(responseBody);
                    joke = json["joke"].ToString();
                }
            }

            return joke;
        }
    }
}
