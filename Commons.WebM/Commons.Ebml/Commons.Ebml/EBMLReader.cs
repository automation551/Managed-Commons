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
using System.IO;

namespace Commons.Ebml
{

    public class EBMLReader
    {

        protected Stream source;
        protected IDocumentRecognizer doc;
        protected ElementPrototype elementTypes;
        protected ElementPrototype lastElementType;

        public EBMLReader(Stream source, IDocumentRecognizer doc)
        {
            this.source = source;
            this.doc = doc;
            this.elementTypes = doc.getElements();
        }

        public Element readNextElement()
        {
             ElementId elementType = ElementId.Read(source);

            if (elementType == null)
                // Failed to read type id
                return null;

            //Read the size.
            long elementSize = readEBMLCode(source);
            if (elementSize == 0)
                // Failed to read element size
                return null;

            Element elem = null;
            elem = doc.CreateElement(elementType);

            if (elem == null)
            {
                return null;
            }

            //Set it's size
            elem.setSize(elementSize);

            //Return the element
            return elem;
        }

        /**
         * Reads an (Unsigned) EBML code from the Stream and encodes it into a long.  This size should be
         * cast into an int for actual use as Java only allows upto 32-bit file I/O operations.
         *
         * @return ebml size
         */
        static public long readEBMLCode(Stream source)
        {
            //Begin loop with byte set to newly read byte.
            byte firstByte = source.readByte();
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
                // Invalid size
                return 0;

            //Setup space to store the bits
            byte[] data = new byte[numBytes];

            //Clear the 1 at the front of this byte, all the way to the beginning of the size
            data[0] = (byte)(firstByte & ((0xFF >> (numBytes))));

            if (numBytes > 1)
            {
                //Read the rest of the size.
                source.read(data, 1, numBytes - 1);
            }

            //Put this into a long
            long size = 0;
            long n = 0;
            for (int i = 0; i < numBytes; i++)
            {
                n = ((long)data[numBytes - 1 - i] << 56) >> 56;
                size = size | (n << (8 * i));
            }
            return size;
        }

        /**
         * Reads an (Unsigned) EBML code from the Stream and encodes it into a long.  This size should be
         * cast into an int for actual use as Java only allows upto 32-bit file I/O operations.
         *
         * @return ebml size
         */
        static public long readEBMLCode(byte[] source)
        {
            return readEBMLCode(source, 0);
        }

        /**
         * Reads an (Unsigned) EBML code from the Stream and encodes it into a long.  This size should be
         * cast into an int for actual use as Java only allows upto 32-bit file I/O operations.
         *
         * @return ebml size
         */
        static public long readEBMLCode(byte[] source, int offset)
        {
            //Begin loop with byte set to newly read byte.
            byte firstByte = source[offset];
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
                // Invalid size
                return 0;

            //Setup space to store the bits
            byte[] data = new byte[numBytes];

            //Clear the 1 at the front of this byte, all the way to the beginning of the size
            data[0] = (byte)(firstByte & ((0xFF >> (numBytes))));

            if (numBytes > 1)
            {
                //Read the rest of the size.
                Array.Copy(data, 1, source, offset + 1, numBytes - 1);
            }

            //Put this into a long
            long size = 0;
            long n = 0;
            for (int i = 0; i < numBytes; i++)
            {
                n = ((long)data[numBytes - 1 - i] << 56) >> 56;
                size = size | (n << (8 * i));
            }
            return size;
        }

        /**
         * Reads an Signed EBML code from the Stream and encodes it into a long.  This size should be
         * cast into an int for actual use as Java only allows upto 32-bit file I/O operations.
         *
         * @return ebml size
         */
        static public long readSignedEBMLCode(byte[] source)
        {
            return readSignedEBMLCode(source, 0);
        }

        /**
         * Reads an Signed EBML code from the Stream and encodes it into a long.  This size should be
         * cast into an int for actual use as Java only allows upto 32-bit file I/O operations.
         *
         * @return ebml size
         */
        static public long readSignedEBMLCode(byte[] source, int offset)
        {
            //Begin loop with byte set to newly read byte.
            byte firstByte = source[offset];
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
                // Invalid size
                return 0;

            //Setup space to store the bits
            byte[] data = new byte[numBytes];

            //Clear the 1 at the front of this byte, all the way to the beginning of the size
            data[0] = (byte)(firstByte & ((0xFF >> (numBytes))));

            if (numBytes > 1)
            {
                //Read the rest of the size.
                Array.Copy(data, 1, source, offset + 1, numBytes - 1);
            }

            //Put this into a long
            long size = 0;
            long n = 0;
            for (int i = 0; i < numBytes; i++)
            {
                n = ((long)data[numBytes - 1 - i] << 56) >> 56;
                size = size | (n << (8 * i));
            }

            // Sign it ;)
            if (numBytes == 1)
            {
                size -= 63;

            }
            else if (numBytes == 2)
            {
                size -= 8191;

            }
            else if (numBytes == 3)
            {
                size -= 1048575;

            }
            else if (numBytes == 4)
            {
                size -= 134217727;
            }

            return size;
        }

        /**
         * Reads an Signed EBML code from the Stream and encodes it into a long.  This size should be
         * cast into an int for actual use as Java only allows upto 32-bit file I/O operations.
         *
         * @return ebml size
         */
        static public long readSignedEBMLCode(Stream source)
        {

            //Begin loop with byte set to newly read byte.
            byte firstByte = source.readByte();
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
                // Invalid size
                return 0;

            //Setup space to store the bits
            byte[] data = new byte[numBytes];

            //Clear the 1 at the front of this byte, all the way to the beginning of the size
            data[0] = (byte)(firstByte & ((0xFF >> (numBytes))));

            if (numBytes > 1)
            {
                //Read the rest of the size.
                source.read(data, 1, numBytes - 1);
            }

            //Put this into a long
            long size = 0;
            long n = 0;
            for (int i = 0; i < numBytes; i++)
            {
                n = ((long)data[numBytes - 1 - i] << 56) >> 56;
                size = size | (n << (8 * i));
            }

            // Sign it ;)
            if (numBytes == 1)
            {
                size -= 63;

            }
            else if (numBytes == 2)
            {
                size -= 8191;

            }
            else if (numBytes == 3)
            {
                size -= 1048575;

            }
            else if (numBytes == 4)
            {
                size -= 134217727;
            }

            return size;
        }

     }
}
