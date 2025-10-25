using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using XLua;

public class M3_StoryManager : M3_Manager
{
    public override string ManagerName
    {
        get { return "Story Manager"; }
    }

    private Dictionary<Hash128, Story> _StoryDict = new Dictionary<Hash128, Story>();

    public override void Initialize()
    {
        M3_EventBus.Subscribe<M3_Event_StoriesReadCompleted>(OnStoriesReadCompleted);
    }

    public override void Destroy()
    {
        M3_EventBus.Unsubscribe<M3_Event_StoriesReadCompleted>(OnStoriesReadCompleted);
    }

    private void OnStoriesReadCompleted(M3_Event_StoriesReadCompleted Event)
    {
        LoadStories(Event.MainInkFileList);
        M3_EventBus.SendEvent<M3_Event_StoriesLoadCompleted>();
    }

    private bool LoadStories(List<string> MainInkFileList)
    {
        List<Task<bool>> LoadTasks = new List<Task<bool>>();
        foreach (string MainInkFilePath in MainInkFileList)
        {
            //LoadTasks.Add(LoadStoryAsync(MainInkFilePath));
            LoadStory(MainInkFilePath);
        }

        //bool[] Results = await Task.WhenAll(LoadTasks);
        //foreach (bool Result in Results)
        //{
        //    if (!Result)
        //    {
        //        return false;
        //    }
        //}

        Debug.Log("[StoryManager] All ink file loaded successfully.");
        return true;
    }

    private bool LoadStory(string MainInkFilePath)
    {
        Hash128 StoryId = Hash128.Compute(MainInkFilePath);
        if (_StoryDict.ContainsKey(StoryId))
        {
            Debug.LogWarning($"[StoryManager] Duplicated story: {MainInkFilePath} Hash: {StoryId}");
            return false;
        }

        //if (!File.Exists(MainInkFilePath))
        //{
        //    Debug.LogWarning($"[StoryManager] {MainInkFilePath} no exist!");
        //    return false;
        //}

        string MainInkFileText = null;

        try
        {
            MainInkFileText = Resources.Load<TextAsset>(MainInkFilePath).text;
        }
        catch (System.Exception Err)
        {
            Debug.LogError($"[StoryManager] Fail to read {MainInkFilePath}\n Error: {Err.Message}");
        }

        //var NewCompiler = new Ink.Compiler(MainInkFileText, new Ink.Compiler.Options
        //{
        //    countAllVisits = true,
        //    fileHandler = new Ink.UnityFileHandler(
        //        System.IO.Path.GetDirectoryName(MainInkFilePath)
        //    )
        //});

        //Story NewStory = null;
        //try
        //{
        //    NewStory = NewCompiler.Compile();
        //}
        //catch(System.Exception Ex)
        //{
        //    Debug.LogError(Ex);
        //}

        Story NewStory = new Ink.Runtime.Story(MainInkFileText);

        M3_CommonHelper.BindModApiToStory(NewStory);

        _StoryDict[StoryId] = NewStory;
        return true;
    }

    public bool GetStory(Hash128 StoryId, out Story OutStory)
    {
        if (_StoryDict.TryGetValue(StoryId, out Story TempStory))
        {
            OutStory = TempStory;
            return true;
        }

        OutStory = null;
        return false;
    }
}
