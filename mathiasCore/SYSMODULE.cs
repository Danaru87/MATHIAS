using mathiasModels.Xtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathiasCore
{
    public static class SYSMODULE
    {
        public static PlugResponse DoAction(PlugCall call)
        {
            
            switch(call.ACTION)
            {
                case "HELLO":
                    return Hello(call);
                case "HUMEUR":
                    return Humeur(call);
                case "EXIT":
                    return CloseApp(call);
                case "CONTEXT":
                    return ContextChange(call);
                case "LOAD CONTEXT":
                    return null; //LoadContext(call);
                default:
                    return null;
            }
        }

        private static PlugResponse ContextChange(PlugCall call)
        {
            PlugResponse response = new PlugResponse();
            response.Response = "Quel contexte voulez vous charger ?";
            response.WaitForChainedAction = true;
            response.NextChainedAction = "LOAD CONTEXT";
            GlobalManager.LastResponse = response;
            return response;
        }

        private static PlugResponse CloseApp(PlugCall call)
        {
            GlobalManager.RUNNING = false;
            System.Threading.Thread.Sleep(1000);
            return new PlugResponse() { Response= "Au revoir"};
        }

        private static PlugResponse Humeur(PlugCall call)
        {
            PlugResponse response = new PlugResponse();
            response.WaitForChainedAction = false;
            response.Response = "Je vais bien, merci";
            return response;
        }
        private static PlugResponse Hello(PlugCall call)
        {
            PlugResponse response = new PlugResponse();
            response.WaitForChainedAction = true;
            response.ChainedQuestion = "Comment ça va ?";
            response.Response = "Bonjour !";
            response.NextChainedAction = "HUMEUR";
            return response;
        }
    }
}
