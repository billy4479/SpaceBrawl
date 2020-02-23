using System;
using System.IO;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace SpaceBrawl_Bot
{
    internal class Program
    {
        private static ITelegramBotClient botClient;
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "/log.txt";
        private static ChatId[] forwardID;
        private static int[] intID = {
        881189098,
        817316539,
        925304642,
        990124314
        };

        private static void Main()
        {
            botClient = new TelegramBotClient("1040727575:AAHcJVi--ewtYCqd8691mVckoAUyabYK4IU");
            var me = botClient.GetMeAsync().Result;
            

            forwardID = new ChatId[intID.Length];
            for (int i = 0; i < intID.Length; i++)
            {
                forwardID[i] = new ChatId(intID[i]);
            }

            Console.WriteLine($"Ciao! Questo bot si chiamo {me.FirstName} e il suo token è {me.Id}");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null && e.Message.Text != "/start")
            {
                Console.WriteLine(
                    $"Hey! Hai ricevuto un messaggio: {e.Message.Text}. L'id del mandante è: {e.Message.Chat.Id}"
                    );
                string loggingString = $"{e.Message.Date}, " +
                        $"UserID: {e.Message.Chat.Id}, " +
                        $"UserName: @{e.Message.Chat.Username}, " +
                        $"Text: \"{e.Message.Text}\" " +
                        $"\n";
                using (var sw = new StreamWriter(
                    path: path,
                    append: true
                    ))
                {
                    sw.Write(loggingString);
                    sw.Flush();
                    sw.Close();
                }
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"Grazie per il feedback! Ti faremo sapere!"
                    );

                foreach (var forwardChat in forwardID)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: forwardChat,
                        text: loggingString
                        );
                }
            }
            if(e.Message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"Ciao! Questo bot serve per mandare un tuo feedback sul gioco. Per favore manda recensioni utili o critiche costruttive in modo da farci capire cosa c'è da migliorare."
                    );
            }
        }
    }
}