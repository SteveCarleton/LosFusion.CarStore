using Microsoft.AspNetCore.TestHost;
using System.Text;
//using System.Net.Http.Headers;
using System.Text.Json;

namespace LosFusion.CarStore.ServiceLayer.IntegrationTest;

public class TestWebHelpers
{
    const string baseAddress = "http://localhost:5038";
    readonly PasswordAuthToken? authToken;
    readonly Type testInterfaceType = default!;
    readonly Type testMockType = default!;

    JsonSerializerOptions jsonSerializationOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public TestWebHelpers()
    {
        authToken ??= PasswordAuthToken.Setup();
    }

    public TestWebHelpers(Type interfaceType, Type mockType)
    {
        testInterfaceType = interfaceType;
        testMockType = mockType;
        authToken ??= PasswordAuthToken.Setup();
    }


    public async Task<string> GetTestHelper(string route, int? id = null)
    {
        using (var factory = new TestWebApplicationFactory())
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(testInterfaceType, testMockType);
                });
            }).CreateClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.id_token);
            httpClient.BaseAddress = new Uri(baseAddress);
            var response = await httpClient.GetAsync($"{route}/{id}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }

    public async Task<string> GetByYearTestHelper(string route, int year)
    {
        using (var factory = new TestWebApplicationFactory())
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(testInterfaceType, testMockType);
                });
            }).CreateClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.id_token);
            httpClient.BaseAddress = new Uri(baseAddress);

            var response = await httpClient.GetAsync($"{route}/GetByYear/{year}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }

    public async Task<string> PostTestHelper<T>(string route, T payload)
    {
        using (var factory = new TestWebApplicationFactory())
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(testInterfaceType, testMockType);
                });
            }).CreateClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.id_token);
            httpClient.BaseAddress = new Uri(baseAddress);


            string json = JsonSerializer.Serialize(payload, jsonSerializationOptions);

            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{route}", requestContent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }

    public async Task<string> PutTestHelper<T>(string route, int id, T payload)
    {
        using (var factory = new TestWebApplicationFactory())
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(testInterfaceType, testMockType);
                });
            }).CreateClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.id_token);
            httpClient.BaseAddress = new Uri(baseAddress);
            string json = JsonSerializer.Serialize(payload, jsonSerializationOptions);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{route}/{id}", requestContent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }

    public async Task<string> PatchTestHelper(string route, int id, StringContent requestContent)
    {
        using (var factory = new TestWebApplicationFactory())
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(testInterfaceType, testMockType);
                });
            }).CreateClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.id_token);
            httpClient.BaseAddress = new Uri(baseAddress);

            //string json = JsonSerializer.Serialize(payload, jsonSerializationOptions);
            //var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync($"{route}/{id}", requestContent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }

    public async Task<string> DeleteTestHelper<T>(string route, int id)
    {
        using (var factory = new TestWebApplicationFactory())
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(testInterfaceType, testMockType);
                });
            }).CreateClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.id_token);
            httpClient.BaseAddress = new Uri(baseAddress);

            var response = await httpClient.DeleteAsync($"{route}/{id}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
    }
}
