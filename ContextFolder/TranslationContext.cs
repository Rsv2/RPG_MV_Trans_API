using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace RPG_MV_Trans_API
{
    public class TranslationContext : DbContext
    {
        public DbSet<Map> MapEnt { get; set; }
        public DbSet<TransUnit> TransEnt { get; set; }
        public DbSet<Game> GamesEnt { get; set; }

        public TranslationContext()
        {
            Database.EnsureCreated();
            MapEnt.Load();
            TransEnt.Load();
            GamesEnt.Load();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>().HasKey(u => new { u.Id, u.GameId });
            modelBuilder.Entity<TransUnit>().HasKey(u => new { u.MapId, u.Id, u.GameId });
        }

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
