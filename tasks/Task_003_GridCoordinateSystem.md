# Task 003 - Grid Coordinate System

## Goal

Create the project's grid coordinate system.

This task creates only the mathematical representation of the grid.

No visual grid.

No placement.

No occupancy.

No gameplay.

---

## Why

Every gameplay system will use the same coordinate system.

A single source of truth prevents future bugs.

---

## Requirements

Create a GridPosition struct.

Location:

Scripts/Grid/GridPosition.cs

The struct represents one cell on the grid.

Properties:

- X
- Y

Coordinates are immutable after creation.

---

## Equality

GridPosition must support:

- ==
- !=

Override:

- Equals()
- GetHashCode()

Implement IEquatable<GridPosition>.

---

## Constructors

Provide one constructor.

Example

GridPosition(3, 5)

---

## Operators

Support addition.

Example

GridPosition + GridPosition

This will later be used for placement offsets.

---

## Constants

Expose common directions.

North

South

East

West

Example usage:

GridPosition.North

---

## Utility

Provide:

ToString()

Useful for debugging.

---

## Rules

GridPosition contains no Unity references.

No MonoBehaviour.

No GameObject.

No Transform.

No Vector3.

No gameplay logic.

Pure C# only.

---

## Do NOT

Do not create GridManager.

Do not create Grid.

Do not create GridCell.

Do not create placement.

Do not create occupancy.

Do not create debug drawing.

---

## Definition of Done

Project compiles.

GridPosition is immutable.

Supports equality.

Supports addition.

Supports cardinal directions.

Nothing else is implemented.
