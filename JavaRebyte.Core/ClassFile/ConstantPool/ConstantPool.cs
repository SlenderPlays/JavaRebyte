using JavaRebyte.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaRebyte.Core.ClassFile
{
	public class ConstantPool
	{
		protected List<ConstantPoolInfo> m_container = new List<ConstantPoolInfo>();

		public int Count => m_container.Count;

		public ConstantPoolInfo this[int index]
		{ 
			get { return m_container[ToInternalIndex(index)];}
			set { m_container[ToInternalIndex(index)] = value;}
		}

		public int ToInternalIndex(int index)
		{
			// TODO: see if this holds considering longs and doubles
			if (index == 0)
				throw new InvalidReferenceException("Constant Pool reference must not be 0.");
			return index - 1;
		}

		public void Add(ConstantPoolInfo constantInfo) => m_container.Add(constantInfo);
		public List<ConstantPoolInfo> GetContainer() => m_container;
	}
}
