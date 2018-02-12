namespace TwitchBot.Classes
{
    public class MyUser
    {
        public string strTwitchID;
        public string strTwitchUsername;
        public int intPoints;
        public int intMinutesWatched;

        public MyUser(string TwitchId, string twitchUsername, int points, int minutesWatched)
        {
            strTwitchID = TwitchId;
            strTwitchUsername = twitchUsername;
            intPoints = points;
            intMinutesWatched = minutesWatched;
        }
    }
}
