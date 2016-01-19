using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macroscop.Client.Presentation.JStreamParser
{
    public interface IStreamReader
    {
        int GetCount { get; }

        byte ReadSingle { get;  }

        void LoadStream(int count);

        byte[] ReadStream(int length);

        bool SkipStream(int length);
 
        bool CheckStream(params byte[] byteseries);
    }
}
