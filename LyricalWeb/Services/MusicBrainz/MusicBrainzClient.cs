using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LyricalWeb.Services.MusicBrainz.Models;
using Newtonsoft.Json;

namespace LyricalWeb.Services.MusicBrainz
{
    public class MusicBrainzClient : IMusicBrainzClient
    {
        private static string JsonMediaType = "application/json";
        private static string UserAgent = "Ash-Test-Lyrical/0.0.1 (ash2093 @hotmail.com )";
        private static int PageLimit = 100;

        public async Task<Guid?> FindArtist(string artistName)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));
                client.DefaultRequestHeaders.Add("user-agent", UserAgent);

                // TODO escape Lucene special characters in query...
                var response = await client.GetAsync($"https://musicbrainz.org/ws/2/artist?query=artist:{artistName}");
                if (response.IsSuccessStatusCode)
                {
                    var model = JsonConvert.DeserializeObject<ArtistSearchResponse>(await response.Content.ReadAsStringAsync());
                    // TODO just use the first hit for now
                    return model.Artists.FirstOrDefault()?.Id;
                }
            }

            return null;
        }

        private async Task<ReleaseListResponse> FetchPageOfReleases(Guid artistId, int limit, int offset)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));
                client.DefaultRequestHeaders.Add("user-agent", UserAgent);

                var response = await client.GetAsync($"https://musicbrainz.org/ws/2/release?artist={artistId}&inc=recordings&limit={limit}&offset={offset}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ReleaseListResponse>(json);
                    return model;
                }
            }
            return null;
        }

        public async Task<IList<string>> FetchSongNames(Guid artistId)
        {
            var current = new ReleaseListResponse { ReleaseCount = 1, ReleaseOffset = -PageLimit, Releases = new List<ReleaseResponse>() };
            while (current.ReleaseCount > current.ReleaseOffset + PageLimit)
            {
                var next = await FetchPageOfReleases(artistId, PageLimit, current.ReleaseOffset + PageLimit);
                current.ReleaseOffset = next.ReleaseOffset;
                current.ReleaseCount = next.ReleaseCount;
                current.Releases.AddRange(next.Releases);
            }

            return current
                           .Releases
                           .SelectMany(x => x.Media)
                           .SelectMany(x => x.Tracks)
                           .Select(x => Regex.Replace(x.Title, @" ?\(.*?\)", string.Empty).Trim())
                           .Select(x => Regex.Replace(x, @" ?\[.*?\]", string.Empty).Trim())
                           .Where(x => !string.IsNullOrWhiteSpace(x))
                           .Distinct()
                           .ToList();
        }

    }
}
