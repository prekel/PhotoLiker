using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NLog;

//using VkApiOAuth;

using VkNet;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace PhotoLiker.Core
{
	[Obsolete]
	public class HappyNewYear : AbstractVkWorker
	{
		private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

		public long Id { get; private set; }
		public string Morning { get; private set; }
		public DateTimeOffset Time { get; private set; }

		public HappyNewYear(VkApi api, long id, string morning, DateTimeOffset time) : base(api)
		{
			Id = id;
			Morning = morning;
			Time = time;
		}

		public override async Task Begin()
		{
			Log.Info("Запуск HappyNewYear");
			
			var r = new Random();
			await Task.Delay(Time - DateTimeOffset.Now);

			await Api.Account.SetOnlineAsync();
			await Api.Messages.SendAsync(new MessagesSendParams() { UserId = Id, Message = Morning, RandomId = r.Next() });

			//while (true)
			//{
			//	try
			//	{
			//		var usr = (await Api.Users.GetAsync(new long[] { Id }, ProfileFields.Online)).First().Online;
			//		if (usr.HasValue && usr.Value)
			//		{
			//			await Api.Account.SetOnlineAsync();
			//			await Api.Messages.SendAsync(new MessagesSendParams() { UserId = Id, Message = Morning, RandomId = r.Next() });
			//			break;
			//		}
			//		//await Task.Delay(r.Next(333, 1500));
			//		await Task.Delay(r.Next(1000, 3000));
			//	}
			//	catch (IndexOutOfRangeException)
			//	{
			//	}
			//	catch (Exception e)
			//	{
			//		Console.WriteLine(e.Message);
			//		await Task.Delay(r.Next(1000, 2000));
			//	}
			//}
		}
	}
}