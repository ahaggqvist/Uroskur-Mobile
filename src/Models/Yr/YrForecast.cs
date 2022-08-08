namespace Uroskur.Models.Yr
{
    namespace Uroskur.Shared.Models
    {
        public partial class YrForecast
        {
            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string? Type { get; set; }

            [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
            public Geometry? Geometry { get; set; }

            [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
            public Properties? Properties { get; set; }
        }

        public class Geometry
        {
            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string? Type { get; set; }

            [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
            public List<long>? Coordinates { get; set; }
        }

        public class Properties
        {
            [JsonProperty("meta", NullValueHandling = NullValueHandling.Ignore)]
            public Meta? Meta { get; set; }

            [JsonProperty("timeseries", NullValueHandling = NullValueHandling.Ignore)]
            public List<Timesery>? Timeseries { get; set; }
        }

        public class Meta
        {
            [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? UpdatedAt { get; set; }

            [JsonProperty("units", NullValueHandling = NullValueHandling.Ignore)]
            public Units? Units { get; set; }
        }

        public class Units
        {
            [JsonProperty("air_pressure_at_sea_level", NullValueHandling = NullValueHandling.Ignore)]
            public string? AirPressureAtSeaLevel { get; set; }

            [JsonProperty("air_temperature", NullValueHandling = NullValueHandling.Ignore)]
            public string? AirTemperature { get; set; }

            [JsonProperty("cloud_area_fraction", NullValueHandling = NullValueHandling.Ignore)]
            public string? CloudAreaFraction { get; set; }

            [JsonProperty("precipitation_amount", NullValueHandling = NullValueHandling.Ignore)]
            public string? PrecipitationAmount { get; set; }

            [JsonProperty("relative_humidity", NullValueHandling = NullValueHandling.Ignore)]
            public string? RelativeHumidity { get; set; }

            [JsonProperty("wind_from_direction", NullValueHandling = NullValueHandling.Ignore)]
            public string? WindFromDirection { get; set; }

            [JsonProperty("wind_speed", NullValueHandling = NullValueHandling.Ignore)]
            public string? WindSpeed { get; set; }
        }

        public class Timesery
        {
            [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? Time { get; set; }

            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public Data? Data { get; set; }
        }

        public class Data
        {
            [JsonProperty("instant", NullValueHandling = NullValueHandling.Ignore)]
            public Instant? Instant { get; set; }

            [JsonProperty("next_12_hours", NullValueHandling = NullValueHandling.Ignore)]
            public Next12_Hours? Next12_Hours { get; set; }

            [JsonProperty("next_1_hours", NullValueHandling = NullValueHandling.Ignore)]
            public NextHours? Next1_Hours { get; set; }

            [JsonProperty("next_6_hours", NullValueHandling = NullValueHandling.Ignore)]
            public NextHours? Next6_Hours { get; set; }
        }

        public class Instant
        {
            [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
            public InstantDetails? Details { get; set; }
        }

        public class InstantDetails
        {
            [JsonProperty("air_pressure_at_sea_level", NullValueHandling = NullValueHandling.Ignore)]
            public double? AirPressureAtSeaLevel { get; set; }

            [JsonProperty("air_temperature", NullValueHandling = NullValueHandling.Ignore)]
            public double? AirTemperature { get; set; }

            [JsonProperty("cloud_area_fraction", NullValueHandling = NullValueHandling.Ignore)]
            public double? CloudAreaFraction { get; set; }

            [JsonProperty("relative_humidity", NullValueHandling = NullValueHandling.Ignore)]
            public double? RelativeHumidity { get; set; }

            [JsonProperty("wind_from_direction", NullValueHandling = NullValueHandling.Ignore)]
            public double? WindFromDirection { get; set; }

            [JsonProperty("wind_speed", NullValueHandling = NullValueHandling.Ignore)]
            public double? WindSpeed { get; set; }
        }

        public class Next12_Hours
        {
            [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
            public Summary? Summary { get; set; }
        }

        public class Summary
        {
            [JsonProperty("symbol_code", NullValueHandling = NullValueHandling.Ignore)]
            public SymbolCode? SymbolCode { get; set; }
        }

        public class NextHours
        {
            [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
            public Summary? Summary { get; set; }

            [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
            public Next1_HoursDetails? Details { get; set; }
        }

        public class Next1_HoursDetails
        {
            [JsonProperty("precipitation_amount", NullValueHandling = NullValueHandling.Ignore)]
            public double? PrecipitationAmount { get; set; }
        }

        public enum SymbolCode
        {
            ClearskyDay,
            ClearskyNight,
            Cloudy,
            FairDay,
            FairNight,
            Lightrain,
            PartlycloudyDay,
            PartlycloudyNight,
            Rain
        }

        public partial class YrForecast
        {
            public static YrForecast? FromJson(string json)
            {
                return JsonConvert.DeserializeObject<YrForecast>(json, Converter.Settings);
            }
        }

        public static class Serialize
        {
            public static string ToJson(this YrForecast self)
            {
                return JsonConvert.SerializeObject(self, Converter.Settings);
            }
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings? Settings = new()
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    SymbolCodeConverter.Singleton,
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                }
            };
        }

        internal class SymbolCodeConverter : JsonConverter
        {
            public static readonly SymbolCodeConverter Singleton = new();

            public override bool CanConvert(Type t)
            {
                return t == typeof(SymbolCode) || t == typeof(SymbolCode?);
            }

            public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }

                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "clearsky_day":
                        return SymbolCode.ClearskyDay;
                    case "clearsky_night":
                        return SymbolCode.ClearskyNight;
                    case "cloudy":
                        return SymbolCode.Cloudy;
                    case "fair_day":
                        return SymbolCode.FairDay;
                    case "fair_night":
                        return SymbolCode.FairNight;
                    case "lightrain":
                        return SymbolCode.Lightrain;
                    case "partlycloudy_day":
                        return SymbolCode.PartlycloudyDay;
                    case "partlycloudy_night":
                        return SymbolCode.PartlycloudyNight;
                    case "rain":
                        return SymbolCode.Rain;
                }

                throw new Exception("Cannot unmarshal type SymbolCode");
            }

            public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }

                var value = (SymbolCode)untypedValue;
                switch (value)
                {
                    case SymbolCode.ClearskyDay:
                        serializer.Serialize(writer, "clearsky_day");
                        return;
                    case SymbolCode.ClearskyNight:
                        serializer.Serialize(writer, "clearsky_night");
                        return;
                    case SymbolCode.Cloudy:
                        serializer.Serialize(writer, "cloudy");
                        return;
                    case SymbolCode.FairDay:
                        serializer.Serialize(writer, "fair_day");
                        return;
                    case SymbolCode.FairNight:
                        serializer.Serialize(writer, "fair_night");
                        return;
                    case SymbolCode.Lightrain:
                        serializer.Serialize(writer, "lightrain");
                        return;
                    case SymbolCode.PartlycloudyDay:
                        serializer.Serialize(writer, "partlycloudy_day");
                        return;
                    case SymbolCode.PartlycloudyNight:
                        serializer.Serialize(writer, "partlycloudy_night");
                        return;
                    case SymbolCode.Rain:
                        serializer.Serialize(writer, "rain");
                        return;
                }

                throw new Exception("Cannot marshal type SymbolCode");
            }
        }
    }
}