using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace JavaRebyte.Tests
{
	public static class HashUtils
	{
		public static string SHA256CheckSum(string filename)
		{
			string hashedValue = string.Empty;

			using (SHA256 SHA256 = SHA256Managed.Create())
			{
				byte[] hashedData = SHA256.ComputeHash(File.ReadAllBytes(filename));
				hashedValue = BitConverter.ToString(hashedData).Replace("-", "").ToLowerInvariant();
			}

			return hashedValue;
		}

		public static string SHA256CheckSum(Stream stream)
		{
			string hashedValue = string.Empty;

			using (SHA256 SHA256 = SHA256Managed.Create())
			{
				byte[] hashedData = SHA256.ComputeHash(stream);
				hashedValue = BitConverter.ToString(hashedData).Replace("-", "").ToLowerInvariant();
			}

			return hashedValue;
		}

		public static string SHA1CheckSum(string filename)
		{
			string hashedValue = string.Empty;

			using (SHA1 SHA1 = SHA1Managed.Create())
			{
				byte[] hashedData = SHA1.ComputeHash(File.ReadAllBytes(filename));
				hashedValue = BitConverter.ToString(hashedData).Replace("-", "").ToLowerInvariant();
			}

			return hashedValue;
		}

		public static string SHA1CheckSum(Stream stream)
		{
			string hashedValue = string.Empty;

			using (SHA1 SHA1 = SHA1Managed.Create())
			{
				byte[] hashedData = SHA1.ComputeHash(stream);
				hashedValue = BitConverter.ToString(hashedData).Replace("-", "").ToLowerInvariant();
			}

			return hashedValue;
		}
	}
}
