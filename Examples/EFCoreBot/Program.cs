using Denpou.Builder;
using EFCoreBot;
using EFCoreBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection()
    .AddDbContext<BotDbContext>(x => x.UseInMemoryDatabase("Denpou"));

var serviceProvider = serviceCollection.BuildServiceProvider();

var bot = BotBaseBuilder.Create()
    .SetApiKey(Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("API_KEY is not set"))
    .SetStartForm<StartForm>()
    .SetServiceProvider(serviceProvider)
    .Build();

await bot.Start();
await Task.Delay(-1);
