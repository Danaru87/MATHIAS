using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.Net.Mail;
using MoreLinq;
using mathiasModels.Xtend;

namespace EmailPlug
{
    class ImapProvider : MailProvider
    {
        #region PROPERTIES
        private ImapClient CLIENT { get; set; }
        public Int32 PORT { get; }
        public String HOST { get; }
        public String LOGIN { get; }
        public String PWD { get; }
        public AuthMethods METHOD { get; }
        public bool USESSL { get; }
        public MailMessage[] MESSAGES { get; }
        public Dictionary<String, String> CONFIGURATION { get; set; }
        public PlugResponse RESPONSE { get; }
        #endregion

        #region CONSTRUCTORS

        public ImapProvider(string Host, string login, string pass, Int32 Port)
        {
            HOST = Host;
            LOGIN = login;
            PWD = pass;
            PORT = Port;
            RESPONSE = new PlugResponse();
        }
        #endregion

        private PlugResponse GetLastMailSubject()
        {
            List<MailMessage> msgs = null;
            using (ImapClient client = new ImapClient(HOST, LOGIN, PWD, AuthMethods.Login, PORT, true))
            {
                DateTime dt = DateTime.Now.AddHours(-1);
                msgs = client.GetMessages(client.GetMessageCount() - 5, client.GetMessageCount(), false).ToList();
            }
            MailMessage oneMessage = msgs.MaxBy(mess => mess.Uid);
            RESPONSE.Params.Add("UID", oneMessage.Uid);
            RESPONSE.WaitForChainedAction = true;
            RESPONSE.NextChainedAction = "GetBodyMessage";
            RESPONSE.ChainedQuestion = "Souhaitez vous une lecture de l'email ?";
            RESPONSE.Response = oneMessage.Subject;
            return RESPONSE;
        }

        public override PlugResponse DoAction(PlugCall Call)
        {

                switch (Call.ACTION)
                {
                    case "GetLastMailSubject":
                        return GetLastMailSubject();
                    default:
                        return new PlugResponse() { Response = "Je ne peux pas traiter votre demande" };
                }
        }
    }
}
