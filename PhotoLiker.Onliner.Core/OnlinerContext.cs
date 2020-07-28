using Microsoft.EntityFrameworkCore;

using NLog;

namespace PhotoLiker.Onliner.Core
{
    public class OnlinerContext : DbContext
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        public DbSet<OnlineCheck> Checks { get; set; }

        public OnlinerContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=onliner;Username=postgres;Password=qwerty123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OnlineCheck>(entity =>
            {
                entity.HasKey(t => new {t.VkId, t.Time});

                //entity.Property(t => t.Time).IsRequired();
                //entity.Property(t => t.VkId).IsRequired();
                entity.Property(t => t.Online).IsRequired();
                entity.Property(t => t.OnlineMobile);
            });
        }
    }
}
