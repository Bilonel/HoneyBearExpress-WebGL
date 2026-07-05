# Honey Bear Express Architecture

## Philosophy
Keep it simple. Ship a polished CrazyGames browser game.

## Layers
- Presentation: Camera, UI, Audio, VFX
- Simulation: TickManager, Buildings, Items, Economy
- Data: ScriptableObjects
- Services: Grid, Save, Scene Loading

## Tick Rules
Simulation runs on TickManager.
Presentation runs on Unity Update.
Default tick rate: 10 ticks/sec (configurable).

## Gameplay uses Tick
- Beehive
- Conveyor
- Items
- Machines
- Economy
- Orders

## Update is allowed only for
- Camera
- Input
- UI
- Animation
- VFX

## Grid
Everything snaps to a square grid.

## Data
Never hardcode gameplay values. Use ScriptableObjects.

## Performance
Target: WebGL.
Avoid GameObject.Find, FindObjectOfType and LINQ in gameplay.
