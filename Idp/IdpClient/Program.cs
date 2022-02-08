// See https://aka.ms/new-console-template for more information


using System.Text.Json.Nodes;
using IdentityModel.Client;
using IdentityServer4.Models;

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");

if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
{
    Address = disco.TokenEndpoint,
    ClientId = "console client",
    ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
    Scope = "scope1"
});
if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);
var response = await apiClient.GetAsync("http://localhost:6001/identity");

if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JsonArray.Parse(content));
}
Console.WriteLine(tokenResponse.Json);
Console.ReadLine();

// public  static IEnumerable<Client> Clients=