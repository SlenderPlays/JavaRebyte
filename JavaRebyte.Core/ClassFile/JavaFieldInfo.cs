using System.Collections.Generic;

namespace JavaRebyte.Core.ClassFile
{
	public class JavaFieldInfo
	{
		private ConstantPool ConstantPool;

		public FieldAccessFlags accessFlags;
		public ushort nameIndex;
		public ushort descriptorIndex;
		public ushort attributesCount;
		public List<AttributeInfo> attributes = new List<AttributeInfo>();

		public ConstantUTF8Info GetName()
		{
			return (ConstantUTF8Info)ConstantPool[nameIndex];
		}
		public ConstantUTF8Info GetDescriptor()
		{
			return (ConstantUTF8Info)ConstantPool[descriptorIndex];
		}
	}
}