﻿using RestSharp;
using System.Text.Json;

namespace LosFusion.CarStore.ServiceLayer.IntegrationTest;

public class ClientCredentialsAuthToken
{
    const string tokenUrl = "https://login.microsoftonline.com/23ccfdc8-db8c-4ab0-83be-70eb4ff1b2ff/oauth2/v2.0/token";
    const string grantType = "client_credentials";
    const string clientId = "3fa1c880-c7b8-4584-b9fb-7a173c285f4a";
    const string scope = "api://5ffa7544-f55b-4560-856f-172d870aa9b6/.default";
    public string access_token = string.Empty;

    /// <summary>
    /// Test token generation from Azure AD.
    /// </summary>
    /// <remarks>Generated by PostMan</remarks>
    public static ClientCredentialsAuthToken? Setup()
    {
        var configRoot = new ConfigurationBuilder().AddUserSecrets("e62105dc-2608-4200-ae21-a184a59b7fad").Build();
        string clientSecret = configRoot["azure-client-secret"];

        var client = new RestClient(tokenUrl) { Timeout = -1 };
        var request = new RestRequest(Method.POST) { AlwaysMultipartFormData = true } ;

        request.AddHeader("Cookie", "esctx=AQABAAAAAAD--DLA3VO7QrddgJg7WevrEVFFiAXDC44dJWbGxNBQTxRN8q7VrcYq56qqYyoojuklDH3sN5C8smgW8p9PwW_Fdg5bg4KZ6X6J0kmiCbt6bIrusErThay3Y8s5I_eAXwMjHK8K2-hUMpYpThRkVHZ7D0KLis_-t2kWck6hFPoTDdqGGoY0uRRTLd1sTjVhFHwgAA; fpc=ApmQSSghxsJBic9y2DyQ4b-YKx5tAQAAAKfDB9gOAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");
        request.AddParameter("grant_type", grantType);
        request.AddParameter("client_id", clientId);
        request.AddParameter("client_secret", clientSecret);
        request.AddParameter("scope", scope);

        IRestResponse response = client.Execute(request);
        ClientCredentialsAuthToken? authToken = JsonSerializer.Deserialize<ClientCredentialsAuthToken>(response.Content);
        return authToken;
    }
}

public class PasswordAuthToken
{
    const string tokenUrl = "https://login.microsoftonline.com/23ccfdc8-db8c-4ab0-83be-70eb4ff1b2ff/oauth2/v2.0/token";
    const string grantType = "password";
    const string clientId = "5ffa7544-f55b-4560-856f-172d870aa9b6";
    const string _scope = "user.read openid profile offline_access";
    const string username = "sdiscover-platform-test-reader@s24hosting.onmicrosoft.com";
    public string token_type = string.Empty;
    public string scope = string.Empty;
    public string expires_in = string.Empty;
    public string ext_expires_in = string.Empty;
    public string access_token = string.Empty;
    public string refresh_token = string.Empty;
    public string id_token = string.Empty;

    /// <summary>
    /// Test token generation from Azure AD.
    /// </summary>
    /// <remarks>Generated by PostMan</remarks>
    public static PasswordAuthToken? Setup()
    {
        var configRoot = new ConfigurationBuilder().AddUserSecrets("e62105dc-2608-4200-ae21-a184a59b7fad").Build();
        string clientSecret = configRoot["azure-password-client-secret"];
        string password = configRoot["azure-password"];

        var client = new RestClient(tokenUrl) { Timeout = -1 };
        var request = new RestRequest(Method.POST) { AlwaysMultipartFormData = true };

        request.AddHeader("Cookie", "fpc=ApmQSSghxsJBic9y2DyQ4b_zYYJ7AQAAAKxKN9gOAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");
        request.AlwaysMultipartFormData = true;
        request.AddParameter("grant_type", grantType);
        request.AddParameter("client_id", clientId);
        request.AddParameter("client_secret", clientSecret);
        request.AddParameter("username", username);
        request.AddParameter("scope", _scope);
        request.AddParameter("password", password);

        IRestResponse response = client.Execute(request);
        PasswordAuthToken? authToken = JsonSerializer.Deserialize<PasswordAuthToken>(response.Content);
        return authToken;
    }
}
