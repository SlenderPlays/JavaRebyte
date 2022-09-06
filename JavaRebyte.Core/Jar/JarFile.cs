using JavaRebyte.Core.ClassFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaRebyte.Core.Jar
{
	public class JarFile: IDisposable
	{
		public string jarFilePath { get; private set; }

		public List<JavaClassFile> javaClassFiles = new List<JavaClassFile>();
		public List<JarEntry> auxiliaryFiles = new List<JarEntry>();
		
		private ZipArchive m_archiveFile;

		public JarFile(string jarFilePath)
		{
			if(File.Exists(jarFilePath) && jarFilePath.EndsWith(".jar"))
			{
				this.jarFilePath = jarFilePath;
				m_archiveFile = ZipFile.OpenRead(jarFilePath);
				
				var fileEntries = m_archiveFile.Entries
					.Where(e => !(e.FullName.EndsWith('/') || e.FullName.EndsWith('\\')))
					.OrderBy(e => e.FullName)
					.ToList();

				foreach (ZipArchiveEntry entry in fileEntries)
				{
					if (entry.Name.EndsWith(".class"))
					{
						javaClassFiles.Add(new JavaClassFile(entry));
					}
					else
					{
						auxiliaryFiles.Add(new JarEntry(entry));
					}
				}
			}
			else
			{
				throw new IOException($"File does not exist, or does not end with the '.jar' extension. Provided path: [{jarFilePath}]");
			}
		}

		public MemoryStream WriteToMemory()
		{
			MemoryStream memoryStream = new MemoryStream();

			using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
			{
				// TODO: Handle classes differently, since they can be edited easily.
				List<JarEntry> entries = new List<JarEntry>();
				entries.AddRange(this.javaClassFiles);
				entries.AddRange(this.auxiliaryFiles);

				foreach (var item in entries)
				{
					var entryFile = archive.CreateEntry(item.jarPath);

					if (item.byteContents == null)
						item.ReadJarAsync().Wait();

					using (var entryStream = entryFile.Open())
					{
						entryStream.Write(item.byteContents, 0, item.byteContents.Length);
					}
				}
			}

			memoryStream.Seek(0, SeekOrigin.Begin);
			return memoryStream;
		}

		public void WriteToFile(string path)
		{
			using (var memoryStream = WriteToMemory())
			{
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					memoryStream.Seek(0, SeekOrigin.Begin);
					memoryStream.CopyTo(fileStream);
				}
			}
		}

		public void WriteToStream(Stream outputStream)
		{
			using (var memoryStream = WriteToMemory())
			{
				memoryStream.Seek(0, SeekOrigin.Begin);
				memoryStream.CopyTo(outputStream);
			}
		}

		public void Dispose()
		{
			((IDisposable)m_archiveFile).Dispose();
		}
	}
}
