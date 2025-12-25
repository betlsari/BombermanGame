
using System;
using System.Data.SQLite;
using System.IO;

namespace BombermanGame.src.Database
{
    public sealed class DatabaseManager
    {
        private static DatabaseManager? _instance;
        private static readonly object _lock = new object();
        private SQLiteConnection? _connection;
        private const string DatabaseFile = "bomberman.db";

        
        private DatabaseManager() { }

        
        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Initialize()
        {
            bool isNewDatabase = !File.Exists(DatabaseFile);

            string connectionString = $"Data Source={DatabaseFile};Version=3;";
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();

            if (isNewDatabase)
            {
                CreateTables();
            }
        }

        private void CreateTables()
        {
            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            string createGameStatisticsTable = @"
                CREATE TABLE IF NOT EXISTS GameStatistics (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Wins INTEGER DEFAULT 0,
                    Losses INTEGER DEFAULT 0,
                    TotalGames INTEGER DEFAULT 0,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );";

            string createHighScoresTable = @"
                CREATE TABLE IF NOT EXISTS HighScores (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Score INTEGER NOT NULL,
                    GameDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );";

            string createPlayerPreferencesTable = @"
                CREATE TABLE IF NOT EXISTS PlayerPreferences (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL UNIQUE,
                    Theme TEXT DEFAULT 'Desert',
                    SoundEnabled INTEGER DEFAULT 1,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );";

            ExecuteNonQuery(createUsersTable);
            ExecuteNonQuery(createGameStatisticsTable);
            ExecuteNonQuery(createHighScoresTable);
            ExecuteNonQuery(createPlayerPreferencesTable);
        }

        public SQLiteConnection GetConnection()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                throw new InvalidOperationException("Database connection is not initialized.");
            }
            return _connection;
        }

        public void ExecuteNonQuery(string query)
        {
            using var command = new SQLiteCommand(query, GetConnection());
            command.ExecuteNonQuery();
        }

        public void Close()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
