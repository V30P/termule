# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- `IRenderTarget` interface for rendering to non-frame targets

### Changed
- Converted existing rendering systems to use `IRenderTarget`
- Modified `PositionalRenderer` to provide a custom target that transforms draw calls
- Renamed `FrameBuffer.Reset()` to `Fill()`
- Renamed `Cell.Char` to `Glyph` (reflected in the rest of the API)
- Moved `FrameBuffer` to the `Engine.Systems.Display` namespace (was in `Systems.Rendering`)
- Get-only `ICameraTarget.RenderTarget` property with dedicated `GetRenderTarget()` method

### Fixed
- `LineRenderer` rendering upside down in world space
- "Raindrops" demo not fully destroying raindrop objects

## [0.3.1] - 2026-05-25

### Fixed
- Crash when randomizing color in "Screensaver" demo
- Incorrect enemy spawning bounds in "Shooter" demo

## [0.3.0] - 2026-05-23

### Added
- Messaging via `MessageBus`
- Routed messaging in the world via `LocalMessageBus`
- Updated existing behavior to emit messages:
  - When the game is started/stopped
  - When a game element is activated/deactivated
  - When the display's size changes or mouse moves
  - When a transform's position changes
- `Connections` flags and extension methods for easily working with box-drawing characters
- `connectBoxDrawingChars` parameter to `Draw()` for proper box-drawing character layering
- `UseBoxDrawingCharacters` property to line renderer for extra-thin lines
- `Activate()` and `Deactivate()` virtual methods for game elements (replacing events)
- `Tick()` virtual method for components (replaces event)
- Re-added StyleCop analyzers for stricter static analysis
- Missing constructors and conversions for color types
- Missing commutative multiplication operators for vectors

### Changed
- `GameObject` is now sealed
- "Lightning" demo to use box drawing characters
- `.editorconfig` for StyleCop compatibility
- Renamed `Game.Root` to `World`
- Renamed `SystemManager.UseDefaults()` to `InstallDefaults`
- `Install` method for systems now takes params

### Removed
- `IConfigurableGame` and `IConfigurableSystemManager` interfaces to get rid of unnecessary complexity
- `Registered` and `Unregistered` events from game elements (replaced with virtual methods)
- `Ticked` event from components (replaced with a virtual method)

### Fixed
- Terminal display always using default background color when drawing a character without one
- Rounding errors in "Shooter" demo collision detection
- `VectorInt`'s scalar division requiring an integer divisor
- `Keyboard` constructor not being public

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
