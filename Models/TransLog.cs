namespace RPG_MV_Trans_API
{
    /// <summary>
    /// Лог перевода.
    /// </summary>
    public class TransLog
    {
        /// <summary>
        /// Id игры
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// Id карты
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// Id элемента
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Время изменения
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Переводчик
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="gameId">Id игры</param>
        /// <param name="mapId">Id карты</param>
        /// <param name="id">Id элемента</param>
        /// <param name="time">Время изменения</param>
        /// <param name="user">Переводчик</param>
        public TransLog(int gameId, int mapId, int id, DateTime time, string user)
        {
            GameId = gameId;
            MapId = mapId;
            Id = id;
            Time = time;
            User = user;
        }
    }
}
