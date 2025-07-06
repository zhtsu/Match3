using LitJson;
using UnityEngine;

public class M3_Data
{
    public static string Serialize<T>(T InData)
    {
        JsonWriter Writer = new JsonWriter();
        Writer.PrettyPrint = true;
        Writer.IndentValue = 4;

        JsonMapper.ToJson(InData, Writer);
        
        return Writer.ToString();
    }

    public static bool Deserialize<T>(string JsonContent, out T OutData)
    {
        if (string.IsNullOrEmpty(JsonContent))
        {
            Debug.LogWarning("Invalid JSON");
            OutData = default(T);
            return false;
        }

        OutData = JsonMapper.ToObject<T>(JsonContent);
        return true;
    }
}
