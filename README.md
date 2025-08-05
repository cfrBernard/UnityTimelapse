# UnityTimelapse

Simple script designed to quickly create timelapses in Unity HDRP. Ideal for **presenting scenes, cinematics, or testing lighting variations** without using Timeline. **Version 0.3.0** -> Has only a directional light rotation, volumetric cloud offset animation and water simulation speed.

> This project is made/test under Unity 6.1 (6000.1.12f1)

![Version](https://img.shields.io/badge/version-v0.3.0-blue)
![License](https://img.shields.io/github/license/cfrBernard/UnityTimelapse)

## Features:
- Interpolated sun rotation (Directional Light)
- Volumetric Clouds offset animation
- Water simulation speed (HDRP Water)
- Customizable duration, loop option
- Speed curve for smoother transitions

> ### Requirements
> - Volumetric Clouds (HDRP) – *if enable*
> - WaterSurface (HDRP) – *if enable*

---

<p align="center">
  <img src="Assets/Demo/DemoGif_v0.3.0.gif" alt="DemoGIF v0.3.0" />
</p>

---

## How to Use:

- Fork and Copy the project folder (`Runtime/` and `Editor/` are required) into your Unity project.
- Make sure to use **HDRP** with the necessary components (**Volumetric Clouds** / **WaterSurface**) – *if enable*

## Setup

1. Create an empty **GameObject** and attach `Timelapse.cs` from `Runtime/`.
2. Assign:
    - The **Directional Light** (the sun)
    - The **Volume** containing the *Volumetric Clouds* component
    - The **WaterSurface** (if used, and loop disabled)

3. Define:
    - `Cycle Duration` in seconds
    - `Speed Curve` preset (controls global timelapse speed)
      > : X=0→1 is progress, Y=0→1 is adjusted speed)
    - Enable/disable Loop, Clouds, Sun, Water as needed
    - `Sun Rotation Start` and `Sun Rotation End` (in degrees, Euler)
    - `Cloud Offset Start` and `Cloud Offset End`
    - And the Water Surface – `Time Multiplier`


> If Loop is enabled, water management is automatically disabled.

---

## Helper – Curve to Keyframes

A small tool is included to quickly convert an AnimationCurve into C# code (Keyframes):

- Accessible via `Tools > Curve To Keyframes` in the Unity toolbar
- Allows you to draw a curve in the editor
- Clicking "Generate C# Keyframes" automatically copies the code to the clipboard
- Useful for creating static presets in `SpeedCurvePresets.cs`

---

## 🔮 Roadmap (v0.4.x → v1.0.0)

- **Multi-element Control:**
    - Fog (density, color)
    - Lens Flare size/intensity
    - Water (orientation, reflection parameters)

- **Object Management:**
    - Enable/disable objects at specific times
    - E.g., disable fill lights after a certain offset

- **Advanced Options :**
    - Ping-pong (back and forth)
    - Separate timers (clouds ≠ sun)
    - Custom editor (cleaner interface, presets manager)

- **Official Unity package**
    - Demo scene + presets
    - Asset Store release (v1.0.0)

---

## 🤝 Contact:
For issues, suggestions, or contributions, feel free to open an issue on the GitHub repository.
