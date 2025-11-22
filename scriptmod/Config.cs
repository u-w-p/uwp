using System.Text.Json.Serialization;

namespace uwp.Unofficial_Webfishing_Patch;

public class Config(ConfigFileSchema configFile)
{
	[JsonInclude]
	public bool infiniteChatRange = configFile.infiniteChatRange;
}
