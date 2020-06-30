using System.Collections.Generic;

namespace LyricalWeb.Services.MusicBrainz.Models
{
    public class ReleaseResponse
    {
        public IList<MediaResponse> Media { get; set; }
    }
}