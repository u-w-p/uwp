using System.Text.Json.Serialization;

namespace MyModName;

/// <summary>
/// This class defines the shape of and default values for your mod's JSON config.
/// This is <strong>entirely optional</strong>! I included this in the example
/// as rather than limiting your mod to a single feature or design, with a config,
/// your mod can have many features that the user can then toggle on/off or configure.
/// (Bear in mind, most users will probably skim your docs at best, and most will never touch a config so
/// do try to pick good defaults!)
/// <br/>
/// Note: the generated JSON file will be placed inside of the GDWeave directory, in the `configs` folder
/// e.g.: WEBFISHING\GDWeave\configs\Toes.MyModName.json
/// </summary>
public class ConfigFileSchema
{
	[JsonInclude]
	public bool infiniteChatRange = false;
}
