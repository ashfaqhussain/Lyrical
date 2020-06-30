namespace LyricalWeb.Services.LyricsOvh
{
    public interface ILyricsOvhClient
    {
        string FetchWords(string artistName, string songName);
    }
}