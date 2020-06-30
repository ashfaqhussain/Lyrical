using System.Collections.Generic;
using Newtonsoft.Json;

namespace LyricalWeb.Services.MusicBrainz.Models
{
    public class ReleaseListResponse
    {
        [JsonProperty(PropertyName = "release-count")]
        public int ReleaseCount { get; set; }

        [JsonProperty(PropertyName = "release-offset")]
        public int ReleaseOffset { get; set; }

        public List<ReleaseResponse> Releases { get; set; }
    }
}