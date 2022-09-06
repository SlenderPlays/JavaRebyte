using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace JavaRebyte.Core.Jar
{
	public class JarEntry
	{
		/// <summary>
		/// The path of this file inside the jar. Does not start with any type of slash.
		/// Example: The path for the MANIFEST.MF file, found insiide teh META-INF folder is <c>META-INF/MANIFEST.MF</c>
		/// <seealso cref="System.IO.Compression.ZipArchiveEntry.FullName"/>
		/// </summary>
		public string jarPath { get; set; }
		public byte[] byteContents { get; set; }

		private ZipArchiveEntry m_archiveEntry = null;

		public JarEntry(string path)
		{
			this.jarPath = path;
		}
		public JarEntry(string path, byte[] inputBytes)
		{
			this.jarPath = path;
			this.byteContents = inputBytes;
		}
		public JarEntry(ZipArchiveEntry archiveEntry)
		{
			m_archiveEntry = archiveEntry;
			jarPath = archiveEntry.FullName;
		}

		/// <summary>
		/// Reads the contents of the filestream from the provided stream.
		/// </summary>
		/// <param name="inputStream"></param>
		public async Task ReadStreamAsync(Stream inputStream)
		{
			using (MemoryStream temp = new MemoryStream())
			{
				await inputStream.CopyToAsync(temp);
				this.byteContents = temp.ToArray();
			}
		}

		/// <summary>
		/// Reads the contents from the jar into the internal buffer (<see cref="byteContents"/>), so that it this entry can be modified.
		/// This only works if the JarEntry has been created from a jar existing on disk or in memory.
		/// If you want to read the contents from another source, use <see cref="ReadStreamAsync(Stream)"/>. Otherwise the method will throw <see cref="InvalidOperationException"/>.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task ReadJarAsync()
		{
			if (m_archiveEntry == null)			
				throw new InvalidOperationException("JarEntry has not been created from an existing jar.");

			using(var stream = m_archiveEntry.Open())
			{
				await ReadStreamAsync(stream);
			}
		}
	}
}
