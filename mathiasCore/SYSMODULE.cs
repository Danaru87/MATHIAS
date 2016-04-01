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
                default:
                    return null;
            }
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
