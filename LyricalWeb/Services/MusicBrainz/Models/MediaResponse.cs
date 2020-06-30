using System.Collections.Generic;

namespace LyricalWeb.Services.MusicBrainz.Models
{
    public class MediaResponse
    {
        public IList<TrackResponse> Tracks { get; set; }
    }
}