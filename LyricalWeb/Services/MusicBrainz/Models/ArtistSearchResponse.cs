using System.Collections.Generic;

namespace LyricalWeb.Services.MusicBrainz.Models
{
    public class ArtistSearchResponse
    {
        public IList<ArtistResponse> Artists { get; set; }
    }
}
