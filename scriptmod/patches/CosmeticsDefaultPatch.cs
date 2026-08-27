using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Replaces the logic that replaces a player's look with default cat when they wear mod cosmetics.
/// It now selectively removes them from the player instead.
/// </summary>
public static class CosmeticsDefaultPatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Cosmetics Default Patch")
			.Patching("res://Scenes/Entities/Player/player.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Default cat fallback to selective missing cosmetic removal")
					.Do(Operation.ReplaceAll)
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern(
						"""if not valid: data = PlayerData.FALLBACK_COSM.duplicate()"""
						)
					)
					.With(
						"""

						if not valid:
							for entry in data:
								var value = data[entry]
								if not typeof(value) == TYPE_ARRAY:
									if Globals._cosmetic_exists(value) == false:
										data[entry] = PlayerData.FALLBACK_COSM.get(entry)
								else:
									for accessory in value:
										if Globals._cosmetic_exists(accessory) == false:
											data[entry].erase(accessory)

						""",
						1
					)
			)
			.Build();
	}
}
