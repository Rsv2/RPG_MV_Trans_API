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
        public string Get(int gameid, int mapid, int id)
        {
            try
            {
                return context.TransEnt.ToListAsync().Result.Find(u => u.GameId == gameid && u.MapId == mapid && u.Id == id).Trans;
            }
            catch { return null; }
        }
        /// <summary>
        /// Получить данные карты.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{gameid}/{mapid}")]
        [Authorize(Policy = "Reader")]
        public IEnumerable<TransUnit> Get(int gameid, int mapid)
        {
            try
            {
                return context.TransEnt.ToListAsync().Result.FindAll(u => u.GameId == gameid && u.MapId == mapid);
            }
            catch { return null; }
        }
        /// <summary>
        /// Получить данные всех карт.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("{gameid}")]
        [Authorize(Policy = "Reader")]
        public IEnumerable<TransUnit> Get(int gameid)
        {
            try
            {
                return context.TransEnt.ToListAsync().Result.FindAll(u => u.GameId == gameid);
            }
            catch { return null; }
        }
        /// <summary>
        /// Перевод текста
        /// </summary>
        /// <param name="trans">Перевод</param>
        /// <returns>Результат выполнения</returns>
        [HttpPut]
        [Authorize(Policy = "Editor")]
        public async void Put([FromBody] List<TransReq> trans)
        {
            try
            {
                bool delay = false;
                List<TransUnit> units = new List<TransUnit>();
                List<TransLog> logs = new List<TransLog>();
                for (int i = 0; i < trans.Count; i++)
                {
                    TransUnit unit = context.TransEnt.ToListAsync().Result.Find(u => u.GameId == trans[i].GameId && u.MapId == trans[i].MapId && u.Id == trans[i].Id);
                    if (unit.Time < trans[i].Time)
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
