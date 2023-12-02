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
        private SteamUser steamUser { get; set; } = new();
        private const string apiKey = "EDF4805634DD51C580C147C7793563F4";
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            // Получение информации об играх пользователя по его steamId
            var games = await GetGamesBySteamId(steamUser);

            // Передаем данные на представление
            return View(games);
        }

        private async Task<List<GameInfo>> GetGamesBySteamId(SteamUser steamUser)
        {
            steamUser.SteamId = "76561198124071517";

            var url1 = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={apiKey}&steamids={steamUser.SteamId}";
            using (HttpClient client1 = new HttpClient())
            {
                var response = await client1.GetAsync(url1);
                if (response.IsSuccessStatusCode)
                {
                    var accountInfo = JsonConvert.DeserializeObject<dynamic>(
                    await response.Content.ReadAsStringAsync());

                    // Извлечение имени аккаунта и его иконки
                    steamUser.NameUser = accountInfo["response"]["players"][0]["personaname"].ToString();
                    steamUser.IconUser = accountInfo["response"]["players"][0]["avatarfull"].ToString();
                    ViewData["accountName"] = steamUser.NameUser;
                    ViewData["accountIcon"] = steamUser.IconUser;
                }
            }

            var url = $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={apiKey}&steamid={steamUser.SteamId}&include_appinfo=true";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<SteamApiResponse>(
                    await response.Content.ReadAsStringAsync());

                    // Преобразование данных из формата Steam API в список игр
                    var games = new List<GameInfo>();
                    foreach (var game in data.response.games)
                    {
                        var gameInfo = new GameInfo
                        {
                            IconUrl = "http://media.steampowered.com/steamcommunity/public/images/apps/" + game.appid.ToString() + "/" + game.img_icon_url.ToString() + ".jpg",
                            AppId = game.appid,
                            Name = game.name,
                            Playtime = Math.Round(game.playtime_forever / 60, 1),
                        };
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