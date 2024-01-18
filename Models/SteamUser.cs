namespace SteamGamesApp.Models
{
    public class SteamUser
    {
        public string? SteamId { get; set; }
        public string? NameUser { get; set; }
        public string Realname { get; set; }
        public string TimeCreate { get; set; }
        public string IconUser { get; set; }
        public List<GameInfo> GameInfo { get; set; }

        public SteamUser(string steamId)
        {
            this.SteamId = steamId;
        }

        public DateTime ConvertToTime(int time)
        {
            var expirationTime = DateTimeOffset
                           .FromUnixTimeSeconds(time)
                           .DateTime;

            return expirationTime;
        }

        public string ConvertTimeToString(int timeCreate)
        {
            return this.TimeCreate = ConvertToTime(timeCreate).ToString();
        }
    }
}
