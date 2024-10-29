using CommandLine;
using static AspNetChat.Constants;

namespace AspNetChat.Core.Entities
{
	public class Options
	{
		[Option('j', "jsons", Required = false, HelpText = "additional jsons to load")]
		public IEnumerable<string>? Jsons { get; set; }

		[Option("staticFiles", Required = false, HelpText = "override static files location")]
		public string? StaticFilesLocation { get; set; }

		[Option("db", Required = true, HelpText = "configuration for database, first connection string, second data base name")]
		public required IEnumerable<string> DataBaseConnection { get; set; }

		#region Kestrel

		[Option(OptionsTitles.UseKestrelLong, Required = false, HelpText = "turn on Kestrel configuration")]
		public bool UseKestrel { get; set; }
		
		[Option(
			OptionsTitles.CustomHttpsLong, 
			Required = false, 
			HelpText = $"need to configure custom https certificate, requires '{OptionsTitles.UseKestrelLong}'")]
		public bool CustomHttpsCertificate { get; set; }
		
		[Option(
			"httpsPort", 
			Required = false, 
			HelpText = $"https port, requires '{OptionsTitles.CustomHttpsLong}' and '{OptionsTitles.UseKestrelLong}' options")]
		public int? HttpsPort { get; set; }
		
		[Option("httpPort", Required = false, HelpText = $"http port, requires '{OptionsTitles.UseKestrelLong}'")]
		public int? HttpPort { get; set; }

		#endregion
	}
}
