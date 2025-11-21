# (Better) Webfishing Mod Template

A (better) example of [GDWeave](https://github.com/NotNite/GDWeave) Webfishing mods.

## Project and GDWeave patching overview

This repository is a developer-friendly replacement for the _old_ vanilla one that Nite
threw together for GDWeave, which has no actual patches being applied in the example and
is entirely devoid of documentation.

I know it's a bit sparse for now; I'll try to document this section more thoroughly in time!

## TokenBuilders, PatternFactories, and SnippetBuilders, Oh My

This project includes an abstraction layer for working with
GDWeave's Tokens, written primarily by [Teemaw](https://teemaw.dev) but _until now_
never exported and made available for reuse in other projects. Without this, mods/patches
need to be written in tedium, an error-prone and monotonous way:

```cs
{
    internal class PlayerScriptMod : IScriptMod
    {
        public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";
        public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
        {
            // scale = clamp(animation_data["player_scale"], 0.6, 1.4) * Vector3.ONE
            var clampMatcher = new MultiTokenWaiter([
                t => t is IdentifierToken { Name: "scale" },
                t => t.Type is TokenType.OpAssign,
                t => t.Type is TokenType.BuiltInFunc, // clamp
                t => t.Type is TokenType.ParenthesisOpen,
                t => t is IdentifierToken { Name: "animation_data" },
                t => t.Type is TokenType.BracketOpen,
                t => t.Type is TokenType.Constant, // player_scale
                t => t.Type is TokenType.BracketClose,
                t => t.Type is TokenType.Comma,
                t => t.Type is TokenType.Constant, // 0.6
            ], allowPartialMatch: false);
            foreach (var token in tokens)
            {
                if (clampMatcher.Check(token))
                {
                    // -2.0, 10.0
                    yield return new ConstantToken(new RealVariant(-2.0));
                    yield return new Token(TokenType.Comma);
                    yield return new ConstantToken(new RealVariant(10.0));
                }
                // [...]
                else
                {
                    // return the original token
                    yield return token;
                }
            }
        }
    }
}
```

Instead this mod could be rewritten within this template as:

```cs
mi.RegisterScriptMod(
   new TransformationRuleScriptModBuilder()
    .ForMod(psmod)
    .Named("Clamp")
    .Patching("res://Scenes/Entities/Player/player.gdc")
    .AddRule(
     new TransformationRuleBuilder()
      .Named("Expand clamp range")
      .Do(Operation.Append)
      .Matching(
       TransformationPatternFactory.CreateGdSnippetPattern(
        """
        scale = clamp(animation_data["player_scale"],
        """
       )
      )
      .With("-2.0, 10.0")
                    )
                // [...]
    .Build()
  )
```

## Examples in this project

I tried to include _many_ example patches in a variety of styles to showcase as much as I could without potentially overwhelming
the reader. For what it's worth, a typical mod might never get this large so don't feel like you've done something wrong if your mod
file ends up being only 50 or so lines-- that's totally normal!

## Building

To build the project, you need to set the GDWeavePath environment variable to your game install's GDWeave directory
(e.g. G:\games\steam\steamapps\common\WEBFISHING\GDWeave).
This can also be done in Rider with File | Settings | Build, Execution, Deployment | Toolset and Build | MSBuild global properties.
You can also achieve this with a .user file, in Visual Studio.

## Testing the DLL

> [!WARNING]
> If you run into snags at runtime with error messages about unknown identifiers which are names of built-in functions (such as `randf` or `clamp`)
please [file an issue](https://github.com/binury/Toes.Tuner/issues) and let me know so I can add the missing token to the list!

> [!TIP]
> I recommend using R2ModManager to manage your mod profiles, and that you create a separate `TEST` profile that only includes your mod
and its dependencies (if any).

After you've built your project you can copy over the manifest file along with the dll into a new folder within your mods directory.
I have [included a script](./webfishing-debugging-mode.bat) you can use to launch the game with GDWeave's console open.

> [!IMPORTANT]
> You won't see as much as you'd expect in the GDWeave log while testing/troubleshooting. You should set your system's environment variables to include
`GDWEAVE_DEBUG` and `GDWEAVE_CONSOLE`, and set their values as 1. When you're done
testing and ready to play the game as usual, you can delete these env vars.
_**Or**_, use the [included script](./webfishing-debugging-mode.bat)

### (Optional) Reviewing the output of your patches

Sometimes it is helpful while debugging to be able to see exactly what your patches did (or did not do), and if it is what you were expecting. This is one of the trickier parts of mod making. It is impossible to speak generally about this process but I will try to at least point you in the right direction.

In order to "dump" the syntax tree tokens into a `gdc` file, you need to run the game with the `GDWEAVE_DUMP_GDSC` environment variable enabled.
I'll assume that you have modified the [included script](./webfishing-debugging-mode.bat) and added a line that says `GDWEAVE_DUMP_GDSC=1`

Run the batch file and let the game start. If you need to reach a certain part of the game for your script to be injected, do so. Then, you can close the game.

You need to have a local clone of the [GDweave project source](https://github.com/NotNite/GDWeave).
Once you've cloned that down, open up a terminal in the root of that new directory.
We are going to use `GDWeave.Dumper` to disassemble your script(s) that we just dumped:

```ps
$file_to_tokenize="C:\Program Files (x86)\Steam\steamapps\common\WEBFISHING\gdc_modded\Scenes\Entities\Player\player.gdc"
dotnet run --project .\GDWeave.Dumper\ -- parse $file_to_tokenize --codegen --output="player-modded.gd"
```

After running it, you should see a new file in the root directory containing the "decompiled" output of the Webfishing process, after all your mods were injected.

## Other examples to reference

- [ChromaChalk](https://github.com/adamantris/ColorfulChalk)
- [BetterLocalChat](https://github.com/binury/Toes.BetterLocalChat)
- [Calico](https://github.com/tma02/calico/tree/main)
- Submit a PR if you know of a good project to add here!

## Publishing your finished mod

For convenience you can checkout and use [included Powershell script](./publish.ps1). Otherwise, manually download another mod and take a look at the structure of the zip file.
Once it's ready, you can [upload your mod to Thunderstore](https://thunderstore.io/c/webfishing/create/).

## I ran into some snags and I'd really appreciate some help

Another -somewhat outdated but- comprehensive guide can be found [here](https://github.com/BlueberryWolf/WEBFISHINGModdingGuide)

Come visit our [Webfishing Modding Discord](https://discord.gg/kjf3FCAMDb)

>
> [!TIP]
> You can [reach out to me](https://ko-fi.com/c/993813af6b) for dedicated help with building your mod project.
