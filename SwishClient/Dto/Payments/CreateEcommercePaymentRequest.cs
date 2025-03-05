using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SwishClient.Dto.Payments
{
    /// <summary>
    /// A request to create a new E-commerce payment.
    /// </summary>
    public class CreateEcommercePaymentRequest
    {
        /// <summary>
        /// Create a new E-commerce CreateEcommercePaymentRequest
        /// </summary>
        /// <param name="payeePaymentReference"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="payerAlias"></param>
        /// <param name="payeeAlias"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="callbackIdentifier"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentException"></exception>
        public CreateEcommercePaymentRequest(
            string payeePaymentReference,
            string callbackUrl,
            string payerAlias,
            string payeeAlias,
            decimal amount,
            string currency,
            string callbackIdentifier,
            string message
            )
        {
            if (!Regex.IsMatch(payeePaymentReference, "^[a-zA-Z0-9-]{1,35}$"))
                throw new ArgumentException("Allowed characters are a-z A-Z 0-9 - and len must be 1 to 35", nameof(payeePaymentReference));

            if (!Uri.IsWellFormedUriString(callbackUrl, UriKind.Absolute))
                throw new ArgumentException("Invalid URL", nameof(callbackUrl));

            if (!Regex.IsMatch(payerAlias, @"^\d{8,15}$"))
                throw new ArgumentException("Allowed characters are 0-9 and len must be 8 to 15", nameof(payerAlias));

            if (amount < 0.01m || amount > 999999999999.99m)
                throw new ArgumentException("Value has to be in the range of 0.01 to 999999999999.99", nameof(amount));

            if (currency != "SEK")
                throw new ArgumentException("The only currently supported value is SEK", nameof(currency));

            if (!Regex.IsMatch(message, @"^[a-zA-Z0-9!?(),.-:;]{1,50}$"))
                throw new ArgumentException("Allowed characters are a-z A-Z 0-9 ! ? ( ) , . - : ; and len must be 1 to 50", nameof(message));

            if (callbackIdentifier != null && !Regex.IsMatch(callbackIdentifier, "^[0-9a-zA-Z-]{32,36}$"))
                throw new ArgumentException("Allowed characters are 0-9 a-z A-Z - and len must be 32 to 36", nameof(callbackIdentifier));

            PayeePaymentReference = payeePaymentReference;
            CallbackUrl = callbackUrl;
            PayerAlias = payerAlias;
            PayeeAlias = payeeAlias;
            Amount = amount;
            Currency = currency;
            Message = message;
            CallbackIdentifier = callbackIdentifier;
        }

        /// <summary>
        /// Payment reference of the payee, which is the merchant that receives the payment. This reference could be order id or similar. Allowed characters are a-z A-Z 0-9 - and lenght must be between 1 and 35 characters.
        /// </summary>
        [JsonPropertyName("payeePaymentReference")]
        public string PayeePaymentReference { get; }

        /// <summary>
        /// URL that Swish will use to notify caller about the outcome of the Payment request. The URL has to use HTTPS.
        /// </summary>
        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; }

        /// <summary>
        /// The registered cellphone number of the person that makes the payment. It can only contain numbers and has to be at least 8 and at most 15 numbers. It also needs to match the following format in order to be found in Swish: country code + cellphone number (without leading zero). E.g.: 46712345678
        /// </summary>
        [JsonPropertyName("payerAlias")]
        public string PayerAlias { get; }

        /// <summary>
        /// The Swish number of the payee.
        /// </summary>
        [JsonPropertyName("payeeAlias")]
        public string PayeeAlias { get; }

        /// <summary>
        /// The amount of money to pay. The amount cannot be less than 0.01 SEK and not more than 999999999999.99 SEK. Valid value has to be all numbers or with 2-digit decimal separated by a period.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; }

        /// <summary>
        /// The currency to use. The only currently supported value is SEK.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; }

        /// <summary>
        /// Merchant supplied message about the payment/order. Max 50 characters. Common allowed characters are the letters a-ö, A-Ö, the numbers 0-9, and special characters !?(),.-:;
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; }

        /// <summary>
        /// callbackIdentifier is a field in the request body, consisting of 32-36 alphanumeric characters (validated by the regex ^[0-9a-zA-Z-]{32,36}$). It is not processed and is returned unchanged as an HTTP header named "callbackIdentifier" in the callback. We highly recommend using the callbackIdentifier as a extra validation that the callback is in fact sent by Swish. The callbackIdentifier is never shared outside the communication between the merchant and the Swish server. For best security practice, the value should be a generated uuid for every request, so that the server can be sure that each callback corresponds to a specific transaction.
        /// </summary>
        [JsonPropertyName("callbackIdentifier")]
        public string CallbackIdentifier { get; }
    }
}