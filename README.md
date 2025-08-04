# UnityTimelapse

Simple script designed to quickly create timelapses in Unity HDRP. Ideal for **presenting scenes, cinematics, or testing lighting variations** without using Timeline. **Version 0.2.0** -> Has only a directional light rotation, volumetric cloud offset animation ans water simulation speed.

> This project is made/test under Unity 6.1 (6000.1.12f1)

[**Download the latest version here**](Runtime/Timelapse.cs)

![Version](https://img.shields.io/badge/version-v0.2.0-blue)
![License](https://img.shields.io/github/license/cfrBernard/UnityTimelapse)

## Features:
- Interpolated sun rotation (Directional Light)
- Volumetric Clouds offset animation
- Water simulation speed (HDRP Water)
- Customizable duration, loop option

> ### Requirements
> - Volumetric Clouds (HDRP) if enable
> - WaterSurface (HDRP) if enable

---

<p align="center">
  <img src="Assets/Demo/DemoGif_v0.2.0.gif" alt="DemoGIF v0.2.0" />
</p>

---

## How to Use:

- Copy the `Timelapse.cs` script into your Unity project.
- Make sure to use **HDRP** with the necessary components (**Volumetric Clouds** / **WaterSurface**) – *if enable*

## Setup

1. Create an empty **GameObject** and attach the script to it.
2. Assign:
    - The **Directional Light** (the sun)
    - The **Volume** containing the *Volumetric Clouds* component
    - The **WaterSurface** (if used, and loop disabled)

3. Define:
    - `Cycle Duration` in seconds
    - Enable/disable Loop, Clouds, Sun, Water as needed
    - `Sun Rotation Start` and `Sun Rotation End` (in degrees, Euler)
    - `Cloud Offset Start` and `Cloud Offset End`
    - And the Water Surface – `Time Multiplier`


> If Loop is true, water management is automatically disabled.

---

## 🔮 Roadmap (v0.3.x → v1.0.0)

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
    - Custom curves (non-linear)
    - Custom editor (cleaner interface in the Inspector)

- **Official Unity package**
    - UPM structure (`package.json`)
    - Demo scene + presets
    - Asset Store release (v1.0.0)

---

## 🤝 Contact:
For issues, suggestions, or contributions, feel free to open an issue on the GitHub repository.
