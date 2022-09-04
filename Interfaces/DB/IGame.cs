
/// <summary>
/// Интерфейс игры для БД.
/// </summary>
public interface IGame
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Название игры.
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// Версия игры.
    /// </summary>
    string Version { get; set; }
    /// <summary>
    /// Автор.
    /// </summary>
    string Author { get; set; }
    /// <summary>
    /// Язык исходника.
    /// </summary>
    string SourceLang { get; set; }
    /// <summary>
    /// Язык перевода.
    /// </summary>
    string TransLang { get; set; }
    /// <summary>
    /// Описание.
    /// </summary>
    string Description { get; set; }
    /// <summary>
    /// Время добавления в БД.
    /// </summary>
    DateTime CreationDate { get; set; }
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    string Creator { get; set; }
}
