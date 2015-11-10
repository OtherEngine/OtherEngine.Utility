using System;
using System.Collections;

namespace OtherEngine.Utility
{
	public static class HashHelper
	{
		/// <summary> Returns a hash code computed from the elements of the specified enumerable. </summary>
		public static int ComputeHashCode(IEnumerable toHash)
		{
			if (toHash == null)
				throw new ArgumentNullException("toHash");
			
			unchecked {
				var set = false;
				var hash = (int)2166136261;

				foreach (var obj in toHash) {
					if (obj == null)
						throw new ArgumentException("toHash can't contain null values", "toHash");
					
					hash = hash * 16777619 ^ obj.GetHashCode();
					set = true;
				}

				if (!set)
					throw new ArgumentException("toHash can't be empty", "toHash");

				return hash;
			}
		}

		/// <summary> Returns a hash code computed from the specified objects. </summary>
		public static int ComputeHashCode(params object[] toHash)
		{
			return ComputeHashCode((IEnumerable)toHash);
		}
	}
}

