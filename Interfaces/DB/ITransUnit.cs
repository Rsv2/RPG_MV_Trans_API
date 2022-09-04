/// <summary>
/// Интерфейс элемента перевода карты для БД.
/// </summary>
public interface ITransUnit
{
    /// <summary>
    /// Id игры.
    /// </summary>
    int GameId { get; set; }
    /// <summary>
    /// Идентификатор карты.
    /// </summary>
    int MapId { get; set; }
    /// <summary>
    /// Идентификартор в карте.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Время последнего изменения.
    /// </summary>
    DateTime Time { get; set; }
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    string User { get; set; }
    /// <summary>
    /// Строка исходник
    /// </summary>
    string Source { get; set; }
    /// <summary>
    /// Строка перевод.
    /// </summary>
    string Trans { get; set; }
}
