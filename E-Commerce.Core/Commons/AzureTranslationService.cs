using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

public class AzureTranslationService
{
    private readonly string _key;
    private readonly string _endpoint;
    private readonly string _region;
    private readonly HttpClient _httpClient;

    public AzureTranslationService(IConfiguration config)
    {
        var section = config.GetSection("AzureTranslator");
        _key = section["Key"];
        _endpoint = section["Endpoint"];
        _region = section["Region"];

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);
        _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", _region);
    }

    public async Task<string> TranslateAsync(string text, string toLanguage)
    {
        var route = $"/translate?api-version=3.0&to={toLanguage}";
        var body = new object[] { new { Text = text } };
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_endpoint + route, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonResponse);
        var translated = jsonDoc.RootElement[0].GetProperty("translations")[0].GetProperty("text").GetString();

        return translated;
    }
}
