# Task 004 - Grid System

## Goal

Create the mathematical grid system used by the entire game.

This system converts between world positions and grid coordinates.

No gameplay.

No placement.

No occupancy.

---

## Files

Create:

Scripts/Grid/GridSystem.cs

---

## Responsibilities

The GridSystem must:

- Know the grid cell size.
- Convert GridPosition -> World Position.
- Convert World Position -> GridPosition.

---

## Grid

Square grid.

Cell Size:

Expose in Inspector.

Default:

1.0

---

## World Origin

Grid (0,0)

must map to

World (0,0,0)

---

## Conversion

Provide methods:

GridPosition WorldToGrid(Vector3 worldPosition)

Vector3 GridToWorld(GridPosition position)

GridToWorld should return the CENTER of the cell.

---

## Rounding

WorldToGrid should always return the nearest cell.

Do not use floor.

Use rounding.

---

## Debug

Draw the grid using Gizmos.

Only editor visualization.

No runtime objects.

Expose grid width and height in Inspector.

Default:

Width = 20

Height = 20

---

## Rules

No occupancy.

No GameObjects.

No placement.

No buildings.

No items.

No MonoBehaviour except GridSystem.

No singleton.

---

## Definition of Done

- Grid can convert both directions.
- Gizmos draw the full grid.
- Cell size is configurable.
- Project compiles.
- Stop after this task.
