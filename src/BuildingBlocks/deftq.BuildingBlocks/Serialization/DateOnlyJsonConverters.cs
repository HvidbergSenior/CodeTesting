using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;

namespace deftq.BuildingBlocks.Serialization
{
    public sealed class SystemTextDateOnlyJsonConverter : System.Text.Json.Serialization.JsonConverter<DateOnly>
    {
        internal const string DateFormat = "dd-MM-yyyy";
        
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString()!, DateFormat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
    
    public class NewtonsoftDateOnlyJsonConverter : Newtonsoft.Json.JsonConverter<DateOnly>
    {
        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return DateOnly.ParseExact((string)reader.Value!, SystemTextDateOnlyJsonConverter.DateFormat, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteValue(value.ToString(SystemTextDateOnlyJsonConverter.DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
