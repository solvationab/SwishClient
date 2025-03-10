using System;
using System.Text.Json.Serialization;

namespace SwishClient.Dto.QrCodes
{
    public class GetPaymentQrCodeRequest
    {
        public GetPaymentQrCodeRequest(
            string token,
            QrImageTypes format,
            int? size = null,
            int? borderWidth = null,
            bool? transparent = false)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));

            if (size.HasValue && size < 300)
                throw new ArgumentException("Size must be at least 300 or null.", nameof(size));

            Token = token;
            Format = format;
            Size = size;
            BorderWidth = borderWidth;
            Transparent = transparent;
        }

        /// <summary>
        /// The Payment Request token to create a QR code for.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; }

        /// <summary>
        /// Possible values: "jpg", "png", "svg".
        /// </summary>
        [JsonPropertyName("format")]
        public QrImageTypes Format { get; }

        /// <summary>
        /// Size of the QR code. The code is a square, so width and height are the same. Not required if the format is svg. Minimum 300
        /// </summary>
        [JsonPropertyName("size")]
        public int? Size { get; }

        /// <summary>
        /// Width of the border.
        /// </summary>
        [JsonPropertyName("border")]
        public int? BorderWidth { get; }

        /// <summary>
        /// Transparent background color. Does not work with jpg format.
        /// </summary>
        [JsonPropertyName("transparent")]
        public bool? Transparent { get; }
    }
}