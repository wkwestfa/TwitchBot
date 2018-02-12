using System;
using System.Collections.Generic;
using System.IO;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;
using TwitchLib.Services;
using TwitchLib.Models.API.v5.Users;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TwitchBot
{
    internal class TwitchChatBot
    {
        private Timer pointsTimer;
        private int points;
        public List<User> lstUsers = new List<User>();
        public List<Classes.MyUser> lstUsersWatching = new List<Classes.MyUser>();
        public Commands botCommands = new Commands();

        readonly ConnectionCredentials credentials = new ConnectionCredentials(Resources.TwitchInfo.BotUsername, Resources.TwitchInfo.BotToken);
        public TwitchClient client;
        public TwitchAPI twitchAPI = new TwitchAPI();

        #region Connection

        internal void Connect()
        {
            Console.WriteLine("Connecting");

            MessageThrottler myMessageThrottler = new TwitchLib.Services.MessageThrottler(client, 15, TimeSpan.FromSeconds(30));
            MessageThrottler myWhisperThrottler = new TwitchLib.Services.MessageThrottler(client, 15, TimeSpan.FromSeconds(30));

            client = new TwitchClient(credentials, Resources.TwitchInfo.ChannelName, logging: false);

            client.OnLog += Client_OnLog;
            client.OnConnectionError += Client_OnConnectionError;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnUserJoined += Client_OnUserJoined;
            client.OnUserLeft += Client_OnUserLeft;
            InitTimer();
            client.Connect();

            twitchAPI.Settings.ClientId = Resources.TwitchInfo.ClientID;
        }

        internal void Disconnect()
        {
            Console.WriteLine("Disconnecting");
        }

        #endregion Connection

        #region Timer Stuff

        public void InitTimer()
        {
            pointsTimer = new Timer();
            pointsTimer.Tick += new EventHandler(pointsTimer_Tick);
            pointsTimer.Interval = 10000;       // 10 seconds
            pointsTimer.Interval = 60000 * 5;   // 5 minutes
            pointsTimer.Start();
        }

        private void pointsTimer_Tick(object sender, EventArgs e)
        {
            foreach(Classes.MyUser user in lstUsersWatching)
            {
                user.intMinutesWatched += 5;
                user.intPoints += 5;

                if (CheckIfUserExists(user.strTwitchID))
                {

                }

            }
        }

        #endregion Timer Stuff


        #region Client

        /// <summary>
        /// Activates when a user joins the chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            string strUsername = "";
            string userID = "";

            User[] users = new User[1];

            strUsername = e.Username;

            users = twitchAPI.Users.v5.GetUserByNameAsync(strUsername).Result.Matches;

            userID = users[0].Id.ToString();

            if (CheckIfUserExists(userID) == false)  // If user is not found
            {
                    WriteToTextFile(@"..\..\Resources\Users.txt", $"{userID} {strUsername}");
                    client.SendMessage($"Everyone welcome {strUsername} to the chat!");
            }

            if(strUsername.ToLower() != "moobot" && strUsername.ToLower() != "nightbot" && strUsername.ToLower() != "60secondgamer")
            {
                lstUsersWatching.Add(new Classes.MyUser(userID, strUsername, 0, 0));
            }
        }

        /// <summary>
        /// Activates when a user has left the chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            Console.WriteLine(e.Username);
        }

        /// <summary>
        /// Check if a user exists in the text file
        /// </summary>
        /// <param name="userID"> User we are looking for</param>
        /// <returns> True: found.  False: Not found </returns>
        private bool CheckIfUserExists(string userID)
        {

            using (StreamReader usersFile = new StreamReader(@"..\..\Resources\Users.txt"))
            {
                string line;
                while (!usersFile.EndOfStream)  // If we have no reached the end of the file
                {
                    line = usersFile.ReadLine();

                    if (line.Contains(userID))    // If the file does not contain the user ID
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void WriteToTextFile(string textfileLocation, string strToWrite)
        {
            using (StreamWriter usersFile = new StreamWriter(textfileLocation, true))
            {
                usersFile.WriteLine(strToWrite);   // Write it to the text file
            }
        }


        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            string userMessage = e.ChatMessage.Message;

            if (userMessage.StartsWith("!"))
            {
                botCommands.CheckCommands(client, e, userMessage);
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

        #endregion Client

    }


}