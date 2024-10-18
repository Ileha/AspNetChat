namespace AspNetChat.Extensions
{
	public static class DateTimeExtension
	{
		private static readonly DateTime _unixEpochStartTime = new DateTime(1970, 1, 1);

		public static long ToUnixDateTime(this DateTime dateTime) 
		{
			return (long) dateTime.Subtract(_unixEpochStartTime).TotalSeconds;
		}

		public static DateTime FromUnixDateTime(this long dateTime) 
		{
			return _unixEpochStartTime.AddSeconds(dateTime);
		}
	}
}
