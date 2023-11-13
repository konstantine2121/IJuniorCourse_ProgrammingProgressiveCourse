using System;
using System.Data.SQLite;
using System.IO;
using _27_Task.Common;

namespace _27_Task.DataAccess
{
    using Columns = VotersTable.Columns;

    public static class VotersTable
    {
        public const string TableName = "voters";

        public static class Columns
        {
            public const string Id = "_id";
            public const string PassportHash = "passport_hash";
            public const string Name = "name";
            public const string CanVote = "can_vote";
        }
    }

    internal class DbManager
    {

        public const string DatabaseFile = "db.sqlite";
        private const string ConnectionString = "Data Source=" + DatabaseFile;

        private readonly ILogger _logger;
        private readonly VotersInfoGenerator _votersInfoGenerator = new VotersInfoGenerator();

        public DbManager(ILogger logger)
        {
            _logger = logger;
        }

        public bool CheckDbFileExists()
        {
            var messageFormat = $"Проверка наличия файла базы данных: '{DatabaseFile}' {{0}}.";
            var existsText = "найден";
            var notExistsText = "не найден";

            var exists = File.Exists(DatabaseFile);
            var message = exists ?
                string.Format(messageFormat, existsText) :
                string.Format(messageFormat, notExistsText);

            _logger.Log(message);

            return exists;
        }

        public void CreateDatabase()
        {
            var table = VotersTable.TableName;

            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    var command = CreateCommand(connection);

                    command.CommandText = $"DROP TABLE IF EXISTS {table}";
                    command.ExecuteNonQuery();

                    command.CommandText = $"CREATE TABLE {table}("
                        + $"{Columns.Id} INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "
                        + $"{Columns.Name} TEXT NOT NULL, "
                        + $"{Columns.PassportHash} TEXT NOT NULL, "
                        + $"{Columns.CanVote} INTEGER NOT NULL)";

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void FillData()
        {
            var table = VotersTable.TableName;

            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();

                    var name = "@name";
                    var passport = "@passport";
                    var vote = "@vote";

                    var insert = $"INSERT INTO {VotersTable.TableName} ({Columns.Name}, {Columns.PassportHash}, {Columns.CanVote}) " +
                        $"VALUES ({name}, {passport}, {vote})";

                    var command = CreateCommand(connection);
                    var records = _votersInfoGenerator.Generate();

                    foreach (var record in records)
                    {
                        command.CommandText = insert;
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter(name, record.Name));
                        command.Parameters.Add(new SQLiteParameter(passport, record.Passport));
                        command.Parameters.Add(new SQLiteParameter(vote, record.CanVote));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #region Create Connections / Commands

        private static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        private static SQLiteCommand CreateCommand(SQLiteConnection connection)
        {
            return new SQLiteCommand(connection);
        }

        #endregion Create Connections / Commands

        private void LogException(Exception ex)
        {
            _logger.Log(ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }
}
