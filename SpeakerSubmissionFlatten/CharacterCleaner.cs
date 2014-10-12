using System;
using System.Text;
using System.Text.RegularExpressions;

namespace IowaCodeCamp.Utility.SpeakerSubmissionFlatten
{
	public class CharacterCleaner
	{
		const char LF = '\x0a';
		const char CR = '\x0d';
		const char DOUBLEQUOTE = '"';

		public string CleanMsWordCharacters(string value)
		{
			StringBuilder buffer = new StringBuilder(value);

			buffer = buffer.Replace('\u2013', '-');
			buffer = buffer.Replace('\u2014', '-');
			buffer = buffer.Replace('\u2015', '-');
			buffer = buffer.Replace('\u2017', '_');
			buffer = buffer.Replace('\u2018', '\'');
			buffer = buffer.Replace('\u2019', '\'');
			buffer = buffer.Replace('\u201a', ',');
			buffer = buffer.Replace('\u201b', '\'');
			buffer = buffer.Replace('\u201c', '\"');
			buffer = buffer.Replace('\u201d', '\"');
			buffer = buffer.Replace('\u201e', '\"');
			buffer = buffer.Replace("\u2026", "...");
			buffer = buffer.Replace('\u2032', '\'');
			buffer = buffer.Replace('\u2033', '\"');

			return buffer.ToString();	
		}


		public string EscapeQuotes(string value)
		{
			bool quotesTrimmed = false;

			if (IsQuoted(value))
			{
				value = value.Substring(1, value.Length - 2);
				quotesTrimmed = true;
			}

			StringBuilder buffer = new StringBuilder();

			foreach (char c in value)
			{
				if (c == DOUBLEQUOTE)
				{
					buffer.Append(DOUBLEQUOTE);
				}

				buffer.Append(c);
			}

			string result = buffer.ToString();

			if (quotesTrimmed)
				return '\"' + result + '"';
				
			return result;
		}

		public string UnixToDosEol(string value)
		{
			StringBuilder buffer = new StringBuilder();
			char previousCharacter = '\x00';
			foreach (char c in value)
			{
				if (c == LF)
				{
					if (previousCharacter != CR)
					{
						buffer.Append(CR);
					}
				}

				buffer.Append(c);
				previousCharacter = c;

			}

			return buffer.ToString();
		}

		public bool IsMultiLine(string value)
		{
			return (value.IndexOf(Environment.NewLine) != -1);
		}

		public bool IsQuoted(string value)
		{
			if ((value.StartsWith("\"")) &&
				(value.EndsWith("\"")))
			{
				return true;
			}

			return false;
		}
	}
}