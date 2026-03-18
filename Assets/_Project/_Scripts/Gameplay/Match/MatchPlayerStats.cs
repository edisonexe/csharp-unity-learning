namespace _Project._Scripts.Gameplay.Match
{
    public struct MatchPlayerStats
    {
        public uint NetId;
        public string Nickname;
        public int Kills;
        public int Deaths;
        public int Score;
        public int JoinOrder;

        public MatchPlayerStats(uint netId, string nickname, int kills, int deaths, int score, int joinOrder)
        {
            NetId = netId;
            Nickname = nickname;
            Kills = kills;
            Deaths = deaths;
            Score = score;
            JoinOrder = joinOrder;
        }

        public MatchPlayerStats AddKill()
        {
            var kills = Kills + 1;
            return new MatchPlayerStats(NetId, Nickname, kills, Deaths, kills, JoinOrder);
        }

        public MatchPlayerStats AddDeath()
        {
            return new MatchPlayerStats(NetId, Nickname, Kills, Deaths + 1, Score, JoinOrder);
        }
    }
}