using System;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Models.API.v5.Users;
using System.Runtime.InteropServices;

namespace TwitchBot
{
    internal class TwitchChatBot
    {
        readonly ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);
        TwitchClient client;

        internal void Connect()
        {

            Console.WriteLine("Connecting");

            client = new TwitchClient(credentials, TwitchInfo.ChannelName, logging: false);

            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Connect();
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            bool firstMessage = false;

            if (e.ChatMessage.Message == "!lastman" && firstMessage == false) {
                client.SendMessage($" {e.ChatMessage.DisplayName} Wants to start Last Man Standing.  Three participants needed to begin.  To join type '!lastman'");
                firstMessage = true;
            }
            else if (e.ChatMessage.Message == "!lastman") {
                client.SendMessage($"{e.ChatMessage.DisplayName} Has joined the fray!!");
            }
            if(e.ChatMessage.Message.StartsWith("hi", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage($"Hey there " + e.ChatMessage.DisplayName);
            }
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //Console.WriteLine(e.Data);
        }

        private void Client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!! {e.Error}");
        }

        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting");
        }

    }


}