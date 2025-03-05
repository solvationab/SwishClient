using System;

namespace SwishClient.Dto.Payments
{
    public class CreateMcommercePaymentResponse
    {
        public CreateMcommercePaymentResponse(Uri location, string paymentRequestToken)
        {
            if (location == null) 
                throw new ArgumentNullException(nameof(location));

            if (paymentRequestToken == null) 
                throw new ArgumentNullException(nameof(paymentRequestToken));

            Location = location;
            PaymentRequestToken = paymentRequestToken;
        }

        public Uri Location { get; }
        public string PaymentRequestToken { get; }
    }
}