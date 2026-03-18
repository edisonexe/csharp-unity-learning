namespace _Project._Scripts.Gameplay.Match
{
    public readonly struct LeaderboardEntryData
    {
        public int Place { get; }
        public string Nickname { get; }
        public int Kills { get; }
        public int Deaths { get; }
        public int Score { get; }
        public bool IsWinner { get; }

        public LeaderboardEntryData(int place, string nickname, int kills, int deaths, int score, bool isWinner)
        {
            Place = place;
            Nickname = nickname;
            Kills = kills;
            Deaths = deaths;
            Score = score;
            IsWinner = isWinner;
        }
    }
}