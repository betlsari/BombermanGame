
using System.Collections.Generic;
using System.Linq;
using Dapper;
using BombermanGame.src.Database;
using BombermanGame.src.Models.Entities;

namespace BombermanGame.src.Patterns.Repository
{
    public class ScoreRepository : IRepository<HighScore>
    {
        public HighScore? GetById(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<HighScore>(
                "SELECT * FROM HighScores WHERE Id = @Id", new { Id = id });
        }

        public IEnumerable<HighScore> GetAll()
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.Query<HighScore>("SELECT * FROM HighScores");
        }

        public void Add(HighScore entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            var sql = @"INSERT INTO HighScores (UserId, Score, GameDate) 
                       VALUES (@UserId, @Score, @GameDate)";
            connection.Execute(sql, entity);
        }

        public void Update(HighScore entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute(
                "UPDATE HighScores SET Score = @Score, GameDate = @GameDate WHERE Id = @Id",
                entity);
        }

        public void Delete(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute("DELETE FROM HighScores WHERE Id = @Id", new { Id = id });
        }

        public IEnumerable<HighScore> GetTopScores(int limit = 10)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            var sql = @"SELECT hs.*, u.Username 
                       FROM HighScores hs
                       JOIN Users u ON hs.UserId = u.Id
                       ORDER BY hs.Score DESC
                       LIMIT @Limit";
            return connection.Query<HighScore>(sql, new { Limit = limit });
        }

        public IEnumerable<HighScore> GetUserScores(int userId)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.Query<HighScore>(
                "SELECT * FROM HighScores WHERE UserId = @UserId ORDER BY Score DESC",
                new { UserId = userId });
        }
    }
}