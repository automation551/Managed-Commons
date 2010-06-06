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
namespace Commons.Ebml
{

    public class SignedIntegerElement
        : BinaryElement
    {

        public SignedIntegerElement(ElementId type, int minSizeLength)
            : base(type, minSizeLength)
        {
        }

        public long Value
        {
            set
            {
                Data = value.Pack();
            }

            get
            {
                long l = 0;
                long tmp = 0;
                l |= ((long)data[0] << (56 - ((8 - data.Length) * 8)));
                for (int i = 1; i < data.Length; i++)
                {
                    tmp = ((long)data[data.Length - i]) << 56;
                    tmp >>= 56 - (8 * (i - 1));
                    l |= tmp;
                }
                return l;
            }
        }

    }
}
