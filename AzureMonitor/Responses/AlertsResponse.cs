using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AzureMonitor.Responses
{
    public class AlertsResponse
    {
        [JsonProperty("nextLink")]
        public Uri NextLink { get; set; }

        [JsonProperty("value")]
        public List<Alerts> Alerts { get; set; }
    }

    public class Alerts
    {
        [JsonProperty("properties")]
        public AlertsProperties Properties { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class AlertsProperties
    {
        [JsonProperty("essentials")]
        public Essentials Essentials { get; set; }
    }

    public class Essentials
    {
        [JsonProperty("alertRule")]
        public string AlertRule { get; set; }

        [JsonProperty("alertState")]
        public AlertState AlertState { get; set; }

        [JsonProperty("monitorCondition")]
        public MonitorCondition MonitorCondition { get; set; }

        [JsonProperty("severity")]
        public Severity Severity { get; set; }

        [JsonProperty("signalType")]
        public SignalType SignalType { get; set; }

        [JsonProperty("monitorService")]
        public string MonitorService { get; set; }

        [JsonProperty("targetResource")]
        public string TargetResource { get; set; }

        [JsonProperty("targetResourceName")]
        public string TargetResourceName { get; set; }

        [JsonProperty("targetResourceGroup")]
        public string TargetResourceGroup { get; set; }

        [JsonProperty("targetResourceType")]
        public string TargetResourceType { get; set; }

        [JsonProperty("sourceCreatedId")]
        public string SourceCreatedId { get; set; }

        [JsonProperty("smartGroupId")]
        public string SmartGroupId { get; set; }

        [JsonProperty("startDateTime")]
        public DateTimeOffset StartDateTime { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("lastModifiedUserName")]
        public string LastModifiedUserName { get; set; }

        [JsonProperty("actionStatus")]
        public ActionStatus ActionStatus { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("smartGroupingReason")]
        public string SmartGroupingReason { get; set; }
    }

    public class ActionStatus
    {
        [JsonProperty("isSuppressed")]
        public bool IsSuppressed { get; set; }
    }

    public enum AlertState
    {
        New,
        Acknowledged,
        Closed
    }

    public enum MonitorCondition { Fired, Resolved }

    public enum Severity
    {
        Sev0,
        Sev1,
        Sev2,
        Sev3,
        Sev4
    }

    public enum SignalType { Log, Metric, Unknown };

    internal static class AlertsConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AlertStateConverter.Singleton,
                MonitorConditionConverter.Singleton,
                SeverityConverter.Singleton,
                SignalTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AlertStateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AlertState) || t == typeof(AlertState?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "New":
                    return AlertState.New;
                case "Acknowledged":
                    return AlertState.Acknowledged;
                case "Closed":
                    return AlertState.Closed;
                default:
                    throw new Exception("Cannot unmarshal type AlertState");
            }
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AlertState)untypedValue;
            switch (value)
            {
                case AlertState.New:
                    serializer.Serialize(writer, "New");
                    return;
                case AlertState.Acknowledged:
                    serializer.Serialize(writer, "Acknowledged");
                    return;
                case AlertState.Closed:
                    serializer.Serialize(writer, "Closed");
                    return;
                default:
                    throw new Exception("Cannot marshal type AlertState");
            }
        }

        public static readonly AlertStateConverter Singleton = new AlertStateConverter();
    }

    internal class MonitorConditionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(MonitorCondition) || t == typeof(MonitorCondition?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Fired":
                    return MonitorCondition.Fired;
                case "Resolved":
                    return MonitorCondition.Resolved;
                default:
                    throw new Exception("Cannot unmarshal type MonitorCondition");
            }
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (MonitorCondition)untypedValue;
            switch (value)
            {
                case MonitorCondition.Fired:
                    serializer.Serialize(writer, "Fired");
                    return;
                case MonitorCondition.Resolved:
                    serializer.Serialize(writer, "Resolved");
                    return;
                default:
                    throw new Exception("Cannot marshal type MonitorCondition");
            }
        }

        public static readonly MonitorConditionConverter Singleton = new MonitorConditionConverter();
    }

    internal class SeverityConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Severity) || t == typeof(Severity?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Sev0":
                    return Severity.Sev0;
                case "Sev1":
                    return Severity.Sev1;
                case "Sev2":
                    return Severity.Sev2;
                case "Sev3":
                    return Severity.Sev3;
                case "Sev4":
                    return Severity.Sev4;
                default:
                    throw new Exception("Cannot unmarshal type Severity");
            }
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Severity)untypedValue;
            switch (value)
            {
                case Severity.Sev0:
                    serializer.Serialize(writer, "Sev0");
                    return;
                case Severity.Sev1:
                    serializer.Serialize(writer, "Sev1");
                    return;
                case Severity.Sev2:
                    serializer.Serialize(writer, "Sev2");
                    return;
                case Severity.Sev3:
                    serializer.Serialize(writer, "Sev3");
                    return;
                case Severity.Sev4:
                    serializer.Serialize(writer, "Sev4");
                    return;
                default:
                    throw new Exception("Cannot marshal type Severity");
            }
        }

        public static readonly SeverityConverter Singleton = new SeverityConverter();
    }

    internal class SignalTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(SignalType) || t == typeof(SignalType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Log":
                    return SignalType.Log;
                case "Metric":
                    return SignalType.Metric;
                case "Unknown":
                    return SignalType.Unknown;
                default:
                    throw new Exception("Cannot unmarshal type SignalType");
            }
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (SignalType)untypedValue;
            switch (value)
            {
                case SignalType.Log:
                    serializer.Serialize(writer, "Log");
                    return;
                case SignalType.Metric:
                    serializer.Serialize(writer, "Metric");
                    return;
                case SignalType.Unknown:
                    serializer.Serialize(writer, "Unknown");
                    return;
                default:
                    throw new Exception("Cannot marshal type SignalType");
            }
        }

        public static readonly SignalTypeConverter Singleton = new SignalTypeConverter();
    }
}
