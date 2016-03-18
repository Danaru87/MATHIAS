using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.Net.Mail;
using MoreLinq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ImapClient client = new ImapClient("imap.gmail.com", "dasilva.arnaud@gmail.com", "6ot04obb", AuthMethods.Login, 993, true);
            DateTime dt = DateTime.Now.AddHours(-1);
            List<MailMessage> msgs = client.GetMessages(client.GetMessageCount() -5, client.GetMessageCount(), false).ToList();

            MailMessage oneMessage = msgs.MaxBy(mess => mess.Uid);
            var message = msgs.First();
            Console.ReadLine();
        }
    }
}
