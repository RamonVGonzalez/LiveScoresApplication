namespace LiveScoresApplication
{
    public class AllMatchesModel
    {
    }


    public class Rootobject
    {
        public Filters filters { get; set; }
        public Resultset resultSet { get; set; }
        public Match[] matches { get; set; }
    }

    public class Filters
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string permission { get; set; }
    }

    public class Resultset
    {
        public int count { get; set; }
        public string competitions { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public int played { get; set; }
    }

    public class Match
    {
        public Area area { get; set; }
        public Competition competition { get; set; }
        public Season season { get; set; }
        public int id { get; set; }
        public DateTime utcDate { get; set; }
        public string status { get; set; }
        public int matchday { get; set; }
        public string stage { get; set; }
        public object group { get; set; }
        public DateTime lastUpdated { get; set; }
        public Hometeam homeTeam { get; set; }
        public Awayteam awayTeam { get; set; }
        public Score score { get; set; }
        public Odds odds { get; set; }
        public object[] referees { get; set; }
    }

    public class Area
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string flag { get; set; }
    }

    public class Competition
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string emblem { get; set; }
    }

    public class Season
    {
        public int id { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int currentMatchday { get; set; }
        public object winner { get; set; }
    }

    public class Hometeam
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string tla { get; set; }
        public string crest { get; set; }
    }

    public class Awayteam
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string tla { get; set; }
        public string crest { get; set; }
    }

    public class Score
    {
        public object winner { get; set; }
        public string duration { get; set; }
        public Fulltime fullTime { get; set; }
        public Halftime halfTime { get; set; }
    }

    public class Fulltime
    {
        public object home { get; set; }
        public object away { get; set; }
    }

    public class Halftime
    {
        public object home { get; set; }
        public object away { get; set; }
    }

    public class Odds
    {
        public string msg { get; set; }
    }

}
