# 🎮 FPS Pro Framework

A professional, production-ready First Person Controller for Unity with advanced parkour mechanics. Perfect for fast-paced FPS games, parkour games, or any game requiring smooth, responsive first-person movement.

![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-blue)
![License](https://img.shields.io/badge/License-Custom-green)
![Platform](https://img.shields.io/badge/Platform-PC%2FMobile-orange)

---

## ✨ Features

### 🏃 Movement System
- **Sprint** - Hold Shift to sprint (2x speed)
- **Air Control** - Full movement control while airborne
- **Crouch** - Hold C to crouch and move slowly

### 🦘 Jump Mechanics
- **Single Jump** - Press Space to jump
- **Double Jump** - Press Space again in mid-air for double jump

### 🧗 Parkour System
- **Wall Run** - Run along walls by holding Shift while running into a wall
- **Wall Jump** - Press Space while wall running to jump off walls
- **Sliding** - Press C to slide with momentum

### 🎯 Camera System
- **Mouse Look** - Smooth mouse rotation with adjustable sensitivity
- **Look Clamping** - Configurable vertical look limits (default: 85°)
- **Dynamic FOV** - Ready for speed effects

### ⚙️ Customization
- All values exposed in Inspector
- Easy setup with CharacterController
- Clean, readable C# code with comments

---

## 📦 Installation

1. Clone or download this repository
2. Open in Unity 2022.3 or later
3. Import required packages:
   - **ProBuilder** (included in Unity)
   - **TextMeshPro** (included in Unity)

---

## 🚀 Quick Setup

### Creating a Player

1. Create a new Scene
2. Create an Empty GameObject named "Player"
3. Add a **Capsule** as a child (for visual representation)
4. Add a **Camera** as a child (position at eye level)
5. Add these components to the Player:
   - `CharacterController`
   - `ParkourFPSController`

### Configuration

In the `ParkourFPSController` component:

| Setting | Default | Description |
|---------|---------|-------------|
| Walk Speed | 8 | Normal movement speed |
| Sprint Speed | 16 | Speed when holding Shift |
| Air Speed | 12 | Movement speed while in air |
| Wall Run Speed | 20 | Speed while wall running |
| Slide Speed | 25 | Initial slide speed |
| Jump Force | 12 | First jump power |
| Double Jump Force | 10 | Second jump power |
| Mouse Sensitivity | 2.5 | Camera rotation speed |
| Look X Limit | 85 | Vertical look clamp |

---

## 🎮 Controls

| Key | Action |
|-----|--------|
| **W A S D** | Move |
| **Shift** | Sprint / Wall Run |
| **Space** | Jump / Double Jump / Wall Jump |
| **C** | Crouch / Slide |
| **Mouse** | Look around |
| **Esc** | Unlock cursor (in editor) |

---

## 📁 Project Structure

```
Assets/
├── FPS Pro Framework/
│   ├── Scripts/
│   │   ├── ParkourFPSController.cs    # Main controller
│   │   └── FPSController.cs           # Basic controller (legacy)
│   ├── Prefabs/                       # Ready-to-use prefabs
│   ├── Materials/                     # Custom materials
│   ├── Scenes/                        # Demo scenes
│   └── UI/                            # UI elements
├── Scenes/
│   └── FPS Scene.unity                # Main demo scene
└── Project Settings/
```

---

## 🔧 Requirements

- **Unity Version**: 2022.3 LTS or later
- **Packages**:
  - ProBuilder (for level design)
  - TextMeshPro (for UI)
- **Build Target**: PC, Mobile, WebGL

---

## 📝 Changelog

### v1.0.0 (Initial Release)
- Basic movement (Walk, Sprint)
- Jump & Double Jump
- Wall Run & Wall Jump
- Sliding & Crouching
- Mouse look with sensitivity control

---

## 🤝 Support

For issues, questions, or contributions:
- Create an Issue on GitHub
- Fork and submit Pull Requests

---

## 📄 License

This project is provided as-is for learning and commercial use. Credit is appreciated but not required.

---

## 🔮 Upcoming Features

- [ ] Weapon System
- [ ] Animations
- [ ] Mobile Touch Controls
- [ ] Head Bobbing
- [ ] Ladder Climbing
- [ ] Zipline Mechanics

---

**Made with ❤️ for Unity Developers**

[![GitHub Stars](https://img.shields.io/github/stars/Munib-Jahangir/FPS-Controller?style=social)](https://github.com/Munib-Jahangir/FPS-Controller/stargazers)
