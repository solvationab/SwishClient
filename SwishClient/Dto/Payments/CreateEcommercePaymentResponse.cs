using System;

namespace SwishClient.Dto.Payments
{
    public class CreateEcommercePaymentResponse
    {
        public CreateEcommercePaymentResponse(Uri location)
        {
            if (location == null) 
                throw new ArgumentNullException(nameof(location));

            Location = location;
        }

        public Uri Location { get; }
    }
}