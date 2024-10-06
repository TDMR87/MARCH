using System.Text.Json;
using System.Text.Json.Serialization;

namespace March.Web.Utils;

public class StringToBooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.String:
                var stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                    return false;
                if (bool.TryParse(stringValue, out bool result))
                    return result;
                return stringValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            default:
                throw new JsonException($"Cannot convert {reader.TokenType} to Boolean");
        }
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
