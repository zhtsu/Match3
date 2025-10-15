using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

public class M3_TextureManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Texture Manager"; }
    }

    private Dictionary<string, Texture2D> _TextureDict = new Dictionary<string, Texture2D>();

    public override void Initialize()
    {
        M3_EventBus.Subscribe<M3_Event_TexturesReadCompleted>(OnTexturesReadCompleted);
    }

    public override void Destroy()
    {
        M3_EventBus.Unsubscribe<M3_Event_TexturesReadCompleted>(OnTexturesReadCompleted);
    }

    private void OnTexturesReadCompleted(M3_Event_TexturesReadCompleted Event)
    {
        LoadTexturesAsync(Event.TextureList).ContinueWith((Task<bool> LoadTask) =>
        {
            M3_EventBus.SendEvent<M3_Event_TexturesLoadCompleted>();
        });
    }

    private async Task<bool> LoadTexturesAsync(List<string> TexturePaths)
    {
        List<Task<bool>> LoadTasks = new List<Task<bool>>();
        foreach (string ScriptPath in TexturePaths)
        {
            LoadTasks.Add(LoadTextureAsync(ScriptPath));
        }

        bool[] Results = await Task.WhenAll(LoadTasks);
        foreach (bool Result in Results)
        {
            if (!Result)
            {
                return false;
            }
        }

        Debug.Log("[TextureManager] All textures loaded successfully.");
        return true;
    }

    private async Task<bool> LoadTextureAsync(string TexturePath)
    {
        if (_TextureDict.ContainsKey(TexturePath))
        {
            Debug.LogWarning($"[TextureManager] Duplicated texture ID: {TexturePath}");
            return false;
        }

        if (!File.Exists(TexturePath))
        {
            Debug.LogWarning($"[TextureManager] {TexturePath} no exist!");
            return false;
        }

        byte[] TextureData = null;

        try
        {
            TextureData = await Task.Run(() => File.ReadAllBytes(TexturePath));
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {TexturePath}\n Error: {Err.Message}");
        }

        Texture2D NewTexture = new Texture2D(2, 2);

        if (NewTexture.LoadImage(TextureData))
        {
            _TextureDict[TexturePath] = NewTexture;
            return true;
        }
        else
        {
            Debug.LogError("Fail to load texture: " + TexturePath);
            UnityEngine.Object.Destroy(NewTexture);
            return false;
        }
    }

    public bool GetTexture(string TextureId, out Texture2D OutTexture)
    {
        if (_TextureDict.TryGetValue(TextureId, out Texture2D TempTexture))
        {
            OutTexture = TempTexture;
            return true;
        }

        OutTexture = null;
        return false;
    }
}
