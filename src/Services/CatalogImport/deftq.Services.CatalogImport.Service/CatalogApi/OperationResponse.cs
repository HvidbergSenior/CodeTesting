using System.Text.Json.Serialization;

namespace deftq.Services.CatalogImport.Service.CatalogApi
{
    public class OperationResponse
    {
        [JsonPropertyName("OperationNumber")]
        public string OperationNumber { get; }
        
        [JsonPropertyName("Text")]
        public string Text { get; }

        [JsonPropertyName("OperationTime")]
        public decimal OperationTime { get; }

        public OperationResponse(string operationNumber, string text, decimal operationTime)
        {
            OperationNumber = operationNumber;
            Text = text;
            OperationTime = operationTime;
        }
    }
}
