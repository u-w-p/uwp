using GDWeave;
using util.LexicalTransformer;

namespace uwp.Unofficial_Webfishing_Patch;

public class Mod : IMod
{
	public Mod(IModInterface mi)
	{
		// Load your mod's configuration file
		//var config = new Config(mi.ReadConfig<ConfigFileSchema>());

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
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
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
				.ForMod(mi)
				// ? Named solely for debugging/logging purposes
				.Named("Remove Modded Tag")
				// ! Note the file extension will end in gdc NOT gd
				.Patching("res://Scenes/Menus/Main Menu/main_menu.gdc")
				.AddRule(
					new TransformationRuleBuilder()
						// ! These names MUST be unique or your mod will throw an System.InvalidOperationException when loading !
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
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
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

							var avg = item_data[item]["file"].average_size
							var sigma = log(1.55)
							var mu = log(avg)
							var RNG = RandomNumberGenerator.new()
							RNG.randomize()

							var rand = RNG.randfn(mu, sigma)
							var size = exp(rand)
							size = max(stepify(size, 0.01), 0.01)

							var chance_of_mutation = 1/5000.0
							var giant_multiplier = 1.5 + (150.0 / avg)
							if RNG.randf() < chance_of_mutation: size *= giant_multiplier
							return size

							""",
							1
						)
				)
				.Build()
		);
		// }
	}

	public void Dispose()
	{
		// Post-injection cleanup (optional)
	}
}
