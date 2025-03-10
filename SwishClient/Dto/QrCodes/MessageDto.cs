using System;
using System.Text.Json.Serialization;

namespace SwishClient.Dto.QrCodes
{
    public class MessageDto
    {
        public MessageDto(string value, bool editable)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            Value = value;
            Editable = editable;
        }

        /// <summary>
        /// Message for payment. Max 50 characters. Common allowed characters are the letters a-ö, A-Ö, the numbers 0-9, and special characters !?(),.-:;
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; }

        /// <summary>
        /// If the value can be changed in the app after being scanned
        /// </summary>
        [JsonPropertyName("editable")]
        public bool Editable { get; }
    }
}