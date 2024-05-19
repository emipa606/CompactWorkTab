using Verse;

namespace CompactWorkTab.Mods;

internal static class PriorityMaster
{
    private const string PackageId = "lauriichen.PriorityMod";
    private const string ModTypeName = "PriorityMod.Core.PriorityMaster";
    private const string SettingsFieldName = "settings";
    private const string GetMaxPriorityMethodName = "GetMaxPriority";
    private const string GetDefPriorityMethodName = "GetDefPriority";

    private static object _modSettings;

    private static object PriorityMasterModSettings
    {
        get
        {
            if (_modSettings != null || !ModsConfig.IsActive(PackageId))
            {
                return _modSettings;
            }

            var modType = GenTypes.GetTypeInAnyAssembly(ModTypeName);
            _modSettings = modType?.GetField(SettingsFieldName).GetValue(LoadedModManager.GetMod(modType));
            return _modSettings;
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