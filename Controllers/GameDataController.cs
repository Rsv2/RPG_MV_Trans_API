using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace RPG_MV_Trans_API.Controllers
{
    /// <summary>
    /// Контроллер заливки данных игры и изменения основной информации.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GameDataController : ControllerBase
    {
        TranslationContext context = new TranslationContext();
        public GameDataController() { }
        /// <summary>
        /// Запись данных в БД.
        /// </summary>
        /// <param name="GameData">Данные для добавления в БД: 0 - заголовок и описание проекта, 1 - Список карт проекта, 2 - Список данных карт проекта</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async void Post([FromBody] Uploading GameData)
        {
            if (GameData.Type == 0)
            {
                Console.WriteLine(GameData.UploadingData);
                await context.GamesEnt.AddAsync(JsonConvert.DeserializeObject<Game>(GameData.UploadingData));
                await context.SaveChangesAsync();
            }
            else if (GameData.Type == 1)
            {
                await context.MapEnt.AddRangeAsync(JsonConvert.DeserializeObject<List<Map>>(GameData.UploadingData));
                await context.SaveChangesAsync();
            }
            else if (GameData.Type == 2)
            {
                await context.TransEnt.AddRangeAsync(JsonConvert.DeserializeObject<List<TransUnit>>(GameData.UploadingData));
                await context.SaveChangesAsync();
            }
            await context.SaveChangesAsync();
        }
        /// <summary>
        /// Изменение названия и/или описания проекта.
        /// </summary>
        /// <param name="Game"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task Put([FromBody] Game Game)
        {
            Game? temp = context.GamesEnt.ToList().Find(u => u.Id == Game.Id);
            temp.Name = Game.Name;
            temp.Version = Game.Version;
            temp.Author = Game.Author;
            temp.SourceLang = Game.SourceLang;
            temp.TransLang = Game.TransLang;
            temp.Description = Game.Description;
            context.GamesEnt.Update(temp);
            await context.SaveChangesAsync();
        }
        /// <summary>
        /// Получить список проектов.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "TransRPGMakerMVMZ")]
        public async Task<IEnumerable<Game>>? Get()
        {
            try { return await context.GamesEnt.ToListAsync(); }
            catch { return null; }
        }
        /// <summary>
        /// Удалить проект по Id.
        /// </summary>
        /// <param name="id">Id удаляемого проекта</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        [Authorize(Roles = "admin")]
        public async void Delete(int id)
        {
            Game game = context.GamesEnt.ToList().Find(u => u.Id == id);
            List<Map> maps = context.MapEnt.Local.ToList().FindAll(u => u.GameId == game.Id);
            for (int j = 0; j < maps.Count; j++)
            {
                List<TransUnit> units = context.TransEnt.ToList().FindAll(u => u.GameId == game.Id && u.MapId == maps[j].Id);
                context.RemoveRange(units.ToArray());
                await context.SaveChangesAsync();
            }
            context.RemoveRange(maps.ToArray());
            context.Remove(game);
            await context.SaveChangesAsync();
        }
    }
}
