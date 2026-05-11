# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Core messaging functionality via `MessageBus`
- Routed messaging in the component tree via `LocalMessageBus`
- Messages to existing behavior:
  - When the game is started/stopped
  - When a game element is registered/unregistered
  - When the display's size changes or mouse moves
  - When a transform's position changes
- `Connections` flags and extension methods for easily creating box-drawing characters
- `connectBoxDrawingChars` param to frame buffer's `Draw()` for proper box-drawing character layering
- `UseBoxDrawingCharacters` field to line renderer for extra-thin lines

### Changed
- `Ticked` event from components to a virtual method
- `Registered` and `Unregistered` events from game elements to virtual methods
- `GameObject` is now sealed
- "Lightning" demo to use box drawing characters

### Removed
- `IConfigurableGame` and `IConfigurableSystemManager` interfaces to get rid of unnecessary complexity

### Fixed
- Terminal display always using default background when drawing a character without one
- Rounding errors in "Shooter" demo collision detection

## [0.2.0] - 2026-04-25

### Added
- Comprehensive test coverage for core types and most components, systems, and POCOs
- `--help`, `--interactive`, and `--stats` flags for the demo project
- Setter for `GameObject` on components to simplify movement
- `ICameraTarget` interface to support rendering to non-display targets
- Mouse tracking for the Windows display implementation
- `Keyboard` class for input handling, replacing the previous controller-based approach
- `.editorconfig` with updated coding conventions

### Changed
- Standardized system names and namespaces to improve consistency and avoid collisions
- Improved display system performance via double buffering
- Reduced terminal display overhead by minimizing string allocations and escape sequences
- Renamed `Frame` to `FrameBuffer` and moved it to the `Systems.Display` namespace
- Improved `FrameBuffer` performance by reducing allocation overhead
- Reduced per-tick allocations by deferring ticking new components until the next tick
- Renamed `TransformRenderer` to `PositionalRenderer` and updated its API
- Replaced `Content` with the `IContent` interface (use `Image` as a default implementation)
- Optimized text content rendering to reduce redundant recalculations
- Reworked render system API for improved flexibility and clarity
- Updated resource path configuration to allow greater customization

### Removed
- Renderer crediting and `GetOverlappers()` due to significant performance costs (use custom collision detection instead)
- Size-related methods from camera; it now always matches the target size
- Base `Controller` and `Bind` classes, replaced by a simplified keyboard-specific system

### Fixed
- Incorrect layering of TPS indicator in demos
- Transforms not properly resetting state during re-parenting
- Camera continuing to reference outdated transforms after movement
- Occasional duplicate IDs in game elements
- Terminal display implementations not fully resetting configuration
- Serializer failing to correctly handle empty 2D arrays

## [0.1.0] - 2026-03-16

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
- Full XML documentation comments for the API
- Initial demo collection