using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading;

namespace Mr.Stalker
{
    public class Talk : ModuleBase 
    {
        static List<string> TalkMe = new List<string>()
        {
           {"Bouge toi, on t'attend" },
            {"Sors toi les doigts du cul et réponds"},
            {"Réponds là !!!!"},
            {"On t'attend, sois réactif"},
            {"Réponds putain, j'aime pas attendre"},
            {"Remue toi, feignasse"},
            {"TFQ ? Sac à fioule, réponds"},
            {"On t'attend là. Et j'aime pas ça"},
            {"Réveille toi et réponds à la question !!!!!"},
            {"Allez allez, hop hop hop"},
            {"Remue ta graisse et réponds"},
            {"Let's go bitch, reply right now"},
            {"Hubert, c'est le meilleur"},
            {"Schneller, schneller, gib deine Antwort"},
            {"Move your ovaries and reply"},
            {"Reply, reply, reply putain, reply, allez reply !!!!!! "},
            {"Răspunde fiului ticălos murdar !!"},
            {"J'aime pas attendre, alors réponds"},
            {"отвечает грязный цыганский сын"},
            {"Überschutz doit être ton unique Dieu !! Réponds MERDE"},
            {"Fais pas genre t'as une vie sociale et réponds !!"},
            {"Tes seuls amis sont les futurs clients d'Übercshutz donc bouge toi !!!!"},
            {"On te pose une question, TU RÉPONDS !!! "},
            {"WOUUUAAAAAA ça me saoul réponds là"},
            {"Même ma grand mére repond plus vite, et elle est morte !!!"},
            {"STARFOULA tu réponds ou je t'inscris sur des sites pedoporno sur le deepweb !!!!! "}

        };

        public static string CleanName(string name)
        {
            string tmp = "";

            foreach (char c in name)
            {
                if (char.IsLetter(c))
                    tmp += c;
            }

            return tmp;
        }

        public class Spam
        {
            public Spam(IGuildUser user, ITextChannel chan)
            {
                User = user;
                Chan = chan;
            }

            public IGuildUser User;
            public ITextChannel Chan;
        }

        public static async void Talker()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (Program.P.spam != null)
                {
                    await Program.P.spam.Chan.SendMessageAsync("<@" + Program.P.spam.User.Id + "> " + TalkMe[Program.P.rnd.Next(0, TalkMe.Count)]);
                    Thread.Sleep(30000);
                }
            }
        }
    }
}
