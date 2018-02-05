using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    enum UserStatus {None, Follower, Subscriber}

    class User
    {
        public string strName;
        public int intPoints;

        public bool blnIsModerator;
        public bool blnIsSubcribed;

        public User(string name, bool isModerator, bool isSubscribed, int points)
        {
            strName = name;
            intPoints = 0;
            blnIsModerator = isModerator;
            blnIsSubcribed = isSubscribed;
        }
    }
}
