using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LyricalWeb.Services.MusicBrainz
{
    public interface IMusicBrainzClient
    {
        Task<Guid?> FindArtist(string artistName);
        Task<IList<string>> FetchSongNames(Guid artistId);
    }
}