using Application.Interfaces;
namespace IntegrationTest
{
    using Microsoft.Data.Sqlite;
    using System.Data.Common;
    using System.Threading.Tasks;

    public class TestDbConnectionFactory : IDbConnectionFactory
    {
        private readonly DbConnection _connection;

        public TestDbConnectionFactory()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            InitializeDatabase().GetAwaiter().GetResult();
        }

        public Task<DbConnection> CreateConnectionAsync()
        {
            return Task.FromResult(_connection);
        }

        private async Task InitializeDatabase()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName VARCHAR(128) NOT NULL, 
                    LastName VARCHAR(128) NULL, 
                    Email VARCHAR(250) NOT NULL, 
                    DateOfBirth datetime NULL,
                    PhoneNumber bigint
                );
                INSERT INTO Users (FirstName, LastName, Email,PhoneNumber )  
                VALUES ('Jhon','Jhon','Jhon@test.com','1234567890');
            ";
            await command.ExecuteNonQueryAsync();
        }
    }


}
