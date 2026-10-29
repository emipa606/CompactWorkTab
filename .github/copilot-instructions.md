# GitHub Copilot Instructions for Compact Work Tab (Continued)

## Mod Overview and Purpose
Compact Work Tab (Continued) is a mod for the popular RimWorld game that aims to improve the readability and usability of the Work tab. The mod achieves this by rotating the column headers by 60° and providing the option for vertically oriented column headers. Additionally, it includes scroll-wheel support for adjusting work priorities.

## Key Features and Systems
- **Rotated Column Headers**: Rotates the column headers by 60° to make the Work tab more usable.
- **Vertical Column Headers**: Provides the option for vertically oriented column headers for an even more compact view.
- **Scroll-Wheel Support**: Enables incrementing or decrementing work priorities via scroll wheel.
- **Mod Settings**: All settings can be adjusted through RimWorld's mod options menu.
- **Translation Support**: Added translation support for internationalization.

## Coding Patterns and Conventions
- **C# Code Organization**: The mod is organized into several C# files, each responsible for a specific feature or system.
  - `Cache.cs`: Contains caching mechanisms for performance optimization.
  - `PriorityMaster.cs`: Manages priority settings and compatibility with other mods.
  - `ModSettings.cs`: Handles mod settings and their UI representation.
  - `Patches/PawnColumnWorker_RemainingSpace_GetMinWidth.cs`: Harmony patch for adjusting column widths.
  - `Patches/PawnColumnWorker_WorkPriority_DoCell.cs`: Harmony patch for handling work priority cells.
- **XML Integration**: XML files are used for defining compatibility information and updates.
  - `PawnTableGrouped.CompatibilityInfoDef.xml`: Defines compatibility with other mods.
  - `UpdateInfo.xml`: Contains update information for the mod.
- **Harmony Patching**: The mod uses Harmony for patching RimWorld's internal methods to implement its features.
  - `PawnColumnWorker_RemainingSpace_GetMinWidth.cs`: Patches the method to adjust column widths.
  - `PawnColumnWorker_WorkPriority_DoCell.cs`: Patches the method to handle work priority cells.

## Suggestions for Copilot
- **C# Code**: When writing C# code, consider the following patterns:
  - Use descriptive variable and method names.
  - Implement caching mechanisms where appropriate.
  - Use Harmony patches for modifying RimWorld's internal methods.
- **XML Files**: When writing XML files, ensure that they conform to the RimWorld XML schema.
  - Define compatibility information in `CompatibilityInfoDef.xml`.
  - Include update information in `UpdateInfo.xml`.
- **Mod Settings**: When implementing mod settings, ensure that they are accessible via RimWorld's mod options menu.
- **Translation Support**: When adding translation support, ensure that all strings are translatable and provide translations for other languages if possible.

## Example C# Code
csharp
public class Cache
{
    public static Dictionary<string, string> GetVerticalRotated(string text)
    {
        // Implementation for vertical text rotation
    }

    public static void Recache()
    {
        // Implementation for recaching data
    }
}


## Example XML File
xml
<CompatibilityInfoDef>
    <modId>Mlie.CompactWorkTab</modId>
    <compatibility>
        <mod>Mlie.PriorityMaster</mod>
        <mod>Mlie.GroupedPawnsLists</mod>
    </compatibility>
</CompatibilityInfoDef>


By following these guidelines, you can ensure that your contributions to the Compact Work Tab (Continued) mod are consistent and effective.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

