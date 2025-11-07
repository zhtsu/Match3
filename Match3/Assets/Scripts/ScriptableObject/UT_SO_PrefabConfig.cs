using UnityEngine;

[CreateAssetMenu(fileName = "PrefabConfig", menuName = "UT Config/Prefab Config")]
public class UT_SO_PrefabConfig : ScriptableObject
{
    [SerializeField] public string[] PrefabAddressList;
};