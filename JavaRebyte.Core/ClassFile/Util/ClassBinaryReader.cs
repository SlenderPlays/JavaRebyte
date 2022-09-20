using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JavaRebyte.Core.ClassFile.Util
{
	/// <summary>
	/// This class is inspired by <see cref="BinaryReader"/> and using <see cref="BinaryPrimitives"/> to operate specifically for
	/// Java's .class file. It so happens that these files are BigEndian-encoded so the base Binary Reader has trouble dealing with them.
	/// <br/>
	/// The implementation should be able to handle generic BigEndian-encoded files, though some methods might not be useful for this generic case.
	/// <br/>
	/// <br/>
	/// Base implementaion can be found at https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/IO/BinaryReader.cs
	/// </summary>
	public class ClassBinaryReader
	{
		protected Stream m_stream;
		private Encoding m_encoding;

		public ClassBinaryReader(Stream stream, Encoding encoding)
		{
			this.m_stream = stream;
			this.m_encoding = encoding;
		}

		public virtual ushort	ReadUShort()	=> BinaryPrimitives.ReadUInt16BigEndian(InternalRead(2));
		public virtual uint		ReadUInt()		=> BinaryPrimitives.ReadUInt32BigEndian(InternalRead(4));
		public virtual ulong	ReadULong()		=> BinaryPrimitives.ReadUInt64BigEndian(InternalRead(8));

		public virtual short	ReadShort()		=> BinaryPrimitives.ReadInt16BigEndian(InternalRead(2));
		public virtual int		ReadInt()		=> BinaryPrimitives.ReadInt32BigEndian(InternalRead(4));
		public virtual long		ReadLong()		=> BinaryPrimitives.ReadInt64BigEndian(InternalRead(8));

		public virtual float ReadFloat()
		{
			if(!BitConverter.IsLittleEndian)
				return BitConverter.ToSingle(InternalRead(4));
			else
			{
				byte[] arr = InternalRead(4).ToArray();
				Array.Reverse(arr, 0, 4);

				return BitConverter.ToSingle(arr);
			}

		}
		

		protected virtual ReadOnlySpan<byte> InternalRead(int numBytes)
		{
			byte[] buffer = new byte[numBytes];
			int bytesRead = m_stream.Read(buffer, 0, numBytes);

			if(bytesRead < numBytes)
				throw new EndOfStreamException();

			return new ReadOnlySpan<byte>(buffer);
		}
	}
}
