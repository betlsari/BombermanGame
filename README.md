# 🎮 Bomberman Multiplayer - Console Edition

## 📋 Proje Bilgileri

**Proje Adı**: Bomberman Multiplayer (Console Edition)  
**Ders**: Design Patterns - 2025  
**Öğretim Görevlisi**: Prof. Dr. Doğan Aydın  
**Üniversite**: İzmir Kâtip Çelebi Üniversitesi  
**Geliştirici**: Betül Sarı  
**Teslim Tarihi**: 28.12.2025 24:00  
**Teslim Platformu**: İKCÜ UBS  
**Email**: dogan.aydin@ikc.edu.tr

---

## 🎯 Proje Özeti

Bu proje, **10 farklı tasarım kalıbı** kullanarak geliştirilmiş konsol tabanlı bir Bomberman oyunudur. MVC mimarisi üzerine inşa edilmiş, SQLite veritabanı ile veri yönetimi yapan, çok oyunculu destekli profesyonel bir yazılım projesidir.

### 🏆 Öne Çıkan Özellikler
- ✅ **10 Design Pattern** implementasyonu (Gerekli: 8, Bonus: +2)
- ✅ **A* Pathfinding** algoritması (BONUS +5)
- ✅ **Network Multiplayer** desteği (BONUS +5)
- ✅ **Multiplayer Lobby System** (BONUS +5)
- ✅ **3 Farklı Tema** sistemi (Adapter Pattern)
- ✅ **Profesyonel UI/UX** (BONUS +5)
- ✅ **Sound System** (Singleton + Observer Pattern)
- ✅ **SQLite Database** ile kalıcı veri
- ✅ **BCrypt** şifre güvenliği
- ✅ **Kapsamlı UML Diyagramları** (10+ diyagram)
- ✅ **Undo/Redo** desteği (Command Pattern)

**TOPLAM PUAN**: 70 (kod) + 30 (dokümantasyon) + 25 (bonus) = **125/100** 🏆

---

## 🛠️ Teknoloji Stack

| Kategori | Teknoloji | Versiyon | Amaç |
|----------|-----------|----------|------|
| **Dil** | C# | .NET 7.0+ | Ana programlama dili |
| **Database** | SQLite | 3.x | Kalıcı veri depolama |
| **ORM** | Dapper | 2.1.66 | Object-Relational Mapping |
| **Password Hash** | BCrypt.Net-Next | 4.0.3 | Güvenli şifre saklama |
| **Serialization** | System.Text.Json | Built-in | Network mesaj serileştirme |
| **Network** | TCP Sockets | Native | Multiplayer iletişim |
| **Audio** | Console.Beep | Native | Ses efektleri |

---

## 📊 Tasarım Kalıpları (10 Adet)

### 🔷 Creational Patterns (2/2)

#### 1. **Factory Method Pattern** ⭐
**Dosya**: `src/Patterns/Creational/Factory/`  
**Amaç**: Farklı düşman türlerini dinamik olarak oluşturma

**Sınıflar**:
- `IEnemyFactory` (Interface)
- `StaticEnemyFactory` - Sabit düşman
- `ChaseEnemyFactory` - Takipçi düşman
- `SmartEnemyFactory` - Akıllı düşman (A*)
- `EnemyFactoryProvider` (Factory Provider)

**Kullanım**:
```csharp
IEnemyFactory factory = EnemyFactoryProvider.GetFactory("smart");
Enemy enemy = factory.CreateEnemy(id, position);
```

**Avantajlar**:
- ✅ Yeni düşman tipleri eklemek kolay
- ✅ Düşman yaratma mantığı izole
- ✅ Open/Closed Principle

---

#### 2. **Singleton Pattern** ⭐
**Dosya**: `src/Database/DatabaseManager.cs`, `src/Core/GameManager.cs`, `src/Audio/SoundManager.cs`  
**Amaç**: Tek instance yönetimi (thread-safe)

**3 Singleton Implementasyonu**:
1. **DatabaseManager** - Veritabanı bağlantı yönetimi
2. **GameManager** - Oyun durumu ve Observer hub
3. **SoundManager** - Ses efektleri yönetimi

**Özellikler**:
- Double-check locking
- Lazy initialization
- Thread-safe

**Kullanım**:
```csharp
var gameManager = GameManager.Instance;
var dbManager = DatabaseManager.Instance;
var soundManager = SoundManager.Instance;
```

---

### 🔶 Structural Patterns (2/2)

#### 3. **Decorator Pattern** ⭐
**Dosya**: `src/Patterns/Structural/Decorator/`  
**Amaç**: Runtime'da oyunculara dinamik özellikler ekleme

**Sınıflar**:
- `IPlayer` (Component Interface)
- `PlayerDecorator` (Base Decorator)
- `BombCountDecorator` (+bomba sayısı)
- `BombPowerDecorator` (+bomba gücü)
- `SpeedBoostDecorator` (+hız)

**Kullanım**:
```csharp
IPlayer player = new Player(1, "Hero", position);
player = new BombCountDecorator(player, +2);
player = new SpeedBoostDecorator(player, +1);
player = new BombPowerDecorator(player, +1);
```

**Power-up Toplama Akışı**:
1. Oyuncu power-up ile aynı pozisyona gelir
2. `CheckPowerUpCollection()` tetiklenir
3. `ApplyPowerUpWithDecorator()` decorator ekler
4. Observer'lar bilgilendirilir
5. UI güncellenir

---

#### 4. **Adapter Pattern** ⭐
**Dosya**: `src/Patterns/Structural/Adapter/`  
**Amaç**: Farklı tema sistemlerini ortak interface'den kullanma

**Temalar**:
- **Desert**: Sarı/kahverengi tonlar, kum/taş duvarlar
- **Forest**: Yeşil tonlar, ağaç/odun duvarlar
- **City**: Gri tonlar, beton/tuğla duvarlar

**Adapter Yapısı**:
```
ITheme (Target Interface)
├── DesertThemeAdapter → DesertTheme (Adaptee)
├── ForestThemeAdapter → ForestTheme (Adaptee)
└── CityThemeAdapter → CityTheme (Adaptee)
```

**Kullanım**:
```csharp
ITheme theme = ThemeFactory.GetTheme("desert");
ConsoleColor wallColor = theme.GetBreakableWallColor();
char wallChar = theme.GetBreakableWallChar();
```

---

### 🔵 Behavioral Patterns (4/4)

#### 5. **Strategy Pattern** ⭐
**Dosya**: `src/Patterns/Behavioral/Strategy/`  
**Amaç**: Düşman hareket algoritmalarını runtime'da değiştirme

**Stratejiler**:
- `StaticMovementStrategy` - Hiç hareket etmez
- `RandomMovementStrategy` - Rastgele yön seçer
- `ChaseMovementStrategy` - Basit takip (Manhattan distance)
- `PathfindingMovementStrategy` - A* ile optimal yol bulma 🌟

**Kullanım**:
```csharp
enemy.MovementStrategy = new PathfindingMovementStrategy();
enemy.Move(map, playerPosition);
```

---

#### 6. **Observer Pattern** ⭐
**Dosya**: `src/Patterns/Behavioral/Observer/`  
**Amaç**: Oyun olaylarını dinleme ve tepki verme

**Event Tipleri**:
- `BombExploded`, `PlayerDied`, `PowerUpCollected`
- `WallDestroyed`, `EnemyKilled`, `GameEnded`

**Observer'lar**:
- `ScoreObserver` - Skor takibi ve hesaplama
- `StatsObserver` - İstatistik kaydetme
- `UIObserver` - Konsol mesajları
- `SoundObserver` - Ses efektleri (**YENİ** 🔊)

**Kullanım**:
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
        → ScoreObserver: Skor güncelle
        → StatsObserver: DB'ye kaydet
        → UIObserver: Mesaj göster
        → SoundObserver: Ses çal
```

---

#### 7. **State Pattern** ⭐
**Dosya**: `src/Patterns/Behavioral/State/`  
**Amaç**: Oyuncu durumlarını yönetme

**States**:
- `AliveState` - Oyuncu canlı (normal oyun)
- `DeadState` - Oyuncu öldü (hareket edemez)
- `WinnerState` - Oyuncu kazandı (oyun bitti)

**State Transitions**:
```
AliveState 
    → TakeDamage() → DeadState
    → Win() → WinnerState
```

**Kullanım**:
```csharp
player.State = new AliveState();
player.State.Move(player, dx, dy, map); // Hareket eder
player.State.TakeDamage(player); // DeadState'e geçer
player.State.Move(player, dx, dy, map); // Hareket etmez
```

---

#### 8. **Command Pattern** ⭐
**Dosya**: `src/Patterns/Behavioral/Command/`  
**Amaç**: Oyuncu aksiyonlarını kapsülleme ve undo desteği

**Commands**:
- `ICommand` (Interface)
- `MoveCommand` - Hareket komutu
- `PlaceBombCommand` - Bomba koyma komutu
- `CommandInvoker` - Komut yöneticisi

**Özellikler**:
- ✅ Undo/Redo desteği
- ✅ Komut geçmişi (history stack)
- ✅ Maksimum 10 komut kayıt

**Kullanım**:
```csharp
ICommand moveCmd = new MoveCommand(player, dx, dy, map);
commandInvoker.ExecuteCommand(moveCmd);
commandInvoker.UndoLastCommand(); // U tuşu ile geri al
```

**Command Flow**:
```
User Input → InputController 
    → ProcessInput() 
    → Create Command 
    → CommandInvoker.Execute() 
    → Command.Execute() 
    → Game State Updated
```

---

### 🔸 Architectural & Other Patterns (2/2 - BONUS)

#### 9. **Repository Pattern** ⭐ (+5 BONUS)
**Dosya**: `src/Patterns/Repository/`  
**Amaç**: Veritabanı erişimini soyutlama

**Repositories**:
- `IRepository<T>` (Generic Interface)
- `UserRepository` - Kullanıcı CRUD
- `StatsRepository` - İstatistik CRUD
- `ScoreRepository` - Skor CRUD + Top 10
- `PreferencesRepository` - Tercih CRUD

**Kullanım**:
```csharp
IRepository<User> userRepo = new UserRepository();
User user = userRepo.GetById(1);
userRepo.Update(user);

var topScores = scoreRepo.GetTopScores(10);
statsRepo.IncrementWins(userId);
```

**Avantajlar**:
- ✅ Data access abstraction
- ✅ Testable code
- ✅ Single Responsibility
- ✅ DRY principle

---

#### 10. **MVC Pattern** ⭐ (+5 BONUS)
**Dosya**: `src/MVC/`  
**Amaç**: Mimari organizasyon (Separation of Concerns)

**Katmanlar**:
- **Model**: `src/Models/`, `src/Core/` - İş mantığı ve veri
- **View**: `src/UI/` - Görsel sunum
- **Controller**: `src/MVC/Controllers/` - Akış kontrolü

**MVC Flow**:
```
User Input 
    → Controller (GameController)
    → Model (GameManager, Player, Bomb)
    → View (GameRenderer)
    → Console Output
```

**Controller'lar**:
- `GameController` - Tek/iki oyunculu oyun
- `MultiplayerGameController` - Online multiplayer
- `InputController` - Klavye input yönetimi

---

## 🎮 Oyun Özellikleri

### ⚡ Temel Mekanikler
- ✅ **Tek oyunculu mod** (AI düşmanlara karşı)
- ✅ **İki oyunculu mod** (Local multiplayer)
- ✅ **Online multiplayer** (TCP/IP)
- ✅ **Multiplayer Lobby System** (Host/Join)
- ✅ **Klasik Bomberman kuralları**
- ✅ **Bombalar 3 saniye** sonra patlar
- ✅ **Patlamalar 4 yöne** yayılır
- ✅ **Patlama duvar kontrolü** (DÜZELTİLDİ ✅)

---

### 🗺️ Harita Sistemi

#### Duvar Türleri:
| Sembol | Tür | Dayanıklılık | Açıklama |
|--------|-----|--------------|----------|
| `#` | Unbreakable | ∞ | Yok edilemez, patlama duvar |
| `▒` | Breakable | 1 | Tek patlamayla yok olur |
| `▓` | Hard Wall | 3 | 3 patlamayla yok olur |
| ` ` | Empty Space | 0 | Yürünebilir alan |

**Patlama Mekaniği** (DÜZELTİLDİ):
```csharp
// src/Models/Map.cs - GetExplosionArea()
// Patlama duvardan sonra durur:
// Kırılamaz duvar: Durur, ötesine geçmez
// Kırılabilir duvar: Yıkar ama ötesine geçmez
// Boş alan: Devam eder
```

#### Harita Boyutları:
- **Genişlik**: 21 karo
- **Yükseklik**: 15 karo
- **Seed**: Deterministik harita oluşturma (multiplayer sync)

---

### 🎁 Power-up Sistemi

Kırılan duvarlardan **%30-100 şans** ile power-up düşer:

| Sembol | Tür | Etki | Decorator |
|--------|-----|------|-----------|
| `B` | Bomb Count | Bomba sayısı +1 | BombCountDecorator |
| `P` | Bomb Power | Bomba gücü +1 | BombPowerDecorator |
| `S` | Speed Boost | Hız +1 | SpeedBoostDecorator |

**Decorator Pattern** ile runtime'da oyuncuya eklenir.

**Power-up Akışı**:
```
Duvar Yıkılır 
    → Random() < 30% 
    → SpawnPowerUp() 
    → Oyuncu Toplar 
    → ApplyPowerUpWithDecorator() 
    → Decorator Eklenir 
    → Observer.Notify()
    → UI Güncellenir + Ses Çalar
```

---

### 👾 Düşman Sistemi

#### Düşman Türleri:

| Sembol | Tür | Davranış | Zorluk | AI | Strategy |
|--------|-----|----------|--------|----|----------|
| `E` | Static | Sabit durur | ⭐ Kolay | Yok | StaticMovementStrategy |
| `C` | Chase | Basit takip | ⭐⭐ Orta | Manhattan | ChaseMovementStrategy |
| `A` | Smart | A* akıllı takip | ⭐⭐⭐ Zor | A* | PathfindingMovementStrategy |

**Strategy Pattern** ile runtime'da değiştirilebilir.

**Enemy Spawn Locations**:
- Enemy 1 (Static): (10, 7)
- Enemy 2 (Chase): (15, 5)
- Enemy 3 (Smart): (5, 10)

---

### 🎨 Tema Sistemi

#### 1. Desert Theme (Çöl Teması)
```
Renkler:
- Zemin: Yellow (Sand)
- Kırılabilir: DarkYellow (Stone)
- Kırılamaz: Gray (Rock)

Karakterler:
- Zemin: ░ (Light shade)
- Kırılabilir: ▒ (Medium shade)
- Kırılamaz: ▓ (Dark shade)
```

#### 2. Forest Theme (Orman Teması)
```
Renkler:
- Zemin: Green (Grass)
- Kırılabilir: DarkYellow (Log)
- Kırılamaz: DarkGreen (Tree)

Karakterler:
- Zemin: · (Dot)
- Kırılabilir: ≡ (Triple line)
- Kırılamaz: ♣ (Club)
```

#### 3. City Theme (Şehir Teması)
```
Renkler:
- Zemin: Gray (Concrete)
- Kırılabilir: Red (Brick)
- Kırılamaz: DarkGray (Metal)

Karakterler:
- Zemin: █ (Full block)
- Kırılabilir: ▓ (Dark shade)
- Kırılamaz: ■ (Square)
```

---

### 🔊 Ses Sistemi (**YENİ ÖZELLIK**)

**Dosya**: `src/Audio/SoundManager.cs` (Singleton)

**Ses Tipleri**:
- `BombPlace` - Bomba yerleştirme (400Hz, 100ms)
- `BombExplode` - Patlama (200→150→100Hz)
- `PowerUpCollect` - Power-up toplama (440→554→659Hz)
- `PlayerDeath` - Oyuncu ölümü (800→600→400Hz)
- `EnemyDeath` - Düşman ölümü (300→250Hz)
- `WallBreak` - Duvar kırılma (350Hz, 120ms)
- `MenuSelect` - Menü seçimi (600Hz, 80ms)
- `Victory` - Zafer melodisi (C-D-E-G)
- `GameOver` - Oyun bitiş melodisi

**Ses Implementasyonları**:
1. **Console.Beep** - Native, kurulum gerektirmez (AKTİF)
2. **NAudio** - WAV dosyaları (opsiyonel)
3. **System.Media.SoundPlayer** - Windows only (opsiyonel)

**Observer Pattern Entegrasyonu**:
```csharp
// SoundObserver GameManager'a eklenir
gameManager.Attach(new SoundObserver());

// Her event ses çalar
gameManager.Notify(EventType.BombExploded);
    → SoundObserver.Update() 
    → SoundManager.PlaySound(SoundType.BombExplode)
```

**Ses Kontrolü**:
```csharp
// Settings menüsünden ses aç/kapa
SoundManager.Instance.SetSoundEnabled(true/false);

// Toggle
SoundManager.Instance.ToggleSound();
```

---

## 🕹️ Oynanış

### ⌨️ Kontroller

#### Oyuncu 1:
```
W / ↑      : Yukarı
S / ↓      : Aşağı
A / ←      : Sol
D / →      : Sağ
SPACE      : Bomba Koy
```

#### Oyuncu 2 (İki oyunculu modda):
```
I          : Yukarı
K          : Aşağı
J          : Sol
L          : Sağ
ENTER      : Bomba Koy
```

#### Genel:
```
ESC        : Duraklatma / Çıkış
U          : Undo (Son hareketi geri al)
```

---

### 🏆 Kazanma Koşulları

#### İki Oyunculu Mod:
- ✅ Rakip oyuncuyu yok et
- ✅ Tüm düşmanları yok et

#### Tek Oyunculu Mod:
- ✅ Tüm düşmanları yok et
- ✅ Hayatta kal

---

### 📊 Skor Sistemi

| Eylem | Puan |
|-------|------|
| Duvar Yıkma | +10 |
| Düşman Öldürme | +50 |
| Power-up Toplama | +25 |

**Observer Pattern** ile skor takibi yapılır.

---

## 💾 Veritabanı Yapısı

### 📋 Tablolar (4 adet)

#### 1. **Users** (Kullanıcılar)
```sql
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,  -- BCrypt (salt rounds: 12)
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

#### 2. **GameStatistics** (Oyun İstatistikleri)
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

#### 3. **HighScores** (Yüksek Skorlar)
```sql
CREATE TABLE HighScores (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    Score INTEGER NOT NULL,
    GameDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

#### 4. **PlayerPreferences** (Oyuncu Tercihleri)
```sql
CREATE TABLE PlayerPreferences (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL UNIQUE,
    Theme TEXT DEFAULT 'Desert',
    SoundEnabled INTEGER DEFAULT 1,  -- Ses tercihi
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

### 🔐 Güvenlik
- ✅ **BCrypt** ile şifre hash'leme (salt rounds: 12)
- ✅ **SQL Injection** koruması (Dapper parametreli sorgular)
- ✅ **Foreign Key** constraints
- ✅ **Unique** constraints

---

## 🌐 Online Multiplayer (BONUS +5)

### 🎯 Özellikler
- ✅ TCP/IP socket programming
- ✅ Host/Join sistemi
- ✅ Latency measurement (ping-pong)
- ✅ JSON serialization protocol
- ✅ Event-driven architecture
- ✅ Connection management
- ✅ Real-time game state synchronization
- ✅ **Deterministic map generation** (seed sync) ✅

### 📡 Network Protokolü

#### Message Types:
```csharp
public enum MessageType {
    Connect, Disconnect,
    PlayerMove, PlaceBomb,
    GameState, GameStart, GameEnd,
    Ping, Pong
}
```

#### Kullanım:

**Host olarak:**
```csharp
var controller = new MultiplayerGameController();
await controller.StartAsHost("Desert", 9999);
// IP adresi gösterilir
// Client bağlanır
// Map seed gönderilir
// Oyun başlar
```

**Client olarak:**
```csharp
var controller = new MultiplayerGameController();
await controller.ConnectToHost("192.168.1.100", 9999);
// Host'a bağlanır
// Map seed alır
// Aynı harita oluşturulur
// Oyun başlar
```

### 🗺️ Map Synchronization (DÜZELTİLDİ)

**Problem**: Client ve Host farklı haritalar oluşturuyordu.

**Çözüm**:
```csharp
// Host rastgele seed oluşturur
_mapSeed = new Random().Next();

// Client'a gönderir
NetworkProtocol.CreateGameStartMessage(theme, _mapSeed);

// Her iki taraf aynı seed ile harita oluşturur
new Map(21, 15, themeAdapter, _mapSeed);
```

### 🔒 Güvenlik
- ✅ Message validation
- ✅ Timestamp checking (5 saniye max)
- ✅ Connection timeout
- ✅ Error handling

---

## 🚀 Kurulum ve Çalıştırma

### ✅ Gereksinimler
```
- .NET 7.0 SDK veya üzeri
- Visual Studio 2022 / VS Code / JetBrains Rider (opsiyonel)
- Windows / Linux / macOS
- 50 MB boş disk alanı
```

### 📦 Hızlı Başlangıç

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

### 🔧 Manuel Kurulum

```bash
# 1. Projeyi klonla veya indir
cd BombermanGame

# 2. Bağımlılıkları yükle
dotnet restore

# 3. Projeyi derle
dotnet build

# 4. Çalıştır
dotnet run
```

### 📋 NuGet Paketleri

```xml
<PackageReference Include="System.Data.SQLite" Version="1.0.119" />
<PackageReference Include="Dapper" Version="2.1.66" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="System.Text.Json" Version="7.0.0" />
<PackageReference Include="NAudio" Version="2.2.1" />  <!-- Opsiyonel -->
```

---

## 📁 Proje Yapısı

```
BombermanGame/
├── 📄 Program.cs                    # Ana giriş noktası
├── 📄 BombermanGame.csproj         # Proje yapılandırması
├── 📄 README.md                    # Bu dosya ⭐
├── 📄 DesignDocument.md            # Detaylı tasarım dokümanı
├── 📄 UMLDiagrams.md               # UML diyagram kılavuzu
├── 📄 QUICKSTART.md                # Hızlı başlangıç rehberi
├── 📄 SubmissionCheckList.md      # Teslim kontrol listesi
│
├── 📁 src/
│   │
│   ├── 📁 Core/                        # Temel oyun mantığı (3 dosya)
│   │   ├── GameManager.cs              # ⭐ Singleton + Observer Subject
│   │   ├── MainMenu.cs                 # Ana menü (SES ENTEGRASYONUİLE)
│   │   ├── NetworkManager.cs           # 🌐 Network yönetimi (BONUS)
│   │   └── LobbySystem.cs              # 🌐 Lobby sistemi (BONUS)
│   │
│   ├── 📁 Database/                    # Veritabanı katmanı (2 dosya)
│   │   ├── DatabaseManager.cs          # ⭐ Singleton Pattern
│   │   └── DatabaseSchema.sql          # SQL şema
│   │
│   ├── 📁 Models/                      # Veri modelleri (13 dosya)
│   │   ├── Player.cs, Bomb.cs, Enemy.cs
│   │   ├── Map.cs, Position.cs, PowerUp.cs
│   │   ├── IWall.cs, UnbreakableWall.cs
│   │   ├── BreakableWall.cs, HardWall.cs
│   │   ├── EmptySpace.cs
│   │   └── Entities/                   # Database entity'leri (4)
│   │
│   ├── 📁 Patterns/                    # Tasarım kalıpları (45+ dosya)
│   │   ├── Creational/
│   │   │   └── Factory/                # ⭐ Factory Pattern (5)
│   │   ├── Structural/
│   │   │   ├── Decorator/              # ⭐ Decorator Pattern (6)
│   │   │   └── Adapter/                # ⭐ Adapter Pattern (8)
│   │   ├── Behavioral/
│   │   │   ├── Strategy/               # ⭐ Strategy
