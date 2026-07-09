# Task 013 - Conveyor Inventory

## Goal

Allow conveyors to hold exactly one HoneyItem.

No movement yet.

---

## Files

Modify:

- Conveyor.cs

---

## Conveyor

Add:

private HoneyItem _currentItem;

Property:

public bool HasItem

public HoneyItem CurrentItem

Methods:

bool TryInsert(HoneyItem item)

HoneyItem RemoveItem()

Rules:

TryInsert

- returns false if occupied
- stores item
- item.SetCurrentConveyor(this)

RemoveItem

- returns current item
- clears conveyor

---

## Rules

One item maximum.

No TickManager.

No Update.

No movement.

No visuals.

---

## Definition of Done

- Conveyor can store one HoneyItem.
- TryInsert works.
- RemoveItem works.
- HoneyItem.CurrentConveyor updates correctly.
- Project compiles.

Stop after implementation.
