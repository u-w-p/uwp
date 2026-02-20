# Changelog

## v1.2.0

- New patch: Players which you have banned from your lobbies in previous sessions will no longer become unbanned after you restart the game [(#63)](https://github.com/u-w-p/uwp/issues/63)
  - You can manage this using the new `bans.txt` file in `%AppData%\godot\app_userdata\webfishing_2_newver`
  - This list is transferable to [Cove](https://github.com/DrMeepso/WebFishingCove/)
- New patch: Drawing outside chalk circles is discarded properly! ([#62](https://github.com/u-w-p/uwp/issues/62))

## v1.1.0 - 🥊🎵, 🚫🎨

- New patch: [chalk canvas prop griefing prevention](https://github.com/u-w-p/uwp/issues/55)
- New patch: [fix punching-a-lot song not playing despite having chosen violence](https://github.com/u-w-p/uwp/issues/6)

## v1.0.0

- Added UWP options to settings menu!
- New patch: [Fix](https://github.com/u-w-p/uwp/issues/37) - Unlisted lobbies are now _actually_ (properly) unlisted from the public games list in Steam
  rather than flagged and hidden in the UI.
  - This behavior is the same as the original game version.
  - Note that unlisted lobbies cannot be joined via a shared code and players must either be sent an invite or otherwise join through Steam

## v0.2.0

- New patch: safeguards to prevent game client from crashing when receiving malformed network packets (#36)
- New patch: granular settings controls for FPS, pixelization, view distance (#28)

## v0.1.0

- Added UWP/Modfishing Discord link to main menu
- New patch: secondary notification when receiving a letter (#23)
- New patch: Pressing E while in the menu will no longer interrupt typing & close the menu

### Internal

- Refactored patches into standalone files for readability, ease of contribution, etc.

## v0.0.10 - 👽

- [Restorarp](https://github.com/u-w-p/uwp/issues/11) tharp securp alurp farp Florp
- Unidentified Fish Object now has a friend

## v0.0.9 - Guitar Audio Enhancement Patches

- Changed polyphony of each string 4 -> 6
- Loosened low-pass filter 8kHz -> 15 kHz
- Added angled (245deg) attenuation for guitar sounds/realism

## v0.0.8

- Hotfixed blinking buddy.owned arrow indicators caused by v0.0.7 changes

## v0.0.7

- Fixed [fish trap/buddy indicators not hiding when player has hidden HUD](https://github.com/u-w-p/uwp/issues/9)
  - Cheers to Eleboots for reporting/requesting this patch!
- Fixed [freecam movement triggering by mistake while typing](https://github.com/u-w-p/uwp/issues/4)

## v0.0.6

- Adjusted fish size calculation from normal distribution to log-normal
  - This correctly addresses the issue of some fish being hard-capped in size without [accidentally increasing the overall average size](https://thunderstore.io/c/webfishing/p/hostileonion/bigfish/)

## v0.0.3

- Removed the player cap filter from the lobby list.
