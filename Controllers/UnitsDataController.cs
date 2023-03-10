using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPG_MV_Trans_API.Controllers
{
    /// <summary>
    /// Контроллер управления данными проекта
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsDataController : ControllerBase
    {
        TranslationContext context = new TranslationContext();
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnitsDataController() { }

        /// <summary>
        /// Получить строку.
        /// </summary>
        /// <returns>текст</returns>
        [HttpGet, Route("{gameid}/{mapid}/{id}")]
        [Authorize(Policy = "Reader")]
        public async Task<IEnumerable<TransUnit>> Get(int gameid, int mapid, int id)
        {
            try { return await Task.Run(() => context.TransEnt.FromSqlRaw($"SELECT `Id`, `MapId`, `GameId`, `Source`, `Trans`, `Time`, `User` FROM `TransEnt` WHERE `GameId`={gameid} AND `MapId`={mapid} AND `Id`={id}")); }
            catch { return null; }

        }
        /// <summary>
        /// Получить данные карты.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{gameid}/{mapid}")]
        [Authorize(Policy = "Reader")]
        public async Task<IEnumerable<TransUnit>> Get(int gameid, int mapid)
        {
            try { return await Task.Run(() => context.TransEnt.FromSqlRaw($"SELECT `Id`, `MapId`, `GameId`, `Source`, `Trans`, `Time`, `User` FROM `TransEnt` WHERE `GameId`={gameid} AND `MapId`={mapid}")); }
            catch { return null; }
        }
        /// <summary>
        /// Получить данные всех карт.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{gameid}")]
        [Authorize(Policy = "Reader")]
        public async Task<IEnumerable<TransUnit>> Get(int gameid)
        {
            try { return await Task.Run(() => context.TransEnt.FromSqlRaw($"SELECT `Id`, `MapId`, `GameId`, `Source`, `Trans`, `Time`, `User` FROM `TransEnt` WHERE `GameId`={gameid}")); }
            catch { return null; }
        }
        /// <summary>
        /// Перевод текста
        /// </summary>
        /// <param name="trans">Перевод</param>
        /// <returns>Результат выполнения</returns>
        [HttpPut]
        [Authorize(Policy = "Editor")]
        public async Task Put([FromBody] List<TransReq> trans)
        {
            bool delay = false;
            List<TransUnit> units = new List<TransUnit>();
            List<TransLog> logs = new List<TransLog>();
            for (int i = 0; i < trans.Count; i++)
            {
                TransUnit? unit = await Task.Run(() => context.TransEnt.SingleOrDefaultAsync(u => u.GameId == trans[i].GameId && u.MapId == trans[i].MapId && u.Id == trans[i].Id));
                if (unit != null && unit.Time < trans[i].Time)
                {
                    delay = false;
                    unit.Trans = trans[i].Trans;
                    unit.User = trans[i].User;
                    unit.Time = trans[i].Time;
                    units.Add(unit);
                    logs.Add(new TransLog(trans[i].GameId, trans[i].MapId, trans[i].Id, DateTime.Now, trans[i].User));
                }
                else
                {
                    delay = true;
                }
            }
            if (delay == false)
            {
                await Task.Run(() => context.TransEnt.UpdateRange(units));
                await Task.Run(() => context.SaveChangesAsync());
                await Task.Run(() => TranslationLogController.Log.AddRange(logs));
            }
        }
    }
}
