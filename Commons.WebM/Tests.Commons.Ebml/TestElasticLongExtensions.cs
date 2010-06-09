using System;
using System.IO;

using Commons.Ebml;

using NUnit.Framework;

namespace Tests.Commons.Ebml
{
    [TestFixture(Description = "Test the serialization of longs")]
    class TestElasticLongExtensions
    {
        [Test, ExpectedException("System.IO.InvalidDataException")]
        public void TestReadElasticUnsignedLongWithZeroedLengthByteExpectsException()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0 });
            ulong result = ms.ReadElasticUnsignedLong();
        }

        [Test]
        public void TestReadElasticUnsignedLongWithOneByteExpectsException()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x8A });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0A, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithTwoBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x45, 0x55 });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0555, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithThreeBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x3A, 0x55, 0xAA });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x1A55AA, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithFourBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x1A, 0x55, 0xAA, 0x55 });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0A55AA55, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithFiveBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x0A, 0x55, 0xAA, 0x55, 0xAA });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0255AA55AA, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithSixBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x04, 0x55, 0xAA, 0x55, 0xAA, 0x55 });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0055AA55AA55, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithSevenBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x02, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0055AA55AA55AA, result);
        }

        [Test]
        public void TestReadElasticUnsignedLongWithEightBytesExpectsPartial()
        {
            MemoryStream ms = new MemoryStream(new byte[] { 0x01, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55 });
            ulong result = ms.ReadElasticUnsignedLong();
            Assert.AreEqual(0x0055AA55AA55AA55, result);
        }

    }
}
