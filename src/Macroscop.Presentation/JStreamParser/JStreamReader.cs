using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace Macroscop.Client.Presentation.JStreamParser
{
    public class JStreamReader
    {
        private BinaryReader BinaryStreamReader;

        private byte[] _buffer;

        private readonly int BuferCapacity;

        private int currentIndex;


        public JStreamReader(BinaryReader _BinaryStreamReader, int _BuferCapacity)
        {
            BinaryStreamReader = _BinaryStreamReader;
            BuferCapacity = _BuferCapacity;
            _buffer = new byte[_BuferCapacity];
            LoadStream(BuferCapacity);
        }


        public int GetCount
        {
            get
            {
                return _buffer.Length - currentIndex;
            }
        }

        protected byte ReadSingle 
        {
            get 
            {
                return ReadStream(1).FirstOrDefault();
            }
        }

        protected IEnumerable<byte> GetBytes
        {
            get
            {
                for (var i = currentIndex; i < GetCount; i++)
                {
                    yield return _buffer[(currentIndex + i) % _buffer.Length];
                }
            }
        }

        protected void LoadStream(int count) 
        {
            if (count == BuferCapacity) 
            {
                currentIndex = 0;

                _buffer = BinaryStreamReader.ReadBytes(count);
            }

            else if (count < BuferCapacity)
            {
                unsafe
                {
                    fixed (byte* ptr = _buffer)
                    {
                        Marshal.Copy(_buffer, 1, (IntPtr)(ptr), _buffer.Length - count);
                    }
                }

                int tempind = currentIndex - count;
                if (tempind >= 0)
                {
                    currentIndex = tempind;
                }
                else
                {
                    currentIndex = 0;
                }

                Array.Copy(BinaryStreamReader.ReadBytes(count), 0, _buffer, currentIndex, count);
            }

            else 
            {
                throw new ArgumentOutOfRangeException();
            }

        }

        protected byte[] ReadStream(int length) 
        {
            var result = new byte[length];

            if ((currentIndex + length) < _buffer.Length)
            {
                Array.Copy(_buffer, currentIndex, result, 0, length);
                currentIndex += length;

                return result;
            }

            else
            {
                var endLen = _buffer.Length - currentIndex;
                var remainingLen = length - endLen;
                Array.Copy(_buffer, currentIndex, result, 0, endLen);
                Array.Copy(BinaryStreamReader.ReadBytes(remainingLen), 0, result, endLen, remainingLen);
                LoadStream(BuferCapacity);

                return result;
            }
        }

        protected bool SkipStream(int length) 
        {
            if (ReadStream(length) != null)
            {
                return true;
            }

            return false;
        }


        protected bool CheckStream(params byte[] byteseries)
        {
            bool IsMatchFound = false;

            while (!IsMatchFound) 
            {
                for (int start = currentIndex; start < _buffer.Length; start++)
                {
                    if (_buffer[start] == byteseries[0]) 
                    {
                        int next;
                        for (next = 1; next < byteseries.Length; next++)
                        {
                            if ((start + next) >= _buffer.Length) 
                            {
                                LoadStream(BuferCapacity / 2);
                                start -= BuferCapacity / 2;
                            }

                            if (_buffer[start + next] != byteseries[next])
                            {
                                break;
                            }
                        }

                        if (next == byteseries.Length)
                        {
                            currentIndex = start;
                            IsMatchFound = true;
                            return true;
                        }
                    }
                }

                LoadStream(BuferCapacity);
            }
            return false;
        }

        //
    }
}
