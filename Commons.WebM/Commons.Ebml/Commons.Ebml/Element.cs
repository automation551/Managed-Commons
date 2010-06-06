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

	public class Element
	{
		protected byte[] data;
		protected bool dataRead = false;

		public Element (ElementId type, int minSizeLength)
		{
			Type = type;
			MinSizeLength = minSizeLength;
		}

		public void ReadData (Stream source)
		{
			if (Size > 0) {
				data = new byte[Size];
				source.Read (data, 0, data.Length);
			}
			dataRead = true;
		}

		public virtual void SkipData (Stream source)
		{
			if (Size > 0 && !dataRead) {
				source.Seek (Size, SeekOrigin.Current);
			}
			dataRead = true;
		}

		public long Write (Stream writer)
		{
			return WriteHeader (writer) + WriteData (writer);
		}

		public long WriteHeader (Stream writer)
		{
			byte[] bytes = Type.ToCode (Size, MinSizeLength);
			int howMany = bytes.Length;
			writer.Write (bytes, 0, howMany);
			return howMany;
		}

		public long WriteData (Stream writer)
		{
			int howMany = data.Length;
			writer.Write (data, 0, howMany);
			return howMany;
		}

		public virtual byte[] Data {
			get { return data; }

			set {
				data = value;
				Size = data.LongLength;
			}
		}

		public void Clear ()
		{
			data = null;
			Size = 0;
		}

		public ElementId Type { get; private set; }
		public ElementPrototype ElementPrototype { get; set; }
		public Element Parent { get; set; }
		public long Size { get; set; }
		public int MinSizeLength { get; set; }

		public long TotalSize {
			get {
				long totalSize = Type.Length;
				totalSize += Size.GetCodedLength();
				totalSize += Size;
				return totalSize;
			}
		}

		public byte[] ToByteArray ()
		{
			byte[] head = Type.ToCode (Size, MinSizeLength);
			byte[] ret = new byte[head.Length + data.Length];
			Array.Copy (head, ret, head.Length);
			Array.Copy (data, 0, ret, head.Length, data.Length);
			return ret;
		}

		public override bool Equals (object obj)
		{
			Element other = obj as Element;
			return other != null && Equals (other.Type);
		}

		public bool Equals (ElementId typeId)
		{
			return ElementId.CompareIDs (Type, typeId);
		}

		public bool Equals (ElementPrototype elemType)
		{
			return Equals (elemType.id);
		}

		public override int GetHashCode ()
		{
			return Type.GetHashCode ();
		}


	}
}
