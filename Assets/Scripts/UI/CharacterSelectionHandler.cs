using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionHandler : UIHandler
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private TMP_InputField _inputNameField;
    [SerializeField] private GameObject _character;
    [SerializeField] private GameObject _characterSelectionTab;
    [SerializeField] private GameObject _playerNameTab;
    [SerializeField] private TMP_Text _characterClass;
    [SerializeField] private TMP_Text _characterDescription;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _strText;
    [SerializeField] private TMP_Text _vitText;
    [SerializeField] private TMP_Text _luckText;
    [SerializeField] private CharacterData _currentCharacter;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _confirmButton;
    private int _currentIndex;
    private int _startingCharacterCount;
    private int _lastCharacterCount;

    private bool _isSelected;

    public override void Start()
    {
        base.Start();
    }

    public override void AddListeners()
    {
        _startButton.onClick.AddListener(StartGame); 
        _nextButton.onClick.AddListener(NextCharacter); 
        _previousButton.onClick.AddListener(PreviousCharacter); 
        _confirmButton.onClick.AddListener(OnConfirm); 
    }

    private void InitializeCurrentCharacter()
    {
        _character.SetActive(true);
        _currentIndex = 0;
        _currentCharacter = GameManager.instance.GetCharacterDataList()[_currentIndex];
        _lastCharacterCount = GameManager.instance.GetCharacterDataList().Count-1;
        _startingCharacterCount = 0;
    }

    private void SetCharacterDetails()
    {
        if(_currentCharacter == null)
        {
            return;
        }
        _character.GetComponent<SpriteRenderer>().sprite = _currentCharacter.StartingSprite; 
        _characterAnimator.runtimeAnimatorController = _currentCharacter.GetCharacterController();
        _characterClass.text = _currentCharacter.CharacterClass.ToString();
        _characterDescription.text = _currentCharacter.Description;
        _hpText.text ="HP: " +  _currentCharacter.HP.ToString();
        _strText.text ="STR: " +  _currentCharacter.Strength.ToString();
        _vitText.text ="VIT: " +  _currentCharacter.Vitality.ToString();
        _luckText.text ="LUK: " +  _currentCharacter.Luck.ToString();
        _characterAnimator.SetFloat("moveY", -1f);
    }

    private void StartGame()
    {
        GameManager.instance.PlayerCharacterData.CharacterData = _currentCharacter;
        LoadScene("GameSCN");
    }

    private void NextCharacter()
    {
        if(_currentIndex == _lastCharacterCount)
        {
            SetCurrentCharacter(_startingCharacterCount);
            return;
        }
        _currentIndex+= 1;
        SetCurrentCharacter(_currentIndex);
    }

    private void PreviousCharacter()
    {
        if(_currentIndex == _startingCharacterCount)
        {
            SetCurrentCharacter(_lastCharacterCount);
            return;
        }
        _currentIndex-= 1;
        SetCurrentCharacter(_currentIndex);
    }

    private void SetCurrentCharacter(int currentIndex)
    {
        _currentIndex = currentIndex;
        _currentCharacter = GameManager.instance.GetCharacterDataList()[_currentIndex];
        SetCharacterDetails();
    }

    private void OnConfirm()
    {
        if (string.IsNullOrEmpty(_inputNameField.textComponent.text) || _inputNameField.textComponent.text.Length < 6)
        {
            return;
        }
        GameManager.instance.PlayerCharacterData.PlayerName = _inputNameField.textComponent.text;
        _playerNameTab.SetActive(false);
        Close(_playerNameTab);
        Open(_characterSelectionTab);
        InitializeCurrentCharacter();
        SetCharacterDetails();
    }

}
