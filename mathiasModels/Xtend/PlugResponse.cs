using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathiasModels.Xtend
{
    public class PlugResponse
    {
        public String Response { get; set; }
        public String NextChainedAction { get; set; }
        public String ChainedQuestion { get; set; }
        public Boolean WaitForChainedAction { get; set; }
        public Dictionary<String, object> Params { get; set; }

        public PlugResponse ()
        {
            Params = new Dictionary<string, object>();
        }

    }
}
