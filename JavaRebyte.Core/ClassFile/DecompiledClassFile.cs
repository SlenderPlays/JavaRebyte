using JavaRebyte.Core.ClassFile.Util;
using JavaRebyte.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace JavaRebyte.Core.ClassFile
{
    // Reference:
    // https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html
    public class DecompiledClassFile
	{
		public uint magic;
		public ushort minor_version;
		public ushort major_version;

		public ushort constant_pool_count;
        // TODO: Replace with ConstantPool object
        public List<ConstantPoolInfo> constant_pool = new List<ConstantPoolInfo>();

        public ushort access_flags;
        public ushort this_class;
        public ushort super_class;

        public ushort interfaces_count;
        public List<ushort> interfaces = new List<ushort>();

        public ushort fields_count;
        public List<JavaFieldInfo> fields = new List<JavaFieldInfo>();

        public ushort methods_count;
        public List<JavaMethodInfo> methods = new List<JavaMethodInfo>();

        public ushort attributes_count;
        public List<AttributeInfo> attributes = new List<AttributeInfo>();

        public DecompiledClassFile(byte[] byteContent)
		{
            using (MemoryStream ms = new MemoryStream(byteContent))
            {
                ClassBinaryReader reader = new ClassBinaryReader(ms, System.Text.Encoding.UTF8);
                this.magic = reader.ReadUInt();

                if (magic != 0xCAFEBABE)
                    throw new DecompilationException($"The provided file isn't a class file, or is corrupted. Expected magic bytes: 'CAFEBABE', got {magic:X}");

                this.minor_version = reader.ReadUShort();
                this.major_version = reader.ReadUShort();
                this.constant_pool_count = reader.ReadUShort();

				// Constant pool start at index 1, for reasons unknown to all
				// constant_pool.Add(new ConstantPoolInfo());
				for (int i = 1; i < constant_pool_count; i++)
				{
                    constant_pool.Add(ReadConstantPoolEntry(reader));
				}

            }
		}
        #region File Reading
        protected virtual ConstantPoolInfo ReadConstantPoolEntry(ClassBinaryReader reader)
		{
            ConstantPoolTag tag = (ConstantPoolTag)reader.ReadByte();
            switch (tag)
			{
                case ConstantPoolTag.UTF8:
                    return new ConstantUTF8Info(reader.ReadString());
                case ConstantPoolTag.INTEGER:
                    return new ConstantIntegerInfo(reader.ReadInt());
                case ConstantPoolTag.FLOAT:
                    return new ConstantFloatInfo(reader.ReadFloat());
                case ConstantPoolTag.CLASS:
                    return new ConstantClassInfo(reader.ReadUShort());
                case ConstantPoolTag.FIELD_REF:
                    return new ConstantFieldReferenceInfo(reader.ReadUShort(), reader.ReadUShort());
                case ConstantPoolTag.METHOD_REF:
                    return new ConstantMethodReferenceInfo(reader.ReadUShort(), reader.ReadUShort());
                case ConstantPoolTag.INTERFACE_METHOD_REF:
                    return new ConstantInterfaceMethodReferenceInfo(reader.ReadUShort(), reader.ReadUShort());
                case ConstantPoolTag.NAME_AND_TYPE:
                    return new ConstantNameAndTypeInfo(reader.ReadUShort(), reader.ReadUShort());
                default: throw new DecompilationException($"Encountered an unknown ConstantPoolTag: {tag} | {(byte)tag}");
			}
		}
        #endregion
    }
}
