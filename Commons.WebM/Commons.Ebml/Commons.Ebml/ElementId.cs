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
using System.IO;
using System.Linq;
using System.Text;

namespace Commons.Ebml
{
    public class ElementId : IEquatable<ElementId>
    {
        private byte[] id;

		public ElementId(ulong value)
		{
			List<byte> bytes = new List<byte>();
			byte b;
			while(true) {
				b = (byte)(value & 0xFF);
				value >>= 8;
				if (b == 0)
					break;
				bytes.Add(b);
			}
			id = bytes.ToArray();
			Array.Reverse(id);
		}
		
		public ElementId(params byte[] idBytes)
        {
            id = idBytes;
        }

        public ElementId(params char[] chars)
        {
            id = new byte[chars.Length];
            for (int i = 0; i < id.Length; i++)
                id[i] = (byte)chars[i];
        }

        public ElementId(string chars)
            : this(chars.ToCharArray())
        { }

        public override bool Equals(object obj)
        {
            return Equals(obj as ElementId);
        }

        public bool Equals(ElementId other)
        {
            return other != null && CompareIDs(this, other);
        }
		
		public override int GetHashCode ()
		{
			return id.GetHashCode ();
		}


        public int Length { get { return id.Length; } }

        public byte[] Bytes { get { return id; } }

        public byte[] ToCode(long size, int minSizeLength)
        {
            byte[] sizeBytes = size.ToBytes(minSizeLength);
            byte[] result = new byte[id.Length + sizeBytes.Length];
            Array.Copy(id, result, id.Length);
            Array.Copy(sizeBytes, 0, result, id.Length, sizeBytes.Length);
            return result;
        }
		
		public static ElementId Read(Stream source)
        {
            //Begin loop with byte set to newly read byte.
            byte firstByte = (byte)source.ReadByte();
            int numBytes = 0;

            //Begin by counting the bits unset before the first '1'.
            long mask = 0x0080;
            for (int i = 0; i < 8; i++)
            {
                //Start at left, shift to right.
                if ((firstByte & mask) == mask)
                { //One found
                    //Set number of bytes in size = i+1 ( we must count the 1 too)
                    numBytes = i + 1;
                    //exit loop by pushing i out of the limit
                    i = 8;
                }
                mask >>= 1;
            }
            if (numBytes == 0)
                // Invalid element
                return null;
            //Setup space to store the bits
            byte[] data = new byte[numBytes];

            //Clear the 1 at the front of this byte, all the way to the beginning of the size
            data[0] = (byte)((firstByte));// & ((0xFF >>> (numBytes))));

            if (numBytes > 1)
            {
                //Read the rest of the size.
                source.Read(data, 1, numBytes - 1);
            }
            return new ElementId(data);
        }

 
        public static bool CompareIDs(ElementId first, ElementId second)
        {
            return (first != null && second != null && CompareIDs(first.id, second.id));
        }

        public static bool CompareIDs(byte[] id1, byte[] id2)
        {
            if ((id1 == null) || (id2 == null) || (id1.Length != id2.Length))
                return false;

            for (int i = 0; i < id1.Length; i++)
            {
                if (id1[i] != id2[i])
                    return false;
            }
            return true;
        }

    }

}
