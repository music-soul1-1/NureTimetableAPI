using JsonRepairUtils;

namespace NureTimetableAPI.Helpers;

public class JsonRepairer
{
    /// <summary>
    /// Fixes the provided JSON.
    /// </summary>
    /// <param name="brokenJson"></param>
    /// <returns></returns>
    public static string RepairJson(string brokenJson, bool throwErrors = false)
    {
        var jsonRepair = new JsonRepair
        { 
            ThrowExceptions = throwErrors
        };

        var repairedJson = jsonRepair.Repair(brokenJson);

        return repairedJson;
    }
}
