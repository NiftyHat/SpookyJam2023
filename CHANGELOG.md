# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- New flash effect on plants during their energy absorbing stage to make the state more visible
- New particle effect on plants during their energy absorbing stage to make the state more visible
- Firefly particles of max grown light plants.
- pck and exe files added to git lfs list.
- placeholder death scream to dark plants
- placeholder damage sound to dark plants
- placeholder audio for when light plants grow stages.0
- Rest/Wake animations to the player
- Findings for doing fades called FadeEvents
- Extension method for Node so you can easily print parent names in debug.
- Path line rendered from the unused effect of showing a plant grow using a line from the lantern.
- Some glow shaders that I don't use.
- Audio listener on the player so world audio falloff will work.
- Added the now Unused SineWaveGenerator.cs
- Added SpriteShaderEffect.cs which is responsible for temporarily setting a sprite to a specific shader for animating.

### Changed
- Removed manual tilling step. The tilling state on the soil is now used to indicate full grown plants
- Added filtering to input on the soil. Soil no longer gets an interaction glow if you aren't carrying a seed.
- Sleeping jingle updated to new placeholder audio. Got rid of the ticks from Monster Masquarade
- Flash shader now takes an amount float. This just makes it easier to animate.
- Re-wrote EffectSleepFade to be called from events in Sim, which actually control the steps to sleeping/waking
- Interactable SetTargeted now takes the targeting entity as the param, which makes writing filters easier.
- Added a boarder around the soil tile so the outline will show correctly when interacting.
- Updated hurt animation loops on the dark stem and adjusted health accordingly so they flash once per 'hit'
- Make dark stems spawn with a random amount of health between 4 and 7
- Made the Lantern Tool Enabled so it can be toggled off during sleep.
- Changed the min threshold for light on light plants to 0.1.
- Exposed FadeEvents in SimController.



## [0.6.2] - 2023-11-05

### Added
- Ambient audio for the cave
- Spatial audio for the pyre
- Placeholder footsteps for the character
- Rocks around pyre.

### Fixed
- A crash that occurred where the rotting state would queue free a plant that was already destroyed
- An issue where state machines would continue to dispatch events after being destroyed.

### Changed
- Updated DarkEyePlant days to regrow to 1 to make it easier to understand where to get extra eyes from.
- added Bin folder to the gitignore since the file size made git sad.

### Removed

## [0.6.1] - 2023-11-05

### Added
- Pillow sprites in the sleeping area.
- A tutorial stone on using the sleeping spot.

## [0.6.0] - 2023-11-04

