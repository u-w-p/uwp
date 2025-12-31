using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

public static class LobbyPlayerCountPatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Set max lobby size to higher limit")
			.Patching("res://Scenes/Menus/Main Menu/PlayercountDial/playercount_dial.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("dial_max -> increase limit")
					.Do(Operation.ReplaceLast)
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern("var dial_max = 12"))
					.With(
						"""
						128

						"""
					)
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("max_set -> increase limit")
					.Do(Operation.ReplaceLast)
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern("var max_set = 12"))
					.With(
						"""
						128

						"""
					)
			)
			.Build();
	}
}
