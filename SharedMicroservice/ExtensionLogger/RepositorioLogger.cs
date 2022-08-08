using System.Collections.Generic;
using System.Data.SqlClient;
using Model;

namespace ExtensionLogger
{
    public class RepositorioLogger
    {
        private string ConnectionString { get; set; }

        public RepositorioLogger(string connection)
        {
            ConnectionString = connection;
        }

        private bool ExecuteNonQuery(string commandStr, List<SqlParameter> paramList)
        {
            var result = false;
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var command = new SqlCommand(commandStr, conn))
                {
                    command.Parameters.AddRange(paramList.ToArray());
                    var count = command.ExecuteNonQuery();
                    result = count > 0;
                }
            }
            return result;
        }

        public bool InsertLog(LogEvento log)
        {
            var command = $@"INSERT INTO [dbo].[EventLog] ([EventID],[LogLevel],[Message],[CreatedTime]) VALUES (@EventID, @LogLevel, @Message, @CreatedTime)";
            var paramList = new List<SqlParameter>
            {
                new SqlParameter("EventID", log.EventId),
                new SqlParameter("LogLevel", log.LogLevel),
                new SqlParameter("Message", log.Message),
                new SqlParameter("CreatedTime", log.CreatedTime)
            };

            return ExecuteNonQuery(command, paramList);
        }
    }
}
