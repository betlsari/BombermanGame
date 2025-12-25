
using System.Collections.Generic;
using Dapper;
using BombermanGame.src.Database;
using BombermanGame.src.Models.Entities;

namespace BombermanGame.src.Patterns.Repository
{
    public class StatsRepository : IRepository<GameStatistic>
    {
        public GameStatistic? GetById(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<GameStatistic>(
                "SELECT * FROM GameStatistics WHERE Id = @Id", new { Id = id });
        }

        public IEnumerable<GameStatistic> GetAll()
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.Query<GameStatistic>("SELECT * FROM GameStatistics");
        }

        public void Add(GameStatistic entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            var sql = @"INSERT INTO GameStatistics (UserId, Wins, Losses, TotalGames) 
                       VALUES (@UserId, @Wins, @Losses, @TotalGames)";
            connection.Execute(sql, entity);
        }

        public void Update(GameStatistic entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute(
                @"UPDATE GameStatistics 
                  SET Wins = @Wins, Losses = @Losses, TotalGames = @TotalGames 
                  WHERE UserId = @UserId",
                entity);
        }

        public void Delete(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute("DELETE FROM GameStatistics WHERE Id = @Id", new { Id = id });
        }

        public GameStatistic? GetByUserId(int userId)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<GameStatistic>(
                "SELECT * FROM GameStatistics WHERE UserId = @UserId",
                new { UserId = userId });
        }

        public void IncrementWins(int userId)
        {
            var stats = GetByUserId(userId);
            if (stats != null)
            {
                stats.Wins++;
                stats.TotalGames++;
                Update(stats);
            }
            else
            {
                Add(new GameStatistic { UserId = userId, Wins = 1, TotalGames = 1 });
            }
        }

        public void IncrementLosses(int userId)
        {
            var stats = GetByUserId(userId);
            if (stats != null)
            {
                stats.Losses++;
                stats.TotalGames++;
                Update(stats);
            }
            else
            {
                Add(new GameStatistic { UserId = userId, Losses = 1, TotalGames = 1 });
            }
        }
    }
}