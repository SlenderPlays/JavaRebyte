using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace JavaRebyte.Core.Jar
{
	public class JavaClassFile : JarEntry
	{
		public JavaClassFile(string path) : base(path)
		{
		}

		public JavaClassFile(ZipArchiveEntry archiveEntry) : base(archiveEntry)
		{
		}

		public JavaClassFile(string path, byte[] inputBytes) : base(path, inputBytes)
		{
		}
	}
}
