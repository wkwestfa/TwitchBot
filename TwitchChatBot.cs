using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Services;
using TwitchLib.Models.API.v5.Users;
using System.Runtime.InteropServices;

namespace TwitchBot
{
    internal class TwitchChatBot
    {
        public List<User> lstUsers = new List<User>();
        
        readonly ConnectionCredentials credentials = new ConnectionCredentials(Resources.TwitchInfo.BotUsername, Resources.TwitchInfo.BotToken);
        TwitchClient client;
        UserFollow follower;
        internal void Connect()
        {       
            Console.WriteLine("Connecting");
            client = new TwitchClient(credentials, Resources.TwitchInfo.ChannelName, logging: false);

            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnUserJoined += Client_OnUserJoined;
            client.Connect();
        }

        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            string strUsername = "";
            e.Username = strUsername;

        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            User newUser;
            string strMessage = "";
            strMessage = e.ChatMessage.Message;

            switch(strMessage)
            {
                case "!lastman":
                    LastMan(e);      // Execute process for Lastman
                    break;
                case "!add":
                    client.SendMessage("Added");
                    newUser = new User(e.ChatMessage.Username, e.ChatMessage.IsModerator, e.ChatMessage.IsSubscriber, 0);
                    using (StreamWriter usersFile = new StreamWriter(@"..\..\Resources\Users.txt"))
                    {
                        usersFile.WriteLine($"{newUser.strName} {newUser.blnIsModerator} {newUser.blnIsSubcribed} {newUser.intPoints}");
                    }
                    break;
                default:
                    break;

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


        private void LastMan(OnMessageReceivedArgs e)
        {
            bool firstMessage = false;

            if (firstMessage == false)
            {
                client.SendMessage($"{e.ChatMessage.DisplayName} Wants to start Last Man Standing.  Three participants needed to begin.  To join type '!lastman'");
                firstMessage = true;
            }
            else if (e.ChatMessage.Message == "!lastman")
            {
                client.SendMessage($"{e.ChatMessage.DisplayName} Has joined the fray!!");
            }
            if (e.ChatMessage.Message.StartsWith("hi", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage($"Hey there " + e.ChatMessage.DisplayName);
            }
        }
    }


}