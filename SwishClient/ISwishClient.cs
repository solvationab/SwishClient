using System.Threading.Tasks;
using Refit;
using SwishClient.Dto.PaymentRequests;

namespace SwishClient
{
    public interface ISwishClient
    {
        #region PaymentRequests

        [Put("/api/v2/paymentrequests/{instructionUUID}")]
        Task<IApiResponse> CreatePaymentRequest(
            [AliasAs("instructionUUID")] string instructionUuid,
            [Body(BodySerializationMethod.Serialized)] SwishPaymentRequestDto paymentRequestDto);

        [Get("/api/v1/paymentrequests/{id}")]
        Task<SwishPaymentRequestDto> GetPaymentRequest(
            [AliasAs("id")] string id);

        [Patch("/api/v1/paymentrequests/{id}")]
        Task<SwishPaymentRequestDto> UpdatePaymentRequest(
            [AliasAs("id")] string id, 
            [Body(BodySerializationMethod.Serialized)] SwishPaymentRequestDto paymentRequestDto);

        #endregion
    }
}
