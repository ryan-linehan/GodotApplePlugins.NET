# GodotPlayGameServices.NET - Implementation Plan

A C# NuGet package that wraps the [godot-play-game-services](https://github.com/godot-sdk-integrations/godot-play-game-services) plugin (MIT licensed), providing idiomatic C# access to Google Play Game Services in Godot 4.3+.

The package is **fully auto-generated** from the upstream plugin's source code. No hand-written wrappers, no manual maintenance. When upstream releases a new version, we bump a tag and the pipeline regenerates everything.

## The Problem

The upstream plugin is written in GDScript + Kotlin. C# Godot users have no typed, idiomatic way to use Google Play Game Services. Manually writing and maintaining C# wrappers is error-prone and falls behind upstream changes.

## The Solution

A code generator that:

1. **Fetches** upstream GDScript and Kotlin source from GitHub (pinned to a release tag)
2. **Parses** both using ANTLR grammars to produce real ASTs
3. **Merges** the two: GDScript defines the public API surface, Kotlin provides exact types
4. **Emits** `doc_classes/` XML files (the standard GDExtension documentation format)
5. **Generates** C# code from those XML files using a shared codegen library

The result is a NuGet package (`GodotPlayGameServices`) that stays perfectly in sync with upstream.

---

## What is ANTLR?

[ANTLR](https://www.antlr.org/) (ANother Tool for Language Recognition) is a parser generator. You give it a grammar file (`.g4`) that describes a language's syntax, and it generates a lexer and parser in your target language (C#, Java, Python, etc.).

### Why use it instead of regex?

Regex-based parsing is fragile. It breaks on edge cases like:
- Multiline method signatures
- Strings containing keywords (`var x = "func not_a_method()"`)
- Nested structures (enums inside classes)
- Comments that look like code

ANTLR gives you a proper **Abstract Syntax Tree (AST)** — a structured tree representation of the source code. You walk the tree and extract exactly what you need, with zero ambiguity.

### How it works in practice

1. You add a `.g4` grammar file to your project (published grammars exist for both [Kotlin](https://github.com/antlr/grammars-v4/tree/master/kotlin/kotlin) and GDScript)
2. The `Antlr4BuildTasks` NuGet package auto-generates C# lexer/parser classes at build time
3. You write a **Visitor** class — a C# class with methods like `VisitFunctionDeclaration()`, `VisitSignalDeclaration()`, etc.
4. The visitor walks the AST and you extract the data you care about into your own models

### Example

Given this GDScript:
```gdscript
## Unlocks the given achievement.
func unlock_achievement(achievement_id: String) -> void:
    if GodotPlayGameServices.android_plugin:
        GodotPlayGameServices.android_plugin.unlockAchievement(achievement_id)
```

ANTLR parses it into an AST. Your visitor extracts:
```
Method {
    Name: "unlock_achievement"
    ReturnType: "void"
    Parameters: [ { Name: "achievement_id", Type: "String" } ]
    DocComment: "Unlocks the given achievement."
}
```

No regex needed. No fragile string splitting.

### NuGet packages needed

| Package | License | Purpose |
|---------|---------|---------|
| `Antlr4.Runtime.Standard` | MIT | Runtime library for generated parsers |
| `Antlr4BuildTasks` | MIT | MSBuild task that generates C# from `.g4` at build time |

These are only used by the generator tool project — they do **not** end up in the published NuGet package.

### Performance impact

Negligible. ANTLR parsing ~40 small source files completes in milliseconds. The NuGet restore is cached after the first build. The grammar-to-C# codegen happens once and is cached by MSBuild.

---

## Repo Structure

```
GodotPlayGameServices.NET/
├── src/
│   ├── GodotPlayGameServices.Generator/       # The generator tool
│   │   ├── GodotPlayGameServices.Generator.csproj
│   │   ├── Grammars/
│   │   │   ├── GDScript.g4                    # GDScript ANTLR grammar
│   │   │   ├── KotlinLexer.g4                 # Kotlin ANTLR grammar (lexer)
│   │   │   └── KotlinParser.g4                # Kotlin ANTLR grammar (parser)
│   │   ├── Parsing/
│   │   │   ├── GdScriptVisitor.cs             # Walks GDScript AST, extracts public API
│   │   │   ├── KotlinVisitor.cs               # Walks Kotlin AST, extracts types + models
│   │   │   └── ApiMerger.cs                   # Merges GDScript API surface with Kotlin types
│   │   ├── XmlEmitter.cs                      # Outputs doc_classes XML from merged model
│   │   ├── CodeGenerator.cs                   # Generates C# from XML (shared with Apple plugin)
│   │   ├── TypeMapper.cs                      # Godot type → C# type mapping
│   │   └── Program.cs                         # CLI entry point
│   │
│   └── GodotPlayGameServices.NET/             # The generated NuGet package
│       ├── GodotPlayGameServices.NET.csproj
│       └── Generated/                         # All C# code in here is auto-generated
│           ├── SignIn/
│           ├── Achievements/
│           ├── Leaderboards/
│           ├── Players/
│           ├── Snapshots/
│           └── Events/
│
├── doc_classes/                               # Intermediate XML (auto-generated, checked in)
│   ├── PlayGamesAchievementsClient.xml
│   ├── PlayGamesAchievement.xml
│   ├── PlayGamesLeaderboardsClient.xml
│   └── ...
│
├── .github/
│   └── workflows/
│       ├── generate.yml                       # CI: fetch upstream → parse → generate → build
│       ├── check-upstream.yml                 # Scheduled: detect new upstream releases
│       └── publish.yml                        # Publish NuGet package on release
│
└── GOOGLE_PLAY_PLAN.md                        # This file
```

---

## What Gets Parsed

### GDScript Files (21 files)

Located at `plugin/export_scripts_template/scripts/` in the upstream repo.

**Client classes** (define the public API surface):
- `sign_in/sign_in_client.gd`
- `achievements/achievements_client.gd`
- `leaderboards/leaderboards_client.gd`
- `players/players_client.gd`
- `snapshots/snapshots_client.gd`
- `events/events_client.gd`

**Data model classes** (define properties, enums):
- `achievements/achievement.gd`
- `leaderboards/leaderboard.gd`, `leaderboard_score.gd`, `leaderboard_scores.gd`, `leaderboard_variant.gd`
- `players/player.gd`, `player_level.gd`, `player_level_info.gd`
- `snapshots/snapshot.gd`, `snapshot_metadata.gd`, `snapshot_conflict.gd`, `game.gd`
- `events/event.gd`

**What we extract from GDScript:**
- `class_name` → C# class name
- `extends` → base class
- `signal name(params)` → C# events
- `func name(params) -> ReturnType:` → C# methods
- `var name: Type` → C# properties
- `enum Name { VALUES }` → C# enums
- `##` doc comments → XML `<description>` tags

### Kotlin Files (18 files)

Located at `plugin/src/main/java/com/jacobibanez/plugin/android/godotplaygameservices/` in the upstream repo.

**What we extract from Kotlin:**
- `SignalInfo` declarations in `signals/Signals.kt` → exact signal parameter types (the GDScript signals often lose type info, e.g. signals that pass JSON strings)
- `data class` fields in mapper files → exact property types for data models (e.g. `val xpValue: Long` tells us the Godot `int` is actually a 64-bit value)
- Enum definitions in `Enums.kt` files → enum values and their integer mappings
- KDoc comments → additional documentation

---

## The Two-Pass Merge Strategy

The key insight: **GDScript tells us WHAT to expose, Kotlin tells us the exact types.**

### Example: Achievement signal

**GDScript says:**
```gdscript
signal achievement_unlocked(is_unlocked: bool, achievement_id: String)
```
This gives us the signal name, parameter names, and types. In this case GDScript has full type info.

**Kotlin confirms:**
```kotlin
val achievementUnlocked = SignalInfo("achievementUnlocked", Boolean::class.javaObjectType, String::class.java)
```
Same types. The merge is trivial — GDScript wins for naming (snake_case), Kotlin confirms types.

### Example: Data model property

**GDScript says:**
```gdscript
var xp_value: int
```
Type is `int` — but is that 32-bit or 64-bit?

**Kotlin says:**
```kotlin
put("xpValue", achievement.xpValue)  // xpValue is a Long in the Google SDK
```
Now we know it should be `long` in C#.

### The merge algorithm

```
For each GDScript class:
    1. Parse all signals, methods, properties, enums from GDScript
    2. Find the corresponding Kotlin file(s) by matching:
       - Client class → Proxy class (e.g. achievements_client.gd → AchievementsProxy.kt)
       - Data model → Mapper class (e.g. achievement.gd → AchievementMapper.kt)
       - Signals → signals/Signals.kt
    3. For each signal/method/property:
       - Use GDScript name (snake_case, user-facing)
       - Use GDScript doc comment
       - Cross-reference Kotlin for exact types where GDScript is ambiguous
    4. Emit merged result as doc_classes XML
```

### Matching heuristic

The method and signal names are nearly identical between GDScript and Kotlin — just different casing:
- GDScript: `unlock_achievement` → Kotlin: `unlockAchievement`
- GDScript: `achievement_unlocked` signal → Kotlin: `achievementUnlocked` signal

A simple `snake_case ↔ camelCase` conversion maps them 1:1.

---

## Generator CLI

```bash
# Full pipeline: fetch → parse → generate XML → generate C#
dotnet run --project src/GodotPlayGameServices.Generator -- generate --tag v3.2.0

# Just fetch upstream source
dotnet run --project src/GodotPlayGameServices.Generator -- fetch --tag v3.2.0

# Just parse and emit XML (assumes source already fetched)
dotnet run --project src/GodotPlayGameServices.Generator -- parse

# Just generate C# from existing XML
dotnet run --project src/GodotPlayGameServices.Generator -- codegen
```

---

## Output: What the C# Package Looks Like

The generated C# should feel native to C# developers:

```csharp
// GDScript signal → C# event
public event Action<bool, string>? AchievementUnlocked;

// GDScript func → C# method
public void UnlockAchievement(string achievementId) { ... }

// GDScript data class → C# record/class with properties
public class PlayGamesAchievement
{
    public string AchievementId { get; set; }
    public string AchievementName { get; set; }
    public PlayGamesPlayer Player { get; set; }
    public AchievementType Type { get; set; }
    public AchievementState State { get; set; }
    public long XpValue { get; set; }
    // ...
}

// GDScript enum → C# enum
public enum AchievementType
{
    TypeStandard = 0,
    TypeIncremental = 1
}
```

---

## CI/CD Pipeline

### `generate.yml` — Build and validate

Triggered on: push to main, PR

```
1. Checkout repo
2. dotnet restore (caches ANTLR packages)
3. dotnet run -- generate --tag <pinned-tag>
4. dotnet build src/GodotPlayGameServices.NET/
5. Compare generated files against checked-in files (fail if drift detected on main)
```

### `check-upstream.yml` — Detect new upstream releases

Triggered on: schedule (weekly) or manual

```
1. Query GitHub API for latest release of godot-sdk-integrations/godot-play-game-services
2. Compare against pinned tag in repo
3. If newer version exists:
   a. Run generator with new tag
   b. Open PR with updated generated code
   c. Include diff summary in PR description
```

### `publish.yml` — Publish NuGet

Triggered on: GitHub release created

```
1. dotnet pack
2. dotnet nuget push to nuget.org
```

---

## Shared Codegen with GodotApplePlugins.NET

The Apple plugin generator (`GodotApplePlugins.Generator`) already has:
- `XmlDocParser` — parses doc_classes XML into models
- `CodeGenerator` — generates C# from those models
- `TypeMapper` — maps Godot types to C# types

These can be extracted into a shared library or simply copied. For now, **copying is fine** — the codegen is ~500 lines and the two plugins may diverge in their C# output style. We can extract a shared NuGet later if the codegen stabilizes.

---

## Upstream Source Reference

| Item | Value |
|------|-------|
| **Repo** | [godot-sdk-integrations/godot-play-game-services](https://github.com/godot-sdk-integrations/godot-play-game-services) |
| **License** | MIT |
| **Latest release** | v3.2.0 (November 2024) |
| **Godot version** | 4.3+ |
| **GDScript files** | 21 files in `plugin/export_scripts_template/scripts/` |
| **Kotlin files** | 18 files in `plugin/src/main/java/.../godotplaygameservices/` |
| **Total API surface** | ~7 client classes, ~8 data models, ~40 methods, ~20 signals |

---

## Implementation Order

1. **Set up repo** — .csproj, ANTLR packages, grammar files
2. **GDScript parser** — visitor that extracts classes, methods, signals, properties, enums, doc comments
3. **Kotlin parser** — visitor that extracts signal types, data class fields, enum values
4. **API merger** — combines GDScript surface with Kotlin types
5. **XML emitter** — outputs doc_classes XML
6. **C# code generator** — generates the NuGet package source (port from Apple plugin)
7. **CLI program** — ties it all together with fetch/parse/generate commands
8. **CI/CD** — GitHub Actions for build, upstream checking, NuGet publishing
