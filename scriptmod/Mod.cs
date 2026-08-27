using GDWeave;
using patches;

namespace uwp.Unofficial_Webfishing_Patch;

public class Mod : IMod
{
	public Mod(IModInterface mi)
	{
		var config = new Config(mi.ReadConfig<ConfigFileSchema>());

		mi.RegisterScriptMod(LobbyPlayerCountPatch.Create(mi));
		mi.RegisterScriptMod(RevertLobbySizeModdedTagsPatch.Create(mi));
		mi.RegisterScriptMod(FishingProbabilityPatch.Create(mi, config));
		mi.RegisterScriptMod(ExtraSmallFishPatch.Create(mi));
		mi.RegisterScriptMod(FreecamInputFix.Create(mi));
		mi.RegisterScriptMod(GuitarQOLPatch.Create(mi));
		mi.RegisterScriptMod(LettersPatch.Create(mi));
		mi.RegisterScriptMod(LeftoverMenuHotkeyFix.Create(mi));
		mi.RegisterScriptMod(AntiNetworkingCrashesPatch.Create(mi));
		OptionsPatches.RegisterAll(mi);
		mi.RegisterScriptMod(UnlistedLobbyPatch.Create(mi));
		mi.RegisterScriptMod(ClampPunchCountPatch.Create(mi));
		mi.RegisterScriptMod(CleanerChalkCanvasses.Create(mi));
		mi.RegisterScriptMod(CosmeticsDefaultPatch.Create(mi));
	}

	public void Dispose()
	{
		// Post-injection cleanup (optional)
	}
}
