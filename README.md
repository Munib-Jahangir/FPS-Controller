# Unity FPS Controller (Polished)

A high-performance, polished First-Person Shooter controller for Unity. Designed for a premium feel with smooth movement, advanced jump mechanics, and procedural camera effects.

## ✨ Key Features

*   **Premium Movement**: Utilizes `SmoothDamp` for non-instant acceleration and deceleration, giving the player a sense of weight.
*   **Advanced Jump System**:
    *   **Jump Buffering**: Input is remembered for a short duration before landing, triggering an instant jump upon touchdown.
    *   **Coyote Time**: Allows the player to jump for a split second after walking off a ledge.
    *   **Snappy Gravity**: Balanced gravity values for a responsive "FPS" feel.
*   **Procedural Camera Motion**:
    *   **Smooth Head Bob**: Subtle sinusoidal motion while walking and sprinting.
    *   **Dynamic FOV Kick**: Field of view smoothly expands when sprinting to enhance the sense of speed.
*   **Polished Mouse Look**: Smooth interpolation for camera rotation to eliminate micro-stutters.
*   **Easy Integration**: Modular C# scripts that can be dropped into any project.

## 🚀 Getting Started

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Munib-Jahangir/FPS-Controller.git
   ```
2. Open the project in Unity (Version 2021.3+ recommended).
3. Open `Assets/Scenes/FPS Scene.unity`.

### Setup
*   Assign the `PlayerMovement` script to your Player Capsule (requires a `CharacterController`).
*   Assign the `MouseLook` script to your Main Camera.
*   Ensure the `PlayerCamera` reference is set in the `PlayerMovement` component for Head Bob and FOV effects.

## 🛠️ Configuration

| Parameter | Description |
| :--- | :--- |
| **Walk/Sprint Speed** | Adjusted for standard FPS pacing. |
| **Jump Buffer Time** | Window size for jump input memory. |
| **Coyote Time** | Grace period for ledge jumps. |
| **Smoothness** | Interpolation speed for mouse and movement. |

## 📄 License
This project is open-source and available under the [MIT License](LICENSE).
