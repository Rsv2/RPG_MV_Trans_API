using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG_MV_Trans_API
{
    /// <summary>
    /// Игра
    /// </summary>
    public class Game : IGame
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string SourceLang { get; set; }
        public string TransLang { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Game() { }

        /// <summary>
        /// Конструктор добавления.
        /// </summary>
        /// <param name="name">Название игры</param>
        /// <param name="creator">Имя заливщика</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="version">Версия</param>
        /// <param name="author">Автор</param>
        /// <param name="sourcelang">Язык исходника</param>
        /// <param name="translang">Яхык перевода</param>
        /// <param name="description">Описание</param>
        public Game(int id, string name, string version, string author, string sourcelang, string translang, string description, string creator)
        {
            Id = id;
            Name = name;
            Version = version;
            Author = author;
            SourceLang = sourcelang;
            TransLang = translang;
            Description = description;
            Creator = creator;
            CreationDate = DateTime.Now;
        }
    }
}
