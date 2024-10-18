using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using CommandLine;

namespace CertificateBuilder
{
	internal class Program
	{
		private const string CertificateExtension = "pfx";

		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(RunOptions)
				.WithNotParsed(HandleParseError);
		}

		static void RunOptions(Options opts)
		{
			using var certivicate = BuildSelfSignedServerCertificate(opts.Name);

			var path = $"{opts.Name}.{CertificateExtension}";

			var data = certivicate.Export(X509ContentType.Pfx, opts.Password);

			using var file = File.Open(path, FileMode.Create);
			file.Write(data);

			Console.WriteLine($"certificate written {Path.GetFullPath(path)}");
		}

		static void HandleParseError(IEnumerable<Error> errs)
		{

		}

		private static X509Certificate2 BuildSelfSignedServerCertificate(string certificateName)
		{
			SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
			sanBuilder.AddIpAddress(IPAddress.Loopback);
			sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
			sanBuilder.AddDnsName("localhost");
			sanBuilder.AddDnsName(Environment.MachineName);

			X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={certificateName}");

			using (RSA rsa = RSA.Create(2048))
			{
				var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

				request.CertificateExtensions.Add(
					new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


				request.CertificateExtensions.Add(
				   new X509EnhancedKeyUsageExtension(
					   new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

				request.CertificateExtensions.Add(sanBuilder.Build());

				var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
				certificate.FriendlyName = certificateName;

				return certificate;
			}
		}

		public class Options
		{
			[Option('n', "name", Required = true, HelpText = "name of certificate")]
			public required string Name { get; set; }

			[Option('p', "password", Required = true, HelpText = "password of certificate")]
			public required string Password { get; set; }
		}
	}
}
