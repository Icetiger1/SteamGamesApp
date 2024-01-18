using Microsoft.AspNetCore.Mvc;
using SteamGamesApp.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SteamGamesApp.Controllers
{
    public class HomeController : Controller
    {
        private SteamUser steamUser { get; set; } = new("76561198124071517");
        private const string apiKey = "EDF4805634DD51C580C147C7793563F4";

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = await GetSteamUser();
            user.GameInfo = await GetGamesBySteamId();

            return View(user);
        }

        private async Task<SteamUser> GetSteamUser()
        {
            string url1 = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={steamUser.SteamId}";
            using (HttpClient client1 = new HttpClient())
            {
                var response = await client1.GetAsync(url1);
                if (response.IsSuccessStatusCode)
                {
                    var accountInfo = JsonConvert.DeserializeObject<dynamic>(
                    await response.Content.ReadAsStringAsync());

                    steamUser.NameUser = accountInfo["response"]["players"][0]["personaname"].ToString();
                    steamUser.Realname = accountInfo["response"]["players"][0]["realname"].ToString();
                    steamUser.TimeCreate = accountInfo["response"]["players"][0]["timecreated"];
                    steamUser.IconUser = accountInfo["response"]["players"][0]["avatarfull"].ToString();

                    return steamUser;
                }
                return null;
            }
        }

        private async Task<List<GameInfo>> GetGamesBySteamId()
        {
            string url = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={apiKey}&steamid={steamUser.SteamId}&include_appinfo=true";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<SteamApiResponse>(
                    await response.Content.ReadAsStringAsync());

                    var games = new List<GameInfo>();

                    foreach (var game in data.Response.Games)
                    {
                        var gameInfo = new GameInfo
                        (
                            "http://media.steampowered.com/steamcommunity/public/images/apps/" +
                            game.appid.ToString() + "/" + game.img_icon_url.ToString() + ".jpg",
                            game.appid,
                            game.name,
                            Math.Round(game.playtime_forever / 60, 2),
                            game.rtime_last_played
                        );
                        games.Add(gameInfo);
                    }
                    return games;
                }
            }
            return null;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}