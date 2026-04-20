# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] 

### Added
- Tests for core types
- Test for components
- Test for systems
- `--help`, `--interactive`, and `--stats` flags for the demo project
- Setter for `gameObject` property of components for easy movement
- `ICameraTarget` for rendering to non-display objects
- Proper mouse tracking for the windows display implementation
- `Keyboard` class for keyboard input (replacing old controller-based implementation)
- `.editorconfig` with updated conventions

### Changed
- System names and namespaces to be more consistent and avoid namespace/name collisions
- Improved display system performance by switching to double buffering
- Improved terminal display system performance by reducing string allocations and unnecessary escape sequences
- Renamed `Frame` to `FrameBuffer` and moved it to the display namespace
- Improved frameBuffer performance by removing unnecessary delegate allocations
- Made game objects only add components to the tick list on the next frame after they are added to avoid snapshot allocation
- Renamed `TransformRenderer` to `PositionalRenderer` and updated its API
- Converted`Content` to the `IContent` interface (use `Image` for an easy implementation)
- Improved text content implementation by reducing and optimizing recalculations 
- Reworked render system API
- Updated resource path configuration to be cleaner and easier to use


### Removed
- Renderer crediting and `GetOverlappers()` for abysmal performance (switch to manually implemented collision detection)
- Size-related methods from `Camera`, will now always match target size
- Base `Controller` and `Bind` classes, replaced by simpler keyboard-specific system

### Fixed
- Improper layering of TPS indicator in debug builds of Demos
- Transforms not properly clearing state when re-parenting
- Camera continuing to use old transform after it moves
- GameElements occasionally getting duplicate ids
- Terminal display system implementations not fully resetting configuration
- Serializer failing to properly read empty 2D arrays

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

0 - 0 - 0
1 - 1 - 1
...
10 - A - 1010
11 - B - 1011
12 - C - 1100
13 - D - 1101
14 - E - 1110
15 - F - 1111