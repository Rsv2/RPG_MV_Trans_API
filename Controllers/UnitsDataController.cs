using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            try { return await context.TransEnt.FromSqlRaw($"SELECT * FROM `TransEnt` WHERE `GameId`={gameid} AND `MapId`={mapid} AND `Id`={id}").ToListAsync(); }
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
            try { return await context.TransEnt.FromSqlRaw($"SELECT * FROM `TransEnt` WHERE `GameId`={gameid} AND `MapId`={mapid}").ToListAsync(); }
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
            try { return await context.TransEnt.FromSqlRaw($"SELECT * FROM `TransEnt` WHERE `GameId`={gameid}").ToListAsync(); }
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
            try
            {
                bool delay = false;
                List<TransUnit> units = new List<TransUnit>();
                List<TransLog> logs = new List<TransLog>();
                for (int i = 0; i < trans.Count; i++)
                {
                    TransUnit? unit = context.TransEnt.FirstOrDefault(u => u.GameId == trans[i].GameId && u.MapId == trans[i].MapId && u.Id == trans[i].Id);
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
                    context.TransEnt.UpdateRange(units);
                    await context.SaveChangesAsync();
                    TranslationLogController.Log.AddRange(logs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
        }
    }
}
