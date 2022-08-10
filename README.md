# Denpou

![License](https://img.shields.io/github/license/ZavaruKitsu/Denpou?style=flat-square)

**Denpou** is a fork of [TelegramBotFramework](https://github.com/MajMcCloud/TelegramBotFramework) that tries to fix
warnings, deadlocks and introduce new features.

## Documentation

TBA, you can still use [TelegramBotFramework's](https://github.com/MajMcCloud/TelegramBotFramework/blob/master/README.md) README.md or examples

## Migration notes

Ensure that your bot is using TelegramBotFramework 5.X before proceeding

- `bot.Start` and `bot.Stop` are now asynchronous
- `LoadSessionStates` and `SaveSessionStates` are now asynchronous
- There's no `IStartFormFactory` anymore. Use dependency injection
- Exceptions are not "muted" now

Also, ensure that:

- you're not using `bot.UploadBotCommands().Wait();`, or, more generic, don't use `Task.Wait();` in asynchronous methods
- you don't have empty asynchronous methods in your forms
