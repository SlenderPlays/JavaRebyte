using System;
using System.Collections.Generic;
using System.Text;

namespace JavaRebyte.Core.ClassFile.Util
{
	/// <summary>
	/// The Java-defined UTF8 format. <br/>
	/// This class contains functions that will ease the conversion between UTF16 (used by C#) and modified UTF8 (used by Java).<br/>
	/// Reference for normal UTF8: <see href="https://en.wikipedia.org/wiki/UTF-8#Encoding"/> <br/>
	/// Reference for modified UTF8: <see href="https://docs.oracle.com/javase/specs/jvms/se18/html/jvms-4.html#jvms-4.4.7"/> <br/>
	/// Differences: <see href="https://py2jdbc.readthedocs.io/en/latest/mutf8.html"/> <br/>
	/// Reference for UTF16: <see href="https://en.wikipedia.org/wiki/UTF-16"/> <br/>
	/// Source to UTF16 being used by C#: <see href="https://stackoverflow.com/questions/14942092/why-does-net-use-the-utf16-encoding-for-string-but-uses-utf-8-as-default-for-s"/>
	/// </summary>
	public static class ModifiedUTF8
	{
		private const int MUTF8_ONE_BYTE_MASK = 0b1000_0000;
		private const int MUTF8_ONE_BYTE_TEST = 0b0000_0000;

		private const int MUTF8_TWO_BYTE_MASK = 0b1110_0000;
		private const int MUTF8_TWO_BYTE_TEST = 0b1100_0000;

		private const int MUTF8_THREE_BYTE_MASK = 0b1111_0000;
		private const int MUTF8_THREE_BYTE_TEST = 0b1110_0000;

		private const int MUTF8_EXTENSION_BYTE_MASK = 0b1100_0000;
		private const int MUTF8_EXTENSION_BYTE_TEST = 0b1000_0000;

		/// <summary>
		/// Converts an MUTF8 byte array to an UTF16 string
		/// </summary>
		/// <param name="bytes">MUTF8 byte array</param>
		/// <returns>UTF16 string</returns>
		public static string GetString(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			while (i < bytes.Length)
			{
				// 0xxx_xxxx
				// 1-byte long character. No modifications here.
				// If we get a null-byte (0x00) then this might not be accurated if re-encoded back to MUTF8.
				// TODO: Should 0x00 act as a string terminator?
				if ((bytes[i] & MUTF8_ONE_BYTE_MASK) == MUTF8_ONE_BYTE_TEST)
				{
					sb.Append((char)bytes[i]);
					i++;
				}
				// 110x_xxxx 10xx_xxxx
				// 2-byte long character.
				// The only difference here from UTF8 is that null is encoded as a 2-byte, but because data-bits are filled with 0s, in conversion
				// we will automatically get a null byte. Technically we could get a null byte from the three-byte and 6-byte variation too, but
				// such behaviour is not specified by Java Docs. To avoid conflict we will treat all of these variations as null bytes too. This
				// approach can cause issues if we decode and re-encode as they will be cast back to the 2-byte form.
				else if ((bytes[i] & MUTF8_TWO_BYTE_MASK) == MUTF8_TWO_BYTE_TEST &&
						(bytes[i + 1] & MUTF8_EXTENSION_BYTE_MASK) == MUTF8_EXTENSION_BYTE_TEST)
				{
					sb.Append((char)(
						((bytes[i] & 0b0001_1111) << 6) + (bytes[i + 1] & 0b0011_1111)
					));
					i += 2;
				}
				// 1110_xxxx 10xx_xxxx 10xx_xxxx
				// 3-byte long character. No difference here from UTF8.
				else if ((bytes[i] & MUTF8_THREE_BYTE_MASK) == MUTF8_THREE_BYTE_TEST &&
						(bytes[i + 1] & MUTF8_EXTENSION_BYTE_MASK) == MUTF8_EXTENSION_BYTE_TEST &&
						(bytes[i + 2] & MUTF8_EXTENSION_BYTE_MASK) == MUTF8_EXTENSION_BYTE_TEST)
				{
					sb.Append((char)(
						((bytes[i] & 0b0000_1111) << 12) + ((bytes[i + 1] & 0b0011_1111) << 6) + (bytes[i + 2] & 0b0011_1111)
					));
					i += 3;
				}
				// 4-byte version does not exist. Instead we have a 6-byte version.
				// Java claims this is a "UTF-16 surrogate" but IDK what they are on about because UTF-16 encodes
				// either 2 bytes or 4 bytes, and not 6 bytes.
				// 11101101 1010xxxx 10xxxxxx
				// 11101101 1011xxxx 10xxxxxx
				else if (bytes[i] == 0b11101101 &&
						(bytes[i + 1] & 0b1111_0000) == 0b1010_0000 &&
						(bytes[i + 2] & 0b1100_0000) == 0b1000_0000 &&
						bytes[i + 3] == 0b11101101 &&
						(bytes[i + 4] & 0b1111_0000) == 0b1011_0000 &&
						(bytes[i + 5] & 0b1100_0000) == 0b1000_0000)
				{
					// Char holds only, at most, 16 bits of information. The encoding the char (seems) to use is UTF-16.
					// As such, to store 20 bits of information it uses 2 chars, each storing 10 bits each like this:
					// https://en.wikipedia.org/wiki/UTF-16#Code_points_from_U.2B010000_to_U.2B10FFFF
					// U' = yyyyyyyyyyxxxxxxxxxx  // U - 0x10000
					// W1 = 110110yyyyyyyyyy      // 0xD800 + yyyyyyyyyy
					// W2 = 110111xxxxxxxxxx      // 0xDC00 + xxxxxxxxxx

					/*
					sb.Append((char)(
						0b1_0000_0000_0000_0000 +
						((bytes[i + 1] & 0b0000_1111) << 16) +
						((bytes[i + 2] & 0b0011_1111) << 10) +
						((bytes[i + 4] & 0b0000_1111) << 6) +
						((bytes[i + 5] & 0b0011_1111))
					));*/
					// this implementation does not support values between U+D800 to U+DFFF
					// but these are reserved by the Unicode Standard and no such character will ever be alocated here... I hope.
					// I can probably use Encoding.UTF16 to support everything, but I'm pretty sure Java's MUTF8 does not support everything either.
					sb.Append((char)(
						0xD800 +
						((bytes[i + 1] & 0b0000_1111) << 6) +
						(bytes[i + 2] & 0b0011_1111)
					));
					sb.Append((char)(
						0xDC00 +
						((bytes[i + 4] & 0b0000_1111) << 6) +
						(bytes[i + 5] & 0b0011_1111)
					));
					i += 6;
				}
				else
				{
					throw new Exception("Malformed MUTF8 string.");
				}
			}

			return sb.ToString();
		}

		private const uint UTF16_SURROGATE_MASK = 0b1111_1100_0000_0000;
		private const uint UTF16_HIGH_TEST = 0b1101_1000_0000_0000;
		private const uint UTF16_LOW_TEST = 0b1101_1100_0000_0000;

		/// <summary>
		/// Converts an UTF16 string (default C# encoding) to MUTF8.
		/// </summary>
		/// <param name="str">UTF16 string</param>
		/// <returns>MUTF8</returns>
		public static byte[] GetBytes(string str)
		{
			List<byte> bytes = new List<byte>();
			// The non-surrogate code points could be translated using the Encoding namespace.
			int i = 0;
			while (i < str.Length)
			{
				// UTF16
				// U' = yyyyyyyyyyxxxxxxxxxx  // U - 0x10000
				// W1 = 110110yyyyyyyyyy      // 0xD800 + yyyyyyyyyy
				// W2 = 110111xxxxxxxxxx      // 0xDC00 + xxxxxxxxxx
				//
				// MUTF8
				// 11101101 1010xxxx 10xxxxxx
				// 11101101 1011xxxx 10xxxxxx
				if ((str[i] & UTF16_SURROGATE_MASK) == UTF16_HIGH_TEST &&
					(str[i + 1] & UTF16_SURROGATE_MASK) == UTF16_LOW_TEST)
				{
					bytes.Add(0b11101101);
					bytes.Add((byte)(
						0b1010_0000 | ((str[i] & 0b000000_1111_000000) >> 6)
					));
					bytes.Add((byte)(
						0b10_000000 | (str[i] & 0b000000_0000_111111)
					));

					bytes.Add(0b11101101);
					bytes.Add((byte)(
						0b1011_0000 | ((str[i + 1] & 0b000000_1111_000000) >> 6)
					));
					bytes.Add((byte)(
						0b10_000000 | (str[i + 1] & 0b000000_0000_111111)
					));

					i += 2;
				}
				// Convert to basic UTF8
				else
				{
					// 0xxx_xxxx
					if (str[i] <= (uint)0x007F)
					{
						bytes.Add((byte)str[i]);
						i++;
					}
					// 110xxxxx	10xxxxxx
					else if (str[i] <= (uint)0x07FF)
					{
						bytes.Add((byte)(
							0b110_00000 | ((str[i] & 0b11111_000000) >> 6)
						));
						bytes.Add((byte)(
							0b10_000000 | (str[i] & 0b00000_111111)
						));
						i++;
					}
					// 1110xxxx	10xxxxxx 10xxxxxx	
					else if (str[i] <= (uint)0xFFFF)
					{
						bytes.Add((byte)(
							0b1110_0000 | ((str[i] & 0b1111_000000_000000) >> 12)
						));
						bytes.Add((byte)(
							0b10_000000 | ((str[i] & 0b0000_111111_000000) >> 6)
						));
						bytes.Add((byte)(
							0b10_000000 | (str[i] & 0b0000_000000_111111)
						));
						i++;
					}
					else
					{
						throw new Exception("Somehow, you managed to get an invalid UTF16 string. Probably because of a mismatched surrogate pair.");
					}
				}
			}

			return bytes.ToArray();
		}
	}
}
