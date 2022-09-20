using System;
using System.Collections;
using System.Collections.Generic;

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
	/// This structure, as one might first believe, does not represent a 'string' <b>object</b> (<see cref="ConstantStringInfo"/>), instead
	/// it represent a string, usually used by other entries in the constant pool to reference the names of classes, the signature of the method, the
	/// name of a field, and a limitless number of things.<br/>
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.7"/>
	/// </summary>
	public class ConstantUTF8Info : ConstantPoolInfo
	{
		public string stringValue;

		public ConstantUTF8Info()
		{
			this.tag = ConstantPoolTag.UTF8;
		}

		public ConstantUTF8Info(string stringValue)
		{
			this.tag = ConstantPoolTag.UTF8;
			this.stringValue = stringValue;
		}
	}

	/// <summary>
	/// Simple enough, it reprsents an integer. Thouuugh it represents an integer and NOT a short or a byte. <br/>
	/// Constant shorts/bytes seem to be loaded onto the stuck using `bipush` and `sipush` (byte/shot as intger push).
	/// Also the values -1 to 5 have special opcodes asigned to them to be pushed onto the stack.<br/>
	/// Although, if a short/byte value is used often enough, the compiler MIGHT put it as a constant reference, though that is a guess. Testing is required.
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.4"/>
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
	/// This structure is used to represent a class or an interface. <br/>
	/// Simply put, this structure is used for instanciating a new class (new <classinfo>) or used by other constant pool entries (or any other number of things). <br/>
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.1"/>
	/// </summary>
	public class ConstantClassInfo : ConstantPoolInfo
	{
		/// <summary>
		/// Index of the <see cref="ConstantUTF8Info"/> entry which hold the name of the class/interface reference.<br/>
		/// Index should be used on a 0-indexed collection, as opposed to how the specification calls for a 1-indexed collection.
		/// </summary>
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

		/// <summary>
		/// This is a shorthand for indexing a 0-indexed ConstantPoolInfo collection.
		/// </summary>
		/// <param name="constantPool">0-indexed collection which holds the constant pool information</param>
		/// <returns>The UTF8 name of the referenced class/interface.</returns>
		public ConstantUTF8Info GetName(IReadOnlyList<ConstantPoolInfo> constantPool)
		{		
			return (ConstantUTF8Info)constantPool[name_index];
		}
	}

	/// <summary>
	/// Holds a reference to a field. Unlike in regular Java, the 'reference' isn't to a particular field in an instance of
	/// a class, but rather it just points to a field inside a class (not a specific instance).<br/>
	/// The opcodes 'putfield' and 'getfield' take both an instance and a FieldReference. <br/>
	/// The opcodes 'getstatic' and 'putstatic' do not need an attached instance since they operate on static (and thus instance-independent) fields.
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.2"/>
	/// </summary>
	public class ConstantFieldReferenceInfo : ConstantPoolInfo
	{
		/// <summary>
		/// Index of the <see cref="ConstantClassInfo"/> entry which holds the class/interface of this field reference.<br/>
		/// Index should be used on a 0-indexed collection, as opposed to how the specification calls for a 1-indexed collection.
		/// </summary>
		public ushort class_index;
		/// <summary>
		/// Index of the <see cref="ConstantNameAndTypeInfo"/> entry this field reference.<br/>
		/// Index should be used on a 0-indexed collection, as opposed to how the specification calls for a 1-indexed collection.
		/// </summary>
		public ushort name_and_type_index;
	}

	public class ConstantNameAndTypeInfo : ConstantClassInfo
	{ }

}