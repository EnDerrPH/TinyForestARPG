using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIGameHandler : UIHandler
{
    [SerializeField] private GameObject _pcUI;
    [SerializeField] private GameObject _mobileUI;
    [SerializeField] private GameObject _device;
    [SerializeField] private GameObject _statUI;
    [SerializeField] private GameObject _skillUI;
    [SerializeField] private Image _expFillBar;
    [SerializeField] private Image _hpFillBar;
    [SerializeField] private TMP_Text _ExpPercentageText;
    [SerializeField] private TMP_Text _LevelText;
    [SerializeField] private Button _statButton;
    [SerializeField] private Button _skillButton;
    [SerializeField] private List<EnemyController> _enemyList = new List<EnemyController>();
    private int _currentHP = 0;
    private PlayerCharacterData _playerCharacterData;

    public override void Start()
    {
        SetUIData();
        SetEnemyList();
        base.Start();
        CheckDevice();
    }

    private void CheckDevice()
    {
        _device = GameManager.instance.Device == Device.PC? _pcUI : _mobileUI;
        _device.SetActive(true);
    }

    private void SetUIData()
    {
        _playerCharacterData = GameManager.instance.PlayerCharacterData;
        UpdateExperience();
        UpdateHealth();
    }

    private void UpdateExperience()
    {
        float experience = (_playerCharacterData.CurrentExperience / _playerCharacterData.NextLevelExperience) * 100;
        float expFillBar = experience * .01f;
        _expFillBar.fillAmount = expFillBar;
        _ExpPercentageText.text = experience.ToString("F2") + "%";  
    }

    private void UpdateHealth()
    {
        _currentHP = _playerCharacterData.MaxHP;
        int health = (_currentHP / _playerCharacterData.MaxHP);
        _hpFillBar.fillAmount = health;
    }

    private void UpdatePlayerLevel()
    {
        UpdateExperience();
        _statButton.gameObject.SetActive(true);
        _skillButton.gameObject.SetActive(true);
        _LevelText.text = "Lvl " + _playerCharacterData.CharacterLevel.ToString();
    }

    private void SetEnemyList()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject gameObject in gameObjects)
        {
            EnemyController enemyController = gameObject.GetComponent<EnemyController>();
            _enemyList.Add(enemyController);
        }
    }

    public override void AddListeners()
    {
        _playerCharacterData.OnGainExperienceEvent.AddListener(UpdateExperience);
        _playerCharacterData.OnLevelUpEvent.AddListener(UpdatePlayerLevel);
        _statButton.onClick.AddListener(() => Open(_statUI));
        _skillButton.onClick.AddListener(() => Open(_skillUI));
    }

}
