using Common.Interfaces;

namespace Common.Entities
{
	public class Identifiable : IIdentifiable
	{
		public Guid Id { get; }
		public Identifiable(Guid guid)
		{
			Id = guid;
		}

		public static explicit operator Identifiable(Guid b) => new Identifiable(b);
	}
}
