using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwishClient;
using SwishClient.Dto.Payments;
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
    var paymentClient = serviceProvider.GetRequiredService<ISwishPaymentClient>();

    var instructionUuid = Guid.NewGuid().ToString("N").ToUpperInvariant();

    var createEcommercePaymentResponse = await paymentClient.CreateEcommercePayment(
        instructionUuid,
        new CreateEcommercePaymentRequest(
            "666",
            "https://localhost",
            "1234567890",
            "1234679304",
            100,
            "SEK",
            "E5F6A9714DC34F83834FB6D46FB31034",
            "a message"
        )
    );

    Console.WriteLine(createEcommercePaymentResponse.Location);
}

Console.WriteLine("TestApp finished");
