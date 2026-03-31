# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] 

### Added
- Tests for core types
- Test for components
- `--help`, `--interactive`, and `--stats` flags for the demo project
- Setter for `gameObject` property of components for easy movement
- `ICameraTarget` for rendering to non-display objects

### Changed

- Improved `Display` performance by switching to double buffering
- Improved `TerminalDisplay` performance by reducing string allocations and unnecessary escape sequences
- Renamed `Frame` to `FrameBuffer` and moved it to the display namespace
- Improved `FrameBuffer` performance by removing unnecessary delegate allocations
- Made GameObjects only add components to the tick list on the next frame after they are added to avoid snapshot allocation
- Rename `TransformRenderer` to `PositionalRenderer` and update the API

### Removed

- Renderer crediting and `GetOverlappers()` for abysmal performance (switch to manually implemented collision detection)
- Size-related methods from `Camera`, will now always match target size

### Fixed

- Improper layering of TPS indicator in debug builds of Demos
- Transforms not properly clearing state when reparenting
- Camera continuing to use a transform after it moves
- GameElements occasionally getting duplicate ids

## [0.1.0] 

### Added
- Core architectural base classes
- Components:
    - `Transform`
    - `Camera`
    - `ContentRenderer`
    - `LineRenderer`
    - `CircleRenderer`
- Systems:
    - `Controller`
    - `Display`
    - `RenderSystem`
    - `ResourceLoader`
- Vector and content POCOs
- Full XML doc comments for API
- Basic demo collection