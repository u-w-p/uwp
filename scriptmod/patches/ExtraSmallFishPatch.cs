using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Fixes issues with extra small fish sizes not being properly assigned size prefixes and worth multipliers.
/// </summary>
public static class ExtraSmallFishPatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Fish size fixes")
			.Patching("res://Scenes/Singletons/playerdata.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Size prefix fix")
					.Do(Operation.Append)
					.Matching(
						TransformationPatternFactory.CreateGdSnippetPattern(
							"""

							var prefix = ""

							for p in size_prefix.keys():
							"""
						)
					)
					.With(
						"""

						if calc < 0.1:
							prefix = size_prefix[0.1]
							break

						elif calc > 3.25:
							prefix = size_prefix[3.25]

						""",
						2
					)
					.ExpectTimes(2)
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Size/worth mult fix")
					.Do(Operation.Append)
					.Matching(
						TransformationPatternFactory.CreateGdSnippetPattern(
							"""
							var average = Globals.item_data[data["id"]]["file"].average_size
							var calc = data["size"] / average
							var mult = 1.0

							for p in size_prefix.keys():
							"""
						)
					)
					.With(
						"""

						if calc < 0.1:
							mult = size_prefix[0.1]
							break

						elif calc > 3.0:
							mult = size_prefix[3.0]
							break


						""",
						2
					)
			)
			.Build();
	}
}
