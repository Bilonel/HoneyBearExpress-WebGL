# Task 002 - Camera

## Goal

Create a camera controller suitable for a WebGL factory game.

The camera is part of the Presentation Layer.

It must never contain gameplay logic.

---

## Requirements

Create a CameraController component.

The camera must support:

- Drag to move
- Mouse wheel zoom

The movement should feel smooth and responsive.

The implementation must work well with future mobile support.

---

## Controls

Desktop

- Hold Left Mouse Button + Drag → Move Camera
- Mouse Wheel → Zoom

Do NOT implement touch controls yet.

The code should be structured so touch input can be added later without changing the camera logic.

---

## Camera Setup

Projection

Perspective

Rotation

X = 35°

Y = 45°

The camera always looks at the game world.

Movement happens on the XZ plane.

The camera height should remain constant.

---

## Zoom

Allow zoom only by changing camera distance.

Do NOT change Field Of View.

Clamp zoom distance.

Minimum: 8

Maximum: 25

Expose these values in the Inspector.

---

## Movement

Movement must be frame-rate independent.

Movement speed must be configurable.

Use Time.deltaTime.

Do not use physics.

---

## Code Style

Create:

Scripts/Core/CameraController.cs

One class only.

No singleton.

No manager.

No gameplay references.

---

## Future Compatibility

The controller should be easy to extend with:

- Touch drag
- Pinch zoom
- Camera bounds

Do not implement these features now.

---

## Do NOT

Do not create an InputService yet.

Do not create Cinemachine.

Do not create camera shake.

Do not create rotation controls.

Do not follow the player.

Do not create camera bounds.

Do not add smoothing systems beyond simple interpolation if needed.

---

## Definition of Done

- Camera can be dragged with the left mouse button.
- Mouse wheel zoom works.
- Zoom is clamped.
- Camera remains at 35° / 45°.
- Movement is smooth.
- Project compiles without warnings.
- Stop after completing this task.
