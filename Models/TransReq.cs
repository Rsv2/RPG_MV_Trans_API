namespace RPG_MV_Trans_API
{
    /// <summary>
    /// Запрос на получение данных
    /// </summary>
    public class TransReq : ITransReq
    {
        public int GameId { get; set; }
        public int MapId { get; set; }
        public int Id { get; set; }
        public string User { get; set; }
        public string Trans { get; set; }
        public DateTime Time { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public TransReq() { }
    }
}
