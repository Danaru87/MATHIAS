using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mathiasModels
{
    public class Context
    {
        public String NAME { get; set; }
        public List<SENTENCES> SENTENCESLIST { get; set; }
        public object GRAMMAR { get; set; }
    }
}
