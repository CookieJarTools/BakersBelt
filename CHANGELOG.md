# Changelog

All notable changes to this package are written here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this package follows
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2026-10-13

### Changed

- The belt now puts its elements on the main toolbar through Unity's own toolbar API instead of
  reaching into the editor's visual tree. Unity 6.3 closed the old route, so the belt had stopped
  working there and warned about it every few minutes.
- Elements are drawn by Unity, so they match the toolbar around them, and they can be moved,
  hidden and reset like any other toolbar element.
- The lock is a toggle rather than a button, so it draws itself pressed while it is holding.
- Compile now lifts the lock before asking for a compile, instead of quietly doing nothing.
- The package needs Unity 6.3 (6000.3) or newer. The toolbar API it is built on arrived there.

### Added

- `BeltToolbar.AddButton`, `AddIconButton`, `AddDropdown` and `AddToggle`, with six slots either
  side of the play controls.
- Everything the belt adds is grouped under `Bakers Belt` in the toolbar's right click menu, with
  your own buttons under `Left of play` and `Right of play`.
- The belt switches its own elements on once, the first time it is installed, and a slot on the
  first occasion something is put in it. Anything switched off afterwards stays off.

### Removed

- `ToolbarExtender` and the arbitrary `VisualElement` it took. Only the kinds Unity provides can
  go on the toolbar now: a button, a dropdown, a toggle, a label and a slider. The method that
  would draw anything else is internal to the editor, so this is not a limit the belt can lift.
- `StyleSheetUtils`, which only existed to style the elements the belt used to draw itself.

## [1.1.2] - 2026-09-23

### Added

- The package, as it stands: a collection of useful utilities for Unity.
