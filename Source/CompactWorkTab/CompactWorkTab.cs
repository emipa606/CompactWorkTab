using System.Reflection;
using HarmonyLib;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompactWorkTab;

public class CompactWorkTab : Mod
{
    public static string CurrentVersion;

    public static readonly FieldInfo DefFieldInfo = AccessTools.Field(typeof(PawnTable), "def");
    private readonly ModSettings settings;

    public CompactWorkTab(ModContentPack content) : base(content)
    {
        settings = GetSettings<ModSettings>();

        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);

        new Harmony(content.PackageId).PatchAll(Assembly.GetExecutingAssembly());
    }

    public override string SettingsCategory()
    {
        return Content.Name;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        ModSettings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }
}