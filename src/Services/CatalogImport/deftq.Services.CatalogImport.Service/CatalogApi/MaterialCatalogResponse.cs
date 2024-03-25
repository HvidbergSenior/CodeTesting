using System.Globalization;
using System.Text.Json.Serialization;

namespace deftq.Services.CatalogImport.Service.CatalogApi
{
    public class MaterialCatalogResponse
    {
        [JsonPropertyName("has_next_page")] public bool HasNextPage { get; }

        [JsonPropertyName("has_previous_page")]
        public bool HasPreviousPage { get; }

        [JsonPropertyName("next_page")] public Uri NextPageUrl { get; }

        [JsonPropertyName("previous_page")] public Uri PreviousPageUrl { get; }

        [JsonPropertyName("data")] public IList<MaterialResponse> Data { get; }

        public MaterialCatalogResponse(bool hasNextPage, bool hasPreviousPage, Uri nextPageUrl, Uri previousPageUrl, IList<MaterialResponse> data)
        {
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
            Data = data;
        }

        public static MaterialCatalogResponse Empty()
        {
            var emptyUrl = new Uri("http://example.org");
            return new MaterialCatalogResponse(false, false, emptyUrl, emptyUrl, new List<MaterialResponse>());
        }
    }

    public class MaterialResponse
    {
        [JsonPropertyName("ean")] public string Ean { get; }

        [JsonPropertyName("name")] public string Name { get; }

        [JsonPropertyName("unit")] public string Unit { get; }

        [JsonPropertyName("mountings")] public IList<MountingResponse> Mountings { get; }

        public MaterialResponse(string ean, string name, string unit, IList<MountingResponse> mountings)
        {
            Ean = ean;
            Name = name;
            Unit = unit;
            Mountings = mountings;
        }
    }

    public class MountingResponse
    {
        [JsonPropertyName("mountingCode")] public string MountingCode { get; }

        [JsonPropertyName("operationTimeMs")] public int OperationTimeMs { get; }

        [JsonPropertyName("SupplementOperations")]
        public IList<SupplementOperation> SupplementOperations { get; }

        public MountingResponse(string mountingCode, int operationTimeMs, IList<SupplementOperation> supplementOperations)
        {
            MountingCode = mountingCode;
            OperationTimeMs = operationTimeMs;
            SupplementOperations = supplementOperations;
        }

        public int MountingCodeAsInt()
        {
            if (Int32.TryParse(MountingCode, NumberStyles.Integer, CultureInfo.InvariantCulture, out var code))
            {
                return code;
            }

            throw new InvalidOperationException($"Mounting code {MountingCode} is not a valid integer");
        }
    }

    public class SupplementOperation
    {
        public string OperationNumber { get; }
        public string Text { get; }
        public string Type { get; }
        public int OperationTimeMs { get; }

        public SupplementOperation(string operationNumber, string text, string type, int operationTimeMs)
        {
            OperationNumber = operationNumber;
            Text = text;
            Type = type;
            OperationTimeMs = operationTimeMs;
        }
    }
}
