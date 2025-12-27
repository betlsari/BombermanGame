🚀 Bomberman Multiplayer - Quick Start Guide
⚡ Quick Start
Windows Users
batch# 1. Setup
setup.bat
# 2. Run the Game
run.bat
Linux/Mac Users
bash# 1. Grant execution permission
chmod +x setup.sh run.sh
# 2. Setup
./setup.sh
# 3. Run the Game
./run.sh
```

---

## 🎮 First Game

### 1. Create Account
```
Main Menu → 2. Register
- Username: hero
- Password: 123456
```

### 2. Login
```
Main Menu → 1. Login
- Username: hero
- Password: 123456
```

### 3. Start Game
```
Game Menu → 1. Start Single Player Game
- Select theme: Desert
- Start!
```

---

## ⌨️ Controls

### Player 1
```
W / ↑   : Move Up
S / ↓   : Move Down
A / ←   : Move Left
D / →   : Move Right
SPACE   : Place Bomb
```

### Player 2 (Two Players)
```
I       : Move Up
K       : Move Down
J       : Move Left
L       : Move Right
ENTER   : Place Bomb
```

### General
```
ESC     : Return to Menu
U       : Undo

🎯 Game Mechanics
Basic Rules

Bombs explode after 3 seconds
Explosion spreads in 4 directions
Don't touch enemies!
Destroy walls to find power-ups

Power-ups

B = Bomb count +1
P = Bomb power +1
S = Speed increase +1

Enemies

E = Static (doesn't move)
C = Chaser (simple AI)
A = Smart (A* algorithm)


🌐 Multiplayer (Online)
As Host

Game Menu → 3. Multiplayer


Host Game


Give your IP address to friend
Wait...

As Client

Game Menu → 3. Multiplayer


Join Game


Enter host's IP
Connect!

Find Your IP:
bash# Windows
ipconfig
# Linux/Mac
ifconfig

❓ Troubleshooting
"dotnet not found"
bash# Download .NET 7.0 SDK
https://dotnet.microsoft.com/download
"Database error"
bash# Reset database
del bomberman.db  # Windows
rm bomberman.db   # Linux/Mac
# Run again
dotnet run
```

### "Port already in use" (Multiplayer)
```
- Try a different port (e.g., 9998, 10000)
- Or close the other program
```

### "Connection failed" (Multiplayer)
```
✓ Are both computers on the same network?
✓ Is firewall allowing connection?
✓ Is the IP address correct?
✓ Did host start first?

📚 More Information

README.md - Overview and features
DesignDocument.md - Design patterns explanation
UMLDiagrams.md - UML diagrams

💡 Tips
Strategy

Don't get cornered!
Escape before bomb explodes
Collect power-ups
Learn enemy patterns

Increase Score

Destroy wall: +10
Kill enemy: +50
Collect power-up: +25
Finish quickly: Bonus!


🏆 First Goals

 Complete first game
 Score 500+
 Kill 5 enemies
 Try all power-ups
 Play online with friend