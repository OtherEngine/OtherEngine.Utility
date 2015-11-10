using System;
using System.Collections;

namespace OtherEngine.Utility
{
	/// <summary> Thrown when there's a problem with an IEnumerable argument. </summary>
	public class ArgumentEnumerableException : ArgumentException
	{
		public IEnumerable Enumerable { get; private set; }

		public override string Message {
			get { return base.Message + Environment.NewLine +
			             "Enumerable: " + (Enumerable ?? "<null>"); }
		}

		public ArgumentEnumerableException(IEnumerable enumerable, string message,
		                                   string paramName, Exception innerException = null)
			: base(message, paramName, innerException)
		{
			Enumerable = enumerable;
		}
	}

	/// <summary> Thrown when there's a problem with an element of an IEnumerable argument. </summary>
	public class ArgumentElementException : ArgumentEnumerableException
	{
		public object Element { get; private set; }

		public override string Message {
			get { return base.Message + Environment.NewLine +
			             "Element: " + (Element ?? "<null>"); }
		}

		public ArgumentElementException(IEnumerable enumerable, object element,
		                                string message, string paramName, Exception innerException = null)
			: base(enumerable, message, paramName, innerException)
		{
			Element = element;
		}
	}

	/// <summary> Thrown when an element of an enumerable argument is null. </summary>
	public class ArgumentElementNullException : ArgumentElementException
	{
		public ArgumentElementNullException(IEnumerable enumerable, string paramName,
		                                    string message = null, Exception innerException = null)
			: base(enumerable, null, (message ?? GetDefaultMessage(paramName)), paramName, innerException) {  }

		static string GetDefaultMessage(string paramName)
		{
			return (paramName + " can't contain null elements");
		}
	}

	public static class EnumerableExceptionExtensions
	{
		/// <summary> Creates and returns an ArgumentElementException wrapper of this exception.
		///           If allowNull is false, the specified element is null and the original exception is an
		///           ArgumentNullException or NullReferenceException, returns an ArgumentElementNullException. </summary>
		public static Exception MakeElementException(this Exception exception, IEnumerable enumerable,
		                                             object element, string paramName, bool allowNull = false)
		{
			if (!allowNull && (element == null) && ((exception is ArgumentNullException) ||
			                                        (exception is NullReferenceException)))
				return new ArgumentElementNullException(enumerable, paramName, innerException: exception);
			return new ArgumentElementException(enumerable, element, paramName,
			                                    exception.Message, exception);
		}
	}
}

