using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LyricalWeb.Services.LyricsOvh;
using LyricalWeb.Services.MusicBrainz;

namespace LyricalWeb.Services
{
    public class ArtistWordsService : IArtistWordsService
    {
        public IMusicBrainzClient _musicBrainzClient;
        public ILyricsOvhClient _lyricsOvhClient;

        public ArtistWordsService(IMusicBrainzClient musicBrainzClient, ILyricsOvhClient lyricsOvhClient)
        {
            _musicBrainzClient = musicBrainzClient;
            _lyricsOvhClient = lyricsOvhClient;
        }

        public async Task<int> FetchAverageWordCount(string artistName)
        {
            var artistId = await _musicBrainzClient.FindArtist(artistName);

            if (artistId == null)
            {
                throw new Exception($"Unable to find artist with name: {artistName}");
            }

            var songNames = await _musicBrainzClient.FetchSongNames(artistId.Value);

            object sync = new object();
            int total = 0;
            Parallel.ForEach(songNames,
                () => 0,
                 (songName, pls, localTotal) =>
                {
                    // TODO parallelise this better using HttpClient, and efficient use of throttling/threadpool
                    var words = _lyricsOvhClient.FetchWords(artistName, songName);
                    return localTotal += StringWordCount(words); ;
                },
                localTotal =>
                {
                    lock (sync)
                    {
                        total += localTotal;
                    }
                });

            return await Task.FromResult(total / songNames.Count);
        }

        private int StringWordCount(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return 0;
            }

            return Regex.Matches(s, @"[A-Za-z0-9]+").Count;
        }

    }
}
