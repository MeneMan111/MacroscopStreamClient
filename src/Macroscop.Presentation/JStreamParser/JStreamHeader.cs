using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macroscop.Client.Presentation.JStreamParser
{
    public class JStreamHeader
    {
        public ResHeader Header { get; private set; }

        public byte[] HEXName { get; private set; }


        public JStreamHeader(ResHeader Header, byte[] HEXName) 
        {
            this.Header = Header; 
            this.HEXName = HEXName; 
        }
    }
}
