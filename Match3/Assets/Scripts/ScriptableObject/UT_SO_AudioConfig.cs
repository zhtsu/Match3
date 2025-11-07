using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "UT Config/Audio Config")]
public class UT_SO_AudioConfig : ScriptableObject
{
    [SerializeField] public string BgMusicAddress;
};