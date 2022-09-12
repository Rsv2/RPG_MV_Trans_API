/// <summary>
/// Интерфейс запроса на получение данных.
/// </summary>
public interface ITransReq
{
    /// <summary>
    /// Id проекта.
    /// </summary>
    int GameId { get; set; }
    /// <summary>
    /// Id карты.
    /// </summary>
    int MapId { get; set; }
    /// <summary>
    /// id перевода.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Переводчик.
    /// </summary>
    string User { get; set; }
    /// <summary>
    /// Перевод
    /// </summary>
    string Trans { get; set; }
}
