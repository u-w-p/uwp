using GDWeave;
using GDWeave.Modding;
using util.LexicalTransformer;

namespace patches;

/// <summary>
/// Prevents chalk from being drawn outside of the canvas
/// </summary>
public static class CleanerChalkCanvasses
{
	public static IScriptMod Create(IModInterface mi)
	{
		return new TransformationRuleScriptModBuilder()
			.ForMod(mi)
			.Named("Cleaner chalk canvasses")
			.Patching("res://Scenes/Entities/ChalkCanvas/chalk_canvas.gdc")
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Add drawing bounds check")
					// Trying to match Vector2 isn't working currently in the snippet generator
					// so we'll just match the (x, y) part of
					// `_clamp_cell(pos + Vector2(x, y))`
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern("(x, y))"))
					.With(
						"""

						if not get_node("/root/uwp/cleanerCanvasses").in_bounds(final.x, final.y, self): continue

						""",
						indent: 4
					)
			)
			.AddRule(
				new TransformationRuleBuilder()
					.Named("Add chalk broadcast bounds check")
					.Matching(TransformationPatternFactory.CreateGdSnippetPattern("color = 0"))
					.With(
						"""

						if not get_node("/root/uwp/cleanerCanvasses").in_bounds(pos.x, pos.y, self): continue

						""",
						indent: 2
					)
			)
			.Build();
	}
}
