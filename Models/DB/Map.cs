using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG_MV_Trans_API
{
    /// <summary>
    /// Карта
    /// </summary>
    public class Map : IMap
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Map() { }

        /// <summary>
        /// Конструктор добавления карты.
        /// </summary>
        /// <param name="gameId">Идентификатор игры карты</param>
        /// <param name="name">Название карты.</param>
        public Map(int id, int gameId, string name)
        {
            GameId = gameId;
            Name = name;
            Id = id;
        }
    }
}
