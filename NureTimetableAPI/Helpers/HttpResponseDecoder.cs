using Newtonsoft.Json;
using System.Text;

namespace NureTimetableAPI.Helpers;

/// <summary>
/// Allows to decode HTTP responses from Cist.
/// </summary>
public class HttpResponseDecoder
{
    /// <summary>
    /// Converts the response to a string.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="encoding">
    /// Cist API uses WINDOWS-1251 encoding.
    /// </param>
    /// <returns></returns>
    public async static Task<string> ConvertToString(HttpResponseMessage response, string encoding = "WINDOWS-1251")
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream, Encoding.GetEncoding(encoding), true);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Deserializes the response to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response"></param>
    /// <param name="encoding">
    /// Cist API uses WINDOWS-1251 encoding.
    /// </param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async static Task<T?> DeserializeResponse<T>(HttpResponseMessage response, string encoding = "WINDOWS-1251")
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(await ConvertToString(response, encoding));
        }
        catch(Exception e)
        {
            throw new Exception($"Failed to deserialize response: {e.Message}");
            //return default;
        }
    }
}
