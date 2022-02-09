// See https://aka.ms/new-console-template for more information


using System.Text.Json;
using IdentityModel.Client;

var client = new HttpClient();
var diso = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (diso.IsError)
{
    Console.WriteLine(diso.Error);
    return;
}


var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
{
    Address = diso.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.AccessToken);


var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);
var response = await apiClient.GetAsync("https://localhost:6001/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc,new JsonSerializerOptions(){WriteIndented = true}));
}

Console.ReadLine();