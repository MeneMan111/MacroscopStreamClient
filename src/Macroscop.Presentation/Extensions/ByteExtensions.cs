using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macroscop.Client.Presentation.Extensions
{
    static class ByteExtensions
    {
        public static int Find(this byte[] buff, byte[] search)
        {
            for (int start = 0; start < buff.Length - search.Length; start++)
            {
                if (buff[start] == search[0])
                {
                    int next;

                    for (next = 1; next < search.Length; next++)
                    {
                        if (buff[start + next] != search[next])
                            break;
                    }

                    if (next == search.Length)
                        return start;
                }
            }
            return -1;
        }
    }
}
