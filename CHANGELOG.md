# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- New flash effect on plants during their energy absorbing stage to make the state more visible
- New particle effect on plants during their energy absorbing stage to make the state more visible
- Firefly particles of max grown light plants.

### Changed
- Removed manual tilling step. The tilling state on the soil is now used to indicate full grown plants
- Added filtering to input on the soil. Soil no longer gets an interaction glow if you aren't carrying a seed.

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

