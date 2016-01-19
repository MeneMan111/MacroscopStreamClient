using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Macroscop.Client.Presentation.JStreamParser
{
    public class JStreamResolver : JStreamReader, IStreamResolver<JpegResult>
    {
        #region Initialize

        private JStreamHeader[] ResponseHeaderDefines =
		{
            new JStreamHeader
            (
                ResHeader.ResponseBegin,
                new byte[] 
                {
                    0x2D, 0x2D, 0x6D, 0x79, 0x62, 
                    0x6F, 0x75, 0x6E, 0x64, 0x61, 
                    0x72, 0x79
                }
            ),
            new JStreamHeader
            (
                ResHeader.Content_Length,
                new byte[] 
                {
                    0x43, 0x6F, 0x6E, 0x74, 0x65, 
                    0x6E, 0x74, 0x2D, 0x4C, 0x65, 
                    0x6E, 0x67, 0x74, 0x68, 0x3A, 
                    0x20
                }
            ),
            new JStreamHeader
            (
                ResHeader.JpegFrameBegin,
                new byte[] 
                {
                    0xFF, 0xD8
                }
            ),
            new JStreamHeader
            (
                ResHeader.JpegFrameEnd,
                new byte[] 
                {
                    0xFF, 0xD9
                }
            )
        };

        #endregion

        public JStreamResolver(BinaryReader _BinaryStreamReader, int _BuferCapacity = 35000)
            : base(_BinaryStreamReader,_BuferCapacity) 
        { }

        public JStreamResolver(BinaryReader _BinaryStreamReader, JStreamHeader[] ResponseHeaderDefines,  int _BuferCapacity = 35000)
            : base(_BinaryStreamReader, _BuferCapacity)
        {
            this.ResponseHeaderDefines = ResponseHeaderDefines;
        }



        public JpegResult ParseJpegResult() 
        {
            byte[] rules = ResponseHeaderDefines.FirstOrDefault(r => r.Header == ResHeader.ResponseBegin).HEXName;
            if (CheckStream(rules))
            {
                int leng = ParseJpegLenght();

                return new JpegResult(leng, ParseJpegBytes(leng));
            }

            return null;
        }

        private int ParseJpegLenght() 
        {
            byte[] rules = ResponseHeaderDefines.FirstOrDefault(r => r.Header == ResHeader.Content_Length).HEXName;
            if (CheckStream(rules)) 
            {
                SkipStream(16);
                byte[] result = ReadStream(5);

                return Int32.Parse(Encoding.UTF8.GetString(result));
            }

            return 0;
        }

        private byte[] ParseJpegBytes(int length) 
        {
            byte[] rules = ResponseHeaderDefines.FirstOrDefault(r => r.Header == ResHeader.JpegFrameBegin).HEXName;
            if (CheckStream(rules)) 
            {
                return ReadStream(length);
            }

            return null;
        }


    }
}
