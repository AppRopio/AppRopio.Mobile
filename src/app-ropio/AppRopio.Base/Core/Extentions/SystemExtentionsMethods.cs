using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace System
{
    public static class SystemExtentionsMethods
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        {
            return self == null || !self.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
                action(item);
        }

        public static string BuildAllMessagesAndStackTrace(this Exception exception)
        {
            var innerException = exception;
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine(exception.Message + " \n " + exception.StackTrace);
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
                messageBuilder.AppendLine(innerException.Message + " \n " + innerException.StackTrace);
            }

            return messageBuilder.ToString();
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            var candidateExpr = one.Parameters[0];
            var body = Expression.Not(one.Body);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        public static bool IsDigit(this string self)
        {
            return Regex.IsMatch(self, "\\d");
        }

        public static System.Boolean IsNumeric(this System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
    }
}

