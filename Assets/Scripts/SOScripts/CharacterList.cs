using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/CharacterList", order = 1)]
public class CharacterList : ScriptableObject
{
    [SerializeField] private List<CharacterData> _characterDataList = new List<CharacterData>();

    public List<CharacterData> GetCharacterDataList()
    {
        return _characterDataList;
    }
}
