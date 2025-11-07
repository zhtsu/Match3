using UnityEngine;

public struct UT_FUIConfigPair
{
    [SerializeField] public UT_EScreenUIType Type;
    [SerializeField] public string Address;
}

[CreateAssetMenu(fileName = "UIConfig", menuName = "UT Config/UI Config")]
public class UT_SO_UIConfig : ScriptableObject
{
    [SerializeField] public UT_UI_LoadingScreen LoadingScreenPrefab;
    [SerializeField] public UT_FUIConfigPair[] UIPrefabAddressDict;
};