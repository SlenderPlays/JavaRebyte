using System;
using System.Collections.Generic;
using System.Text;

namespace JavaRebyte.Core.Exceptions
{
	[Serializable]
	public class InvalidReferenceException : Exception
	{
		public InvalidReferenceException():base("The reference into the constant pool is not valid."){ }
		public InvalidReferenceException(string message) : base(message) { }
		public InvalidReferenceException(string message, Exception inner) : base(message, inner) { }
		protected InvalidReferenceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
