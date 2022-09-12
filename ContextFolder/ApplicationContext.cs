using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RPG_MV_Trans_API.Models;

namespace RPG_MV_Trans_API.ContextFolder
{
    /// <summary>
    /// Контекст БД Identity.
    /// </summary>
    public class ApplicationContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="options">настройки по умолчанию</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {        }
    }
}
