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
	public class ClassBinaryReader: IDisposable
	{
		protected Stream m_stream;

		public ClassBinaryReader(Stream stream)
		{
			this.m_stream = stream;
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
		
		public virtual byte ReadByte()
		{
			byte[] buffer = new byte[1];
			int bytesRead = m_stream.Read(buffer, 0, 1);

			if (bytesRead < 1)
				throw new EndOfStreamException();

			return buffer[0];
		}

		public virtual sbyte ReadSByte()
		{
			return unchecked((sbyte)ReadByte());
		}

		public virtual byte[] ReadBytes(int numBytes)
		{
			byte[] buffer = new byte[numBytes];
			int bytesRead = m_stream.Read(buffer, 0, numBytes);

			if (bytesRead < numBytes)
				throw new EndOfStreamException();

			return buffer;
		}

		public string ReadString()
		{
			ushort len = ReadUShort();
			return ModifiedUTF8.GetString(ReadBytes(len));
		}

		protected virtual ReadOnlySpan<byte> InternalRead(int numBytes)
		{
			byte[] buffer = new byte[numBytes];
			int bytesRead = m_stream.Read(buffer, 0, numBytes);

			if(bytesRead < numBytes)
				throw new EndOfStreamException();

			return new ReadOnlySpan<byte>(buffer);
		}

		public void Dispose()
		{
			((IDisposable)m_stream).Dispose();
		}
	}
}
