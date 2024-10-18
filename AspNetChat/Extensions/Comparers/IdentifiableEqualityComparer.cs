using AspNetChat.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace AspNetChat.Extensions.Comparers
{
	public class IdentifiableEqualityComparer : IEqualityComparer<IIdentifiable>
	{
		public bool Equals(IIdentifiable? x, IIdentifiable? y)
		{
			if (x == null && y == null)
				return true;

			if (x == null || y == null) 
				return false;

			if (ReferenceEquals(x, y)) 
				return true;

			return x.Id.Equals(y.Id);
		}

		public int GetHashCode([DisallowNull] IIdentifiable obj)
		{
			return obj.Id.GetHashCode();
		}
	}
}
