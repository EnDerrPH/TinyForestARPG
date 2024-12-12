using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField] private CharacterClass _characterClass;
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private GameObject _characterHitPrefab;
    [SerializeField] private string _description;

    #region ProgressionStats
    [SerializeField] private int _characterLevel = 1;
    [SerializeField] private int _statPoints;
    [SerializeField] private int _skillPoints;
    [SerializeField] private float _currentExperience = 0;
    [SerializeField] private float _nextLevelExperience = 300;
    #endregion
    #region CharacterMainStats
    [Header("MAIN STATS")]
    [SerializeField] private int _characterHP;
    [SerializeField] private int _characterStrength;
    [SerializeField] private int _characterVitality;
    [SerializeField] private int _characterLuck;
    #endregion
    #region Sprites
    [Header("Sprites")]
    [SerializeField] private Sprite _startingSprite;
    [SerializeField] private Sprite _dashUp;
    [SerializeField] private Sprite _dashDown;
    [SerializeField] private Sprite _dashLeft;
    [SerializeField] private Sprite _dashRight;
    #endregion
    #region Audio
    [Header("Audio")]
    [SerializeField] protected AudioClip _attackAudio;
    [SerializeField] protected AudioClip _dashAudio;
    #endregion
    #region Get/Set
    public int HP { get => _characterHP; set { _characterHP = value; } }
    public int Strength { get => _characterStrength; set { _characterStrength = value; } }
    public int Vitality { get => _characterVitality; set { _characterVitality = value; } }
    public int Luck { get => _characterLuck; set { _characterLuck = value; } }
    public float CurrentExperience { get => _currentExperience; set { _currentExperience = value; } }
    public float NextLevelExperience { get => _nextLevelExperience; set { _nextLevelExperience = value; } }
    public CharacterClass CharacterClass { get => _characterClass; set { _characterClass = value; } }
    public Sprite StartingSprite { get => _startingSprite; set { _startingSprite = value; } }
    public Sprite DashUp { get => _dashUp; set { _dashUp = value; } }
    public Sprite DashDown { get => _dashDown; set { _dashDown = value; } }
    public Sprite DashLeft { get => _dashLeft; set { _dashLeft = value; } }
    public Sprite DashRight { get => _dashRight; set { _dashRight = value; } }
    public string Description { get => _description; set { _description = value; } }
    public AudioClip AttackAudio => _attackAudio;
    public AudioClip DashAudio => _dashAudio;
    #endregion

    public RuntimeAnimatorController GetCharacterController()
    {
        return _animatorController;
    }

    public GameObject GetCharacterHit()
    {
        return _characterHitPrefab;
    }
}
