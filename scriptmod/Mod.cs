using GDWeave;
using patches;
using util.LexicalTransformer;

namespace uwp.Unofficial_Webfishing_Patch;

public class Mod : IMod
{
	public Mod(IModInterface mi)
	{
		mi.RegisterScriptMod(LobbyPlayerCountPatch.Create(mi));

		mi.RegisterScriptMod(RevertLobbySizeModdedTagsPatch.Create(mi));

		mi.RegisterScriptMod(FishingProbabilityPatch.Create(mi));

		mi.RegisterScriptMod(ExtraSmallFishPatch.Create(mi));

		mi.RegisterScriptMod(FreecamInputFix.Create(mi));

		mi.RegisterScriptMod(FishingProbabilityPatch.Create(mi));

		mi.RegisterScriptMod(GuitarQOLPatch.Create(mi));
	}

	public void Dispose()
	{
		// Post-injection cleanup (optional)
	}
}
