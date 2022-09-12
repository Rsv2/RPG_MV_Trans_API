﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RPG_MV_Trans_API.Controllers
{
    /// <summary>
    /// Контроллер обмена логами перевода.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "TransRPGMakerMVMZ")]
    public class TranslationLogController : ControllerBase
    {
        TranslationContext context = new TranslationContext();
        /// <summary>
        /// Коллекция логов.
        /// </summary>
        public static List<TransLog> Log { get; set; }
        static TranslationLogController()
        {
            Log = new List<TransLog>();
        }
        /// <summary>
        /// Получить список логов.
        /// </summary>
        /// <returns>логи</returns>
        [HttpGet]
        public async Task<IEnumerable<TransLog>>? Get()
        {
            if (Log.Count > 0 && DateTime.Now > Log[Log.Count - 1].Time.AddMinutes(1))
            {
                Log.Clear();
            }
            return Log;
        }
    }
}