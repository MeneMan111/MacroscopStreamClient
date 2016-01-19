using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macroscop.Client.Presentation.JStreamParser
{
    public enum ResHeader
    {
        ResponseBegin,
        //Content_Type,
        Content_Length,
        JpegFrameBegin,
        JpegFrameEnd
    }
}
