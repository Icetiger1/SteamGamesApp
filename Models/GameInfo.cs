namespace SteamGamesApp.Models
{
    public class GameInfo
    {
        public string IconUrl { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
        public double Playtime { get; set; }
        public string Lastplayed { get; set; }

        public GameInfo(string iconUrl, int appId, string name, double playtime, int lastplayed)
        {            
            this.IconUrl = iconUrl;
            this.AppId = appId;
            this.Name = name;
            this.Playtime = playtime;
            this.Lastplayed = DateIsNull(lastplayed);
        }

        public string DateIsNull(int lastplayed)
        {
            string date = ConvertToTime(lastplayed).ToString();
            if (date.Equals("01.01.1970 0:00:00"))
            {
                return "0";
            }

            return date;
        }

        public DateTime ConvertToTime(int time)
        {
            var expirationTime = DateTimeOffset
                           .FromUnixTimeSeconds(time)
                           .DateTime;

            return expirationTime;
        }

    }
}
