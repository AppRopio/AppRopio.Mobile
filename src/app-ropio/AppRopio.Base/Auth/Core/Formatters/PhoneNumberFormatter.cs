using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AppRopio.Base.Auth.Core.Formatters
{
	public static class PhoneNumberFormatter
	{
		private const int _maxLength = 10;

		public static string Format(string cleanStr)
		{
			if (cleanStr.Length == 0)
			{
				return string.Empty;
			}

			if (cleanStr.Length == 11 && cleanStr[0] == '7')
			{
				cleanStr = cleanStr.Remove(0, 1);
			}

			if (cleanStr.Length >= _maxLength)
			{

				cleanStr.Substring(0, _maxLength);
			}

			var fullString = cleanStr.PadRight(_maxLength, ' ');

			var list = new List<string>();

			string code = string.Format("({0}", fullString.Substring(0, 3).Trim(' '));
			if (cleanStr.Length > 3)
			{
				code += ")";
			}
			list.Add(code);

			var firstPart = fullString.Substring(3, 3).Trim(' ');
			if (firstPart.Length > 0)
			{
				list.Add(firstPart);
			}
			var secondPart = fullString.Substring(6, 2).Trim(' ');
			if (secondPart.Length > 0)
			{
				list.Add(secondPart);
			}
			var thirdPart = fullString.Substring(8, 2).Trim(' ');
			if (thirdPart.Length > 0)
			{
				list.Add(thirdPart);
			}

			string result = "+ 7";

			foreach (var item in list)
			{
				result = result + string.Format(" {0}", item);
			}

			return result;
		}

		private static Regex digitsOnly = new Regex(@"[^\d]");


		public static string FormatWithClean(string text)
		{
			if (text.IsNullOrEmtpy())
			{
				return string.Empty;
			}
			string clean = GetCleanStr(text);
			clean = digitsOnly.Replace(clean, "");

			return Format(clean);
		}

		public static string GetCleanStr(string input)
		{
			string result = input;

			result = result.Replace(" ", string.Empty);
			result = result.Replace("+7", string.Empty);
			result = result.Replace("(", string.Empty);
			result = result.Replace(")", string.Empty);
			result = result.Replace("-", string.Empty);

			return result;
		}


		public static string GetCleanStrWithoutFormat(string input, bool showPlus = true)
		{
			input = digitsOnly.Replace(input, "");
			return showPlus ? string.Format("+{0}", input) : input;
		}


		public static bool IsValid(string input)
		{
			if (input.IsNullOrEmtpy())
				return false;
			string clearNumber = GetCleanStr(input);

			if (clearNumber.Length != _maxLength)
			{
				return false;
			}

			long value;
			return string.IsNullOrEmpty(clearNumber) || long.TryParse(clearNumber, out value);
		}
	}
}
