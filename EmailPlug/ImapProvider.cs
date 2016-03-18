using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.Net.Mail;
using MoreLinq;
namespace EmailPlug
{
    class ImapProvider : MailProvider
    {
        #region PROPERTIES
        private ImapClient CLIENT { get; set; }
        public Int32 PORT { get; }
        public String HOST { get; }
        public AuthMethods METHOD { get; }
        public bool USESSL { get; }
        public MailMessage[] MESSAGES { get; }
        public Dictionary<String, String> CONFIGURATION { get; set; }
        #endregion

        #region CONSTRUCTORS
        public ImapClient(Dictionary<String,String> EmailConfiguration)
        {

        }
        #endregion

        private override String GetLastMailSubject()
        {
            List<MailMessage> msgs = null;
            using (ImapClient client = new ImapClient("imap.gmail.com", "dasilva.arnaud@gmail.com", "6ot04obb", AuthMethods.Login, 993, true))
            {
                DateTime dt = DateTime.Now.AddHours(-1);
                msgs = client.GetMessages(client.GetMessageCount() - 5, client.GetMessageCount(), false).ToList();
            }
            MailMessage oneMessage = msgs.MaxBy(mess => mess.Uid);
            return oneMessage.Subject;
        }

        public override string DoAction(string ActionName)
        {
            throw new NotImplementedException();
        }
    }
}
