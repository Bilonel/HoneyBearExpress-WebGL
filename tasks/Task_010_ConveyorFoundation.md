# Task 010 - Conveyor Foundation

## Goal

Create the foundation of the conveyor system.

No item movement yet.

Only build the conveyor graph.

---

## Files

Create:

- Conveyor.cs

Modify:

- BuildingPlacement.cs

---

## Conveyor

Conveyor is a MonoBehaviour.

Fields:

- GridPosition GridPosition
- int RotationIndex

Properties:

- GridPosition InputPosition
- GridPosition OutputPosition

Input/Output are calculated from RotationIndex.

Example:

North
Input = South
Output = North

East
Input = West
Output = East

---

## Initialization

After Instantiate:

Initialize(GridPosition position, int rotationIndex)

Store:

- GridPosition
- RotationIndex

Calculate InputPosition

Calculate OutputPosition

---

## BuildingPlacement

After Instantiate:

If the placed building has Conveyor component:

Call Initialize()

Do nothing else.

---

## Rules

Do NOT move items.

Do NOT search neighbours.

Do NOT create ConveyorManager.

Do NOT use TickManager.

Do NOT use Update.

Do NOT use Physics.

Only create the data structure.

---

## Definition of Done

- Conveyor prefab has Conveyor component.
- Conveyor stores its grid position.
- Conveyor stores its rotation.
- Conveyor calculates input/output cells correctly.
- Project compiles.

Stop after implementation.
