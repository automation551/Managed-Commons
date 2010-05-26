//
// Copyright ©2010 Rafael 'Monoman' Teixeira
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Commons.Ebml.Commons.Ebml
{
    public class ElementSize
    {
        private ulong size;
        public ElementSize(ulong size) { this.size = size; }
        public ElementSize(long size) { unchecked { this.size = (ulong)size; } }
        public byte[] Bytes
        {
            get
            {
                return new byte[0];
            }
        }

        public int CodedLength
        {
            get
            {
                int size = 8;
                ulong mask = 0xFF00000000000000L;
                for (int i = 0; i < 8; i++)
                {
                    if ((this.size & mask) == 0)
                    {
                        mask = mask >> 8;
                        size--;
                    }
                    else
                    {
                        return size;
                    }
                }
                return 8;
            }
        }

        public long Value { get { return (long)size; } }
        public static readonly ElementSize Zero = new ElementSize(0);
        public static ElementSize Read(Stream source)
        {
            return new ElementSize(0);
        }
    }
}
