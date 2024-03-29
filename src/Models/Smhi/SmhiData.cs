﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Uroskur.Shared.Models;
//
//    var smhiForecast = SmhiForecast.FromJson(jsonString);

namespace Uroskur.Models.Smhi
{
    public partial class SmhiData
    {
        [JsonProperty("approvedTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ApprovedTime { get; set; }

        [JsonProperty("referenceTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ReferenceTime { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public Geometry Geometry { get; set; }

        [JsonProperty("timeSeries", NullValueHandling = NullValueHandling.Ignore)]
        public List<TimeSery> TimeSeries { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<double>> Coordinates { get; set; }
    }

    public partial class TimeSery
    {
        [JsonProperty("validTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ValidTime { get; set; }

        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public List<Parameter> Parameters { get; set; }
    }

    public partial class Parameter
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public Name? Name { get; set; }

        [JsonProperty("levelType", NullValueHandling = NullValueHandling.Ignore)]
        public LevelType? LevelType { get; set; }

        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public long? Level { get; set; }

        [JsonProperty("unit", NullValueHandling = NullValueHandling.Ignore)]
        public Unit? Unit { get; set; }

        [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
        public List<double> Values { get; set; }
    }

    public enum LevelType { Hl, Hmsl };

    public enum Name { Gust, HccMean, LccMean, MccMean, Msl, Pcat, Pmax, Pmean, Pmedian, Pmin, R, Spp, T, TccMean, Tstm, Vis, Wd, Ws, Wsymb2 };

    public enum Unit { Category, Cel, Degree, HPa, KgM2H, Km, MS, Octas, Percent };

    public partial class SmhiData
    {
        public static SmhiData FromJson(string json) => JsonConvert.DeserializeObject<SmhiData>(json, Uroskur.Models.Smhi.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this SmhiData self) => JsonConvert.SerializeObject(self, Uroskur.Models.Smhi.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                LevelTypeConverter.Singleton,
                NameConverter.Singleton,
                UnitConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class LevelTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LevelType) || t == typeof(LevelType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "hl":
                    return LevelType.Hl;
                case "hmsl":
                    return LevelType.Hmsl;
            }
            throw new Exception("Cannot unmarshal type LevelType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LevelType)untypedValue;
            switch (value)
            {
                case LevelType.Hl:
                    serializer.Serialize(writer, "hl");
                    return;
                case LevelType.Hmsl:
                    serializer.Serialize(writer, "hmsl");
                    return;
            }
            throw new Exception("Cannot marshal type LevelType");
        }

        public static readonly LevelTypeConverter Singleton = new LevelTypeConverter();
    }

    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Wsymb2":
                    return Name.Wsymb2;
                case "gust":
                    return Name.Gust;
                case "hcc_mean":
                    return Name.HccMean;
                case "lcc_mean":
                    return Name.LccMean;
                case "mcc_mean":
                    return Name.MccMean;
                case "msl":
                    return Name.Msl;
                case "pcat":
                    return Name.Pcat;
                case "pmax":
                    return Name.Pmax;
                case "pmean":
                    return Name.Pmean;
                case "pmedian":
                    return Name.Pmedian;
                case "pmin":
                    return Name.Pmin;
                case "r":
                    return Name.R;
                case "spp":
                    return Name.Spp;
                case "t":
                    return Name.T;
                case "tcc_mean":
                    return Name.TccMean;
                case "tstm":
                    return Name.Tstm;
                case "vis":
                    return Name.Vis;
                case "wd":
                    return Name.Wd;
                case "ws":
                    return Name.Ws;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            switch (value)
            {
                case Name.Wsymb2:
                    serializer.Serialize(writer, "Wsymb2");
                    return;
                case Name.Gust:
                    serializer.Serialize(writer, "gust");
                    return;
                case Name.HccMean:
                    serializer.Serialize(writer, "hcc_mean");
                    return;
                case Name.LccMean:
                    serializer.Serialize(writer, "lcc_mean");
                    return;
                case Name.MccMean:
                    serializer.Serialize(writer, "mcc_mean");
                    return;
                case Name.Msl:
                    serializer.Serialize(writer, "msl");
                    return;
                case Name.Pcat:
                    serializer.Serialize(writer, "pcat");
                    return;
                case Name.Pmax:
                    serializer.Serialize(writer, "pmax");
                    return;
                case Name.Pmean:
                    serializer.Serialize(writer, "pmean");
                    return;
                case Name.Pmedian:
                    serializer.Serialize(writer, "pmedian");
                    return;
                case Name.Pmin:
                    serializer.Serialize(writer, "pmin");
                    return;
                case Name.R:
                    serializer.Serialize(writer, "r");
                    return;
                case Name.Spp:
                    serializer.Serialize(writer, "spp");
                    return;
                case Name.T:
                    serializer.Serialize(writer, "t");
                    return;
                case Name.TccMean:
                    serializer.Serialize(writer, "tcc_mean");
                    return;
                case Name.Tstm:
                    serializer.Serialize(writer, "tstm");
                    return;
                case Name.Vis:
                    serializer.Serialize(writer, "vis");
                    return;
                case Name.Wd:
                    serializer.Serialize(writer, "wd");
                    return;
                case Name.Ws:
                    serializer.Serialize(writer, "ws");
                    return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }

    internal class UnitConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Unit) || t == typeof(Unit?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Cel":
                    return Unit.Cel;
                case "category":
                    return Unit.Category;
                case "degree":
                    return Unit.Degree;
                case "hPa":
                    return Unit.HPa;
                case "kg/m2/h":
                    return Unit.KgM2H;
                case "km":
                    return Unit.Km;
                case "m/s":
                    return Unit.MS;
                case "octas":
                    return Unit.Octas;
                case "percent":
                    return Unit.Percent;
            }
            throw new Exception("Cannot unmarshal type Unit");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Unit)untypedValue;
            switch (value)
            {
                case Unit.Cel:
                    serializer.Serialize(writer, "Cel");
                    return;
                case Unit.Category:
                    serializer.Serialize(writer, "category");
                    return;
                case Unit.Degree:
                    serializer.Serialize(writer, "degree");
                    return;
                case Unit.HPa:
                    serializer.Serialize(writer, "hPa");
                    return;
                case Unit.KgM2H:
                    serializer.Serialize(writer, "kg/m2/h");
                    return;
                case Unit.Km:
                    serializer.Serialize(writer, "km");
                    return;
                case Unit.MS:
                    serializer.Serialize(writer, "m/s");
                    return;
                case Unit.Octas:
                    serializer.Serialize(writer, "octas");
                    return;
                case Unit.Percent:
                    serializer.Serialize(writer, "percent");
                    return;
            }
            throw new Exception("Cannot marshal type Unit");
        }

        public static readonly UnitConverter Singleton = new UnitConverter();
    }
}
