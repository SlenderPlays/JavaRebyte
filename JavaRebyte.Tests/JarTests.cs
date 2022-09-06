using JavaRebyte.Core.Jar;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace JavaRebyte.Tests
{
	public class JarTests
	{
		public const string TEST_JAR_FILENAME = "./RebyteHello.jar";

		private readonly ITestOutputHelper output;

		public JarTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public void TestReadAndWrite()
		{
			// Read
			JarFile jar = new JarFile(TEST_JAR_FILENAME);

			// Read the data to test that `WriteToFile()` can write without re-reading file.
			jar.javaClassFiles[0].ReadJarAsync().Wait();

			string outFile = Path.GetTempFileName();
			output.WriteLine(outFile);
			jar.WriteToFile(outFile);

			// Can't actually just compare checksums because re-zipping yields different compressed size compared to java's `jar.exe`
			// This makes the jars be interpreted the same by java, so this difference is not a problem in regards to functionality.
			//Assert.Equal(HashUtils.SHA1CheckSum(TEST_JAR_FILENAME), HashUtils.SHA1CheckSum(outFile));
			//Assert.Equal(HashUtils.SHA256CheckSum(TEST_JAR_FILENAME), HashUtils.SHA256CheckSum(outFile));

			var original = ZipFile.OpenRead(TEST_JAR_FILENAME);
			var created = ZipFile.OpenRead(outFile);

			// The original has more entries, counting each folder as an entry.
			// This is not an issue for the java runtime interpreter.
			// Assert.Equal(original.Entries.Count, created.Entries.Count);

			var originalEntries = original.Entries
				.Where(e => !(e.FullName.EndsWith('/') || e.FullName.EndsWith('\\')))
				.OrderBy(e => e.FullName)
				.ToList();

			var createdEntries = created.Entries
				//.Where(e => !(e.FullName.EndsWith('/') || e.FullName.EndsWith('\\')))
				.OrderBy(e => e.FullName)
				.ToList();

			Assert.Equal(originalEntries.Count, createdEntries.Count);

			for (int i = 0; i < originalEntries.Count; i++)
			{
				var originalEntry = originalEntries[i];
				var createdEntry = createdEntries[i];

				Assert.Equal(originalEntry.FullName, createdEntry.FullName);
				Assert.Equal(originalEntry.Crc32, createdEntry.Crc32);

				using (var originalStream = originalEntry.Open())
				using (var createdStream = createdEntry.Open())
					Assert.Equal(HashUtils.SHA1CheckSum(originalStream), HashUtils.SHA1CheckSum(createdStream));

				// We must re-open the stream. Cannot seek back to begining.

				using (var originalStream = originalEntry.Open())
				using (var createdStream = createdEntry.Open())
					Assert.Equal(HashUtils.SHA256CheckSum(originalStream), HashUtils.SHA256CheckSum(createdStream));

			}

			jar.Dispose();
		}
	}
}
