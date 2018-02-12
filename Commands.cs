using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;

namespace TwitchBot
{
    class Commands
    {
        public List<String> lstLastMan = new List<String>();
        /// <summary>
        /// Checks to see what command a user has inserted
        /// </summary>
        /// <param name="client">Twitch Client we are connecting to (60SecondGamer)</param>
        /// <param name="e"> Functionality to interact with chat</param>
        /// <param name="commands"> the command that a user has input </param>
        public void CheckCommands(TwitchClient client, OnMessageReceivedArgs e, string commands)
        {
            string strMessage = "";
            strMessage = e.ChatMessage.Message;

            switch (strMessage)
            {
                case "!lastman":
                    LastManJoin(client, e);      // Execute process for Lastman
                    break;
                case "!add":
                    client.SendMessage("Added");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initiates and event that slowly mutes users in chat until there is only one left who can talk
        /// </summary>
        /// <param name="e">OnMessageReceivedArgs</param>
        private void LastManJoin(TwitchClient client, OnMessageReceivedArgs e)
        {
            string user = e.ChatMessage.Username;

            if (lstLastMan.Count == 0)
            {
                lstLastMan.Add(user);
                client.SendMessage($"{e.ChatMessage.DisplayName} wants to see who will be the Last Man Standing.  Three participants are needed to begin.  To join type '!lastman'");
            }
            else if (e.ChatMessage.Message == "!lastman" && lstLastMan.Contains(user) == false)
            {
                lstLastMan.Add(user);
                client.SendMessage($"{e.ChatMessage.DisplayName} has joined the fray!!");
            }
        }


    }
}
