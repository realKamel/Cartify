using System.Text.Json;

namespace Cartify.Persistence;

public static class DataSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        // to accept pascalCase into CamelCase 
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static async Task<IEnumerable<T>?> SeedItemsFromJson<T>(string filePath)
    {
        var itemsFileStream = File.OpenRead(filePath);
        var items = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(itemsFileStream,
            JsonOptions);
        return items;
    }
}