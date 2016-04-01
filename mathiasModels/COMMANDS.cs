using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathiasModels
{
    public class COMMANDS
    {
        public int ID { get; set; }
        public String CMD { get; set; }
        private String MODULENAME { get; set; }
        public MODULES MODULE { get; set; } 
    }
}
