using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class M3_LocaleManager : M3_IManager
{
    public string ManagerName
    {
        get { return "LocaleManager"; }
    }

    static private Dictionary<string, Dictionary<string, Dictionary<string, string>>> _LocaleStringDict
        = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    public void Initialize()
    {
        M3_EventManager.Subscribe<M3_Event_ReadLocaleFile>(LoadLocaleData);
    }

    public void Destroy()
    {
        M3_EventManager.Unsubscribe<M3_Event_ReadLocaleFile>(LoadLocaleData);
        _LocaleStringDict.Clear();
    }

    public static string GetString(string Namespace, string LanguageCode, string StringId)
    {
        if (_LocaleStringDict.TryGetValue(Namespace, out Dictionary<string, Dictionary<string, string>> LanguageDict))
        {
            if (LanguageDict.TryGetValue(LanguageCode, out Dictionary<string, string> StringDict))
            {
                if (StringDict.TryGetValue(StringId, out string LocaleString))
                {
                    return LocaleString;
                }
                else
                {
                    return $"Error string id: {StringId}";
                }
            }
            else
            {
                return $"Error language code: {LanguageCode}";
            }
        }
        else
        {
            return $"Error namespace: {Namespace}";
        }
    }

    private void LoadLocaleData(M3_Event_ReadLocaleFile Event)
    {
        if (_LocaleStringDict.ContainsKey(Event.Namespace) == false)
        {
            _LocaleStringDict[Event.Namespace] = new Dictionary<string, Dictionary<string, string>>();
        }

        if (!File.Exists(Event.LocaleFilePath))
        {
            Debug.LogError(Event.LocaleFilePath + " no exist!");
            return;
        }

        try
        {
            string LocaleFileContent = File.ReadAllText(Event.LocaleFilePath, System.Text.Encoding.UTF8);
            JsonData LocaleJsonData = JsonMapper.ToObject(LocaleFileContent);

            foreach (string LanguageCode in LocaleJsonData.Keys)
            {
                if (_LocaleStringDict[Event.Namespace].ContainsKey(LanguageCode) == false)
                {
                    _LocaleStringDict[Event.Namespace][LanguageCode] = new Dictionary<string, string>();
                }

                JsonData StringJsonData = LocaleJsonData[LanguageCode];

                foreach (string StringId in StringJsonData.Keys)
                {
                    string LocaleString = (string)StringJsonData[StringId];
                    _LocaleStringDict[Event.Namespace][LanguageCode].Add(StringId, LocaleString);
                }
            }
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {Event.LocaleFilePath}\n Error: {Err.Message}");
        }
    }
}
