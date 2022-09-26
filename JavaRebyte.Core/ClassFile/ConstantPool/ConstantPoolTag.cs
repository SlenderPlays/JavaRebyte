using System;
using System.Collections.Generic;
using System.Text;

namespace JavaRebyte.Core.ClassFile
{
	/// <summary>
	/// The tag (byte) which specifies the type of constnat being stored in this ConstantPool entry. <br/>
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4-210"/>
	/// </summary>
	public enum ConstantPoolTag : byte
	{
		NONE	= 0, // As definition for the 'default' value of ConstantPoolTag. Not used by JDK/JVM
		UTF8	= 1,
		INTEGER = 3,
		FLOAT	= 4,
		LONG	= 5,
		DOUBLE	= 6,
		CLASS	= 7,
		STRING	= 8,
		
		FIELD_REF	= 9,
		METHOD_REF	= 10,
		INTERFACE_METHOD_REF = 11,
		
		NAME_AND_TYPE	= 12,
		METHOD_HANLDE	= 15,
		METHOD_TYPE		= 16,
		DYNAMIC			= 17,
		INVOKE_DYNAMIC	= 18,

		MODULE	= 19,
		PACKAGE = 20,

	}
}
