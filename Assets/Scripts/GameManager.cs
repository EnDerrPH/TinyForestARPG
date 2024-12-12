using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioList _audioList;
    [SerializeField] private PlayerCharacterData _playerCharacterData;
    [SerializeField] private List<CharacterData> _characterDataList;
    [SerializeField] private ColorUtilities _colorUtilities;
    [SerializeField] private SortOrderUtilities _sortOrderUtilities;
    [SerializeField] private Device _device;
    public static GameManager instance;
    public PlayerCharacterData PlayerCharacterData { get => _playerCharacterData; set { _playerCharacterData = value; } }
    public Device Device { get => _device; set { _device = value; } }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public ColorUtilities GetColorUtilities()
    {
        return _colorUtilities;
    }

    public SortOrderUtilities GetSortOrderUtilities()
    {
        return _sortOrderUtilities;
    }

    public AudioList GetAudioList()
    {
        return _audioList;
    }

    public List<CharacterData> GetCharacterDataList()
    {
        return _characterDataList;
    }
}
