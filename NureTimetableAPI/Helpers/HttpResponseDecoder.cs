using Newtonsoft.Json;
using System.Text;

namespace NureTimetableAPI.Helpers;

/// <summary>
/// Allows to decode HTTP responses from Cist.
/// </summary>
public class HttpResponseDecoder
{
    /// <summary>
    /// Converts the response to a string and fixes the JSON.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="encoding">
    /// Cist API uses WINDOWS-1251 encoding.
    /// </param>
    /// <returns></returns>
    public async static Task<string> ConvertToString(HttpResponseMessage response, bool repairJson = true, bool throwErrors = false, string encoding = "WINDOWS-1251")
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream, Encoding.GetEncoding(encoding), true);
        var json = await reader.ReadToEndAsync();

        if (repairJson)
        {
            json = JsonRepairer.RepairJson(json, throwErrors);
        }

        stream.Dispose();
        reader.Dispose();

        return json;
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
    public async static Task<T?> DeserializeResponse<T>(HttpResponseMessage response, bool repairJson = true, bool throwErrors = false, string encoding = "WINDOWS-1251")
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(await ConvertToString(response, repairJson, throwErrors, encoding));
        }
        catch(Exception e)
        {
            throw new Exception($"Failed to deserialize response: {e.Message}");
        }
    }

    /// <summary>
    /// Deserializes the JSON object to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T? DeserializeResponse<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to deserialize response: {e.Message}");
        }
    }
}
