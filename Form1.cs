using System;
using System.Windows.Forms;

namespace TwitchBot
{
    public partial class frmMain : Form
    {
        TwitchChatBot bot = new TwitchChatBot();

        public frmMain()
        { 
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            bot.Connect();

            Console.ReadLine();

            bot.Disconnect();

        }
    }

}
