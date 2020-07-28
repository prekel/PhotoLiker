using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

using PhotoLiker.Core;

using VkNet.NLog.Extensions.Logging;
using VkNet.NLog.Extensions.Logging.Extensions;

using NLog;

namespace PhotoLiker.Console
{
    public class Program
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        private static Config GetConfigOrCreateAndExitIfNotExist()
        {
            Config config;
            if (File.Exists("config.json"))
            {
                Log.Info("Загружается конфирурация");
                var configjson = new StreamReader("config.json", Encoding.Default).ReadToEnd();
                config = JsonConvert.DeserializeObject<Config>(configjson);
                Log.Info("Загружена конфигурация");
            }
            else
            {
                Log.Warn("Нет файла конфигурации ");
                Log.Info("Создаётся и сохраняется конфигурция");
                
                config = new Config();
                
                using (var w = new StreamWriter("config.json"))
                {
                    w.WriteLine(JsonConvert.SerializeObject(config, Formatting.Indented));
                }

                Log.Fatal("Завершение программы");
                LogManager.Shutdown();
                Environment.Exit(-1);
            }

            return config;
        }

        public static void Main(string[] args)
        {

            System.Console.OutputEncoding = Encoding.UTF8;
            System.Console.InputEncoding = Encoding.UTF8;
            System.Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            
            var services = new ServiceCollection();
            // Регистрация логгера
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                });
            });
            
            LogManager.LoadConfiguration("NLog.config");
            LogManager.Configuration.Variables["starttime"] = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff");
            
            var api = new VkApi();

            var config = GetConfigOrCreateAndExitIfNotExist();
            
            

            Authorize(config, api);

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
                foreach (var i in config.GoodMornings)
                {
                    new GoodMorning(api, i.GoodMorningId, i.GoodMorningMessage, i.GoodMorningDelay)
                        .Begin();
                }
            }

            if (config.Mode.HasFlag(Config.AppMode.HappyNewYear))
            {
                foreach (var i in config.NewYears)
                {
                    new HappyNewYear(api, i.HappyNewYearId, i.HappyNewYearMessage, i.HappyNewYearTime)
                        .Begin();
                }
            }

            if (config.Mode.HasFlag(Config.AppMode.Saver))
            {
                new SavedPhotoSaver(api, config.SaverId)
                    .Begin();
            }

            if (config.Mode.HasFlag(Config.AppMode.Onliner))
            {
                new Onliner.Core.Onliner(api, config.OnlinerIds)
                    .Begin();
            }

            while (true)
            {
                Thread.Sleep(50000);
            }
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Info("Завершение програмы");
            LogManager.Shutdown();
            Environment.Exit(0);
        }

        private static void Authorize(Config config, IVkApiAuth api)
        {
            if (config.AccessToken != null)
            {
                api.Authorize(new ApiAuthParams
                {
                    AccessToken = config.AccessToken
                });
            }
            else if (config.Login != null)
            {
                var email = config.Login;

                var pass = new StringBuilder();
                ConsoleKeyInfo key;
                System.Console.Write("Enter password: ");
                while (true)
                {
                    key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }

                    pass.Append(key.KeyChar);
                    System.Console.Write("*");
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        pass.Clear();
                    }
                }

                System.Console.WriteLine();

                var appID = config.AppId;
                var scope = Settings.All;

                api.Authorize(new ApiAuthParams
                {
                    ApplicationId = appID ?? 0,
                    Login = email,
                    Password = pass.ToString(),
                    Settings = scope
                });
            }
        }
    }
}
