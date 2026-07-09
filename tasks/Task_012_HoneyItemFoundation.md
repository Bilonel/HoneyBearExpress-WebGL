# Task 012 - Honey Item Foundation

## Goal

Create the logical item system.

No visuals.

No Hive.

No Extractor.

No movement.

Only the data model.

---

## Files

Create:

- HoneyItem.cs

---

## HoneyItem

Create a plain C# class.

Fields:

- int Id
- Conveyor CurrentConveyor

Constructor:

HoneyItem(int id)

Methods:

SetCurrentConveyor(Conveyor conveyor)

---

## Rules

Do NOT inherit MonoBehaviour.

Do NOT instantiate GameObjects.

Do NOT use Update.

Do NOT use TickManager.

This is gameplay data only.

---

## Definition of Done

- HoneyItem exists.
- Stores unique id.
- Stores current conveyor.
- No Unity dependencies except Conveyor reference.

Stop after implementation.
