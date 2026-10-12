using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WallyAnmSpinzor;

internal class Sample
{
    private static readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        WriteIndented = true,
    };

    public static async Task SerializeToJson(AnmVersionEnum version, string filePath, string outputPath)
    {
        AnmFile anm;
        using (FileStream file = new(filePath, FileMode.Open, FileAccess.Read))
            anm = await AnmFile.CreateFromAsync(version, file);
        using (FileStream outFile = new(outputPath, FileMode.Create, FileAccess.Write))
            await JsonSerializer.SerializeAsync(outFile, anm, JSON_OPTIONS);
    }

    public static async Task JsonToAnm(AnmVersionEnum version, string jsonPath, string outputPath)
    {
        AnmFile anm;
        using (FileStream file = new(jsonPath, FileMode.Open, FileAccess.Read))
            anm = await JsonSerializer.DeserializeAsync<AnmFile>(file, JSON_OPTIONS) ?? throw new Exception("bad json format");
        using (FileStream outFile = new(outputPath, FileMode.Create, FileAccess.Write))
            await anm.WriteToAsync(version, outFile);
    }
}