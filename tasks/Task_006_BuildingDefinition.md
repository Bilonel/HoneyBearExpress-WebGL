# Task 006 - Building Definition

## Goal

Create the data definition used for every placeable building.

This is a data-only task.

No gameplay.

No placement.

No production.

---

## Files

Create:

Scripts/Buildings/BuildingDefinition.cs

Scripts/Buildings/BuildingType.cs

---

## BuildingDefinition

Implement as a ScriptableObject.

CreateAssetMenu should be enabled.

---

## Fields

Building Name (string)

Building Type (enum)

Prefab (GameObject)

Grid Size (Vector2Int)

Can Rotate (bool)

---

## Rules

This object stores only configuration data.

It must not contain gameplay logic.

Do not add methods except validation if absolutely necessary.

No Update.

No Tick.

No Production.

No Runtime State.

---

## BuildingType

Create an enum.

Values:

Hive

Conveyor

HoneyExtractor

ShippingDock

---

## Definition of Done

Project compiles.

A BuildingDefinition asset can be created from the Unity menu.

No gameplay has been implemented.

Stop after this task.
