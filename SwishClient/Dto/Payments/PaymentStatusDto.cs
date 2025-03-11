using System.Text.Json.Serialization;

namespace SwishClient.Dto.Payments
{
    public enum PaymentStatusDto
    {
        [JsonPropertyName("CREATED")]
        Created,

        [JsonPropertyName("PAID")]
        Paid,

        [JsonPropertyName("CANCELLED")]
        Cancelled,

        [JsonPropertyName("ERROR")]
        Error
    }
}