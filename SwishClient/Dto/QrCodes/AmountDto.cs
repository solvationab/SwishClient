using System;
using System.Text.Json.Serialization;

namespace SwishClient.Dto.QrCodes
{
    public class AmountDto
    {
        public AmountDto(decimal value, bool editable)
        {
            if (value <= 0)
                throw new ArgumentException("Value must be greater than 0", nameof(value));

            Value = value;
            Editable = editable;
        }

        /// <summary>
        /// Payment amount. Max 12 digits, 2 decimals. The value must be greater than 0.
        /// </summary>
        [JsonPropertyName("value")]
        public decimal Value { get; }

        /// <summary>
        /// If the value can be changed in the app after being scanned
        /// </summary>
        [JsonPropertyName("editable")]
        public bool Editable { get; }
    }
}