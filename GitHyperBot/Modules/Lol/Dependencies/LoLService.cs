using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GitHyperBot.Core.Config;
using GitHyperBot.Core.Handlers;

namespace GitHyperBot.Modules.Lol.Dependencies
{
    public class LoLService
    {
        public static async Task SendLoLStatistics(string region, string summonername, ISocketMessageChannel channel)
        {
            var hch = new HttpClientHandler
            {
                Proxy = null,
                UseProxy = false
            };
            using (var http = new HttpClient(hch))
            {
                summonername = summonername.Replace(" ", "%20");
                region = region.ToLower();
                var regions = new[] { "ru", "kr", "br", "oc", "jp", "na", "eun", "euw", "tr", "la" };
                if (!regions.Contains(region, StringComparer.CurrentCultureIgnoreCase))
                {
                    await channel.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Erro...","Região não encontrada",EmbedMessageType.Error,false));
                }
                else
                {
                    if (region != "ru" && region != "kr") region += "1";

                    try
                    {
                        var profileData = LoLProfile.FromJson(await http.GetStringAsync(
                            $"https://{region}.api.riotgames.com/lol/summoner/v3/summoners/by-name/{summonername}?api_key={Config.Bot.LoLApiKey}"));

                        var matchList = LoLMatchlist.FromJson(await http.GetStringAsync(
                            $"https://{region}.api.riotgames.com/lol/match/v3/matchlists/by-account/{profileData.AccountId}?endIndex=5&api_key={Config.Bot.LoLApiKey}"));

                        var rolelist = new List<string>();
                        var upper = matchList.Matches.Length > 5 ? 5 : matchList.Matches.Length;
                        var taskList = new List<Task<LoLResult>>();
                        for (var i = upper - 1; i >= 0; i--)
                            taskList.Add(GetLoLStatsAsync(region, matchList.Matches[i].GameId,
                                profileData.AccountId, matchList.Matches[i], http));

                        rolelist.AddRange(taskList.Select(x => x.Result.Role));
                        var kills = taskList.Sum(x => x.Result.Kills);
                        var deaths = taskList.Sum(x => x.Result.Deaths);
                        var wins = taskList.Sum(x => x.Result.Win);
                        var minionKills = taskList.Sum(x => x.Result.MinionKills);
                        var leaguearray = LoLLeague.FromJson(await http.GetStringAsync(
                            $"https://{region}.api.riotgames.com/lol/league/v3/positions/by-summoner/{profileData.Id}?api_key={Config.Bot.LoLApiKey}"));

                        var league = leaguearray.FirstOrDefault();

                        //var league = leaguearray.Length > 0 ? leaguearray.FirstOrDefault() : 
                        //    new LoLLeague
                        //    {
                        //        FreshBlood = false,
                        //        HotStreak = false,
                        //        Inactive = true,
                        //        LeagueId = "",
                        //        LeagueName = "unranked",
                        //        LeaguePoints = 0,
                        //        Losses = 0,
                        //        PlayerOrTeamId = "",
                        //        PlayerOrTeamName = "",
                        //        QueueType = "None",
                        //        Tier = "Unranked",
                        //        Rank = "",
                        //        Veteran = false
                        //    };

                        if (league == null)
                        {
                            await channel.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Erro!", "Não foi possível coletar os daddos", EmbedMessageType.Error, false));
                            return;
                        }

                        var leaguepoints = league.LeaguePoints == 0 ? "" : league.LeaguePoints + " PDL";

                        var latestVersion = LoLVersionsData.FromJson(
                            await http.GetStringAsync("https://ddragon.leagueoflegends.com/api/versions.json"))[0];
                        var icon = LoLProfilePictureData.
                            FromJson(await http.GetStringAsync(
                                $"http://ddragon.leagueoflegends.com/cdn/{latestVersion}/data/en_US/profileicon.json"))
                            .Data.FirstOrDefault(x => x.Value.Id == profileData.ProfileIconId).Value.Image.Full;
                        var profileUrl =
                            $"http://ddragon.leagueoflegends.com/cdn/{latestVersion}/img/profileicon/{icon}";

                        var tierpng = league.Tier.ToLower() + (string.Equals(league.Tier, "Unranked",
                                          StringComparison.CurrentCultureIgnoreCase)
                                          ? ""
                                          : "_") + (string.Equals(league.Tier, "Unranked",
                                          StringComparison.CurrentCultureIgnoreCase)
                                          ? ""
                                          : RomanNumberService.RomanToInteger(league.Rank).ToString());
                        var eb = new EmbedBuilder();
                        eb.WithAuthor($"{profileData.Name} (Nível {profileData.SummonerLevel})", profileUrl)
                            .WithColor(Color.Blue)
                            .WithThumbnailUrl(
                                $"http://raw.communitydragon.org/latest/plugins/rcp-fe-lol-postgame/global/default/images/{tierpng}.png")
                            .WithFooter($"Status das ultimas {upper} partidas",
                                "https://vignette.wikia.nocookie.net/leagueoflegends/images/1/12/League_of_Legends_Icon.png/revision/latest?cb=20150414082403&path-prefix=de")
                            .WithDescription(
                                $"**Abates/Mortes:** {kills}/{deaths} ({Math.Round(kills / (double)deaths, 4)})\n" +
                                $"**Vitórias/Derrotas:** {wins}/{upper - wins} ({Math.Round(wins / (upper - (double)wins), 4)})\n" +
                                $"**Minions matados:** {minionKills}\n" +
                                $"**Liga:** {league.Tier[0].ToString().ToUpper() + league.Tier.Substring(1).ToLower()} {league.Rank} {leaguepoints}\n" +
                                $"**Ranked Vitórias/Derrotas:** {league.Wins}/{league.Losses} ({Math.Round(league.Wins / (double)league.Losses, 4)})\n" +
                                $"**Fila favorita:** {rolelist.Max()}");
                        await channel.SendMessageAsync("", false, eb.Build());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        await channel.SendMessageAsync("", false, EmbedHandler.CriarEmbed("Erro!", "Invocador não encontrado", EmbedMessageType.Error, false));
                    }
                }
            }
        }

        private static async Task<LoLResult> GetLoLStatsAsync(string region, long matchid, long accountid, Match match,
            HttpClient http)
        {
            var matchData = LoLMatch.FromJson(await http.GetStringAsync(
                $"https://{region}.api.riotgames.com/lol/match/v3/matches/{matchid}?api_key={Config.Bot.LoLApiKey}"));
            var first = matchData.ParticipantIdentities.FirstOrDefault(x => x.Player.AccountId == accountid);

            var participantId = first.ParticipantId;
            var data = matchData.Participants.FirstOrDefault(x => x.ParticipantId == participantId);
            var stats = data.Stats;

            var result = new LoLResult
            {
                Kills = (int)stats.Kills,
                Deaths = (int)stats.Deaths,
                Win = stats.Win ? 1 : 0,
                MinionKills = (int)stats.TotalMinionsKilled,
                Role = match.Role.ToString()
            };
            return result;
        }

        private class LoLResult
        {
            public int Deaths;
            public int Kills;
            public int MinionKills;
            public string Role;
            public int Win;
        }
    }
}