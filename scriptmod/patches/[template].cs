using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// A template patch to use as a starting point for new patches.
/// </summary>
public static class TemplatePatch
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("template patch")
			.Patching("res://example.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("template rule")
					.Do(Operation.Append)
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern(""))
					.With(
						"""

						"""
					)
			)
			.Build();
	}
}
