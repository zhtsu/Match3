using System.IO;
using UnityEngine;

public class M3_PathHelper
{
    private static string _ModsPath = null;

    public static string GetModsPath()
    {
        if (_ModsPath == null)
        {
            _ModsPath = NormalizePath(Path.Combine(Application.persistentDataPath, "Mods"));
            if (!Directory.Exists(_ModsPath))
            {
                Directory.CreateDirectory(_ModsPath);
            }
        }

        return _ModsPath;
    }

    public static string GetModSubfilePath(string FilePath)
    {
        if (string.IsNullOrEmpty(FilePath))
            return GetModsPath();

        return NormalizePath(Path.Combine(GetModsPath(), FilePath));
    }

    public static string GetModSubfilePath_Resources(string FilePath)
    {
        if (string.IsNullOrEmpty(FilePath))
            return "Mods";

        string FileNameWithoutExt = Path.GetFileNameWithoutExtension(FilePath);
        string FileDir = Path.GetDirectoryName(FilePath);

        return NormalizePath(Path.Combine("Mods", FileDir + "/" + FileNameWithoutExt));
    }

    public static Hash128 GetHash(string FilePath)
    {
        return Hash128.Compute(GetModSubfilePath_Resources(FilePath));
    }

    private static string NormalizePath(string InPath)
    {
        return InPath.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
    }
}
