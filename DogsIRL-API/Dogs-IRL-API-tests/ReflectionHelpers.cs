using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dogs_IRL_API_tests
{
	public class ReflectionHelpers
	{
		public static string GetMethodName<T, U>(Expression<Func<T, U>> expression)
		{
			var method = expression.Body as MethodCallExpression;
			if (method != null)
				return method.Method.Name;

			throw new ArgumentException("Expression is wrong");
		}
	}
}
