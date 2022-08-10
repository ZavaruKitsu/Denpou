namespace SystemCommandsBot.Commands
{
    public class Commando
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string ShellCmd { get; set; }

        public bool Enabled { get; set; } = true;

        public string Action { get; set; }

        public bool UseShell { get; set; } = true;

        public int? MaxInstances { get; set; }

        public string ProcName { get; set; }
    }
}