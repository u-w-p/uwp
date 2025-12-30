using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Fixes chat input moving the freecam when active
/// </summary>
public static class FreecamInputFix
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Fix player stuff")
			.Patching("res://Scenes/Entities/Player/player.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Fix chat input moving cam while freemcam active")
					.Do(Operation.Append)
					.Matching(TransformationPatternFactory.CreateFunctionDefinitionPattern("_freecam_input", ["delta"]))
					.With(
						"""

						if busy: return

						""",
						1
					)
			)
			.Build();
	}
}
