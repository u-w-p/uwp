using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Fixes Fishing Trap Buddy UI stuff
/// </summary>
public static class FishingBuddyFix
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Fix Fishing Trap Buddy stuff")
			.Patching("res://Scenes/Entities/Props/fish_trap.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Fix fish trap/buddy icon showing when HUD should be hidden")
					.Do(Operation.Append)
					.Matching(
						TransformationPatternFactory.CreateFunctionDefinitionPattern("_physics_process", ["delta"])
					)
					.With(
						"""

						var HUD = get_node("/root/playerhud")
						$caught.visible = has_fish and not HUD.hud_hidden
						$owned.visible = has_fish and not HUD.hud_hidden

						""",
						1
					)
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Buddy/trap arrow visibility reduction")
					.Do(Operation.Append)
					.Matching(TransformationPatternFactory.CreateFunctionDefinitionPattern("_ready", []))
					.With(
						"""

						var owned = get_node("owned")
						owned.opacity = 0.1

						""",
						1
					)
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Fix blinking owned indicator caused by previous patches")
					.Do(Operation.ReplaceAll)
					.Matching(
						TransformationPatternFactory.CreateGdSnippetPattern(
							"""
							$owned.visible = get_parent().controlled
							""",
							1
						)
					)
					.With(
						"""

						""",
						1
					)
			)
			.Build();
	}
}
