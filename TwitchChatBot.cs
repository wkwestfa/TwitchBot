using System;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Models.API.v5.Users;

namespace TwitchBot
{
    internal class TwitchChatBot
    {
        public const string BOT_USERNAME = "60bot";
        public const string BOT_TOKEN = "u1pf5dt74i6emwmk45jjw7rrs1i676";
        public const string CHANNEL_NAME = "60SecondGamer";
        public const string CLIENT_ID = "r45p6xwsohryer8fxizdjagu7xo2mq";

        readonly ConnectionCredentials credentials = new ConnectionCredentials(BOT_USERNAME, BOT_TOKEN);
        TwitchClient client;

        internal void Connect()
        {
            Console.WriteLine("Connecting");

            client = new TwitchClient(credentials, CHANNEL_NAME, logging: true);

            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;

            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine(e.Data);
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