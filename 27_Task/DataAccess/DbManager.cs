using System;
using System.Data;
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

    public interface IDbManager
    {
        bool CheckDbFileExists();
        void CreateDatabase();
        void FillVotersInfo();
    }

    public class DbManager : IDbManager
    {
        public const string DatabaseFile = "db.sqlite";
        private const string ConnectionString = "Data Source=" + DatabaseFile;

        private readonly ILogger _logger;
        private readonly VotersInfoGenerator _votersInfoGenerator = new VotersInfoGenerator();

        public DbManager(ILogger logger)
        {
            _logger = logger;
        }

        #region Data Check / Init

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

        public void FillVotersInfo()
        {
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

                    var records = _votersInfoGenerator.GenerateInfos();

                    foreach (var record in records)
                    {
                        using (var command = CreateCommand(connection))
                        {
                            command.CommandText = insert;
                            command.Parameters.Add(new SQLiteParameter(name, record.Name));
                            command.Parameters.Add(new SQLiteParameter(passport, record.Passport));
                            command.Parameters.Add(new SQLiteParameter(vote, record.CanVote));
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        #endregion Data Check / Init

        public DataTable FindRecords(string passportHash)
        {
            if (passportHash == null)
            {
                throw new ArgumentNullException(nameof(passportHash));
            }

            DataTable result = new DataTable();

            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();

                    var passport = "@passport";
                    var select =
                        $"SELECT * FROM {VotersTable.TableName} " +
                        $"WHERE {Columns.PassportHash}={passport} " +
                        "LIMIT 1;";

                    using (var command = CreateCommand(connection))
                    {
                        command.CommandText = select;
                        command.Parameters.Add(new SQLiteParameter(passport, passportHash));

                        SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                        dataAdapter.Fill(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            return result;
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
