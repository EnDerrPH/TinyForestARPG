using UnityEngine;

public class UICharacter : MonoBehaviour
{
    [SerializeField] private CharacterData _characterData;

    public CharacterData GetCharacterData()
    {
        return _characterData;
    }
}
