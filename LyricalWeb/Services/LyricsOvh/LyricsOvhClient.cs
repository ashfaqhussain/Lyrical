using System.Net.Http;
using System.Net.Http.Headers;
using LyricalWeb.Services.LyricsOvh.Models;
using Newtonsoft.Json;

namespace LyricalWeb.Services.LyricsOvh
{
    public class LyricsOvhClient : ILyricsOvhClient
    {
        private static string JsonMediaType = "application/json";

        public string FetchWords(string artistName, string songName)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

                var response = client.GetAsync($"https://api.lyrics.ovh/v1/{artistName}/{songName}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var model = JsonConvert.DeserializeObject<LyricsResponse>(response.Content.ReadAsStringAsync().Result);
                    return model.Lyrics;
                }
            }

            return null;
        }
    }
}
