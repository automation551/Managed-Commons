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

    public class FloatElement : BinaryElement
    {
        public FloatElement(ElementId type, int minSizeLength)
            : base(type, minSizeLength)
        {
        }

        public double Value
        {
            get { return getValue(); }
            set { setValue(value); }
        }

        private void setValue(double value)
        {
			// TODO: deal with floats
            /*try
            {
                if (value < Float.MAX_VALUE)
                {
                    ByteArrayOutputStream bIO = new ByteArrayOutputStream(4);
                    DataOutputStream dIO = new DataOutputStream(bIO);
                    dIO.writeFloat((float)value);

                    setData(bIO.toByteArray());

                }
                else if (value < Double.MAX_VALUE)
                {
                    ByteArrayOutputStream bIO = new ByteArrayOutputStream(8);
                    DataOutputStream dIO = new DataOutputStream(bIO);
                    dIO.writeDouble(value);

                    setData(bIO.toByteArray());

                }
                else
                {
                    throw new ArithmeticException(
                        "80-bit floats are not supported, BTW How did you create such a large float in Java?");
                }
            }
            catch (IOException ex)
            {
                return;
            } */
        }

        private double getValue()
        {
            /*try
            {
                if (size == 4)
                {
                    float value = 0;
                    ByteArrayInputStream bIS = new ByteArrayInputStream(data);
                    DataInputStream dIS = new DataInputStream(bIS);
                    value = dIS.readFloat();
                    return value;

                }
                else if (size == 8)
                {
                    double value = 0;
                    ByteArrayInputStream bIS = new ByteArrayInputStream(data);
                    DataInputStream dIS = new DataInputStream(bIS);
                    value = dIS.readDouble();
                    return value;

                }
                else
                {
                    throw new ArithmeticException(
                        "80-bit floats are not supported");
                }
            }
            catch (IOException ex)
            {
                return 0;
            }*/
			return 0;
        }
    }
}