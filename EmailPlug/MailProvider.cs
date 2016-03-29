using mathiasModels.Xtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailPlug
{
    public abstract class MailProvider: IPlugin
    {
        public bool CONNECTED { get; set; }
        public Dictionary<String, String> CFG { get; set; }

        public abstract PlugResponse DoAction(PlugCall Call);

        public abstract void Install();

        public abstract void Init();
    }
}
