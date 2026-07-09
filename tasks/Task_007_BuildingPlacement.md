# Task 007 - Building Placement

## Goal

Allow the player to place a building on the grid.

The player already has a moving preview.

When the player left-clicks, the selected building should be instantiated and snapped to the preview position.

This task only implements basic placement.

No placement validation.

No occupancy.

No rotation.

---

## Files

Create:

Scripts/Placement/BuildingPlacement.cs

---

## PlacementPreview

Expose the following read-only properties:

- Current GridPosition
- Current World Position

Do not expose internal fields.

Do not move placement logic into PlacementPreview.

---

## BuildingPlacement

Responsibilities:

- Hold the currently selected BuildingDefinition.
- Listen for left mouse click.
- Read the current position from PlacementPreview.
- Instantiate BuildingDefinition.BuildingPrefab.
- Place the object exactly at the preview world position.

---

## Rules

- Use the existing BuildingDefinition.
- Do not instantiate preview objects.
- Only instantiate the real building prefab.
- Keep placement independent from preview rendering.
- Do not implement occupancy.
- Do not implement validation.
- Do not implement rotation.
- Do not parent placed buildings.
- Instantiate using Quaternion.identity.

---

## Inspector

Expose:

- Current Building Definition
- Placement Preview reference

---

## Definition of Done

- Left click places one building.
- Multiple clicks place multiple buildings.
- Buildings snap perfectly to the grid.
- Preview continues working after placement.
- Project compiles.
- Stop after this task.
