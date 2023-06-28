using Newtonsoft.Json;
using System.Net;

public class Program
{
    public async static Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public async static Task<int> getTotalScoredGoalsAsync(string team, int year)
    {
        int totalGoals = 0;
        
        for (int i = 1; i < 3; i++)
        {
            int page = 1;
            bool hasMoreResults = true;

            while (hasMoreResults)
            {
                var uriBuilder = new UriBuilder("https://jsonmock.hackerrank.com/api/football_matches");
                uriBuilder.Query = $"year={year}&team{i}={team}&page={page}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        Request data = JsonConvert.DeserializeObject<Request>(json);

                        foreach (var match in data.Data)
                        {
                            int goalsTeam = (int)match.GetType().GetProperty($"Team{i}goals").GetValue(match, null);
                            totalGoals += goalsTeam;
                        }

                        if (page >= data.Total_pages)
                        {
                            hasMoreResults = false;
                        }
                        else
                        {
                            page++;
                        }
                    }
                    else
                    {
                        // Handle the error response
                        Console.WriteLine("Error: " + response.StatusCode);
                        hasMoreResults = false;
                    }
                }
            }
        }

        return totalGoals;
    }

    public class Request
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public List<Match> Data { get; set; }
    }

    public class Match
    {
        public string Competition { get; set; }
        public int Year { get; set; }
        public string Round { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public int Team1goals { get; set; }
        public int Team2goals { get; set; }

    }
}