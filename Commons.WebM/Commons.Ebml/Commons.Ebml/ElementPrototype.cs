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
using Commons.Ebml.IO;

namespace Commons.Ebml
{
    public enum FundamentalType
    {
        UNKNOWN_ELEMENT = 0,
        MASTER_ELEMENT = 1,
        BINARY_ELEMENT = 2,
        SINTEGER_ELEMENT = 3,
        UINTEGER_ELEMENT = 4,
        FLOAT_ELEMENT = 5,
        STRING_ELEMENT = 6,
        ASCII_STRING_ELEMENT = 7,
        DATE_ELEMENT = 8,
        LAST_ELEMENT_TYPE = 100
    }

    public class ElementPrototype
    {

        public string name;
        public short level;
        public ElementId id;
        public short minSizeLength;
        public FundamentalType type;
        public List<ElementPrototype> children;

        public ElementPrototype()
        {

        }

        public ElementPrototype(string name, short level, ElementId id, FundamentalType type, List<ElementPrototype> children) :
            this(name, level, id, type, children, 0) { }
 
        public ElementPrototype(string name, short level, ElementId id, FundamentalType type, List<ElementPrototype> children, short minSizeLength)
        {
            this.name = name;
            this.level = level;
            this.id = id;
            this.type = type;
            this.children = children;
            this.minSizeLength = minSizeLength;
        }

        public ElementPrototype this[ElementId id]
        {
            get
            {
                if (this.Is(id))
                    return this;

                if (children != null)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        ElementPrototype child = children[i];
                        if (child.Is(id))
                            return child;
                        ElementPrototype grandChild = child[id];
                        if (grandChild != null)
                            return grandChild;
                    }
                }
                return null;
            }
        }

        public bool Is(ElementId id)
        {
            return ElementId.CompareIDs(this.id, id);
        }


        public Element CreateElement()
        {
            Element elem;

            if (this.type == FundamentalType.MASTER_ELEMENT)
            {
                elem = new MasterElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.BINARY_ELEMENT)
            {
                elem = new BinaryElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.STRING_ELEMENT)
            {
                elem = new StringElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.ASCII_STRING_ELEMENT)
            {
                elem = new StringElement(this.id, "US-ASCII", minSizeLength);

            }
            else if (this.type == FundamentalType.SINTEGER_ELEMENT)
            {
                elem = new SignedIntegerElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.UINTEGER_ELEMENT)
            {
                elem = new UnsignedIntegerElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.FLOAT_ELEMENT)
            {
                elem = new FloatElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.DATE_ELEMENT)
            {
                elem = new DateElement(this.id, minSizeLength);

            }
            else if (this.type == FundamentalType.UNKNOWN_ELEMENT)
            {
                elem = new BinaryElement(this.id, minSizeLength);

            }
            else
            {
                return null;
            }
            elem.ElementPrototype = this;
            return elem;
        }
    }
}