namespace NureTimetableAPI.Jobs;

public class KeepAliveJob
{
    private static readonly HttpClient _httpClient = new();

    public async Task Execute()
    {
        await _httpClient.GetAsync("https://nure-time.runasp.net/keepalive");
    }
}
