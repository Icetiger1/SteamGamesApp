namespace SteamGamesApp.Models
{
    public class SteamUser
    {
        public string? SteamId { get; set; }
        public string? NameUser { get; set; }
        public string Realname { get; set; }
        public int TimeCreate { get; set; }
        public string IconUser { get; set; }

        public SteamUser(string steamId)
        {
            this.SteamId = steamId;
        }
    }
}
