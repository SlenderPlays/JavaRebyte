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
        public ConstantPool constant_pool = new ConstantPool();

        public ClassAccessFlags access_flags;
        public ushort this_class;
        public ConstantClassInfo ThisClass => (ConstantClassInfo)constant_pool[this_class];
        public ushort super_class;
        public ConstantClassInfo SuperClass => (ConstantClassInfo)constant_pool[super_class];

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
			using MemoryStream ms = new MemoryStream(byteContent);
			using ClassBinaryReader reader = new ClassBinaryReader(ms);

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

            this.access_flags = (ClassAccessFlags)reader.ReadUShort();
            this.this_class = reader.ReadUShort();
            this.super_class = reader.ReadUShort();

            this.interfaces_count = reader.ReadUShort();
			for (int i = 0; i < interfaces_count; i++)
			{
                this.interfaces.Add(reader.ReadUShort());
			}

            this.fields_count = reader.ReadUShort();
			for (int i = 0; i < fields_count; i++)
			{

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

        public ConstantClassInfo GetInterface(int index)
        {
            if(index < 0 || index >= interfaces_count)
                return null;

            return (ConstantClassInfo)constant_pool[interfaces[index]];
        }
    }
}
