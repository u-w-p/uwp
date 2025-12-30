using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Guitar sound enhancements and fixes
/// </summary>
public static class GuitarQOLPatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Guitar sound fixes")
			.Patching("res://Scenes/Entities/Player/guitar_string_sound.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Boost polypony")
					.Do(Operation.ReplaceLast)
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern("for i in 4"))
					.With("6")
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Fret intervals maxxing")
					.Do(Operation.ReplaceLast)
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern("fret_intervals = 7.38"))
					.With("7.379")
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Guitar audio simulation revisions")
					.Do(Operation.Append)
					.Matching(
						TransformationPatternFactory.CreateGdSnippetPattern("new.attenuation_filter_cutoff_hz = 8000")
					)
					.With(
						"""

						new.attenuation_filter_cutoff_hz = 15000
						new.unit_db = 10.0
						new.emission_angle_filter_attenuation_db = -20.0
						new.emission_angle_enabled = true
						new.emission_angle_degrees = 245.0

						""",
						2
					)
			)
			.Build();
	}
}
