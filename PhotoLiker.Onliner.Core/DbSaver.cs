using System.Threading.Tasks;

using NLog;

using PhotoLiker.Core;

namespace PhotoLiker.Onliner.Core
{
    public class DbSaver : AbstractWorker
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();
        
        private Onliner Onliner { get;  }
        
        public DbSaver(Onliner onliner)
        {
            Onliner = onliner;
        }

        public override async Task Begin()
        {
            Log.Info("Запущен DbSaver");
            
            Onliner.OnlinerChecked += OnlinerOnOnlinerChecked;
            while (true)
            {
                await Task.Delay(50000);
            }
        }

        private async void OnlinerOnOnlinerChecked(object sender, OnlinerEventArgs e)
        {
            Log.Debug("Сохранение в БД");
            await using var context = new OnlinerContext();
            
            await context.Checks.AddRangeAsync(e.Checks);

            await context.SaveChangesAsync();
        }
    }
}
