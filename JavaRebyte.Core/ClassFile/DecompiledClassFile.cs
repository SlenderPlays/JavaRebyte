using System;
using System.Collections.Generic;
using System.Reflection;

namespace JavaRebyte.Core.ClassFile
{
	public class DecompiledClassFile
	{
		public uint magic;
		public ushort minor_version;
		public ushort major_version;

		public ushort constant_pool_count;
        public List<ConstantPoolInfo> constant_pool = new List<ConstantPoolInfo>();

        public ushort access_flags;
        public ushort this_class;
        public ushort super_class;

        public ushort interfaces_count;
        public List<ushort> interfaces = new List<ushort>();

        public ushort fields_count;
        public List<FieldInfo> fields = new List<FieldInfo>();

        public ushort methods_count;
        public List<JavaMethodInfo> methods = new List<JavaMethodInfo>();

        public ushort attributes_count;
        public List<AttributeInfo> attributes = new List<AttributeInfo>();

		
        public DecompiledClassFile(string filepath)
		{

		}
        #region File Reading
        #endregion
    }
}
