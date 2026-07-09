# Task 011 - Conveyor Registry

## Goal

Create a registry that allows conveyors to instantly find each other.

No item movement yet.

---

## Files

Create:

- ConveyorRegistry.cs

Modify:

- BuildingPlacement.cs
- Conveyor.cs

---

## ConveyorRegistry

Create a MonoBehaviour.

Internally store:

Dictionary<GridPosition, Conveyor>

Methods:

Register(Conveyor conveyor)

Unregister(Conveyor conveyor)

TryGetConveyor(GridPosition position, out Conveyor conveyor)

Contains(GridPosition position)

---

## BuildingPlacement

After Conveyor.Initialize()

Register the conveyor.

---

## Conveyor

Add:

public Conveyor InputConveyor { get; private set; }

public Conveyor OutputConveyor { get; private set; }

Add method:

RefreshConnections(ConveyorRegistry registry)

Inside this method:

InputConveyor =
registry.TryGetConveyor(InputPosition)

OutputConveyor =
registry.TryGetConveyor(OutputPosition)

Nothing else.

---

## BuildingPlacement

After registering the new conveyor:

Refresh the new conveyor.

Refresh every neighbouring conveyor
(North, South, East, West).

Only those four.

Do NOT refresh the whole map.

---

## Rules

No Update

No TickManager

No Physics

No FindObjectsOfType

No Singleton

Only use Dictionary lookups.

---

## Definition of Done

- Conveyors register successfully.
- Newly placed conveyor finds neighbours.
- Existing neighbours refresh correctly.
- Dictionary lookups only.
- Project compiles.

Stop after implementation.
