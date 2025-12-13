# Changelog

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
