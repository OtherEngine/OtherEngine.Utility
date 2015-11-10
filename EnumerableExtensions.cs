using System;
using System.Linq;
using System.Collections.Generic;

namespace OtherEngine.Utility
{
	public static class EnumerableExtensions
	{
		/// <summary> Returns an enumerable that, when iterated, returns the
		///           provided elements before the source enumerable. </summary>
		public static IEnumerable<T> Precede<T>(this IEnumerable<T> enumerable, params T[] elements)
		{
			return elements.Concat(enumerable);
		}

		/// <summary> Returns an enumerable that, when iterated, returns the
		///           provided elements after the source enumerable. </summary>
		public static IEnumerable<T> Follow<T>(this IEnumerable<T> enumerable, params T[] elements)
		{
			return enumerable.Concat(elements);
		}


		/// <summary> Returns if the source enumerable contains all elements in the specified enumerable. </summary>
		public static bool ContainsAll<T>(this IEnumerable<T> enumerable, IEnumerable<T> elements)
		{
			return !elements.Except(enumerable).Any();
		}

		/// <summary> Returns if the enumerable contains all the specified elements. </summary>
		public static bool ContainsAll<T>(this IEnumerable<T> enumerable, params T[] elements)
		{
			if (elements.Length <= 0)
				throw new ArgumentException("elements is empty", "elements");
			return enumerable.ContainsAll((IEnumerable<T>)elements);
		}


		/// <summary> Returns the contents of this enumerable as a read-only list. </summary>
		public static IReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.ToList().AsReadOnly();
		}
	}
}

