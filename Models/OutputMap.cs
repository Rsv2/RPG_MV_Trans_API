namespace RPG_MV_Trans_API
{
    public class OutputMap
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Trans { get; set; }
        public int Type { get; set; }

        public OutputMap(int id, string name, int gameId, string source, string trans, int type)
        {
            Id = id;
            GameId = gameId;
            Name = name;
            Source = source;
            Trans = trans;
            Type = type;
        }
    }
}
