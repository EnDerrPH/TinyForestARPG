using UnityEngine;

[CreateAssetMenu(fileName = "AudioList", menuName = "Scriptable Objects/AudioList")]
public class AudioList : ScriptableObject
{
    [SerializeField] private AudioClip _MainMenuBGM;
    [SerializeField] private AudioClip _forestBGM;
    public AudioClip MainMenuBGM => _MainMenuBGM;
    public AudioClip ForestBGM => _forestBGM;

}
