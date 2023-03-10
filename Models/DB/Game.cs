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
        public string TitlePic { get; set; }
        public DateTime CreationDate { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string SourceLang { get; set; }
        public string TransLang { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string UrlExtra { get; set; }
        public string Substitution { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public Game() { }

        /// <summary>
        /// Конструктор добавления.
        /// </summary>
        /// <param name="name">Название игры</param>
        /// <param name="titlepic">Url титульной картинки</param>
        /// <param name="creator">Имя заливщика</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="version">Версия</param>
        /// <param name="author">Автор</param>
        /// <param name="sourcelang">Язык исходника</param>
        /// <param name="translang">Яхык перевода</param>
        /// <param name="description">Описание</param>
        /// <param name="urlExtra">Url архива с дополнительными файлами (переведённые картинки, java scripts и пр.)</param>
        /// /// <param name="substitution">Заменяемые карты, через пробел, обязательно указывать расширение .json</param>
        public Game(int id, string name, string titlepic, string version, string author, string sourcelang, string translang, string description, string creator, string urlExtra, string substitution)
        {
            Id = id;
            Name = name;
            TitlePic = titlepic;
            Version = version;
            Author = author;
            SourceLang = sourcelang;
            TransLang = translang;
            Description = description;
            Creator = creator;
            CreationDate = DateTime.Now;
            UrlExtra = urlExtra;
            Substitution = substitution;
        }
    }
}
