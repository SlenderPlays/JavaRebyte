using JavaRebyte.Core.Jar;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Xunit;
using Xunit.Abstractions;

namespace JavaRebyte.Tests
{
	/// <summary>
	/// This test suite is reserved for debugging tests, or in general for testing while in development.
	/// There are no concrete design patterns being applied in these tests, just ephemeral code meant to be ran to debug the program.
	/// It is best practice to redesign your temporary tests into proper tests afterwards.
	/// </summary>
	public class DynamicTests
	{
		private readonly ITestOutputHelper output;

		public DynamicTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public void TestEntry()
		{
			JarFile jarFile = new JarFile("./HelloWorldJava8.jar");

			using (var memoryStream = new MemoryStream())
			{
				using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
				{
					List<JarEntry> entries = new List<JarEntry>();
					entries.AddRange(jarFile.javaClassFiles);
					entries.AddRange(jarFile.auxiliaryFiles);

					foreach (var item in entries)
					{
						var demoFile = archive.CreateEntry(item.jarPath);

						if(item.byteContents == null)
							item.ReadJarAsync().Wait();

						using (var entryStream = demoFile.Open())
						{
							entryStream.Write(item.byteContents, 0, item.byteContents.Length);
						}
					}
				}

				using (var fileStream = new FileStream(Path.GetTempPath() + "test.jar", FileMode.Create))
				{
					memoryStream.Seek(0, SeekOrigin.Begin);
					memoryStream.CopyTo(fileStream);
				}
			}
		}
	}
}
