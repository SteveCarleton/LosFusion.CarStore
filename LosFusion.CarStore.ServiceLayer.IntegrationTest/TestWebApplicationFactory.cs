using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Moq;
using Moq.Protected;

namespace LosFusion.CarStore.ServiceLayer.IntegrationTest;

public class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    const string baseAddress = "http://localhost:5000";

    public TestWebApplicationFactory()
    {
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        handlerMock
            .Protected()
            // Setup the PROTECTED method to mock.
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            // Prepare the expected response of the mocked http call.
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{'id':1,'value':'1'}]"),
            })
            .Verifiable();
        ;

        var builder = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            })
            .ConfigureAppConfiguration((configBuilder) =>
            {
                configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configBuilder.AddJsonFile("appsettings.development.json", optional: false);
                // Simulate with User Secrets
                configBuilder.AddUserSecrets("e62105dc-2608-4200-ae21-a184a59b7fad");
                configBuilder.AddEnvironmentVariables();

                var settings = configBuilder.Build();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        builder.ConfigureLogging(b =>
        {
            b.SetMinimumLevel(LogLevel.None);
        });

        return builder;
    }
}


public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "sdiscover-platform-test-reader"),
        };

        var identity = new ClaimsIdentity(claims, "");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");
        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
