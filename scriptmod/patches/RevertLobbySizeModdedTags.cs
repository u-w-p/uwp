using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Reverts the automatic addition of the "modded" tag for lobbies with more than 12 players.
/// </summary>
public static class RevertLobbySizeModdedTagsPatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Remove Modded Tag")
			.Patching("res://Scenes/Menus/Main Menu/main_menu.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Remove the lobby size > 12 check")
					.Do(Operation.ReplaceAll)
					.Matching(
						TransformationPatternFactory.CreateGdSnippetPattern(
							"""if int(lobby_cap) > 12: lobby_tags.append("modded")"""
						)
					)
					.With(
						"""


						"""
					)
			)
			.Build();
	}
}
