using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppRopio.Payments.Core.Utils
{
    public static class CardsHelper
	{
		public static string FormatCardNumber(string cardNumber)
		{
			return cardNumber.Without(' ').SeparatedBy(' ', 4);
		}

		public static string FormatExpirationDate(string expirationDate)
		{
			return expirationDate.Without('/').SeparatedBy('/', 2);
		}

		/// <summary>
		/// 1. Цифры проверяемой последовательности нумеруются справа налево.
		/// 2. Цифры, оказавшиеся на нечётных местах, остаются без изменений.
		/// 3. Цифры, стоящие на чётных местах, умножаются на 2.
		/// 4. Если в результате такого умножения возникает число больше 9, оно заменяется суммой цифр получившегося произведения — однозначным числом, то есть цифрой.
		/// 5. Все полученные в результате преобразования цифры складываются. Если сумма кратна 10, то исходные данные верны.
		/// </summary>
		public static bool IsValidNumber(string creditCardNumber)
		{
			if (string.IsNullOrEmpty(creditCardNumber) || creditCardNumber.Length < 14 || creditCardNumber.Length > 19)
				return false;

			// вернуть только последовательность символов, которые обозначают цифры
			int sumOfDigits = creditCardNumber.Where(e => (e >= '0') && (e <= '9'))
				// обратить последовательность (описание - пункт 1)
				.Reverse()
				// вернуть последовательность, где цифры на четных местах умножены на 2 
				// (переводим символ в цифру вычитанием 48 и учитываем, что индексатор i начинается с ноля. 
				//  Это значит, что позиции  0, 2, 4... - нечетные для индксатора начинающегося с 1 (1,2,3...), который подразумевается в алгоритме
				.Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
				// учитываем пункт 4 описания ( e/10 - деление нацело - первая цифра,  e%10 - остаток от деления - вторая цифра)
				.Sum(e => e / 10 + e % 10);

			return sumOfDigits % 10 == 0;
		}

		public static bool IsValidCvc(string cvc)
		{
			return !string.IsNullOrWhiteSpace(cvc) && Regex.IsMatch(cvc, @"^\d{3,4}$");
		}

		public static bool IsValidExpirationDate(string expirationDate)
		{
			if (string.IsNullOrWhiteSpace(expirationDate) || expirationDate.Length < 5 || !expirationDate.Contains("/"))
				return false;

			bool result = false;

			var month = Convert.ToInt32(expirationDate.Split(new[] { '/' })[0]);
			var year = Convert.ToInt32(expirationDate.Split(new[] { '/' })[1]);

			if (year + 2000 == DateTime.Now.Year)
			{
				if (month >= DateTime.Now.Month && month <= 12)
				{
					result = true;
				}
			}

			if (year + 2000 > DateTime.Now.Year && month <= 12)
			{
				result = true;
			}

			return result;
		}

		public static string Without(this string input, char c)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return new string(input
				.ToCharArray()
				.Where(chr => chr != c).ToArray());
		}

		private static string SeparatedBy(this string input, char separator, int groupLen = 4)
		{
			if (string.IsNullOrEmpty(input) || input.Length < groupLen)
				return input;

			return new string(input
				.ToCharArray()
				.SelectMany((chr, indx) => indx % groupLen == 0 ? new[] { separator, chr } : new[] { chr })
				.ToArray()).Trim(new char[] { separator });
		}
	}
}