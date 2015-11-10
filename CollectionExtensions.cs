using System;
using System.Collections.Generic;
using System.Linq;

namespace OtherEngine.Utility
{
	public static class CollectionExtensions
	{
		/// <summary> Returns a new array, populated with the elements of this collection. </summary>
		public static T[] ArrayCopy<T>(this ICollection<T> collection)
		{
			var array = new T[collection.Count];
			collection.CopyTo(array, 0);
			return array;
		}


		/// <summary> Returns a read-only wrapper of this collection. </summary>
		public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection)
		{
			return new ReadOnlyCollectionWrapper<T>(collection);
		}

		#region ReadOnlyCollectionWrapper class definition

		class ReadOnlyCollectionWrapper<T> : IReadOnlyCollection<T>
		{
			readonly ICollection<T> _collection;


			public int Count { get { return _collection.Count; } }


			public ReadOnlyCollectionWrapper(ICollection<T> collection)
			{
				_collection = collection;
			}


			public IEnumerator<T> GetEnumerator() { return _collection.GetEnumerator(); }

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }
		}

		#endregion


		public static IReadOnlyCollection<TResult> CollectionSelect<TSource, TResult>(
			this IReadOnlyCollection<TSource> collection, Func<TSource, TResult> selector)
		{
			return new ReadOnlySelectEnumerator<TSource, TResult>(collection, selector);
		}

		#region ReadOnlySelectEnumerator class definition

		class ReadOnlySelectEnumerator<TSource, TResult> : IReadOnlyCollection<TResult>
		{
			readonly IReadOnlyCollection<TSource> _source;

			readonly Func<TSource, TResult> _selector;


			public int Count { get { return _source.Count; } }


			public ReadOnlySelectEnumerator(IReadOnlyCollection<TSource> source, Func<TSource, TResult> selector)
			{
				_source = source;
				_selector = selector;
			}


			public IEnumerator<TResult> GetEnumerator()
			{
				return _source.Select(_selector).GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		#endregion
	}
}

