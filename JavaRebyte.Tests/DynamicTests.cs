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
			JarFile jarFile = new JarFile("./RebyteHello.jar");
			jarFile.GetClassFromName("rebyte/helloworld/Constants").DecompileClass();
		}
	}
}
