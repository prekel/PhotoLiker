using Microsoft.EntityFrameworkCore;

using NLog;

namespace PhotoLiker.Onliner.Core
{
    public class OnlinerContext : DbContext
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        public OnlinerContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=demo;Username=postgres;Password=qwerty123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OnlineCheck>(entity =>
            {
                entity.HasKey(t => t.CheckId);

                entity.Property(t => t.Time).IsRequired();
                entity.Property(t => t.VkId).IsRequired();
                entity.Property(t => t.Online).IsRequired();
                entity.Property(t => t.OnlineMobile).IsRequired();
                entity.Property(t => t.OnlineApp).IsRequired();
            });
        }
    }
}
