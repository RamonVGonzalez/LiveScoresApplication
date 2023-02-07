using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace LiveScoresApplication
{
    public class LiveScore
    {
        private static Dictionary<string, MatchResultModel> Result = new ();

        public async Task<string> GetDataFromLivescores()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api-football-v1.p.rapidapi.com/v3/timezone"),
                    Headers =
    {
        { "X-RapidAPI-Key", "edc8cedac0msh390f1fd439dcb2ap132e04jsna68b0469e6eb" },
        { "X-RapidAPI-Host", "api-football-v1.p.rapidapi.com" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
                return "Hello";
            }
            catch (Exception ex)
            {

                return $"Failed: {ex.Message}";
            }          
        }
        public async Task<string> GetDataFromFootballDataDotOrg()
        {
            try
            {
                var clubsDict = new Dictionary<int, string>
                {
                    {81, "Barcelona"},
                    {86, "RealMadrid"},
                    {61, "Chelsea"},
                    {64, "Liverpool"},
                    {66, "Manchester United"},
                    {57, "Arsenal"},
                };

                var dateFrom = new DateTime(2023, 02, 04).ToString("yyyy-MM-dd");
                var dateTo = new DateTime(2023, 02, 06).ToString("yyyy-MM-dd");
                //var dateFrom = DateTime.Now.ToString("yyyy-MM-dd");
                //var dateTo = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                var status = "FINISHED";
                var specificPage = $"v4/matches/?dateFrom={dateFrom}&dateTo={dateTo}";
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {

                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.football-data.org/{specificPage}"),
                    Headers =
                        {
                            { "X-Auth-Token", "3860bdd43fd54d2c994afd1faae90fd5" },
                        },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var scoresObject = JsonSerializer.Deserialize<Rootobject>(body);

                    if (scoresObject.resultSet.count < 1)
                    {
                        return "No Matches Found";
                    }

                    var releventMatches = scoresObject.matches.Where(
                        x => (clubsDict.ContainsKey(x.homeTeam.id)
                        || clubsDict.ContainsKey(x.awayTeam.id)) &&
                        x.status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();


                    foreach (var club in clubsDict)
                    {
                        var isHomeTeam = releventMatches.Any(x => x.homeTeam?.id == club.Key);
                        var isAwayTeam = releventMatches.Any(x => x.awayTeam?.id == club.Key);

                        if (isHomeTeam)
                        {
                            var match = releventMatches.FirstOrDefault(x => x.homeTeam.id == club.Key);

                            SaveToDictionary($"{match.id}{match.homeTeam.tla}{match.utcDate.ToString("yyyy-MM-dd")}",
                                new MatchResultModel
                                {
                                    TeamName = match.homeTeam.name,
                                    GoalCount = int.Parse(match.score.fullTime.home.ToString()),
                                    MatchDate = match.utcDate
                                });
                        }
                        if (isAwayTeam)
                        {
                            var match = releventMatches.FirstOrDefault(x => x.awayTeam.id == club.Key);

                            SaveToDictionary($"{match.id}{match.awayTeam.tla}{match.utcDate.ToString("yyyy-MM-dd")}",
                                new MatchResultModel
                                {
                                    TeamName = match.awayTeam.name,
                                    GoalCount = int.Parse(match.score.fullTime.away.ToString()),
                                    MatchDate = match.utcDate
                                });
                        }
                    }
                    Console.WriteLine($"{JsonSerializer.Serialize(Result)}");
                    return $"{JsonSerializer.Serialize(Result)}";
                }
            }
            catch (Exception ex)
            {

                return $"Failed: {ex.Message}";
            }

        }

        private static void SaveToDictionary(string matchId, MatchResultModel model)
        {
            if (!Result.ContainsKey(matchId))
            {
                Result.Add(matchId, model);
            }
        }
    }

    public class MatchResultModel
    {
        public string TeamName { get; set; }
        public int GoalCount { get; set; }
        public DateTime MatchDate { get; set; }

    }
}
