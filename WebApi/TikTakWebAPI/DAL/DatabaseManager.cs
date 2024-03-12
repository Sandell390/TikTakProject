namespace TikTakWebAPI.DAL;
using System.Data;
using Npgsql;


public class DatabaseManager{

    static string _connString = "Host=ep-tight-sun-a2xbcubr-pooler.eu-central-1.aws.neon.tech;Username=Jesper;Password=r16yjxbBFDpC;Database=tiktak";
    NpgsqlConnection db;

    public DatabaseManager(){
        db = new NpgsqlConnection(_connString);
    }

    public IEnumerable<T> Query<T>(string sql, Func<IDataReader, T> map)
    {
        var results = new List<T>();
        using (var connection = new NpgsqlConnection(_connString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(map(reader));
                    }
                }
            }
        }
        return results;
    }



}