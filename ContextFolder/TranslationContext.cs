using Microsoft.EntityFrameworkCore;

namespace RPG_MV_Trans_API
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    public class TranslationContext : DbContext
    {
        /// <summary>
        /// Карты
        /// </summary>
        public DbSet<Map> MapEnt { get; set; }
        /// <summary>
        /// Элементы перевода
        /// </summary>
        public DbSet<TransUnit> TransEnt { get; set; }
        /// <summary>
        /// Игры
        /// </summary>
        public DbSet<Game> GamesEnt { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public TranslationContext()
        {
            Database.EnsureCreated();
            MapEnt.Load();
            TransEnt.Load();
            GamesEnt.Load();
        }
        /// <summary>
        /// Компоновка сущностей.
        /// </summary>
        /// <param name="modelBuilder">modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>().HasKey(u => new { u.Id, u.GameId });
            modelBuilder.Entity<TransUnit>().HasKey(u => new { u.MapId, u.Id, u.GameId });
        }
        /// <summary>
        /// Конфигурация подключения БД переводов (MySQL, MariaDB).
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("TransConnection");
            var options = optionsBuilder
                .UseMySql(connectionString,
            new MySqlServerVersion(new Version(8, 0, 25)))
            .Options;
        }
    }
}
