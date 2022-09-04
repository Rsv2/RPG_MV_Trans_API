/// <summary>
/// Интерфейс карты.
/// </summary>
public interface IMap
{
    /// <summary>
    /// Идентификатор карты.
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Id игры карты.
    /// </summary>
    int GameId { get; set; }
    /// <summary>
    /// Имя карты.
    /// </summary>
    string Name { get; set; }
}
