namespace SteamGamesApp.Models
{
    public class SteamApiGame
    {
        public string img_icon_url { get; set; }
        public int appid { get; set; }
        public string name { get; set; }
        public double playtime_forever { get; set; }
        public int rtime_last_played { get; set; }
    }
}
