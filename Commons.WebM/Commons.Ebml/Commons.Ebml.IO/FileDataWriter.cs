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

	public class FileDataWriter : IDataWriter
	{
		RandomAccessFile file = null;

		public FileDataWriter (string filename)
		{
			file = new RandomAccessFile (filename, "rw");
		}
		public FileDataWriter (string filename, string mode)
		{
			file = new RandomAccessFile (filename, mode);
		}
		public int write (byte b)
		{
			try {
				file.write (b);
				return 1;
			} catch (IOException ex) {
				return 0;
			}
		}
		public int write (byte[] buff)
		{
			try {
				file.write (buff);
				return buff.Length;
			} catch (IOException ex) {
				return 0;
			}
		}
		public int write (byte[] buff, int offset, int length)
		{
			try {
				file.write (buff, offset, length);
				return length;
			} catch (IOException ex) {
				return 0;
			}
		}

		public long length ()
		{
			try {
				return file.Length ();
			} catch (IOException ex) {
				return -1;
			}
		}
		public long getFilePointer ()
		{
			try {
				return file.getFilePointer ();
			} catch (IOException ex) {
				return -1;
			}
		}
		public bool isSeekable ()
		{
			return true;
		}
		public long seek (long pos)
		{
			try {
				file.seek (pos);
				return file.getFilePointer ();
			} catch (IOException ex) {
				return -1;
			}
		}
	}
}
