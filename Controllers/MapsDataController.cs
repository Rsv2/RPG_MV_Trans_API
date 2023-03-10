using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RPG_MV_Trans_API.Controllers
{
    /// <summary>
    /// Контроллер управления картами проекта
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Reader")]
    public class MapsDataController : ControllerBase
    {
        TranslationContext context = new TranslationContext();
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MapsDataController() { }

        /// <summary>
        /// Получить данные карт проекта.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{gameId}")]
        public async Task<IEnumerable<Map>>? Get(int gameId)
        {
            try { return await Task.Run(() => context.MapEnt.FromSqlRaw($"SELECT `Id`, `GameId`, `Name` FROM `MapEnt` WHERE `GameId`={gameId}")); }
            catch { return null; }
        }
    }
}
