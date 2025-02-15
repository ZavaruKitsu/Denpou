﻿using System.Linq;
using System.Threading.Tasks;
using Denpou.Base;
using Denpou.Form;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace DenpouControls.Examples;

public class DataForm : AutoCleanForm
{
    public override async Task SentData(DataResult data)
    {
        var tmp = "";
        InputOnlineFile file;

        switch (data.Type)
        {
            case MessageType.Contact:

                tmp += $"Firstname: {data.Contact.FirstName}\n";
                tmp += $"Lastname: {data.Contact.LastName}\n";
                tmp += $"Phonenumber: {data.Contact.PhoneNumber}\n";
                tmp += $"UserId: {data.Contact.UserId}\n";

                await Device.Send($"Your contact: \n{tmp}", replyTo: data.MessageId);

                break;

            case MessageType.Document:

                file = new InputOnlineFile(data.Document.FileId);

                await Device.SendDocument(file, "Your uploaded document");


                break;

            case MessageType.Video:

                file = new InputOnlineFile(data.Document.FileId);

                await Device.SendDocument(file, "Your uploaded video");

                break;

            case MessageType.Audio:

                file = new InputOnlineFile(data.Document.FileId);

                await Device.SendDocument(file, "Your uploaded audio");

                break;

            case MessageType.Location:

                tmp += $"Lat: {data.Location.Latitude}\n";
                tmp += $"Lng: {data.Location.Longitude}\n";

                await Device.Send($"Your location: \n{tmp}", replyTo: data.MessageId);

                break;

            case MessageType.Photo:

                var photo = new InputOnlineFile(data.Photos.Last().FileId);

                await Device.Send("Your image: ", replyTo: data.MessageId);
                await Client.TelegramClient.SendPhotoAsync(Device.DeviceId, photo);

                break;

            default:

                await Device.Send("Unknown response");

                break;
        }
    }

    public override async Task Action(MessageResult message)
    {
        await message.ConfirmAction();

        if (message.Handled)
            return;

        switch (message.RawData)
        {
            case "contact":

                await Device.RequestContact();

                break;

            case "location":

                await Device.RequestLocation();

                break;

            case "back":

                message.Handled = true;

                var start = new Menu();

                await NavigateTo(start);

                break;
        }
    }

    public override async Task Render(MessageResult message)
    {
        var bf = new ButtonForm();

        bf.AddButtonRow(new ButtonBase("Request User contact", "contact"));

        bf.AddButtonRow(new ButtonBase("Request User location", "location"));

        bf.AddButtonRow(new ButtonBase("Back", "back"));

        InlineKeyboardMarkup ikv = bf;

        await Device.Send("Please upload a contact, photo, video, audio, document or location.", bf);
    }
}