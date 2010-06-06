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

namespace Commons.Ebml
{
	public static class ElasticLongExtensions
	{
		public static byte[] ToBytes (this long value, int minLength)
		{
			int codedLength = GetCodedLength (value);
			if (minLength < 9 && minLength > codedLength)
				codedLength = minLength;
			return new byte[codedLength];
		}


		public static byte[] ToBytes (this ulong value, int minLength)
		{
			int codedLength = GetCodedLength (value);
			if (minLength < 9 && minLength > codedLength)
				codedLength = minLength;
			return new byte[codedLength];
		}


		public static int GetCodedLength (this ulong value)
		{
			int size = 8;
			ulong mask = 0xff00000000000000uL;
			for (int i = 0; i < 8; i++) {
				if ((value & mask) == 0) {
					mask = mask >> 8;
					size--;
				} else {
					return size;
				}
			}
			return 8;
		}

		public static int GetCodedLength (this long value)
		{
			int size = 8;
			unchecked {
				ulong mask = 0xff00000000000000uL;
				ulong uvalue = (ulong)value;				
				for (int i = 0; i < 8; i++) {
					if ((uvalue & mask) == 0) {
						mask = mask >> 8;
						size--;
					} else {
						return size;
					}
				}
			}
			return 8;
		}

		public static ulong ReadElasticUnsignedLong (this Stream source)
		{
			return 0;
		}

		public static long ReadElasticSignedLong (this Stream source)
		{
			return 0;
		}
		public static byte[] makeEbmlCode (byte[] typeID, long size, int minSizeLength)
		{
			int codedLen = codedSizeLength (size, minSizeLength);
			byte[] ret = new byte[typeID.Length + codedLen];
			Array.Copy (typeID, ret, typeID.Length);
			byte[] codedSize = makeEbmlCodedSize (size, minSizeLength);
			Array.Copy (codedSize, 0, ret, typeID.Length, codedSize.Length);
			return ret;
		}

		public static byte[] makeEbmlCodedSize (long size, int minSizeLength)
		{
			int len = codedSizeLength (size, minSizeLength);
			byte[] ret = new byte[len];
			long mask = 0xffL;
			for (int i = 0; i < len; i++) {
				ret[len - 1 - i] = (byte)((size & mask) >> (i * 8));
				mask <<= 8;
			}
			//The first size bits should be clear, otherwise we have an error in the size determination.
			ret[0] |= (byte)(0x80 >> (len - 1));
			return ret;
		}

		public static int getMinByteSize (long value)
		{
			if (value <= 0x7f && value >= 0x80) {
				return 1;
			} else if (value <= 0x7fff && value >= 0x8000) {
				return 2;
			} else if (value <= 0x7fffff && value >= 0x800000) {
				return 3;
			} else if (value <= 0x7fffffff && value >= 0x80000000u) {
				return 4;
			} else if (value <= 0x7fffffffffL && value >= 0x8000000000L) {
				return 5;
			} else if (value <= 0x7fffffffffffL && value >= 0x800000000000L) {
				return 6;
			} else if (value <= 0x7fffffffffffffL && value >= 0x80000000000000L) {
				return 7;
			} else {
				return 8;
			}
		}

		public static int getMinByteSizeUnsigned (ulong value)
		{
			int size = 8;
			ulong mask = 0xff00000000000000uL;
			for (int i = 0; i < 8; i++) {
				if ((value & mask) == 0) {
					mask = mask >> 8;
					size--;
				} else {
					return size;
				}
			}
			return 8;
		}

		public static int codedSizeLength (long value, int minSizeLength)
		{
			int codedSize = 0;
			if (value < 127) {
				codedSize = 1;
			} else if (value < 16383) {
				codedSize = 2;
			} else if (value < 2097151) {
				codedSize = 3;
			} else if (value < 268435455) {
				codedSize = 4;
			}
			if ((minSizeLength > 0) && (codedSize <= minSizeLength)) {
				codedSize = minSizeLength;
			}
			return codedSize;
		}

		public static byte[] Pack (this ulong value)
		{
			int size = getMinByteSizeUnsigned (value);
			byte[] ret = new byte[size];
			ulong mask = 0xffL;
			int b = size - 1;
			for (int i = 0; i < size; i++) {
				ret[b] = (byte)(((value >> (8 * i)) & mask));
				b--;
			}
			return ret;
		}

		public static byte[] Pack (this long value)
		{
			int size = getMinByteSize (value);
			return packInt (value, size);
		}

		public static byte[] packInt (long value, int size)
		{
			byte[] ret = new byte[size];
			long mask = 0xffL;
			int b = size - 1;
			for (int i = 0; i < size; i++) {
				ret[b] = (byte)(((value >> (8 * i)) & mask));
				b--;
			}
			return ret;
		}
	}
}
