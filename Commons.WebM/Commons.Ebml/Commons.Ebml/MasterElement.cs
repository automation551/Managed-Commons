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

using System.Collections.Generic;

namespace Commons.Ebml
{

    public class MasterElement : Element
    {
        protected long usedSize;
        protected List<Element> children = new List<Element>();

        public MasterElement(ElementId type, int minSizeLength)
            : base(type, minSizeLength)
        {
            usedSize = 0;
        }

        public Element readNextChild(EBMLReader reader)
        {
            if (usedSize >= this.getSize())
                return null;

            Element elem = reader.readNextElement();
            if (elem == null)
                return null;

            elem.setParent(this);

            usedSize += elem.getTotalSize();

            return elem;
        }

        /* Skip the element data */
        public void SkipData(Stream source)
        {
            // Skip the child elements
            source.skip(size - usedSize);
        }

        public long writeData(DataWriter writer)
        {
            long len = 0;
            for (int i = 0; i < children.size(); i++)
            {
                Element elem = (Element)children.get(i);
                len += elem.Write(writer);
            }
            return len;
        }

        public void addChildElement(Element elem)
        {
            children.add(elem);
            size += elem.getTotalSize();
        }
    }
}