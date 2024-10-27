using Common.Extensions;
using Newtonsoft.Json;

namespace AspNetChat.Extensions.Converters
{
	public class DateTimeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(DateTime).IsAssignableFrom(objectType);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			var stingLongValue = reader.ReadAsString();

			if (!long.TryParse(stingLongValue, out var unixTime))
				throw new InvalidOperationException($"can't parse int64 from sting {stingLongValue}");

			return unixTime.FromUnixDateTime();
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (!(value is DateTime date))
				throw new InvalidOperationException($"{nameof(value)} is not {typeof(DateTime)}");

			writer.WriteValue($"{date.ToUnixDateTime().ToString()}");
		}
	}
}
