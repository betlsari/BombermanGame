
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using BombermanGame.src.Database;
using BombermanGame.src.Models.Entities;

namespace BombermanGame.src.Patterns.Repository
{
    public class UserRepository : IRepository<User>
    {
        public User? GetById(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<User>(
                "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        }

        public IEnumerable<User> GetAll()
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.Query<User>("SELECT * FROM Users");
        }

        public void Add(User entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            var sql = @"INSERT INTO Users (Username, PasswordHash, CreatedAt) 
                       VALUES (@Username, @PasswordHash, @CreatedAt);
                       SELECT last_insert_rowid();";

            entity.Id = connection.ExecuteScalar<int>(sql, entity);
        }

        public void Update(User entity)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute(
                "UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash WHERE Id = @Id",
                entity);
        }

        public void Delete(int id)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            connection.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = id });
        }

        public User? GetByUsername(string username)
        {
            var connection = DatabaseManager.Instance.GetConnection();
            return connection.QueryFirstOrDefault<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username });
        }

        public bool UsernameExists(string username)
        {
            return GetByUsername(username) != null;
        }
    }
}
