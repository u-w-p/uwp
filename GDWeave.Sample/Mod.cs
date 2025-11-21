using GDWeave;
using util.LexicalTransformer;

namespace MyModName;

/*
 The main entrypoint of your mod project
 This code here is invoked by GDWeave when loading your mod's DLL assembly, at runtime
*/

public class Mod : IMod
{
	public Mod(IModInterface mi)
	{
		// Load your mod's configuration file
		var config = new Config(mi.ReadConfig<ConfigFileSchema>());

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
				.ForMod(mi)
				// ? Named solely for debugging/logging purposes
				.Named("My Very First Mod")
				// ! Note the file extension will end in gdc NOT gd
				.Patching("res://Scenes/World/world.gdc")
				.AddRule(
					new TransformationRuleBuilder()
						// ! These names MUST be unique or your mod will throw an System.InvalidOperationException when loading !
						.Named("Amend default global chat message(s)")
						.Do(Operation.Append)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"""
								Network._update_chat("Welcome to GLOBAL chat!")
								"""
							)
						)
						.With(
							"""

							Network._update_chat(["Your backpack is starting to smell sorta fishy...", "You're back! Try not to drown this time?", "Listen, can we just be reel for a moment?"][randi() % 3])

							""",
							indent: 2
						)
				)
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Birdpocalypse!")
						.Do(Operation.ReplaceAll)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"get_tree().get_nodes_in_group(\"bird\").size() > 8"
							)
						)
						.With("get_tree().get_nodes_in_group(\"bird\").size() > 99")
				)
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Birdpocalypse 2!!!")
						.Do(Operation.Append)
						.Matching(TransformationPatternFactory.CreateGdSnippetPattern("var count = randi() % 3 + 1"))
						.With(
							"""

							count = 10

							""",
							indent: 1
						)
				)
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
				.ForMod(mi)
				.Named("local chat shortcut command")
				.Patching("res://Scenes/HUD/playerhud.gdc")
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Add new local flag")
						.Do(Operation.Append)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"""
								var current_effect = "none"
								"""
							)
						)
						.With(
							"""

							var local_shortcut = false

							""",
							indent: 1
						)
				)
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Add /l shortcut")
						.Do(Operation.Append)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"""
								"/wag": PlayerData.emit_signal("_wag_toggle")
								"""
							)
						)
						.With(
							"""

							"/l": local_shortcut = true

							""",
							indent: 4
						)
				)
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Amend `local` conditional to include check for local_shortcut")
						.Do(Operation.ReplaceAll)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"Network._send_message(final, final_color, chat_local)"
							)
						)
						.With("Network._send_message(final, final_color, chat_local or local_shortcut)")
				)
				.Build()
		);

		mi.RegisterScriptMod(
			new TransformationRuleScriptModBuilder()
				.ForMod(mi)
				.Named("Better Local Chat (Enhancements)")
				.Patching("res://Scenes/Singletons/SteamNetwork.gdc")
				.AddRule(
					new TransformationRuleBuilder()
						.Named("Remove redundant & ugly local prefix")
						.Do(Operation.ReplaceAll)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								""" "[color=#a4756a][​local​] [/color]" """
							)
						)
						.With("\"\"")
				)
				.AddRule(
					new util.LexicalTransformer.TransformationRuleBuilder()
						.Named("Infinite local chat range limit")
						.Do(Operation.Append)
						// ? This rule will be SKIPPED unless this value was set to true in the mod's config
						.When(config.infiniteChatRange)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"""
								 if dist < 25.0: _recieve_safe_message(user_id, user_color, user_message, true)
								"""
							)
						)
						.With(
							"""

							if dist >= 25.0: _recieve_safe_message(user_id, user_color, user_message, true)

							""",
							indent: 6
						)
				)
				.AddRule(
					new util.LexicalTransformer.TransformationRuleBuilder()
						.Named("Print prefixed local chat under global tab too")
						.Do(Operation.Append)
						.Matching(
							TransformationPatternFactory.CreateGdSnippetPattern(
								"""
								 _recieve_safe_message(user_id, user_color, user_message, true)
								"""
							)
						)
						.With(
							"""

							 if dist < 25.0: _recieve_safe_message(user_id, user_color, "(local) " + user_message, false)

							""",
							indent: 6
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
