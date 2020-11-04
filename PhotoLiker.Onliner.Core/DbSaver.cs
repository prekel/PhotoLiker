using System.Linq;
using System.Threading.Tasks;

using NLog;

using PhotoLiker.Core;

namespace PhotoLiker.Onliner.Core
{
    public class DbSaver : AbstractWorker
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        private Onliner Onliner { get; }

        public DbSaver(Onliner onliner)
        {
            Onliner = onliner;
        }

        public override async Task Begin()
        {
            Log.Info("Запущен DbSaver");

            await using var context = new OnlinerContext();

            var a = context.Checks
                .Where(u => u.VkId == 113647880)
                .OrderByDescending(u => u.Time);
            var b = context.Checks
                .Where(u => u.VkId == 113647880)
                .OrderByDescending(u => u.Time)
                .Skip(1).ToList();
            
            var c = a.Zip(b,
                    (check1, check2) => new
                        {check1.VkId, check1.Time, Online1 = check1.Online, Online2 = check2.Online})
                .Where(u => u.Online1 != u.Online2);
            
            var d = c.ToList();

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

            //await context.SaveChangesAsync();
        }
    }
}
