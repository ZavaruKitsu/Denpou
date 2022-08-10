using System;
using System.Threading.Tasks;
using Denpou.Builder;
using JoinHiderBot.Forms;

namespace JoinHiderBot
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("API_KEY is not set");

            var bot = BotBaseBuilder.Create()
                .SetApiKey(apiKey)
                .SetStartForm<Start>()
                .Build();

            await bot.Start();

            Console.ReadLine();
            await bot.Stop();
        }
    }
}