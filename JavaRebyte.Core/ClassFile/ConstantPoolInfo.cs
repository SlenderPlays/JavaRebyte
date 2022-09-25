using System;
using System.Collections;
using System.Collections.Generic;

namespace JavaRebyte.Core.ClassFile
{
	/// <summary>
	/// An entry into the Constant Pool. This class and all classes that inherit from this should be treated as "data-holders", they represent what was
	/// written in a java class file and little more. Auxiliary methods are welcome, though reading/writing should be handeled by <see cref="ClassFile.Util.ClassBinaryReader"/>.
	/// </summary>
	public abstract class ConstantPoolInfo
	{
		// TODO: if needed in the future, implement this. A static method with a switch state could also suffice for more central code.
		// public virtual ConstantPoolTag GetTag()
		// {
		//	return ConstantPoolTag.NONE;
		// }
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

		public ConstantUTF8Info() { }

		public ConstantUTF8Info(string stringValue)
		{
			this.stringValue = stringValue;
		}
	}

	/// <summary>
	/// Simple enough, it represents an integer. Thouuugh it usually represents an integer and NOT a short or a byte. <br/>
	/// Constant shorts/bytes seem to be loaded onto the stuck using `bipush` and `sipush` (byte/shot as intger push).
	/// Also the values -1 to 5 have special opcodes asigned to them to be pushed onto the stack.<br/>
	/// Although, if a short/byte value is used often enough, the compiler MIGHT put it as a constant reference, though that is a guess. Testing is required.
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.4"/>
	/// </summary>
	public class ConstantIntegerInfo : ConstantPoolInfo
	{
		public int value;

		public ConstantIntegerInfo() { }

		public ConstantIntegerInfo(int value)
		{
			this.value = value;
		}
	}

	/// <summary>
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.4"/>
	/// </summary>
	public class ConstantFloatInfo : ConstantPoolInfo
	{
		public float value;

		public ConstantFloatInfo() { }

		public ConstantFloatInfo(float value)
		{
			this.value = value;
		}
	}

	/// <summary>
	/// This structure is used to represent a class or an interface. <br/>
	/// Simply put, this structure is used for instantiating a new class (new <classinfo>) or used by other constant pool entries (or any other number of things). <br/>
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.1"/>
	/// </summary>
	public class ConstantClassInfo : ConstantPoolInfo
	{
		/// <summary>
		/// Index of the <see cref="ConstantUTF8Info"/> entry which hold the name of the class/interface reference.<br/>
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort name_index;

		public ConstantClassInfo() { }

		public ConstantClassInfo(ushort name_index)
		{
			this.name_index = name_index;
		}

		// TODO: Look at this - do we need this?
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
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort class_index;
		/// <summary>
		/// Index of the <see cref="ConstantNameAndTypeInfo"/> entry this field reference. The descriptor (type) must be of type "FieldDescriptor".<br/>
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort name_and_type_index;

		public ConstantFieldReferenceInfo() { }

		public ConstantFieldReferenceInfo(ushort class_index, ushort name_and_type_index)
		{
			this.class_index = class_index;
			this.name_and_type_index = name_and_type_index;
		}
	}

	/// <summary>
	/// Holds a reference to a method. Basically a function pointer. 
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.2"/>
	/// </summary>
	public class ConstantMethodReferenceInfo : ConstantPoolInfo
	{
		/// <summary>
		/// Index of the <see cref="ConstantClassInfo"/> entry which holds the class <b>(not interface)</b> of this method reference.<br/>
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort class_index;
		/// <summary>
		/// Index of the <see cref="ConstantNameAndTypeInfo"/> entry this field reference. <br/>
		/// The descriptor (type) must be of type "MethodDescriptor". <br/>
		/// If the name begins with a '<' ('\u003c'), then the name must be the special name <init>, representing an instance initialization method.
		/// The return type of such a method must be void. <br/>
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort name_and_type_index;

		public ConstantMethodReferenceInfo() { }

		public ConstantMethodReferenceInfo(ushort class_index, ushort name_and_type_index)
		{
			this.class_index = class_index;
			this.name_and_type_index = name_and_type_index;
		}
	}

	/// <summary>
	/// Holds a reference to a method belonging to an interface. Basically a function pointer... but for interfaces. 
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.2"/>
	/// </summary>
	public class ConstantInterfaceMethodReferenceInfo : ConstantPoolInfo
	{
		/// <summary>
		/// Index of the <see cref="ConstantClassInfo"/> entry which holds the interface <b>(not class)</b> of this method reference.<br/>
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort class_index;
		/// <summary>
		/// Index of the <see cref="ConstantNameAndTypeInfo"/> entry this field reference. <br/>
		/// The descriptor (type) must be of type "MethodDescriptor". <br/>
		/// Reminder: constant pool is 1-indexed.
		/// </summary>
		public ushort name_and_type_index;

		public ConstantInterfaceMethodReferenceInfo() { }

		public ConstantInterfaceMethodReferenceInfo(ushort class_index, ushort name_and_type_index)
		{
			this.class_index = class_index;
			this.name_and_type_index = name_and_type_index;
		}
	}


	public class ConstantNameAndTypeInfo : ConstantClassInfo
	{

	}
}