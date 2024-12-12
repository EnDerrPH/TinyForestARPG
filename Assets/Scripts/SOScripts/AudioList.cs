using UnityEngine;

[CreateAssetMenu(fileName = "AudioList", menuName = "Scriptable Objects/AudioList")]
public class AudioList : ScriptableObject
{
    [SerializeField] private AudioClip _arrowRelease;
    public AudioClip ArrowRelease => _arrowRelease;

}
