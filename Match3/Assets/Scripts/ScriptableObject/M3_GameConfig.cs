using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "M3 Config/Game Config")]
public class M3_GameConfig : ScriptableObject
{
    [SerializeField]
    private GameObject _UIRootPrefab;
    public GameObject UIRootPrefab { get { return _UIRootPrefab; } }
    [SerializeField]
    private string[] _PrefabAddressList;
    public string[] PrefabAddressList { get { return _PrefabAddressList; } }
    [SerializeField]
    private float _MouseDragThreshold = 5f;
    public float MouseDragThreshold { get { return _MouseDragThreshold; } }
};