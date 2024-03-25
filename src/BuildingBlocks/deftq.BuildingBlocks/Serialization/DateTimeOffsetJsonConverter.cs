using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace deftq.BuildingBlocks.Serialization
{
    public sealed class SystemTextDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        public static readonly string IsoFormat = "dd-MM-yyyy'T'HH:mm:ss.fffzzz";
        private static readonly DateTimeFormatInfo DateTimeFormatInfo = DateTimeFormatInfo.InvariantInfo;

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTimeOffset.ParseExact(reader.GetString()!, IsoFormat, DateTimeFormatInfo, DateTimeStyles.None);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStringValue(value.ToString(IsoFormat, DateTimeFormatInfo));
        }
    }
}
