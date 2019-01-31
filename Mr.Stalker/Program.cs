using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mr.Stalker
{
    class Program
    {

        public  DiscordSocketClient Client;
        private CommandService Commande;
        public Random rnd;
        private Thread spamThread;
        public Talk.Spam spam;
        public static Program P;
        public static DateTime countdown;

        static async Task Main(string[] args)
        {
            await new Program().MainAsync();    
        }

        async Task    MainAsync()
        {
            P = this;
            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = Discord.LogSeverity.Verbose
            });

            rnd = new Random();
            countdown = DateTime.Now;
            spam = null;
            spamThread = new Thread(Talk.Talker);
            spamThread.Start();
            Commande = new CommandService();
            await Commande.AddModuleAsync<Talk>(null);

            Client.MessageReceived += Client_MessageReceived;

            Client.Log += Client_Log;
            Commande.Log += Commande_Log;
            await Client.LoginAsync(TokenType.Bot, File.ReadAllText("key.txt"));
            await  Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            SocketUserMessage SUM = arg as SocketUserMessage;

            if (SUM == null && arg.Author.IsBot)
                return;
            if (spam != null && spam.User.Id == arg.Author.Id || (DateTime.Now.Subtract(DateTime.Now).TotalSeconds > 60000))
                spam = null;
            int pos = 0;
            if (SUM.HasMentionPrefix(Client.CurrentUser, ref pos) || SUM.HasStringPrefix("#bot", ref pos))
            {
                ITextChannel chan = arg.Channel as ITextChannel;
                if (chan != null)
                {
                    IGuildUser user = (await chan.Guild.GetUsersAsync()).FirstOrDefault((x) =>
                    {
                        string enterName = SUM.Content.Substring(pos, SUM.Content.Length - pos).ToLower();
                        if (Talk.CleanName(x.Username.ToLower()) == enterName)
                            return (true);
                        return (!string.IsNullOrEmpty(x.Nickname) && Talk.CleanName(x.Nickname.ToLower()) == enterName);
                    });
                    if (user != null)
                        spam = new Talk.Spam(user, chan);
                }
                SocketCommandContext context = new SocketCommandContext(Client, SUM);

                await Commande.ExecuteAsync(context, pos, null);

            }
        }

        private Task Commande_Log(LogMessage arg)
        {
            Client_Log(arg);
            return Task.CompletedTask;
        }

        private System.Threading.Tasks.Task Client_Log(Discord.LogMessage arg)
        {
            var cc = Console.ForegroundColor;
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine(arg);
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }
    }
}
