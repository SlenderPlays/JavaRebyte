using System;
using System.Runtime.Serialization;

namespace JavaRebyte.Core.Exceptions
{
	[Serializable]
	internal class DecompilationException : Exception
	{
		public DecompilationException() : base("An error has occured while trying to decompile a file.")
		{
			
		}

		public DecompilationException(string message) : base(message)
		{
		}

		public DecompilationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected DecompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}