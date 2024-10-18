using System.Security.Cryptography.X509Certificates;

namespace AspNetChat.Core.Entities
{

	public class HttpsCertificateSettings 
	{
		public string Key { get; set; }
		public string CertificateBase64 { get; set; }

		public X509Certificate2 GetCertificate() 
		{
			byte[] data = Convert.FromBase64String(CertificateBase64);

			return new X509Certificate2(data, Key);
		}
	}
}
