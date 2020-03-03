using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AzureMonitor.Responses
{
    public class RecommendationsResponse
    {
        [JsonProperty("value")]
        public List<Recommendations> Recommendations { get; set; }
    }

    public class Recommendations
    {
        [JsonProperty("properties")]
        public RecommendationsProperties Properties { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class RecommendationsProperties
    {
        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("impact")]
        public Impact Impact { get; set; }

        [JsonProperty("impactedField")]
        public string ImpactedField { get; set; }

        [JsonProperty("impactedValue")]
        public string ImpactedValue { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }

        [JsonProperty("recommendationTypeId")]
        public string RecommendationTypeId { get; set; }

        [JsonProperty("shortDescription")]
        public ShortDescription ShortDescription { get; set; }

        [JsonProperty("extendedProperties", NullValueHandling = NullValueHandling.Ignore)]
        public ExtendedProperties ExtendedProperties { get; set; }
    }

    public class ExtendedProperties
    {
        [JsonProperty("assessmentKey")]
        public Guid AssessmentKey { get; set; }

        [JsonProperty("score")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Score { get; set; }
    }

    public class ShortDescription
    {
        [JsonProperty("problem")]
        public string Problem { get; set; }

        [JsonProperty("solution")]
        public string Solution { get; set; }
    }

    public enum Category { Cost, HighAvailability, OperationalExcellence, Performance, Security }

    public enum Impact { High, Low, Medium }

    internal static class RecommendationsConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CategoryConverter.Singleton,
                ImpactConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Category?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Cost":
                    return Category.Cost;
                case "HighAvailability":
                    return Category.HighAvailability;
                case "OperationalExcellence":
                    return Category.OperationalExcellence;
                case "Performance":
                    return Category.Performance;
                case "Security":
                    return Category.Security;
                default:
                    throw new Exception("Cannot unmarshal type Category");
            }
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Category)untypedValue;
            switch (value)
            {
                case Category.Cost:
                    serializer.Serialize(writer, "Cost");
                    return;
                case Category.HighAvailability:
                    serializer.Serialize(writer, "HighAvailability");
                    return;
                case Category.OperationalExcellence:
                    serializer.Serialize(writer, "OperationalExcellence");
                    return;
                case Category.Performance:
                    serializer.Serialize(writer, "Performance");
                    return;
                case Category.Security:
                    serializer.Serialize(writer, "Security");
                    return;
                default:
                    throw new Exception("Cannot marshal type Category");
            }

        }

        public static readonly CategoryConverter Singleton = new CategoryConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class ImpactConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Impact) || t == typeof(Impact?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "High":
                    return Impact.High;
                case "Low":
                    return Impact.Low;
                case "Medium":
                    return Impact.Medium;
            }
            throw new Exception("Cannot unmarshal type Impact");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Impact)untypedValue;
            switch (value)
            {
                case Impact.High:
                    serializer.Serialize(writer, "High");
                    return;
                case Impact.Low:
                    serializer.Serialize(writer, "Low");
                    return;
                case Impact.Medium:
                    serializer.Serialize(writer, "Medium");
                    return;
            }
            throw new Exception("Cannot marshal type Impact");
        }

        public static readonly ImpactConverter Singleton = new ImpactConverter();
    }
}
