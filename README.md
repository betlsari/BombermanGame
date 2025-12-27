# 🎮 Bomberman Multiplayer - Console Edition

## 📋 Project Information

**Project Name**: Bomberman Multiplayer (Console Edition)  
**Course**: Design Patterns - 2025  
**Instructor**: Prof. Dr. Doğan Aydın  
**University**: Izmir Katip Celebi University  
**Developers**: Betül Sarı, Ece Akın  
**Submission Date**: December 28, 2025 24:00  
**Submission Platform**: IKCU UBS  
**Email**: dogan.aydin@ikc.edu.tr

---

## 🎯 Project Summary

This project is a console-based Bomberman game developed using **10 different design patterns**. Built on MVC architecture with SQLite database for data management and multiplayer support, it's a professional software project.

### 🏆 Key Features
- ✅ **10 Design Pattern** implementation
- ✅ **A* Pathfinding** algorithm
- ✅ **SignalR Online Multiplayer** support
- ✅ **Multiplayer Lobby System**
- ✅ **3 Different Themes** system (Adapter Pattern)
- ✅ **Professional UI/UX**
- ✅ **Sound System** (Observer Pattern integration)
- ✅ **SQLite Database** for persistent data
- ✅ **BCrypt** password security
- ✅ **Comprehensive UML Diagrams** (13+ diagrams)
- ✅ **Undo/Redo** support (Command Pattern)

---

## 🛠️ Technology Stack

| Category | Technology | Version | Purpose |
|----------|-----------|---------|---------|
| **Language** | C# | .NET 7.0+ | Main programming language |
| **Database** | SQLite | 3.x | Persistent data storage |
| **ORM** | Dapper | 2.1.66 | Object-Relational Mapping |
| **Password Hash** | BCrypt.Net-Next | 4.0.3 | Secure password storage |
| **Network** | SignalR | 8.0.0 | Real-time multiplayer |
| **Audio** | Console.Beep | Native | Sound effects |

---

## 📊 Design Patterns (10 Total)

### 🔷 Creational Patterns (2/2)

#### 1. **Factory Method Pattern** ⭐
**Location**: `src/Patterns/Creational/Factory/`  
**Purpose**: Dynamically create different enemy types

**Classes**:
- `IEnemyFactory` (Interface)
- `StaticEnemyFactory` - Static enemy
- `ChaseEnemyFactory` - Chasing enemy
- `SmartEnemyFactory` - Smart enemy (A*)
- `EnemyFactoryProvider` (Factory Provider)

**Usage**:
```csharp
IEnemyFactory factory = EnemyFactoryProvider.GetFactory("smart");
Enemy enemy = factory.CreateEnemy(id, position);
```

**Advantages**:
- ✅ Easy to add new enemy types
- ✅ Enemy creation logic is isolated
- ✅ Open/Closed Principle

---

#### 2. **Singleton Pattern** ⭐
**Location**: `src/Database/DatabaseManager.cs`, `src/Core/GameManager.cs`, `src/Audio/SoundManager.cs`  
**Purpose**: Single instance management (thread-safe)

**3 Singleton Implementations**:
1. **DatabaseManager** - Database connection management
2. **GameManager** - Game state and Observer hub
3. **SoundManager** - Sound effects management

**Features**:
- Double-check locking
- Lazy initialization
- Thread-safe

**Usage**:
```csharp
var gameManager = GameManager.Instance;
var dbManager = DatabaseManager.Instance;
var soundManager = SoundManager.Instance;
```

---

### 🔶 Structural Patterns (2/2)

#### 3. **Decorator Pattern** ⭐
**Location**: `src/Patterns/Structural/Decorator/`  
**Purpose**: Add dynamic features to players at runtime

**Classes**:
- `IPlayer` (Component Interface)
- `PlayerDecorator` (Base Decorator)
- `BombCountDecorator` (+bomb count)
- `BombPowerDecorator` (+bomb power)
- `SpeedBoostDecorator` (+speed)

**Usage**:
```csharp
IPlayer player = new Player(1, "Hero", position);
player = new BombCountDecorator(player, +2);
player = new SpeedBoostDecorator(player, +1);
player = new BombPowerDecorator(player, +1);
```

**Power-up Collection Flow**:
1. Player moves to same position as power-up
2. `CheckPowerUpCollection()` is triggered
3. `ApplyPowerUpWithDecorator()` adds decorator
4. Observers are notified
5. UI updates

---

#### 4. **Adapter Pattern** ⭐
**Location**: `src/Patterns/Structural/Adapter/`  
**Purpose**: Use different theme systems through common interface

**Themes**:
- **Desert**: Yellow/brown tones, sand/stone walls
- **Forest**: Green tones, tree/wood walls
- **City**: Gray tones, concrete/brick walls

**Adapter Structure**:
```
ITheme (Target Interface)
├── DesertThemeAdapter → DesertTheme (Adaptee)
├── ForestThemeAdapter → ForestTheme (Adaptee)
└── CityThemeAdapter → CityTheme (Adaptee)
```

**Usage**:
```csharp
ITheme theme = ThemeFactory.GetTheme("desert");
ConsoleColor wallColor = theme.GetBreakableWallColor();
char wallChar = theme.GetBreakableWallChar();
```

---

### 🔵 Behavioral Patterns (4/4)

#### 5. **Strategy Pattern** ⭐
**Location**: `src/Patterns/Behavioral/Strategy/`  
**Purpose**: Change enemy movement algorithms at runtime

**Strategies**:
- `StaticMovementStrategy` - Never moves
- `RandomMovementStrategy` - Random direction
- `ChaseMovementStrategy` - Simple chase (Manhattan distance)
- `PathfindingMovementStrategy` - Optimal path with A* 🌟

**Usage**:
```csharp
enemy.MovementStrategy = new PathfindingMovementStrategy();
enemy.Move(map, playerPosition);
```

---

#### 6. **Observer Pattern** ⭐
**Location**: `src/Patterns/Behavioral/Observer/`  
**Purpose**: Listen to and react to game events

**Event Types**:
- `BombExploded`, `PlayerDied`, `PowerUpCollected`
- `WallDestroyed`, `EnemyKilled`, `GameEnded`

**Observers**:
- `ScoreObserver` - Score tracking and calculation
- `StatsObserver` - Statistics recording
- `UIObserver` - Console messages
- `SoundObserver` - Sound effects

**Usage**:
```csharp
gameManager.Attach(new ScoreObserver());
gameManager.Attach(new SoundObserver());
gameManager.Notify(new GameEvent(EventType.WallDestroyed));
```

**Event Flow**:
```
GameManager (Subject) 
    → Notify(GameEvent) 
    → All Observers.Update(GameEvent)
        → ScoreObserver: Update score
        → StatsObserver: Save to DB
        → UIObserver: Show message
        → SoundObserver: Play sound
```

---

#### 7. **State Pattern** ⭐
**Location**: `src/Patterns/Behavioral/State/`  
**Purpose**: Manage player states

**States**:
- `AliveState` - Player alive (normal gameplay)
- `DeadState` - Player dead (cannot move)
- `WinnerState` - Player won (game over)

**State Transitions**:
```
AliveState 
    → TakeDamage() → DeadState
    → Win() → WinnerState
```

**Usage**:
```csharp
player.State = new AliveState();
player.State.Move(player, dx, dy, map); // Can move
player.State.TakeDamage(player); // Transitions to DeadState
player.State.Move(player, dx, dy, map); // Cannot move
```

---

#### 8. **Command Pattern** ⭐
**Location**: `src/Patterns/Behavioral/Command/`  
**Purpose**: Encapsulate player actions and undo support

**Commands**:
- `ICommand` (Interface)
- `MoveCommand` - Movement command
- `PlaceBombCommand` - Bomb placement command
- `CommandInvoker` - Command manager

**Features**:
- ✅ Undo/Redo support
- ✅ Command history (history stack)
- ✅ Maximum 10 command records

**Usage**:
```csharp
ICommand moveCmd = new MoveCommand(player, dx, dy, map);
commandInvoker.ExecuteCommand(moveCmd);
commandInvoker.UndoLastCommand(); // Undo with U key
```

---

### 🔸 Architectural & Other Patterns

#### 9. **Repository Pattern** ⭐
**Location**: `src/Patterns/Repository/`  
**Purpose**: Abstract database access

**Repositories**:
- `IRepository<T>` (Generic Interface)
- `UserRepository` - User CRUD
- `StatsRepository` - Statistics CRUD
- `ScoreRepository` - Score CRUD + Top 10
- `PreferencesRepository` - Preferences CRUD

**Usage**:
```csharp
IRepository<User> userRepo = new UserRepository();
User user = userRepo.GetById(1);
userRepo.Update(user);

var topScores = scoreRepo.GetTopScores(10);
statsRepo.IncrementWins(userId);
```

**Advantages**:
- ✅ Data access abstraction
- ✅ Testable code
- ✅ Single Responsibility
- ✅ DRY principle

---

#### 10. **MVC Pattern** ⭐
**Location**: `src/MVC/`  
**Purpose**: Architectural organization (Separation of Concerns)

**Layers**:
- **Model**: `src/Models/`, `src/Core/` - Business logic and data
- **View**: `src/UI/` - Visual presentation
- **Controller**: `src/MVC/Controllers/` - Flow control

**MVC Flow**:
```
User Input 
    → Controller (GameController)
    → Model (GameManager, Player, Bomb)
    → View (GameRenderer)
    → Console Output
```

**Controllers**:
- `GameController` - Single/two player game
- `OnlineGameController` - Online multiplayer
- `InputController` - Keyboard input management

---

## 🎮 Game Features

### ⚡ Core Mechanics
- ✅ **Single player mode** (against AI enemies)
- ✅ **Two player mode** (Local multiplayer)
- ✅ **Online multiplayer** (SignalR)
- ✅ **Multiplayer Lobby System** (Host/Join)
- ✅ **Classic Bomberman rules**
- ✅ **Bombs explode after 3 seconds**
- ✅ **Explosions spread in 4 directions**
- ✅ **Explosion wall control**

---

### 🗺️ Map System

#### Wall Types:
| Symbol | Type | Durability | Description |
|--------|------|------------|-------------|
| `#` | Unbreakable | ∞ | Cannot be destroyed, blocks explosions |
| `▒` | Breakable | 1 | Destroyed with one explosion |
| `▓` | Hard Wall | 3 | Takes 3 explosions to destroy |
| ` ` | Empty Space | 0 | Walkable area |

**Explosion Mechanics**:
```csharp
// src/Models/Map.cs - GetExplosionArea()
// Explosion stops after walls:
// Unbreakable wall: Stops, doesn't pass through
// Breakable wall: Destroys but doesn't pass through
// Empty space: Continues
```

#### Map Dimensions:
- **Width**: 21 tiles
- **Height**: 15 tiles
- **Seed**: Deterministic map generation (multiplayer sync)

---

### 🎁 Power-up System

Power-ups drop from destroyed walls with **30-100% chance**:

| Symbol | Type | Effect | Decorator |
|--------|------|--------|-----------|
| `B` | Bomb Count | Bomb count +1 | BombCountDecorator |
| `P` | Bomb Power | Bomb power +1 | BombPowerDecorator |
| `S` | Speed Boost | Speed +1 | SpeedBoostDecorator |

Added to player at runtime using **Decorator Pattern**.

**Power-up Flow**:
```
Wall Destroyed 
    → Random() < 30% 
    → SpawnPowerUp() 
    → Player Collects 
    → ApplyPowerUpWithDecorator() 
    → Decorator Added 
    → Observer.Notify()
    → UI Updates + Sound Plays
```

---

### 👾 Enemy System

#### Enemy Types:

| Symbol | Type | Behavior | Difficulty | AI | Strategy |
|--------|------|----------|------------|-----|----------|
| `E` | Static | Stands still | ⭐ Easy | None | StaticMovementStrategy |
| `C` | Chase | Simple chase | ⭐⭐ Medium | Manhattan | ChaseMovementStrategy |
| `A` | Smart | Smart A* chase | ⭐⭐⭐ Hard | A* | PathfindingMovementStrategy |

Can be changed at runtime using **Strategy Pattern**.

**Enemy Spawn Locations**:
- Enemy 1 (Static): (10, 7)
- Enemy 2 (Chase): (15, 5)
- Enemy 3 (Smart): (5, 10)

---

### 🎨 Theme System

#### 1. Desert Theme
```
Colors:
- Ground: Yellow (Sand)
- Breakable: DarkYellow (Stone)
- Unbreakable: Gray (Rock)

Characters:
- Ground: ░ (Light shade)
- Breakable: ▒ (Medium shade)
- Unbreakable: ▓ (Dark shade)
```

#### 2. Forest Theme
```
Colors:
- Ground: Green (Grass)
- Breakable: DarkYellow (Log)
- Unbreakable: DarkGreen (Tree)

Characters:
- Ground: · (Dot)
- Breakable: ≡ (Triple line)
- Unbreakable: ♣ (Club)
```

#### 3. City Theme
```
Colors:
- Ground: Gray (Concrete)
- Breakable: Red (Brick)
- Unbreakable: DarkGray (Metal)

Characters:
- Ground: █ (Full block)
- Breakable: ▓ (Dark shade)
- Unbreakable: ■ (Square)
```

---

### 🔊 Sound System

**Location**: `src/Audio/SoundManager.cs` (Singleton)

**Sound Types**:
- `BombPlace` - Bomb placement (400Hz, 100ms)
- `BombExplode` - Explosion (200→150→100Hz)
- `PowerUpCollect` - Power-up collection (440→554→659Hz)
- `PlayerDeath` - Player death (800→600→400Hz)
- `EnemyDeath` - Enemy death (300→250Hz)
- `WallBreak` - Wall breaking (350Hz, 120ms)
- `MenuSelect` - Menu selection (600Hz, 80ms)
- `Victory` - Victory melody (C-D-E-G)
- `GameOver` - Game over melody

**Observer Pattern Integration**:
```csharp
// SoundObserver added to GameManager
gameManager.Attach(new SoundObserver());

// Each event plays sound
gameManager.Notify(EventType.BombExploded);
    → SoundObserver.Update() 
    → SoundManager.PlaySound(SoundType.BombExplode)
```

---

## 🕹️ Gameplay

### ⌨️ Controls

#### Player 1:
```
W / ↑      : Up
S / ↓      : Down
A / ←      : Left
D / →      : Right
SPACE      : Place Bomb
```

#### Player 2 (Two player mode):
```
I          : Up
K          : Down
J          : Left
L          : Right
ENTER      : Place Bomb
```

#### General:
```
ESC        : Pause / Exit
U          : Undo (Undo last move)
```

---

### 🏆 Win Conditions

#### Two Player Mode:
- ✅ Eliminate opponent player
- ✅ Eliminate all enemies

#### Single Player Mode:
- ✅ Eliminate all enemies
- ✅ Survive

---

### 📊 Scoring System

| Action | Points |
|--------|--------|
| Wall Destruction | +10 |
| Enemy Kill | +50 |
| Power-up Collection | +25 |

Score tracking done with **Observer Pattern**.

---

## 💾 Database Structure

### 📋 Tables (4 total)

#### 1. **Users**
```sql
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,  -- BCrypt (salt rounds: 12)
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

#### 2. **GameStatistics**
```sql
CREATE TABLE GameStatistics (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    Wins INTEGER DEFAULT 0,
    Losses INTEGER DEFAULT 0,
    TotalGames INTEGER DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

#### 3. **HighScores**
```sql
CREATE TABLE HighScores (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    Score INTEGER NOT NULL,
    GameDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

#### 4. **PlayerPreferences**
```sql
CREATE TABLE PlayerPreferences (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL UNIQUE,
    Theme TEXT DEFAULT 'Desert',
    SoundEnabled INTEGER DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

### 🔐 Security
- ✅ **BCrypt** password hashing (salt rounds: 12)
- ✅ **SQL Injection** protection (Dapper parameterized queries)
- ✅ **Foreign Key** constraints
- ✅ **Unique** constraints

---

## 🌐 Online Multiplayer

### 🎯 Features
- ✅ SignalR real-time communication
- ✅ Host/Join room system
- ✅ Lobby system with player list
- ✅ Latency measurement (ping)
- ✅ JSON serialization protocol
- ✅ Event-driven architecture
- ✅ Connection management
- ✅ Real-time game state synchronization
- ✅ **Deterministic map generation**

### 📡 Network Protocol

#### Message Types:
```csharp
// SignalR Hub Methods
- CreateRoom(CreateRoomRequest)
- JoinRoom(JoinRoomRequest)
- LeaveRoom(roomId)
- StartGame(roomId)
- PlayerMove(PlayerMoveMessage)
- PlaceBomb(PlaceBombMessage)
- UpdateGameState(roomId, gameState)
- GetRoomList()
```

#### Usage:

**As Host:**
```csharp
// 1. Connect to SignalR Server
await signalRClient.ConnectAsync("http://localhost:5274");

// 2. Create room
var response = await signalRClient.CreateRoomAsync("My Room", "Player1", "Desert", 2);

// 3. Wait for clients
// 4. Start game
await signalRClient.StartGameAsync(roomId);
```

**As Client:**
```csharp
// 1. Connect to SignalR Server
await signalRClient.ConnectAsync("http://localhost:5274");

// 2. Get room list
var rooms = await signalRClient.GetRoomListAsync();

// 3. Join room
await signalRClient.JoinRoomAsync(roomId, "Player2");

// 4. Wait for host to start
```

### 🗺️ Map Synchronization (FIXED)

**Problem**: Client and Host were generating different maps.

**Solution**:
```csharp
// Host generates random seed
_mapSeed = new Random().Next();

// Sends to client
NetworkProtocol.CreateGameStartMessage(theme, _mapSeed);

// Both sides create map with same seed
new Map(21, 15, themeAdapter, _mapSeed);
```

---

## 🚀 Installation and Running

### ✅ Requirements
```
- .NET 7.0 SDK or higher
- Visual Studio 2022 / VS Code / JetBrains Rider (optional)
- Windows / Linux / macOS
- 50 MB free disk space
```

### 📦 Quick Start

#### Windows:
```batch
setup.bat
run.bat
```

#### Linux/Mac:
```bash
chmod +x setup.sh run.sh
./setup.sh
./run.sh
```

### 🔧 Manual Installation

```bash
# 1. Clone or download project
cd BombermanGame

# 2. Install dependencies
dotnet restore

# 3. Build project
dotnet build

# 4. Run
dotnet run
```

### 📋 NuGet Packages

```xml
<PackageReference Include="System.Data.SQLite" Version="1.0.119" />
<PackageReference Include="Dapper" Version="2.1.66" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Microsoft.AspNetCore.SignalR.Client" Version="8.0.0" />
```

---

## 📁 Project Structure

```
BombermanMultiplayer/
│
├── 📁 BombermanGame/                   # Main game project
│   │
│   ├── 📄 Program.cs                   # ⭐ Main entry point
│   ├── 📄 BombermanGame.csproj        # Project configuration
│   ├── 📄 bomberman.db                # SQLite database (runtime)
│   │
│   ├── 📁 src/
│   │   │
│   │   ├── 📁 Core/                    # Core game logic
│   │   │   ├── GameManager.cs         # ⭐ Singleton + Subject
│   │   │   ├── MainMenu.cs            # Main menu
│   │   │   └── LobbySystem.cs         # Multiplayer lobby
│   │   │
│   │   ├── 📁 Database/                # Database layer
│   │   │   ├── DatabaseManager.cs     # ⭐ Singleton
│   │   │   └── DatabaseSchema.sql     # SQL schema
│   │   │
│   │   ├── 📁 Models/                  # Domain models
│   │   │   ├── Player.cs, Bomb.cs, Enemy.cs
│   │   │   ├── Map.cs, Position.cs, PowerUp.cs
│   │   │   ├── IWall.cs, UnbreakableWall.cs
│   │   │   ├── BreakableWall.cs, HardWall.cs
│   │   │   ├── EmptySpace.cs
│   │   │   ├── 📁 Entities/            # Database entities
│   │   │   └── 📁 Network/             # Network DTOs
│   │   │
│   │   ├── 📁 Patterns/                # 🌟 Design patterns
│   │   │   ├── 📁 Creational/Factory/  # ⭐ Factory Method
│   │   │   ├── 📁 Structural/Decorator/# ⭐ Decorator
│   │   │   ├── 📁 Structural/Adapter/  # ⭐ Adapter
│   │   │   ├── 📁 Behavioral/Strategy/ # ⭐ Strategy
│   │   │   ├── 📁 Behavioral/Observer/ # ⭐ Observer
│   │   │   ├── 📁 Behavioral/State/    # ⭐ State
│   │   │   ├── 📁 Behavioral/Command/  # ⭐ Command
│   │   │   └── 📁 Repository/          # ⭐ Repository
│   │   │
│   │   ├── 📁 MVC/Controllers/         # ⭐ MVC Pattern
│   │   ├── 📁 UI/                      # View layer
│   │   ├── 📁 Network/                 # 🌐 SignalR client
│   │   ├── 📁 Audio/                   # 🔊 Sound system
│   │   └── 📁 Utils/                   # A* algorithm
│   │
│   ├── 📁 assets/sounds/               # Sound files
│   ├── 📄 setup.bat / setup.sh
│   └── 📄 run.bat / run.sh
│
├── 📁 BombermanServer/                 # SignalR server project
│   ├── 📄 Program.cs
│   ├── 📁 Hubs/GameHub.cs              # SignalR hub
│   ├── 📁 Services/RoomService.cs
│   ├── 📁 Models/
│   └── 📁 Controllers/
│
├── 📄 BombermanMultiplayer.sln
├── 📄 .gitignore
│
└── 📁 Documentation/
    ├── 📄 README.md                    # ⭐ This file
    ├── 📄 DesignDocument.md            # Design document
    ├── 📄 UMLDiagrams.md               # UML diagrams
    ├── 📄 QUICKSTART.md                # Quick start guide
    └── 📄 SubmissionCheckList.md       # Submission checklist
```

---

## 🔧 Technical Details

### Data Flow

```
┌─────────────────────────────────────────────────────────┐
│                     USER INPUT                          │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              InputController                            │
│  - ProcessInput()                                       │
│  - ProcessMultiplayerInput()                            │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              CommandInvoker                             │
│  - ExecuteCommand()                                     │
│  - UndoLastCommand()                                    │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              GameManager (Singleton)                    │
│  - Players, Bombs, Enemies                              │
│  - CurrentMap                                           │
│  - Notify(GameEvent)                                    │
└────────────────────┬────────────────────────────────────┘
                     │
                     ├─────────────────────────────┐
                     │                             │
                     ▼                             ▼
┌──────────────────────────────┐    ┌──────────────────────────┐
│     Observer Pattern         │    │   Repository Pattern     │
│  - ScoreObserver             │    │  - UserRepository        │
│  - StatsObserver             │    │  - StatsRepository       │
│  - UIObserver                │    │  - ScoreRepository       │
│  - SoundObserver             │    └──────────┬───────────────┘
└──────────────┬───────────────┘               │
               │                               ▼
               ▼                    ┌──────────────────────────┐
┌──────────────────────────┐       │  DatabaseManager         │
│     GameRenderer         │       │  (Singleton)             │
│  - Render()              │       │  - SQLite Connection     │
│  - RenderMap()           │       └──────────────────────────┘
└──────────────┬───────────┘
               │
               ▼
┌─────────────────────────────────────────────────────────┐
│                   CONSOLE OUTPUT                        │
└─────────────────────────────────────────────────────────┘
```

---

## 🔗 Useful Links

### Documentation
- [Quick Start](QUICKSTART.md) - Start in 5 minutes


### References
- [Head First Design Patterns](https://www.oreilly.com/library/view/head-first-design/0596007124/)
- [Refactoring Guru](https://refactoring.guru/design-patterns)
- [Game Programming Patterns](https://gameprogrammingpatterns.com/)
- [Microsoft C# Docs](https://docs.microsoft.com/en-us/dotnet/csharp/)

---

## ❓ Frequently Asked Questions (FAQ)

### Installation

**Q: How to install .NET SDK?**  
A: Download and install .NET 7.0 SDK from [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).

**Q: Game won't start?**  
A: Run `dotnet restore`, `dotnet clean`, `dotnet build` commands in order.

### Gameplay

**Q: Two player controls?**  
A: P1: WASD+Space | P2: IJKL+Enter

**Q: How to use undo?**  
A: Press `U` key during game.

### Multiplayer

**Q: How does online multiplayer work?**  
A: Start server, Host creates room, Client joins.

---

## 🎊 Final Notes

This project was developed to demonstrate **Design Patterns** learned theoretically in a practical application. It shows when and how to use each pattern and what advantages they provide.

**Enjoy the game and learn the patterns! 🎮💣**

---

<div align="center">

### 🎮 BOMBERMAN MULTIPLAYER 💣

**Design Patterns in Action**

Betül Sarı, Ece Akın  
Izmir Katip Celebi University  
2025

**Email**: dogan.aydin@ikc.edu.tr

</div>
