using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SwishClient.Dto.QrCodes
{
    public class PayeeDto
    {
        public PayeeDto(string value, bool editable)
        {
            if (!Regex.IsMatch(value, "^[a-zA-Z0-9-]{1,36}$"))
                throw new ArgumentException("Allowed characters are a-z A-Z 0-9 - and len must be 1 to 36", nameof(value));

            Value = value;
            Editable = editable;
        }

        /// <summary>
        /// Payment receiver. Max 36 characters. Common allowed characters are the letters a-ö, A-Ö, the numbers 0-9, and special characters -.
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