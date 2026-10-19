using MusicComanderGUI;

namespace CustomMusic
{
    public partial class ConsoleLog : Form
    {
        public static ConsoleLog? Instance { get; private set; }
        public ConsoleLog()
        {
            InitializeComponent();
            Server.Logging = true;
            Instance = this;
        }

        public void SendConsoleLog(string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            ConsoleLogBox.AppendText(currentTime + " " +  message + Environment.NewLine);
        }

        public void SendJson(string message)
        {
            message = message.Replace("\n", Environment.NewLine);
            Json.Text = message;
        }
        ~ConsoleLog() { 
            Server.Logging = false;
        }
    }
}
