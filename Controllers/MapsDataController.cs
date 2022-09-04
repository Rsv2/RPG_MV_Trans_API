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
    [Authorize(Roles = "admin")]
    public class MapsDataController : ControllerBase
    {
        TranslationContext context = new TranslationContext();
        public MapsDataController() { }

        /// <summary>
        /// Получить данные карт проекта.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{gameId}")]
        public async Task<IEnumerable<Map>>? Get(int gameId)
        {
            try
            {
                List<Map> maps = await context.MapEnt.ToListAsync();
                return maps.FindAll(u => u.GameId == gameId);
            }
            catch { return null; }
        }
    }
}
