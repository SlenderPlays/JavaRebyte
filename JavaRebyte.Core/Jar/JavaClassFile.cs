using JavaRebyte.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace JavaRebyte.Core.Jar
{
	public class JavaClassFile : JarEntry
	{
		public const string FILE_SUFFIX = ".class";

		public DecompiledClassFile DecompiledClass { get; private set; } = null;
		public bool IsDecompiled => DecompiledClass != null;
		public string fullClassName { get; private set; }

		public JavaClassFile(string path) : base(path)
		{
			fullClassName = path.Substring(0,path.Length - FILE_SUFFIX.Length);
		}

		public JavaClassFile(ZipArchiveEntry archiveEntry) : base(archiveEntry)
		{
			fullClassName = this.jarPath.Substring(0, this.jarPath.Length - FILE_SUFFIX.Length);
		}

		public JavaClassFile(string path, byte[] inputBytes) : base(path, inputBytes)
		{
			fullClassName = path.Substring(0, path.Length - FILE_SUFFIX.Length);
		}

		/// <summary>
		/// This function will attempt to decompile the class file so that it may be modified. The result is an object
		/// with the class <see cref="DecompiledClassFile"/>. The result will be stored in <see cref="DecompiledClass"/>, and also returned by this method.
		/// <br/>
		/// <br/>
		/// To decompile the class, first this JarEntry must be read. If the entry has not been read but has been produced from a <see cref="ZipArchiveEntry"/> then
		/// the <see cref="JarEntry.ReadJarAsync"/> method will be executed synchronously. Otherwise, the method will throw <see cref="InvalidOperationException"/>.
		/// <br/>
		/// <br/>
		/// Warning! If this method is called a second time, the first result will be overwriten and all changes will be lost. This effecively resets the decompiled
		/// class.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public DecompiledClassFile DecompileClass()
		{
			if (!this.IsRead)
			{
				try
				{
					this.ReadJarAsync().Wait();
				}
				catch (InvalidOperationException e)
				{
					throw new InvalidOperationException("Could not decompile the class because the file hasn't been read yet. An attempt to read from the jar was made, but failed.", e);
				}
			}

			DecompiledClassFile temp = new DecompiledClassFile(this.byteContents);
			this.DecompiledClass = temp;

			return this.DecompiledClass;
		}
	}
}
