using System.Threading.Tasks;

namespace LyricalWeb.Services
{
    public interface IArtistWordsService
    {
        Task<int> FetchAverageWordCount(string artistName);
    }
}