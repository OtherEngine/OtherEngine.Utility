using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OtherEngine.Utility
{
	public static class DictionaryExtensions
	{
		/// <summary> Returns the value behind the specified key in this
		///           dictionary, or the default value if it doesn't exist. </summary>
		public static TValue GetOrDefault<TKey, TValue>(
			this IDictionary<TKey, TValue> dict,
			TKey key, TValue @default = default(TValue))
		{
			TValue value;
			return (dict.TryGetValue(key, out value) ? value : @default);
		}


		/// <summary> Returns the value behind the specified key in this dictionary,
		///           or adds and returns the specified new value if it doesn't exist. </summary>
		public static TValue GetOrAdd<TKey, TValue>(
			this IDictionary<TKey, TValue> dict,
			TKey key, TValue newValue)
		{
			TValue value;
			if (!dict.TryGetValue(key, out value))
				dict.Add(key, (value = newValue));
			return value;
		}

		/// <summary> Returns the value behind the specified key in this dictionary, or
		///           adds and returns a new value from the factory if it doesn't exist. </summary>
		public static TValue GetOrAdd<TKey, TValue>(
			this IDictionary<TKey, TValue> dict,
			TKey key, Func<TKey, TValue> newValueFactory)
		{
			TValue value;
			if (!dict.TryGetValue(key, out value))
				dict.Add(key, (value = newValueFactory(key)));
			return value;
		}


		/// <summary> Returns a read-only wrapper of this dictionary. </summary>
		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
			this IDictionary<TKey, TValue> dictionary)
		{
			return new ReadOnlyDictionary<TKey, TValue>(dictionary);
		}
	}
}

