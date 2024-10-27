namespace Common.Extensions
{
	public static class DateTimeExtension
	{
		private static readonly DateTime UnixEpochStartTime = new DateTime(1970, 1, 1);

		public static long ToUnixDateTime(this DateTime dateTime) 
		{
			return (long) dateTime.Subtract(UnixEpochStartTime).TotalSeconds;
		}

		public static DateTime FromUnixDateTime(this long dateTime) 
		{
			return UnixEpochStartTime.AddSeconds(dateTime);
		}
	}
}
