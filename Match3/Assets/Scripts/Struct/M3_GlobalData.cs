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
    public bool IsPrefabsLoadCompleted;
    public M3_GameState CurrentGameState;
};