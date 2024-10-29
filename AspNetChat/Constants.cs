namespace AspNetChat
{
	public static class Constants
	{
		public static class Ports
		{
			public const int HttpsPort = 8080;
			public const int HttpPort = 8081;
			
			public static string HttpsPortName => HttpsPort.ToString();
		}
		
		public static class OptionsTitles
		{
			public const string CustomHttpsLong = "customHttps";
			public const string UseKestrelLong = "useKestrel";
		}
	}
}
