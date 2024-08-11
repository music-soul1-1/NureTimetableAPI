using JsonRepairUtils;

namespace NureTimetableAPI.Helpers;

public class JsonRepairer
{
    /// <summary>
    /// Fixes the provided JSON.
    /// </summary>
    /// <param name="brokenJson"></param>
    /// <returns></returns>
    public static string RepairJson(string brokenJson)
    {
        var jsonRepair = new JsonRepair();

        var repairedJson = jsonRepair.Repair(brokenJson);

        return repairedJson;
    }
}
