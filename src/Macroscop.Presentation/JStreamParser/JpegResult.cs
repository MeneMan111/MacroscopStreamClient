using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macroscop.Client.Presentation.JStreamParser
{
    public class JpegResult
    {
        public int JpegLenght { get;  set; }

        public byte[] JpegBytes { get;  set; }

        public JpegResult() { }

        public JpegResult(int JpegLenght, byte[] JpegBytes) 
        {
            this.JpegLenght = JpegLenght;
            this.JpegBytes = JpegBytes;
        }
    }
}
