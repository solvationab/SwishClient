using SwishClient.Dto.Payments;
using System.Threading.Tasks;

namespace SwishClient
{
    public interface IPaymentClient
    {
        Task<CreateEcommercePaymentResponse> CreateEcommercePayment(string instructionUuid, CreateEcommercePaymentRequest request);
        Task<CreateMcommercePaymentResponse> CreateMcommercePayment(string instructionUuid, CreateMcommercePaymentRequest request);
        Task<PaymentDto> GetPayment(string id);
        Task<PaymentDto> UpdatePayment(string id, UpdatePaymentRequest request);
    }
}