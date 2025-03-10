using System;
using System.Text.Json.Serialization;

namespace SwishClient.Dto.QrCodes
{
    public class GetPrefilledQrCodeRequest
    {
        public GetPrefilledQrCodeRequest(
            QrImageTypes format,
            PayeeDto payee,
            AmountDto amount,
            MessageDto message,
            int? size = null,
            int? borderWidth = null,
            bool? transparent = null
        )
        {
            if (payee == null && amount == null && message == null)
                throw new ArgumentException("At least one of the following properties must be set: payee, amount, message", nameof(payee));

            if (size.HasValue && size < 300)
                throw new ArgumentException("Size must be at least 300 or null.", nameof(size));

            Format = format;
            Payee = payee;
            Amount = amount;
            Message = message;
            Size = size;
            BorderWidth = borderWidth;
            Transparent = transparent;
        }

        /// <summary>
        /// Format of the response image, can be jpg, png or svg
        /// </summary>
        [JsonPropertyName("format")]
        public QrImageTypes Format { get; }

        /// <summary>
        /// Payment receiver
        /// </summary>
        [JsonPropertyName("payee")]
        public PayeeDto Payee { get; }

        /// <summary>
        /// Payment amount
        /// </summary>
        [JsonPropertyName("amount")]
        public AmountDto Amount { get; }

        /// <summary>
        /// Message for payment.
        /// </summary>
        [JsonPropertyName("message")]
        public MessageDto Message { get; }

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