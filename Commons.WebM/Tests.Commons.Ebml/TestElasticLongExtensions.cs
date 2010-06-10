using System;
using System.IO;

using Commons.Ebml;

using NUnit.Framework;

namespace Tests.Commons.Ebml
{
	[TestFixture(Description = "Test the serialization of longs")]
	public class TestElasticLongExtensions
	{
		[Test, ExpectedException("System.IO.InvalidDataException")]
		public void TestReadElasticUnsignedLongWithZeroedLengthByteExpectsException ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0 });
			ulong result = ms.ReadElasticUnsignedLong ();
		}

		[Test]
		public void TestReadElasticUnsignedLongWithOneByteExpectsException ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x8a });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0xa, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithTwoBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x45, 0x55 });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0x555, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithThreeBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x3a, 0x55, 0xaa });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0x1a55aa, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithFourBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x1a, 0x55, 0xaa, 0x55 });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0xa55aa55, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithFiveBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0xa, 0x55, 0xaa, 0x55, 0xaa });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0x255aa55aaL, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithSixBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x4, 0x55, 0xaa, 0x55, 0xaa, 0x55 });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0x55aa55aa55L, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithSevenBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x2, 0x55, 0xaa, 0x55, 0xaa, 0x55, 0xaa });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0x55aa55aa55aaL, result);
		}

		[Test]
		public void TestReadElasticUnsignedLongWithEightBytesExpectsPartial ()
		{
			MemoryStream ms = new MemoryStream (new byte[] { 0x1, 0x55, 0xaa, 0x55, 0xaa, 0x55, 0xaa, 0x55 });
			ulong result = ms.ReadElasticUnsignedLong ();
			Assert.AreEqual (0x55aa55aa55aa55L, result);
		}
		
	}
}
