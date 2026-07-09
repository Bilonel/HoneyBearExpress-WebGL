# Task 015 - Building Processor Base

## Goal

Create a common base class for all production buildings.

Future buildings such as Hive, Honey Extractor and Shipping Dock will inherit from this class.

No production logic yet.

---

## Files

Create:

- BuildingProcessor.cs

---

## BuildingProcessor

Create an abstract MonoBehaviour.

SerializeField:

- TickManager tickManager

Lifecycle:

Awake()

- Validate TickManager
- Subscribe to TickManager.OnTick

OnDestroy()

- Unsubscribe from TickManager.OnTick

Tick Handling

When TickManager raises OnTick:

Call:

protected abstract void ProcessTick(long tick);

Do not implement any gameplay logic.

---

## Rules

- No Update()
- No Coroutines
- No HoneyItem references
- No Conveyor references
- No production logic
- Keep implementation minimal

---

## Definition of Done

- BuildingProcessor exists.
- Automatically subscribes to TickManager.
- Calls ProcessTick() every tick.
- Future buildings can inherit from it.
- Project compiles.

Stop after implementation.
