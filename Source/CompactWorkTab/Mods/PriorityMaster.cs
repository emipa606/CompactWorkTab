using System.Collections.Generic;
using Verse;

namespace CompactWorkTab.Mods;

internal static class PriorityMaster
{
    private const string ModTypeName = "PriorityMod.Core.PriorityMaster";
    private const string SettingsFieldName = "settings";
    private const string GetMaxPriorityMethodName = "GetMaxPriority";
    private const string GetDefPriorityMethodName = "GetDefPriority";
    private static readonly List<string> PackageIds = ["Lauriichen.PriorityMod", "Lauriichan.PriorityMaster"];

    private static object modSettings;

    private static object PriorityMasterModSettings
    {
        get
        {
            if (modSettings != null || !PackageIds.Any(ModsConfig.IsActive))
            {
                return modSettings;
            }

            var modType = GenTypes.GetTypeInAnyAssembly(ModTypeName);
            modSettings = modType?.GetField(SettingsFieldName).GetValue(LoadedModManager.GetMod(modType));
            return modSettings;
        }
    }

    public static int? MaxPriority
    {
        get
        {
            var method = PriorityMasterModSettings?.GetType().GetMethod(GetMaxPriorityMethodName);
            var value = (int?)method?.Invoke(PriorityMasterModSettings, null);
            return value;
        }
    }

    public static int? DefaultPriority
    {
        get
        {
            var method = PriorityMasterModSettings?.GetType().GetMethod(GetDefPriorityMethodName);
            var value = (int?)method?.Invoke(PriorityMasterModSettings, null);
            return value;
        }
    }
}