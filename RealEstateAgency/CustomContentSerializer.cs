using Refit;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RealEstateAgency
{
    public class CustomContentSerializer : IHttpContentSerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public CustomContentSerializer() : this(new JsonSerializerOptions(JsonSerializerDefaults.Web))
        {
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            jsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            jsonSerializerOptions.WriteIndented = true;
            //jsonSerializerOptions.IgnoreNullValues = true;
            jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public CustomContentSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            this.jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
        {
            // this needs to be added
            if (content.Headers.ContentType != null && content.Headers.ContentType.MediaType == "application/json")
            {
                var item = await content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
                return item;
            }

            return default;
        }

        public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return propertyInfo.GetCustomAttributes<JsonPropertyNameAttribute>(true)
                       .Select(a => a.Name)
                       .FirstOrDefault();
        }

        public HttpContent ToHttpContent<T>(T item)
        {
            var content = JsonContent.Create(item, options: jsonSerializerOptions);
            return content;
        }

        // From https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#deserialize-inferred-types-to-object-properties
        public class ObjectToInferredTypesConverter
           : JsonConverter<object>
        {
            public override object? Read(
              ref Utf8JsonReader reader,
              Type typeToConvert,
              JsonSerializerOptions options) => reader.TokenType switch
              {
                  JsonTokenType.True => true,
                  JsonTokenType.False => false,
                  JsonTokenType.Number when reader.TryGetInt64(out var l) => l,
                  JsonTokenType.Number => reader.GetDouble(),
                  JsonTokenType.String when reader.TryGetDateTime(out var datetime) => datetime,
                  JsonTokenType.String => reader.GetString(),
                  _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
              };

            public override void Write(
                Utf8JsonWriter writer,
                object objectToWrite,
                JsonSerializerOptions options) =>
                JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
        }
    }
}
