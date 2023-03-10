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
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GameDataController() { }
        /// <summary>
        /// Запись данных в БД.
        /// </summary>
        /// <param name="GameData">Данные для добавления в БД: 0 - заголовок и описание проекта, 1 - Список карт проекта, 2 - Список данных карт проекта</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task Post([FromBody] Uploading GameData)
        {
            if (GameData.Type == 0)
            {
                await Task.Run(() => context.GamesEnt.AddAsync(JsonConvert.DeserializeObject<Game>(GameData.UploadingData)));
            }
            else if (GameData.Type == 1)
            {
                await Task.Run(() => context.MapEnt.AddRangeAsync(JsonConvert.DeserializeObject<List<Map>>(GameData.UploadingData)));
            }
            else if (GameData.Type == 2)
            {
                await Task.Run(() => context.TransEnt.AddRangeAsync(JsonConvert.DeserializeObject<List<TransUnit>>(GameData.UploadingData)));
            }
            await Task.Run(() => context.SaveChangesAsync());
        }
        /// <summary>
        /// Изменение названия и/или описания проекта.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task Put([FromBody] Game game)
        {
            await Task.Run(() => context.Database.ExecuteSqlRawAsync($"UPDATE `GamesEnt` SET `Name`=\"{game.Name}\",`TitlePic`=\"{game.TitlePic}\"," +
                $"`Version`=\"{game.Version}\",`Author`=\"{game.Author}\",`SourceLang`=\"{game.SourceLang}\",`TransLang`=\"{game.TransLang}\"," +
                $"`Creator`=\"{game.Creator}\",`Description`= \"{game.Description}\",`UrlExtra`= \"{game.UrlExtra}\",`Substitution`= \"{game.Substitution}\"" +
                $" WHERE `Id`= {game.Id};"));
        }
        /// <summary>
        /// Получить список проектов.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Reader")]
        public async Task<IEnumerable<Game>>? Get()
        {
            try { return await Task.Run(() => context.GamesEnt.FromSqlRaw($"SELECT `Id`, `Name`, `TitlePic`, `CreationDate`, `Version`, `Author`, `SourceLang`," +
                $" `TransLang`, `Creator`, `Description`, `UrlExtra`, `Substitution` FROM `GamesEnt`")); }
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
            int gameId = context.GamesEnt.First(u => u.Id == id).Id;
            await Task.Run(() => context.Database.ExecuteSqlRawAsync($"DELETE FROM `TransEnt` WHERE `GameId` = {gameId};"));
            await Task.Run(() => context.Database.ExecuteSqlRawAsync($"DELETE FROM `MapEnt` WHERE `GameId` = {gameId};"));
            await Task.Run(() => context.Database.ExecuteSqlRawAsync($"DELETE FROM `GamesEnt` WHERE `Id` = {id};"));
            await Task.Run(() => context.SaveChangesAsync());
        }
    }
}
