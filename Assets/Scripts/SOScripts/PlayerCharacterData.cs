using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Scriptable Objects/PlayerCharacterData")]
public class PlayerCharacterData : ScriptableObject
{
    [SerializeField] private CharacterData _characterData;
    [SerializeField] private string _playerName;
    #region ProgressionStats
    [SerializeField] private int _characterLevel = 1;
    [SerializeField] private int _statPoints;
    [SerializeField] private int _skillPoints;
    [SerializeField] private float _currentExperience;
    [SerializeField] private float _nextLevelExperience = 600;
    #endregion
     #region CharacterMainStats
    [Header("MAIN STATS")]
    [SerializeField] private int _characterMaxHP;
    [SerializeField] private int _characterStrength;
    [SerializeField] private int _characterVitality;
    [SerializeField] private int _characterLuck;
    #endregion
    #region CharacterSubStats
    [Header("SUB STATS")]
    [SerializeField] private double _attackPower;
    [SerializeField] private double _criticalChance;
    [SerializeField] private double _criticalDamage;
    [SerializeField] private int _defense;
    [SerializeField] private float _lifeRegeneration;
    #endregion

    #region Get/Set
    public int MaxHP { get => _characterMaxHP; set { _characterMaxHP = value; } }
    public int CharacterLevel { get => _characterLevel; set { _characterLevel = value; } }
    public int Strength { get => _characterStrength; set { _characterStrength = value; } }
    public int Vitality { get => _characterVitality; set { _characterVitality = value; } }
    public int Luck { get => _characterLuck; set { _characterLuck = value; } }
    public double AttackPower { get => _attackPower; set { _attackPower = value; } }
    public float CurrentExperience { get => _currentExperience; set { _currentExperience = value; } }
    public float NextLevelExperience { get => _nextLevelExperience; set { _nextLevelExperience = value; } }
    #endregion

    public CharacterData CharacterData { get => _characterData; set { _characterData = value; } }
    public string PlayerName { get => _playerName; set { _playerName = value; } }

    public UnityEvent OnGainExperienceEvent, OnLevelUpEvent;


    public void SetSubStats()
    {
        _criticalChance = _characterLuck/3;
        _criticalDamage = _attackPower + (_characterLuck / 3);
        _lifeRegeneration = (float)_characterVitality / 50;
        _attackPower = (_characterStrength * 10) / 2;
        _defense = (_characterVitality * 3) / 2;
    }

    public void GainExperience(float experience)
    {
        _currentExperience += experience;
        OnGainExperienceEvent.Invoke();
        if(_currentExperience >= _nextLevelExperience)
        {
            _characterLevel += 1;
            _statPoints += 5;
            _skillPoints += 1;
            _currentExperience = 0;
            _nextLevelExperience = (_nextLevelExperience * 3) / 2;
            OnLevelUpEvent.Invoke();
        }
    }

    public void InitializeStats()
    {
        _characterLevel = 1;
        _statPoints = 0;
        _skillPoints = 0;
        _currentExperience = 0;
        _criticalChance = 0;
        _criticalDamage = 0;
        _lifeRegeneration = 0;
        _attackPower = 0;
        _defense = 0;
    }

    public void SetPlayerCharacterStats()
    {
        _characterMaxHP = _characterData.HP;
        _characterStrength = _characterData.Strength;
        _characterVitality = _characterData.Vitality;
        _characterLuck = _characterData.Luck;
        _currentExperience = _characterData.CurrentExperience;
        _nextLevelExperience = _characterData.NextLevelExperience;
    }
}
