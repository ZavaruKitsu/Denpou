﻿using System;
using System.Threading.Tasks;
using Denpou.Builder;
using SystemCommandsBot.Forms;

namespace SystemCommandsBot
{
    internal class Program
    {
        public static Config BotConfig { get; set; }


        public static async Task Main(string[] args)
        {
            BotConfig = Config.Load();

            if (BotConfig.ApiKey == null || BotConfig.ApiKey.Trim() == "")
            {
                Console.WriteLine("Config created...");
                Console.ReadLine();
                return;
            }

            var bot = BotBaseBuilder.Create()
                .QuickStart(BotConfig.ApiKey, typeof(StartForm))
                .Build();

            await bot.Start();

            Console.WriteLine("Bot started");

            Console.ReadLine();
            await bot.Stop();
        }
    }
}