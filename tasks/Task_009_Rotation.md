# Task 009 - Rotation System

## Goal

Allow rotatable buildings to rotate before placement.

Rotation must persist between placements.

The rotation is global.

Never reset it after placing a building.

---

## Files

Modify:

- PlacementPreview.cs
- BuildingPlacement.cs

---

## PlacementPreview

Add:

private int rotationIndex;

Supported values:

0 = North
1 = East
2 = South
3 = West

Default value:

0

Expose:

public Quaternion CurrentRotation

CurrentRotation should return:

Quaternion.Euler(0f, rotationIndex * 90f, 0f)

---

## Rotation Input

Pressing R:

rotationIndex = (rotationIndex + 1) % 4;

Only allow rotation if:

buildingDefinition != null

AND

buildingDefinition.CanRotate == true

---

## Preview Rotation

Whenever rotation changes:

Apply CurrentRotation to the preview mesh.

Do not rotate the parent object.

Rotate only the preview mesh transform.

---

## Placement

BuildingPlacement must instantiate using:

placementPreview.CurrentRotation

instead of

Quaternion.identity

---

## Important

Do NOT reset rotation after placement.

The next placed building must use the previous rotation.

Changing building type must NOT reset rotation.

The only time rotation is reset is when the game starts.

---

## Rules

No Singleton

No InputService

No Events

No Multi-cell support

No Rotation validation

Only 90 degree rotations

---

## Definition of Done

- Pressing R rotates preview 90 degrees.
- Conveyor can be placed in different directions.
- Rotation persists after placement.
- Building placement uses the preview rotation.
- Project compiles.

Stop after implementation.
