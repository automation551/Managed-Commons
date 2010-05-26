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

    public class StringElement
        : BinaryElement
    {

        private string charset = "UTF-8";

        public StringElement(ElementId type, int minSizeLength)
            : base(type, minSizeLength)
        {
        }

        public StringElement(ElementId type, string encoding, int minSizeLength)
            : base(type, minSizeLength)
        {
            charset = encoding;
        }

        private bool checkForCharsetHack()
        {
            // Check if we are trying to read UTF-8, if so lets try UTF8.
            // Microsofts Java supports "UTF8" but not "UTF-8"
            if (charset.compareTo("UTF-8") == 0)
            {
                charset = "UTF8";
                // Let's try again
                return true;
            }
            else if (charset.compareTo("US-ASCII") == 0)
            {
                // This is the same story as UTF-8, 
                // If Microsoft is going to hijack Java they should at least support the orignal :>
                charset = "ASCII";
                // Let's try again
                return true;
            }
            return false;
        }

        public string getValue()
        {
            try
            {
                if (data == null)
                    throw new java.lang.IllegalStateException("Call ReadData() before trying to extract the string value.");

                return new string(data, charset);
            }
            catch (java.IO.UnsupportedEncodingException ex)
            {
                if (checkForCharsetHack())
                {
                    return getValue();
                }
                ex.printStackTrace();
                return "";
            }
        }

        public void setValue(string value)
        {
            try
            {
                setData(value.getBytes(charset));
            }
            catch (java.IO.UnsupportedEncodingException ex)
            {
                if (checkForCharsetHack())
                {
                    setValue(value);
                    return;
                }
                ex.printStackTrace();
            }
        }

        public string getEncoding()
        {
            return charset;
        }
    }
}