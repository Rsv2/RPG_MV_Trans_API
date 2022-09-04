using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG_MV_Trans_API
{
    /// <summary>
    /// Элемент перевода
    /// </summary>
    public class TransUnit : ITransUnit
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int GameId { get; set; }
        public string Source { get; set; }
        public string Trans { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public TransUnit() { }

        /// <summary>
        /// Конструктор добавления элемента карты
        /// </summary>
        /// <param name="mapId">Идентификатор карты</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="source">Исходная строка</param>
        /// <param name="trans">Строка перевода</param>
        public TransUnit(int id, int mapId, int gameId, string source, string trans, string user)
        {
            Id = id;
            MapId = mapId;
            GameId = gameId;
            Source = source;
            Trans = trans;
            Time = DateTime.Now;
            User = user;
        }
    }
}
