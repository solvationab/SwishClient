using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwishClient;
using SwishClient.Dto.Payments;
using SwishClient.Dto.QrCodes;
using SwishClient.Extensions;

Console.WriteLine("TestApp started");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder.AddConfiguration(configuration.GetSection("Logging"));

    builder.AddSimpleConsole();

#if DEBUG
    builder.AddDebug();
#endif
});

services.AddSwishClient(configuration);

await using (var serviceProvider = services.BuildServiceProvider())
{
    //var paymentClient = serviceProvider.GetRequiredService<ISwishPaymentClient>();

    //var createEcommercePaymentResponse = await paymentClient.CreateEcommercePayment(
    //    Guid.NewGuid().ToString("N").ToUpperInvariant(),
    //    new CreateEcommercePaymentRequest(
    //        "666",
    //        "https://localhost",
    //        "1234567890",
    //        "1234679304",
    //        100,
    //        "SEK",
    //        "E5F6A9714DC34F83834FB6D46FB31034",
    //        "a message"
    //    )
    //);

    //Console.WriteLine(createEcommercePaymentResponse.Location);

    //var qrCodeClient = serviceProvider.GetRequiredService<ISwishQrCodeClient>();

    //var qrCode = await qrCodeClient.GetPrefilledQrCode(
    //    new GetPrefilledQrCodeRequest(
    //        QrImageTypes.Svg,
    //        new PayeeDto("1234567890", false),
    //        new AmountDto(666, true),
    //        new MessageDto("Hej Hopp", true)
    //        )
    //    );

    //var svgImage = System.Text.Encoding.UTF8.GetString(qrCode);

    //File.WriteAllText("qrCode.svg", svgImage);
}

Console.WriteLine("TestApp finished");
