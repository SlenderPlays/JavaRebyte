using System;
using System.Collections.Generic;
using System.Text;

namespace JavaRebyte.Core.ClassFile
{
	/// <summary>
	/// Access Flags for a class. <br/>
	/// Reference: <see href="https://docs.oracle.com/javase/specs/jvms/se19/html/jvms-4.html#jvms-4.1-200-E.1"/>
	/// </summary>
	[Flags]
	public enum ClassAccessFlags :ushort
	{
		NONE = 0,
		/// <summary>
		/// Declared <c>public;</c> may be accessed from outside its package.
		/// </summary>
		PUBLIC = 0x0001,
		/// <summary>
		/// Declared <c>final;</c> no subclasses allowed.
		/// </summary>
		FINAL = 0x0010,
		/// <summary>
		/// Treat superclass methods specially when invoked by the <c>invokespecial</c> instruction.
		/// </summary>
		SUPER = 0x0020,
		/// <summary>
		/// Is an interface, not a class.
		/// </summary>
		INTERFACE = 0x0200,
		/// <summary>
		/// Declared <c>abstract;</c> must not be instantiated.
		/// </summary>
		ABSTRACT = 0x0400,
		/// <summary>
		/// Declared synthetic; not present in the source code.
		/// </summary>
		SYNTHETHIC = 0x1000,
		/// <summary>
		/// Declared as an annotation interface.
		/// </summary>
		ANNOTATION = 0x2000,
		/// <summary>
		/// Declared as an <c>enum</c> class.
		/// </summary>
		ENUM = 0x4000,
		/// <summary>
		/// Is a module, not a class or interface.
		/// </summary>
		MODULE = 0x8000
	}
}
