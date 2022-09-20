using System;

namespace JavaRebyte.Core.ClassFile
{
	public class ConstantPoolInfo
	{
		public ConstantPoolTag tag = ConstantPoolTag.NONE;

		public ConstantPoolInfo() { }
		public ConstantPoolInfo(ConstantPoolTag tag)
		{
			this.tag = tag;
		}
	}

	/// <summary>
	/// https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.7
	/// </summary>
	public class ConstantUTF8Info : ConstantPoolInfo
	{
		public ushort length { get; private set; }
		public byte[] byteData { get; private set; }

		protected string cachedString = "";
		public string StringValue
		{
			get
			{
				if (!cacheValid)
					CreateStringValueCache();
				return cachedString;				
			}
		}

		protected bool cacheValid = false;

		public ConstantUTF8Info()
		{
			this.tag = ConstantPoolTag.UTF8;
		}

		public ConstantUTF8Info(ushort length, byte[] byteData)
		{
			this.tag = ConstantPoolTag.UTF8;
			SetData(length, byteData, true);
		}

		/// <summary>
		/// Set the byte data of this UTF8-entry.
		/// </summary>
		/// <param name="length">Length of the string</param>
		/// <param name="byteData">UTF8 string encoded acording to java specifications</param>
		/// <param name="createCache">If set to true, the string value of the byte data will be interpreted now, and not when the string value is first requested.</param>
		public void SetData(ushort length, byte[] byteData, bool createCache = false)
		{
			this.length = length;
			this.byteData = byteData;
			cacheValid = false;

			if (createCache)
				CreateStringValueCache();
		}

		/// <summary>
		/// Sets the string value for this entry, and byteData will be automatically calculated.
		/// </summary>
		/// <param name="str"></param>
		public void SetValue(string str)
		{
			throw new NotImplementedException();
		}

		protected void CreateStringValueCache()
		{
			throw new NotImplementedException();
			cachedString = "something";
			cacheValid = true;
		}
	}

	/// <summary>
	/// https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.4
	/// </summary>
	public class ConstantIntegerInfo : ConstantPoolInfo
	{
		public int value;

		public ConstantIntegerInfo()
		{
			this.tag = ConstantPoolTag.INTEGER;
		}

		public ConstantIntegerInfo(int value)
		{
			this.tag = ConstantPoolTag.INTEGER;
			this.value = value;
		}
	}

	/// <summary>
	/// https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.4
	/// </summary>
	public class ConstantFloatInfo : ConstantPoolInfo
	{
		public float value;

		public ConstantFloatInfo()
		{
			this.tag = ConstantPoolTag.FLOAT;
		}

		public ConstantFloatInfo(float value)
		{
			this.tag = ConstantPoolTag.FLOAT;
			this.value = value;
		}
	}

	/// <summary>
	/// https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.1
	/// </summary>
	public class ConstantClassInfo : ConstantPoolInfo
	{
		public ushort name_index;

		public ConstantClassInfo()
		{
			this.tag = ConstantPoolTag.CLASS;
		}

		public ConstantClassInfo(ushort name_index)
		{
			this.tag = ConstantPoolTag.CLASS;
			this.name_index = name_index;
		}
	}
}