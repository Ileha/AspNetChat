using CommandLine;

namespace AspNetChat.Core.Entities
{
	public class Options
	{
		[Option("customHttps", Required = false, HelpText = "need to configure custom https certificate")]
		public bool CustomHttpsCertificate { get; set; }
		
		[Option('j', "jsons", Required = false, HelpText = "additional jsons to load")]
		public IEnumerable<string>? Jsons { get; set; }

		[Option("staticFiles", Required = false, HelpText = "override static files location")]
		public string? StaticFilesLocation { get; set; }

		[Option("db", Required = true, HelpText = "configuration for database, first connection string, second data base name")]
		public required IEnumerable<string> DataBaseConnection { get; set; }
	}
}
