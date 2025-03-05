using System.Text.Json.Serialization;

namespace SwishClient.Dto.Payments
{
    public class UpdatePaymentRequest
    {
        public UpdatePaymentRequest(string op, string path, string value)
        {
            if (op != "replace")
                throw new System.ArgumentException("Invalid value", nameof(op));

            if (path != "/status")
                throw new System.ArgumentException("Invalid value", nameof(path));

            if (value != "cancelled")
                throw new System.ArgumentException("Invalid value", nameof(value));

            Op = op;
            Path = path;
            Value = value;
        }

        /// <summary>
        /// The operation to perform. Possible values: “replace”
        /// </summary>
        [JsonPropertyName("op")]
        public string Op { get; }

        /// <summary>
        /// Document path. Possible values: "/status"
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; }

        /// <summary>
        /// The new value. Possible values: “cancelled”
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; }
    }
}