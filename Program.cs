using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

using VkApiOAuth;

using VkNet;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

using Newtonsoft.Json;

namespace PhotoLiker
{
	public class Program
	{
		public class Config
		{
			public ulong AppId { get; set; } 
			
			public string Login { get; set; }

			public enum AppMode { AutoLiker = 1, Liker = 2, GoodMorning = 4, Saver = 8 }
			public AppMode Mode { get; set; }

			public long[] AutoLikerIds { get; set; }

			public TimeSpan LikerWaitOfflineTime { get; set; }
			public long LikerId { get; set; }
			public int LikerCount { get; set; }

			public long GoodMorningId { get; set; }
			public string GoodMorningMessage { get; set; }
			public TimeSpan GoodMorningDelay { get; set; }

			public long SaverId { get; set; }
		}

		public static void Main(string[] args)
		{
			var api = new VkApi();
			var r = new Random();

			var configjson = new StreamReader("config.json", Encoding.Default).ReadToEnd();
			var config = JsonConvert.DeserializeObject<Config>(configjson);

			{
				var email = config.Login;

				var pass = new StringBuilder();
				ConsoleKeyInfo key;
				Console.Write("Enter password: ");
				while (true)
				{
					key = Console.ReadKey(true);
					if (key.Key == ConsoleKey.Enter) break;
					pass.Append(key.KeyChar);
					Console.Write("*");
					if (key.Key == ConsoleKey.Backspace) pass.Clear();
				};
				Console.WriteLine();

				//var appID = 0ul;
				var appID = config.AppId;
				var scope = Settings.All;

				api.Authorize(new ApiAuthParams
				{
					ApplicationId = appID,
					Login = email,
					Password = pass.ToString(),
					Settings = scope
				});
			}

			if (config.Mode.HasFlag(Config.AppMode.AutoLiker))
			{
				new AutoLiker(api, config.AutoLikerIds)
					.Begin();
			}

			if (config.Mode.HasFlag(Config.AppMode.Liker))
			{
				new SavedPhotoLiker(api, config.LikerId, config.LikerCount, config.LikerWaitOfflineTime)
					.Begin();
			}

			if (config.Mode.HasFlag(Config.AppMode.GoodMorning))
			{
				new GoodMorning(api, config.GoodMorningId, config.GoodMorningMessage, config.GoodMorningDelay)
					.Begin();
			}

			if (config.Mode.HasFlag(Config.AppMode.Saver))
			{
				new SavedPhotoSaver(api, config.SaverId)
					.Begin();
			}

			while (true)
			{
				Thread.Sleep(50000);
			}
		}
	}
}
