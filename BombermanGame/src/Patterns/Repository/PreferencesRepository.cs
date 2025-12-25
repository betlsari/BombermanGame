
using System.Collections.Generic;
using Dapper;
using BombermanGame.src.Database;
using BombermanGame.src.Models.Entities;

namespace BombermanGame.src.Patterns.Repository
{
    public class PreferencesRepository : IRepository<PlayerPreference>
    {
        public PlayerPreference? GetById(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<PlayerPreference>(
                "SELECT * FROM PlayerPreferences WHERE Id = @Id", new { Id = id });
        }

        public IEnumerable<PlayerPreference> GetAll()
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.Query<PlayerPreference>("SELECT * FROM PlayerPreferences");
        }

        public void Add(PlayerPreference entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            var sql = @"INSERT INTO PlayerPreferences (UserId, Theme, SoundEnabled) 
                       VALUES (@UserId, @Theme, @SoundEnabled)";
            connection.Execute(sql, entity);
        }

        public void Update(PlayerPreference entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute(
                @"UPDATE PlayerPreferences 
                  SET Theme = @Theme, SoundEnabled = @SoundEnabled 
                  WHERE UserId = @UserId",
                entity);
        }

        public void Delete(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute("DELETE FROM PlayerPreferences WHERE Id = @Id", new { Id = id });
        }

        public PlayerPreference? GetByUserId(int userId)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<PlayerPreference>(
                "SELECT * FROM PlayerPreferences WHERE UserId = @UserId",
                new { UserId = userId });
        }
    }
}