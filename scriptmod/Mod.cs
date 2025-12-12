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
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
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
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
				.ForMod(mi)
				.Named("Fix player stuff")
				.Patching("res://Scenes/Entities/Player/player.gdc")
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Fix chat input moving cam while freemcam active")
						.Do(Operation.Append)
						.Matching(
							TransformationPatternFactory.CreateFunctionDefinitionPattern("_freecam_input", ["delta"])
						)
						.With(
							"""

							if busy: return

							""",
							1
						)
				)
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
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
				.Build()
		);
	}

	public void Dispose()
	{
		// Post-injection cleanup (optional)
	}
}
