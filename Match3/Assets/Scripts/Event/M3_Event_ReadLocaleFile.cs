using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3_Event_ReadLocaleFile : M3_Event
{
    public M3_Event_ReadLocaleFile(string InNamespace, string InLocaleFilePath)
    {
        Namespace = InNamespace;
        LocaleFilePath = InLocaleFilePath;
    }

    public string Namespace { get; private set; }
    public string LocaleFilePath { get; private set; }
}
