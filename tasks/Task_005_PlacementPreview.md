# Task 005 - Placement Preview

## Goal

Create a placement preview system.

The player should always see which grid cell is currently under the mouse cursor.

No buildings are placed yet.

No placement validation.

No clicks.

Only preview.

---

## Files

Create:

Scripts/Placement/PlacementPreview.cs

---

## Responsibilities

The system should:

- Read the mouse position.
- Cast a ray from the camera.
- Detect the ground.
- Convert the hit point into a GridPosition.
- Convert the GridPosition back into a world position.
- Move a preview object to the center of the selected cell.

---

## Preview Object

Create a serialized Transform field.

The preview object already exists in the scene.

Do not instantiate anything.

---

## Ground

Use a Physics Raycast.

The ground must use a LayerMask.

Expose the LayerMask in the Inspector.

Do not hardcode layer indices.

---

## Movement

The preview object should instantly move.

No smoothing.

No animation.

---

## Rules

Do not place buildings.

Do not check occupancy.

Do not rotate anything.

Do not create ghost materials.

Do not create placement validation.

Do not instantiate prefabs.

---

## Definition of Done

- Mouse movement updates the preview.
- Preview snaps to the grid.
- Preview uses GridSystem for conversion.
- No allocations each frame.
- Project compiles.
- Stop after this task.
