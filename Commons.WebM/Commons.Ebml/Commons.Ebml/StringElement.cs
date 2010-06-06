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
using System.Text;

namespace Commons.Ebml
{

	public class StringElement : BinaryElement
	{

		private string charset = "UTF-8";

		public StringElement (ElementId type, int minSizeLength) : base(type, minSizeLength)
		{
		}

		public StringElement (ElementId type, string encoding, int minSizeLength) : base(type, minSizeLength)
		{
			charset = encoding;
		}

		public string Value {
			get {
				if (data == null)
					throw new System.InvalidOperationException ("Call ReadData() before trying to extract the string value.");
				
				return EncodingToUse.GetString (data);
			}

			set { Data = EncodingToUse.GetBytes (value); }
		}

		private Encoding EncodingToUse {
			get { return Encoding.GetEncoding (charset); }
		}

		public string getEncoding ()
		{
			return charset;
		}
	}
}
