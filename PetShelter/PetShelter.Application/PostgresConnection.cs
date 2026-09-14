using System.Data;
using System.Text.Json;
using Npgsql;

namespace PetShelter.Application;

public static class PostgresConnection
{
    private static readonly string ConnectionString = LoadConnectionString();

    public static IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    private static string LoadConnectionString()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Konfiguracioni fajl nije pronađen na putanji: {configPath}");
        }

        var json = File.ReadAllText(configPath);
        using var document = JsonDocument.Parse(json);

        var connectionString = document.RootElement
            .GetProperty("ConnectionStrings")
            .GetProperty("DefaultConnection")
            .GetString();

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Konekcioni string 'DefaultConnection' nije pronađen u konfiguraciji.");
        }

        return connectionString;
    }
}
