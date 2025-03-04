using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task CreateUser(User user)
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                        INSERT INTO Users (FirstName, LastName, Email, DateOfBirth,PhoneNumber ) 
                        VALUES (@FirstName, @LastName, @Email, @DateOfBirth,@PhoneNumber);";
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = user.FirstName });
            command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar) { Value = user.LastName });
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email });
            command.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.DateTime) { Value = user.DateOfBirth });
            command.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.BigInt) { Value = user.PhoneNumber });

            var response = await command.ExecuteScalarAsync();
        }

        public async Task DeleteUser(int id)
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Users WHERE Id = @Id";
            command.CommandType = CommandType.Text;
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            await command.ExecuteScalarAsync();
        }

        public async Task<bool> EmailExistis(string email)
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            command.CommandType = CommandType.Text;


            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = email });


            var response = Convert.ToInt32(await command.ExecuteScalarAsync());

            return response > 0;
        }

        public async Task<User> GetUserById(int id)
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            const string query = @"SELECT Id, FirstName, LastName, Email, DateOfBirth, PhoneNumber FROM Users WHERE Id = @Id";
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            using var reader = await command.ExecuteReaderAsync();
            var user = new User();

            while (await reader.ReadAsync())
            {
                user.Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0;
                user.FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString()! : string.Empty;
                user.LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString()! : string.Empty;
                user.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString()! : string.Empty;
                user.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : DateTime.MinValue;
                user.PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? Convert.ToInt64(reader["PhoneNumber"]) : 0;


            }
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            const string query = "SELECT Id, FirstName, LastName, Email, DateOfBirth, PhoneNumber FROM Users";
            command.CommandText = query;

            using var reader = await command.ExecuteReaderAsync();
            var results = new List<User>();

            while (await reader.ReadAsync())
            {
                results.Add(new User
                {
                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                    FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString()! : string.Empty,
                    LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString()! : string.Empty,
                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString()! : string.Empty,
                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : DateTime.MinValue,
                    PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? Convert.ToInt64(reader["PhoneNumber"]) : 0,

                });
            }
            return results;
        }

        public async Task UpdateUser(int id, User user)
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                       UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email 
                       , DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber WHERE Id = @Id;";
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = user.FirstName });
            command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar) { Value = user.LastName });
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email });
            command.Parameters.Add(new SqlParameter("@DateOfBirth", SqlDbType.DateTime) { Value = user.DateOfBirth });
            command.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.BigInt) { Value = user.PhoneNumber });

            await command.ExecuteScalarAsync();    
        }
    }

}

