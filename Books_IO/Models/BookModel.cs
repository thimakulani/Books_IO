using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Books_IO
{
    public partial class BookModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("doi")]
        public string Doi { get; set; }

        [JsonProperty("pmid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Pmid { get; set; }

        [JsonProperty("uri")]
        public Uri Uri { get; set; }

        [JsonProperty("isbns")]
        public List<string> Isbns { get; set; }

        [JsonProperty("cohorts")]
        public Cohorts Cohorts { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("authors_or_editors")]
        public List<string> AuthorsOrEditors { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attribution")]
        public string Attribution { get; set; }

        [JsonProperty("altmetric_id")]
        public long AltmetricId { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }

        [JsonProperty("is_oa")]
        public bool IsOa { get; set; }

        [JsonProperty("cited_by_posts_count")]
        public long CitedByPostsCount { get; set; }

        [JsonProperty("cited_by_feeds_count")]
        public long CitedByFeedsCount { get; set; }

        [JsonProperty("cited_by_policies_count")]
        public long CitedByPoliciesCount { get; set; }

        [JsonProperty("cited_by_tweeters_count")]
        public long CitedByTweetersCount { get; set; }

        [JsonProperty("cited_by_fbwalls_count")]
        public long CitedByFbwallsCount { get; set; }

        [JsonProperty("cited_by_gplus_count")]
        public long CitedByGplusCount { get; set; }

        [JsonProperty("cited_by_msm_count")]
        public long CitedByMsmCount { get; set; }

        [JsonProperty("cited_by_accounts_count")]
        public long CitedByAccountsCount { get; set; }

        [JsonProperty("last_updated")]
        public long LastUpdated { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("history")]
        public History History { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("added_on")]
        public long AddedOn { get; set; }

        [JsonProperty("published_on")]
        public long PublishedOn { get; set; }

        [JsonProperty("readers")]
        public Readers Readers { get; set; }

        [JsonProperty("readers_count")]
        public long ReadersCount { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("details_url")]
        public Uri DetailsUrl { get; set; }
    }

    public partial class Cohorts
    {
        [JsonProperty("pub")]
        public long Pub { get; set; }

        [JsonProperty("doc")]
        public long Doc { get; set; }

        [JsonProperty("sci")]
        public long Sci { get; set; }

        [JsonProperty("com")]
        public long Com { get; set; }
    }

    public partial class History
    {
        [JsonProperty("1y")]
        public double The1Y { get; set; }

        [JsonProperty("6m")]
        public double The6M { get; set; }

        [JsonProperty("3m")]
        public double The3M { get; set; }

        [JsonProperty("1m")]
        public double The1M { get; set; }

        [JsonProperty("1w")]
        public long The1W { get; set; }

        [JsonProperty("6d")]
        public long The6D { get; set; }

        [JsonProperty("5d")]
        public long The5D { get; set; }

        [JsonProperty("4d")]
        public long The4D { get; set; }

        [JsonProperty("3d")]
        public long The3D { get; set; }

        [JsonProperty("2d")]
        public long The2D { get; set; }

        [JsonProperty("1d")]
        public long The1D { get; set; }

        [JsonProperty("at")]
        public double At { get; set; }
    }

    public partial class Images
    {
        [JsonProperty("small")]
        public Uri Small { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }

        [JsonProperty("large")]
        public Uri Large { get; set; }
    }

    public partial class Readers
    {
        [JsonProperty("citeulike")]
        public long Citeulike { get; set; }

        [JsonProperty("mendeley")]
        public long Mendeley { get; set; }

        [JsonProperty("connotea")]
        public long Connotea { get; set; }
    }

    public partial class BookModel
    {
        public static BookModel FromJson(string json) => JsonConvert.DeserializeObject<BookModel>(json, Books_IO.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this BookModel self) => JsonConvert.SerializeObject(self, Books_IO.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
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
}
