# Task 008 - Grid Occupancy

## Goal

Prevent multiple buildings from being placed on the same grid cell.

Occupancy is a gameplay system.

Do not add occupancy logic into GridSystem.

---

## Files

Create:

Scripts/Grid/GridOccupancy.cs

---

## GridOccupancy

Responsibilities:

- Store placed buildings.
- Know which cells are occupied.
- Register a building.
- Check whether a cell is occupied.

Use:

Dictionary<GridPosition, GameObject>

---

## Public API

bool IsOccupied(GridPosition position)

void Register(GridPosition position, GameObject building)

---

## BuildingPlacement

Before placing a building:

1. Read CurrentGridPosition from PlacementPreview.
2. Ask GridOccupancy if the cell is occupied.
3. If occupied:
    - Do nothing.
4. Otherwise:
    - Instantiate building.
    - Register it.

---

## PlacementPreview

Add:

public bool IsPlacementValid { get; private set; }

Update it every frame.

If the current cell is occupied:

- IsPlacementValid = false
- Preview uses Invalid Preview Material

Otherwise:

- IsPlacementValid = true
- Preview uses Valid Preview Material

Only switch material when the state changes.

Do not assign the material every frame.

---

## Rules

Do not modify GridSystem.

Do not add singleton.

Do not add managers.

Do not add rotation.

Do not add multi-cell buildings.

Only support 1x1 buildings.

---

## Definition of Done

- First building can be placed.
- Second building cannot be placed on the same cell.
- Preview turns red on occupied cells.
- Preview turns green on free cells.
- Project compiles.

Stop after this task.
