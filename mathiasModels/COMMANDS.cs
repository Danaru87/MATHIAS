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
        public List<SENTENCES> SENTENCESLIST { get; set; }
        public String CMD { get; set; }
        private METHODS METHOD { get; set; }
        private MODULES MODULE { get; set; } 
    }
}
