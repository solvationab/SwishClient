using System.Threading.Tasks;
using SwishClient.Dto.QrCodes;

namespace SwishClient
{
    public interface ISwishQrCodeClient
    {
        Task<byte[]> GetPaymentQrCode(GetPaymentQrCodeRequest request);
        Task<byte[]> GetPrefilledQrCode(GetPrefilledQrCodeRequest request);
    }
}