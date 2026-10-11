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

        ~ConsoleLog() { 
            Server.Logging = false;
        }
    }
}
