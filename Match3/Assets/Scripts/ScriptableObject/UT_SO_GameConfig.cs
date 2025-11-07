using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "UT Config/Game Config")]
public class UT_SO_GameConfig : ScriptableObject
{
    [SerializeField] public UT_UIRoot UIRootPrefab;
    [SerializeField] public Camera MainCameraPrefab;
    [SerializeField] public UT_AudioRoot AudioRootPrefab;

    [SerializeField] public UT_ServiceContainer ServiceContainerPrefab;

    [SerializeField] public float MouseDragThreshold = 5f;
};