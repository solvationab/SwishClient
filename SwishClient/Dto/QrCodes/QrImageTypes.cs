using System.Runtime.Serialization;

namespace SwishClient.Dto.QrCodes
{
    public enum QrImageTypes
    {
        [EnumMember(Value = "png")]
        Png,

        [EnumMember(Value = "jpg")]
        Jpeg,

        [EnumMember(Value = "svg")]
        Svg
    }
}