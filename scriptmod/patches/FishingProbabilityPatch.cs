using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Fixes the fishing size probability distribution (from linear to logarithmic)
/// </summary>
public static class FishingProbabilityPatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Fix global distribution linear -> log")
			.Patching("res://Scenes/Singletons/globals.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Replace roll_for_size function")
					.Do(Operation.Append)
					.Matching(
						TransformationPatternFactory.CreateGdSnippetPattern(
							"""if not item_data.keys().has(item): return""",
							1
						)
					)
					.With(
						"""

						return _fixed_roll_item_size(item)

						""",
						1
					)
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Add fixed roll function")
					.Do(Operation.Append)
					.Matching(TransformationPatternFactory.CreateGlobalsPattern())
					.With(
						"""
						func _fixed_roll_item_size(item):
							var avg = item_data[item]["file"].average_size
							var sigma = log(1.55)
							var mu = log(avg)
							var RNG = RandomNumberGenerator.new()
							RNG.randomize()

							var rand = RNG.randfn(mu, sigma)
							var fishsize = exp(rand)
							fishsize = max(stepify(fishsize, 0.01), 0.01)
							var chance_of_mutation = 1/5000.0
							var giant_multiplier = 1.5 + (150.0 / avg)
							if RNG.randf() < chance_of_mutation: fishsize *= giant_multiplier
							return fishsize

						"""
					)
			)
			.Build();
	}
}
