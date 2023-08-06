using System.Text.Json;

namespace DataTrack.Config;

public static class ScadaConfig
{
    public static string simulationType;
    public static int updateFrequency;
    public static SemaphoreSlim logSemaphore = new SemaphoreSlim(1);
    public static SemaphoreSlim dbSemaphore = new SemaphoreSlim(1);

    public static void LoadScadaConfig()
    {
        var jsonString = File.ReadAllText("scadaConfig.json");
        var jsonDocument = JsonDocument.Parse(jsonString);
        foreach (var property in jsonDocument.RootElement.EnumerateObject())
        {
            var propertyName = property.Name;
            var propertyValue = property.Value;

            switch (propertyName)
            {
                case "simulationType":
                    simulationType = propertyValue.ToString();
                    break;
                case "updateFrequency":
                    updateFrequency = int.Parse(propertyValue.ToString());
                    break;
            }
        }
    }
}