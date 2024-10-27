using Newtonsoft.Json;

namespace Chat.Extensions.Converters
{
	public class GuidConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(Guid).IsAssignableFrom(objectType);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			var stingValue = reader.ReadAsString();

			if (!Guid.TryParse(stingValue, out var guid))
				throw new InvalidOperationException($"can't parse {stingValue} as {typeof(Guid)}");

			return guid;
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (!(value is Guid guid))
				throw new InvalidOperationException($"{nameof(value)} is not {typeof(Guid)}");

			writer.WriteValue(guid.ToString("N"));
		}
	}
}
