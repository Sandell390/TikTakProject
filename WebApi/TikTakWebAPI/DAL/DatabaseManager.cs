namespace TikTakWebAPI.DAL;
using System.Data;
using Npgsql;


public class DatabaseManager
{

    static readonly string _connString = "Host=ep-tight-sun-a2xbcubr-pooler.eu-central-1.aws.neon.tech;Username=Jesper;Password=r16yjxbBFDpC;Database=tiktak";
    private readonly NpgsqlConnection db;

    private readonly ILogger _logger;

    public DatabaseManager(ILogger logger)
    {
        db = new NpgsqlConnection(_connString);
        _logger = logger;
    }

    public bool NonQuery(string sql, Dictionary<string, object> parameters = null)
    {
        try
        {
            int rowsAffected = 0;
            using (var connection = new NpgsqlConnection(_connString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    foreach (KeyValuePair<string, object> keyValuePair in parameters)
                    {
                        command.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }
        catch (System.Exception e)
        {
            _logger.LogError($"SQL could not be executed: {e.Message}");
            return false;
        }
    }

    public object? QueryScalar(string sql, Dictionary<string, object> parameters = null)
    {

        using (var connection = new NpgsqlConnection(_connString))
        {
            connection.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
            {
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
                try
                {
                    object sqlResult = cmd.ExecuteScalar()!;

                    return sqlResult;
                }
                catch (Exception e)
                {
                    _logger.LogError("Database Exception: " + e.Message);
                    return null;
                }
            }
        }


    }

    public IEnumerable<T>? Query<T>(string sql, Func<IDataReader, T> map, out bool success, Dictionary<string, object> parameters = null)
    {
        success = false;
        var results = new List<T>();
        try
        {
            using (var connection = new NpgsqlConnection(_connString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    foreach (KeyValuePair<string, object> keyValuePair in parameters)
                    {
                        command.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                            success = true;

                        while (reader.Read())
                        {
                            results.Add(map(reader));
                        }
                    }
                }
            }
            return results;
        }
        catch (System.Exception e)
        {
            _logger.LogError($"SQL could not be executed: {e.Message}");
            return default;
        }

    }



}