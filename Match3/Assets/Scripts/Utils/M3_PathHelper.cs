using System.IO;
using UnityEngine;

public class M3_PathHelper
{
    private static string ModsPath = null;

    public static string GetModsPath()
    {
        if (ModsPath == null)
        {
            ModsPath = NormalizePath(Path.Combine(Application.dataPath, "Mods"));
            if (!Directory.Exists(ModsPath))
            {
                Directory.CreateDirectory(ModsPath);
            }
        }

        return ModsPath;
    }

    public static string GetModSubfilePath(string FilePath)
    {
        return NormalizePath(Path.Combine(GetModsPath(), FilePath));
    }

    private static string NormalizePath(string InPath)
    {
        return InPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }
}
