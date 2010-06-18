using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Commons.Ebml;

namespace Tests.Commons.Ebml
{
    [TestFixture(Description="Test the utf-8 like serialization of Ids")]
    public class TestElementId
    {
        [Test]
        public void TestCreationFromBytes() {
            ElementId id = new ElementId(1, 2, 3, 4);
            Assert.IsNotNull(id);
            Assert.AreEqual(4, id.Bytes.Length);
            Assert.IsTrue(compareBytes(id.Bytes, new byte[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void TestCreationFromChars() {
            ElementId id = new ElementId('1', '2', '3', '4');
            Assert.IsNotNull(id);
            Assert.AreEqual(4, id.Bytes.Length);
            Assert.IsTrue(compareBytes(id.Bytes, new byte[] { 0x31, 0x32, 0x33, 0x34 }));
        }

        [Test]
        public void TestCreationFromString() {
            ElementId id = new ElementId("1234");
            Assert.IsNotNull(id);
            Assert.AreEqual(4, id.Bytes.Length);
            Assert.IsTrue(compareBytes(id.Bytes, new byte[] { 0x31, 0x32, 0x33, 0x34 }));
        }

        [Test]
        public void TestCreationFromLong() {
            ElementId id = new ElementId(0x31323334L);
            Assert.IsNotNull(id);
            Assert.AreEqual(4, id.Bytes.Length);
            Assert.IsTrue(compareBytes(id.Bytes, new byte[] { 0x31, 0x32, 0x33, 0x34 }));
        }

        private bool compareBytes(byte[] first, byte[] second)
        {
            if (first == null || second == null || first.Length != second.Length) return false;
            for (int i = 0; i < first.Length; i++)
                if (first[i] != second[i])
                    return false;
            return true;
        }
    }
}
