using System;
using TelegramBot.Functional;

namespace TelegramBot {

    class Program {
        static void Main() {

            //5665516734:AAHmaBdq0M - vY - 8_AXNJj8Lp2Bc5bpAimkg
            //5782491665:AAGhqvAtzgbk40kBp9Uaeudj5raRHL4vYzg
            try {
                Shazampy py = new Shazampy();
                TelegramBotProject bot = new TelegramBotProject(token: "5665516734:AAHmaBdq0M-vY-8_AXNJj8Lp2Bc5bpAimkg");

                py.ShazamInit();
                bot.GetUpdates();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }










        }
    }
}