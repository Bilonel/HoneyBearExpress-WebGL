# Task 014 - Conveyor System

## Goal

Create a central system that updates all conveyors on every tick.

No visuals.

No Hive.

No Extractor.

---

## Files

Create:

- ConveyorSystem.cs

Modify:

- ConveyorRegistry.cs
- TickManager.cs

---

## ConveyorRegistry

Expose:

IReadOnlyCollection<Conveyor> Conveyors

---

## ConveyorSystem

MonoBehaviour

References:

- TickManager
- ConveyorRegistry

On Awake:

Subscribe to TickManager.OnTick

On Destroy:

Unsubscribe

On Tick:

Loop through every conveyor.

If conveyor.HasItem == false

continue

If OutputConveyor == null

continue

If OutputConveyor.HasItem

continue

Transfer:

HoneyItem item = conveyor.RemoveItem();

OutputConveyor.TryInsert(item);

Nothing else.

---

## Rules

Do NOT use Update.

Do NOT use Coroutines.

Do NOT use Physics.

One Tick = One Conveyor Update.

---

## Definition of Done

- ConveyorSystem receives Tick events.
- Every tick moves items forward one conveyor.
- No allocations inside tick loop.
- Project compiles.

Stop after implementation.
