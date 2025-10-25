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

    private Dictionary<Hash128, Texture2D> _TextureDict = new Dictionary<Hash128, Texture2D>();

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
        LoadTextures(Event.TextureList);
        M3_EventBus.SendEvent<M3_Event_TexturesLoadCompleted>();
    }

    private bool LoadTextures(List<string> TexturePaths)
    {
        List<Task<bool>> LoadTasks = new List<Task<bool>>();
        foreach (string ScriptPath in TexturePaths)
        {
            //LoadTasks.Add(LoadTextureAsync(ScriptPath));
            LoadTexture(ScriptPath);
        }

        //bool[] Results = await Task.WhenAll(LoadTasks);
        //foreach (bool Result in Results)
        //{
        //    if (!Result)
        //    {
        //        return false;
        //    }
        //}

        Debug.Log("[TextureManager] All textures loaded successfully.");
        return true;
    }

    private bool LoadTexture(string TexturePath)
    {
        Hash128 TextureId = Hash128.Compute(TexturePath);
        if (_TextureDict.ContainsKey(TextureId))
        {
            Debug.LogWarning($"[TextureManager] Duplicated texture: {TexturePath} Hash: {TextureId}");
            return true;
        }

        //if (!File.Exists(TexturePath))
        //{
        //    Debug.LogWarning($"[TextureManager] {TexturePath} no exist!");
        //    return false;
        //}

        Texture2D NewTexture = null;

        try
        {
            NewTexture = Resources.Load<Texture2D>(TexturePath);
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"Fail to read {TexturePath}\n Error: {Err.Message}");
        }

        if (NewTexture != null)
        {
            _TextureDict[TextureId] = NewTexture;
            return true;
        }
        else
        {
            Debug.LogError("Fail to load texture: " + TexturePath);
            UnityEngine.Object.Destroy(NewTexture);
            return false;
        }
    }

    public bool GetTexture(Hash128 TextureId, out Texture2D OutTexture)
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
