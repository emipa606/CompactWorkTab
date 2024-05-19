using System.Reflection;
using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace CompactWorkTab;

public class CompactWorkTab : Mod
{
    public static string currentVersion;
    private readonly ModSettings _settings;

    public CompactWorkTab(ModContentPack content) : base(content)
    {
        _settings = GetSettings<ModSettings>();

        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);

        new Harmony(content.PackageId).PatchAll(Assembly.GetExecutingAssembly());
    }

    public override string SettingsCategory()
    {
        return Content.Name;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        _settings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }
}