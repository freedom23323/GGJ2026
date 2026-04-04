# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 2D game project (GGJ2026) featuring an Angel/Demon state-switching mechanic. The player toggles between two forms with different abilities and can interact with NPCs that respond differently based on the current form.

## Build & Run

- Open the project in Unity Hub (Unity 2022.x recommended)
- Load `Assets/Scenes/GameScene.unity` as the startup scene
- Press Play in the Unity Editor to run

## Architecture

### Singleton Managers

| Class | Responsibility |
|-------|----------------|
| `GameManager` | Game state machine (`MainMenu`, `Playing`, `Paused`, `GameOver`, `Victory`), scene reset, ConsumableItem registration |
| `UIManager` | All UI panels, dialogue system (typewriter effect), score/level/key display |
| `MapManager` | Level/map switching via `levels` list in Inspector |
| `ScoreManager` | Score tracking with `OnScoreChanged` event |

### Player State Machine

Located in `Assets/Scripts/Player/PlayerController.cs`:
- `IPlayerState` interface with `EnterState()` and `ExecuteSkill()`
- `AngelState` — default form, visible in normal camera
- `DemonState` — toggled via `Space`, reveals hidden "Demon" layer, skill destroys nearby objects

Controls: WASD/Arrow keys to move, `Space` to toggle Angel/Demon, `K` to use skill, `F` to interact with NPCs.

### Dialogue System

NPCs have `InteractableNPC` component with separate `angelDialogue` and `demonDialogue` fields. When player presses `F` near an NPC, `UIManager.ShowDialogue()` displays the appropriate line based on current player state.

### ConsumableItems

Items like `Key`, `Ball`, `SecretDocument` inherit from `ConsumableItem`. They self-register with `GameManager` on `Start()` so the scene reset system can call `ResetItem()` on all of them.

### Map/Level System

`MapManager.levels` is a list of GameObjects (maps/levels). `LevelPortal` triggers `MapManager.NextLevel()` or `PreviousLevel()` on player collision. The current level index is displayed via `UIManager.UpdateLevelDisplay()`.

## Script Location Conventions

- `Assets/Scripts/Manager/` — singleton managers
- `Assets/Scripts/Player/` — PlayerController, PlayerAnimation
- `Assets/Scripts/NPC/` — InteractableNPC, NPCController, NPCAnimation
- `Assets/Scripts/Map/` — LevelPortal, Key, LockedDoor, ConsumableItem, Ball, ScoreWall, SecretDocument
- `Assets/Scripts/Story/` — StoryBegin, ScrollClick
- `Assets/Scripts/Global/` — enums (GlobalEnum)
- `Assets/Scripts/Tool/` — utility components like DialogBoxConfig, PointGizmo

## Key Patterns

- Singleton via `Instance { get; private set; }` + `DontDestroyOnLoad` on GameManager
- Events: `ScoreManager.OnScoreChanged`, `PlayerController.PlayerExecuteSkillEvent`
- Scene reset: `GameManager.ResetScene()` resets score, map, all ConsumableItems, and player position
- Player start position: set via `startPoint` Transform reference in Inspector
