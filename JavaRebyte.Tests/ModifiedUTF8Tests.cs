using JavaRebyte.Core.ClassFile.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JavaRebyte.Tests
{
	public class ModifiedUTF8Tests
	{
		[Theory]
		[InlineData("a")]
		[InlineData("abcd123")]
		[InlineData("$")]
		[InlineData("£")]
		[InlineData("€")]
		[InlineData("𐍈")]
		[InlineData("𐍈€$£𐍈€$£𐍈")]
		public void TestReEncoding(string original)
		{
			var encoded = ModifiedUTF8.GetBytes(original);
			var reEncoded = ModifiedUTF8.GetString(encoded);
			Assert.Equal(original, reEncoded);
		}

		[Fact]
		public void StressReEncoding()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 10000_000; i++)
			{
				sb.Append("a𐍈");
			}
			TestReEncoding(sb.ToString());
		}
	}
}
