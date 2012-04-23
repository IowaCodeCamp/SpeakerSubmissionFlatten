#region Namespaces

using IowaCodeCamp.Utility.SpeakerSubmissionFlatten;

using NUnit.Framework;

#endregion Namespaces

namespace SpeakerSubmissionFlatten.UnitTest
{
	[TestFixture]
	public class StringCleaningTests
	{
		[Test]
		public void CleanMsWordCharacters_WordQuotes_ConvertSuccess()
		{
			CharacterCleaner cleaner = new CharacterCleaner();

			const string value = "Now is the time “for” all good men to come to the “aid” of their party.";

			string result = cleaner.CleanMsWordCharacters(value);

			Assert.AreEqual(value.Length, result.Length);

			Assert.AreEqual(result[16], '"');
			Assert.AreEqual(result[20], '"');
			Assert.AreEqual(result[50], '"');
			Assert.AreEqual(result[54], '"');

			for (int index = 0; index < value.Length; index++)
			{
				if ((index == 16) ||
					(index == 20) ||
					(index == 50) ||
					(index == 54))
					continue;
				Assert.AreEqual(value[index], result[index]);
			}
		}

		[Test]
		public void UnixToDosEol_Convert_Success()
		{
			const string value = "abc" + "\x0a" + "def" + "\x0a";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.UnixToDosEol(value);

			Assert.AreEqual(value.Length + 2, result.Length);

			for (int index = 0; index < 3; index++)
			{
				Assert.AreEqual(value[index], result[index]);
			}

			for (int index = 3; index < 7; index++)
			{
				Assert.AreEqual(value[index], result[index + 1]);
			}

			Assert.AreEqual('\x0c', result[3]);
			Assert.AreEqual('\x0a', result[4]);
			Assert.AreEqual('\x0c', result[8]);
			Assert.AreEqual('\x0a', result[9]);
		}

		[Test]
		public void UnixToDosEol_ConcurrentCharacters_Success()
		{
			const string value = "abc" + "\x0a\x0a\x0a" + "def";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.UnixToDosEol(value);

			Assert.AreEqual(value.Length + 3, result.Length);

			for (int index = 0; index < 3; index++)
			{
				Assert.AreEqual(value[index], result[index]);
			}

			for (int index = 6; index < 9; index++)
			{
				Assert.AreEqual(value[index], result[index + 3]);
			}

			Assert.AreEqual('\x0c', result[3]);
			Assert.AreEqual('\x0a', result[4]);
			Assert.AreEqual('\x0c', result[5]);
			Assert.AreEqual('\x0a', result[6]);
			Assert.AreEqual('\x0c', result[7]);
			Assert.AreEqual('\x0a', result[8]);
		}

		[Test]
		public void UnixToDosEol_NoConvert_Success()
		{
			const string value = "abc" + "\x0c\x0a" + "def" + "\x0c\x0a";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.UnixToDosEol(value);

			Assert.AreEqual(value.Length, result.Length);

			for (int index = 0; index < value.Length; index++)
			{
				Assert.AreEqual(value[index], result[index]);
			}
		}

		[Test]
		public void EscapeQuotes_NoQuotes_OriginalString()
		{
			const string value = "abc";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreEqual(value, result);
		}

		[Test]
		public void EscapeQuotes_OneAtBeginning_OriginalString()
		{
			const string value = "\"abc";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreNotEqual(value, result);

			Assert.AreEqual(value.Length + 1, result.Length);

			Assert.AreEqual(result[0], '"');
			Assert.AreEqual(result[1], '"');

			for (int index = 1; index < value.Length; index++)
			{
				Assert.AreEqual(value[index], result[index + 1]);
			}
		}

		[Test]
		public void EscapeQuotes_OneAtEnd_OriginalString()
		{
			const string value = "abc\"";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreNotEqual(value, result);

			Assert.AreEqual(value.Length + 1, result.Length);

			Assert.AreEqual(result[result.Length - 1], '"');
			Assert.AreEqual(result[result.Length - 2], '"');

			for (int index = 0; index < value.Length - 1; index++)
			{
				Assert.AreEqual(value[index], result[index]);
			}
		}

		[Test]
		public void EscapeQuotes_EmbeddedQuotes_OriginalString()
		{
			const string value = "abc\"def\"ghi";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreNotEqual(value, result);

			Assert.AreEqual(value.Length + 2, result.Length);

			const string expected = "abc\"\"def\"\"ghi";

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void EscapeQuotes_QuotedNoEmbeddedQuotes_OriginalString()
		{
			const string value = "\"abc\"";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreEqual(value, result);
		}

		[Test]
		public void EscapeQuotes_QuotedOneAtBeginning_OriginalString()
		{
			const string value = "\"\"abc\"";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreNotEqual(value, result);

			Assert.AreEqual(value.Length + 1, result.Length);

			Assert.AreEqual(result[0], '"');
			Assert.AreEqual(result[1], '"');
			Assert.AreEqual(result[2], '"');

			for (int index = 2; index < value.Length; index++)
			{
				Assert.AreEqual(value[index], result[index + 1]);
			}
		}

		[Test]
		public void EscapeQuotes_QuotedOneAtEnd_OriginalString()
		{
			const string value = "\"abc\"\"";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreNotEqual(value, result);

			Assert.AreEqual(value.Length + 1, result.Length);

			Assert.AreEqual(result[result.Length - 1], '"');
			Assert.AreEqual(result[result.Length - 2], '"');
			Assert.AreEqual(result[result.Length - 3], '"');

			for (int index = 0; index < value.Length - 2; index++)
			{
				Assert.AreEqual(value[index], result[index]);
			}
		}

		[Test]
		public void EscapeQuotes_QuotedEmbeddedQuotes_OriginalString()
		{
			const string value = "\"abc\"def\"ghi\"";

			CharacterCleaner cleaner = new CharacterCleaner();

			string result = cleaner.EscapeQuotes(value);

			Assert.AreNotEqual(value, result);

			Assert.AreEqual(value.Length + 2, result.Length);

			const string expected = "\"abc\"\"def\"\"ghi\"";

			Assert.AreEqual(expected, result);
		}


	}
}
