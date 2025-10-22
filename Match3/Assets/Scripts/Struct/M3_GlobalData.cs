using UnityEngine;

public enum M3_MouseMoveDirection
{
    None = 0,
    Up,
    Down,
    Left,
    Right
}

public struct M3_GlobalData
{
    public M3_MouseMoveDirection MouseDragDirection;
    public bool IsGameReady;
    public bool IsManagerHubReady;
    public bool IsPrefabsLoadCompleted;
    public bool IsTexturesLoadCompleted;
    public bool IsScriptsLoadCompleted;
    public bool IsStoriesLoadCompleted;
    public bool IsAllowInput;
    public M3_GameState CurrentGameState;
    public M3_Match3Battle CurrentM3Battle;
    public M3_Gem CurrentClickedGem;
};